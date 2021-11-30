using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetProto;
using UnityEngine;

public class ClientPlayer
{
    public int playerIndex;
    public int team;
    public int uid;

    public int ctrlHeroGuid;


}
public class BattleManager : Singleton<BattleManager>
{
    //战斗信息
    public int battleGuid;
    public int battleConfigId;
    public int battleRoomId;

    //玩家信息
    public Dictionary<int, ClientPlayer> players;
    ClientPlayer localPlayer;
    //本地玩家控制的英雄
    BattleEntity localCtrlEntity;
    //public BattleEntityInfo LocalCtrlEntity
    //{
    //    get
    //    {
    //        var entity = battleInfo.FindEntityById(LocalPlayer.ctrlHeroGuid);
    //        return entity;
    //    }
    //}

    public void Init()
    {
        this.RegisterListener();
    }

    public void RegisterListener()
    {
        //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
    }

    /// <summary>
    /// 创建战斗
    /// </summary>
    /// <param name="battleInit"></param>
    public void CreateBattle(scNotifyCreateBattle battleInit)
    {
        Logx.Log("battle manager : CreateBattle");
        var battleInitArg = battleInit.BattleInitArg;
        //战斗信息
        this.battleGuid = battleInitArg.Guid;
        this.battleConfigId = battleInitArg.ConfigId;
        this.battleRoomId = battleInitArg.RoomId;

        //玩家信息
        players = new Dictionary<int, ClientPlayer>();
        foreach (var serverPlayer in battleInitArg.BattlePlayerInitArg.PlayerList)
        {
            ClientPlayer player = new ClientPlayer()
            {
                playerIndex = serverPlayer.PlayerIndex,
                team = serverPlayer.Team,
                uid = serverPlayer.Uid,
                ctrlHeroGuid = serverPlayer.CtrlHeroGuid
            };

            this.players.Add(player.uid, player);
        }

        //设置本地玩家
        var userDataStore = GameDataManager.Instance.UserGameDataStore;
        var uid = (int)userDataStore.Uid;
        if (players.ContainsKey(uid))
        {
            this.localPlayer = players[uid];
        }
        else
        {
            Logx.LogError("the uid of localPlayer is not found : " + uid);
        }

        //实体信息
        Logx.Log("battle manager : CreateInitEntity");
        foreach (var serverEntity in battleInitArg.EntityInitArg.BattleEntityInitList)
        {
            BattleEntityManager.Instance.CreateViewEntityInfo(serverEntity);
        }

        //设置本地玩家控制的英雄
        this.localCtrlEntity = BattleEntityManager.Instance.FindEntity(this.localPlayer.ctrlHeroGuid);

        //进入战斗状态
        CtrlManager.Instance.Enter<BattleCtrl>();
    }

    public void AllPlayerLoadFinish()
    {
        EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);
    }

    public void StartBattle()
    {
        EventDispatcher.Broadcast(EventIDs.OnBattleStart);

        //已经加载好的实体统一走一遍创建流程
        BattleEntityManager.Instance.NotifyCreateAllEntities();
    }

    public void CreateEntity(NetProto.BattleEntityProto serverEntity)
    {
        var entity = BattleEntityManager.Instance.CreateEntity(serverEntity);
        EventDispatcher.Broadcast<BattleEntity>(EventIDs.OnCreateEntity, entity);
    }

    public void EntityStartMove(scNotifyEntityMove notifyEntityMove)
    {
        var guid = notifyEntityMove.Guid;
        var targetPos = BattleConvert.ConverToVector3(notifyEntityMove.EndPos);
        var moveSpeed = BattleConvert.GetValue(notifyEntityMove.MoveSpeed);
        var entity = BattleEntityManager.Instance.FindEntity(guid);
        if (entity != null)
        {
            entity.StartMove(targetPos, moveSpeed);
        }
    }

    public void EntityStopMove(scNotifyEntityStopMove stop)
    {
        var guid = stop.Guid;
        var endPos = BattleConvert.ConverToVector3(stop.EndPos);
        var entity = BattleEntityManager.Instance.FindEntity(guid);
        if (entity != null)
        {
            entity.StopMove(endPos);
        }
    }

    public void EntityUseSkill(scNotifyEntityUseSkill releaseSkill)
    {
        var guid = releaseSkill.Guid;
        var entity = BattleEntityManager.Instance.FindEntity(guid);
        if (entity != null)
        {
            entity.ReleaseSkill();
        }

    }

    public void CreateSkillEffect(scNotifyCreateSkillEffect createSkillEffect)
    {
        var guid = createSkillEffect.Guid;
        var resId = createSkillEffect.ResId;
        var pos = BattleConvert.ConverToVector3(createSkillEffect.Position);
        BattleSkillEffectManager.Instance.CreateSkillEffect(guid, resId, pos);
    }

    public void SkillEffectStartMove(scNotifySkillEffectStartMove effectStartMove)
    {
        var guid = effectStartMove.EffectGuid;
        var skillEffect = BattleSkillEffectManager.Instance.FindSkillEffect(guid);
        if (skillEffect != null)
        {
            var targetPos = BattleConvert.ConverToVector3(effectStartMove.TargetPos);
            var targetGuid = effectStartMove.TargetGuid;
            //var isFollow = effectStartMove.IsFollow;
            var moveSpeed = BattleConvert.GetValue(effectStartMove.MoveSpeed);
            skillEffect.StartMove(targetPos, targetGuid, moveSpeed);
        }
    }

    public void DestroySkillEffect(scNotifySkillEffectDestroy skillEffectDestroy)
    {
        var effectGuid = skillEffectDestroy.EffectGuid;
        BattleSkillEffectManager.Instance.DestorySkillEffect(effectGuid);
    }

    internal void SyncEntityAttr(scNotifySyncEntityAttr sync)
    {
        var entityGuid = sync.EntityGuid;
        var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
        if (entity != null)
        {
            entity.SyncAttr(sync.Attrs);
        }
    }


    internal void SyncEntityValue(scNotifySyncEntityValue sync)
    {
        var entityGuid = sync.EntityGuid;
        var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
        if (entity != null)
        {
            entity.SyncValue(sync.Values);
        }
    }

    //get --------------------

    public void GetSkillByIndex()
    {

    }

    //


    public void RemoveListener()
    {
        //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
    }

    public void Clear()
    {
        this.RemoveListener();
    }

    public int GetCtrlHeroSkillIdByIndex(int index)
    {
        return this.localCtrlEntity.GetSkillIdByIndex(index);
    }

    public GameObject GetLocalCtrlHeroGameObject()
    {
        return this.localCtrlEntity.gameObject;
    }

}
