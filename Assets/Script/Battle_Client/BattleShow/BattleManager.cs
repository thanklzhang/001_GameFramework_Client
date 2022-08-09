using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
namespace Battle_Client
{
    public enum BattleState
    {
        Null = 0,
        Loading = 1,
        Running = 2,
        End = 3
    }

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
        public int battleTableId;
        public int battleRoomId;

        //玩家信息
        public Dictionary<int, ClientPlayer> players;
        ClientPlayer localPlayer;
        //本地玩家控制的英雄
        BattleEntity localCtrlEntity;

        private BattleState battleState;
        public BattleState BattleState { get => battleState; set => battleState = value; }

        LocalBattleLogic_Executer localBattleExecuter;
        //public BattleEntityInfo LocalCtrlEntity
        //{
        //    get
        //    {
        //        var entity = battleInfo.FindEntityById(LocalPlayer.ctrlHeroGuid);
        //        return entity;
        //    }
        //}

        IBattleClientMsgSender msgSender;
        public IBattleClientMsgSender MsgSender { get => msgSender; set => msgSender = value; }

        private IBattleClientMsgReceiver msgReceiver;
        public IBattleClientMsgReceiver MsgReceiver { get => msgReceiver; set => msgReceiver = value; }

        public void Init()
        {
            this.RegisterListener();
            BattleState = BattleState.Null;
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
        }

        internal Battle.Battle GetBattle()
        {
            return this.localBattleExecuter.GetBattle();
        }




        ///// <summary>
        ///// 创建战斗
        ///// </summary>
        ///// <param name="battleInit"></param>
        //public void CreateBattle(scNotifyCreateBattle battleInit)
        //{
        //    Logx.Log("battle manager : CreateBattle");
        //    var battleInitArg = battleInit.BattleInitArg;
        //    //战斗信息
        //    this.battleGuid = battleInitArg.Guid;
        //    this.battleTableId = battleInitArg.TableId;
        //    this.battleRoomId = battleInitArg.RoomId;

        //    //玩家信息
        //    players = new Dictionary<int, ClientPlayer>();
        //    foreach (var serverPlayer in battleInitArg.BattlePlayerInitArg.PlayerList)
        //    {
        //        ClientPlayer player = new ClientPlayer()
        //        {
        //            playerIndex = serverPlayer.PlayerIndex,
        //            team = serverPlayer.Team,
        //            uid = serverPlayer.Uid,
        //            ctrlHeroGuid = serverPlayer.CtrlHeroGuid
        //        };

        //        this.players.Add(player.uid, player);
        //    }

        //    //设置本地玩家
        //    var userDataStore = GameDataManager.Instance.UserStore;
        //    var uid = (int)userDataStore.Uid;
        //    if (players.ContainsKey(uid))
        //    {
        //        this.localPlayer = players[uid];
        //    }
        //    else
        //    {
        //        Logx.LogError("the uid of localPlayer is not found : " + uid);
        //    }

        //    //实体信息
        //    Logx.Log("battle manager : CreateInitEntity");
        //    foreach (var serverEntity in battleInitArg.EntityInitArg.BattleEntityInitList)
        //    {
        //        BattleEntityManager.Instance.CreateViewEntityInfo(serverEntity);
        //    }

        //    //设置本地玩家控制的英雄
        //    this.localCtrlEntity = BattleEntityManager.Instance.FindEntity(this.localPlayer.ctrlHeroGuid);

        //    battleState = BattleState.Loading;

        //    //进入战斗状态
        //    CtrlManager.Instance.Enter<BattleCtrl>();
        //}

        /// <summary>
        /// 创建战斗
        /// </summary>
        /// <param name="battleInit"></param>
        public void CreateBattle(BattleClient_CreateBattleArgs battleInit)
        {
            Logx.Log("battle manager : CreateBattle");

            //战斗信息
            this.battleGuid = battleInit.guid;
            this.battleTableId = battleInit.configId;
            this.battleRoomId = battleInit.roomId;

            //玩家信息
            players = new Dictionary<int, ClientPlayer>();
            foreach (var serverPlayer in battleInit.clientPlayers)
            {
                ClientPlayer player = new ClientPlayer()
                {
                    playerIndex = serverPlayer.playerIndex,
                    team = serverPlayer.team,
                    uid = (int)serverPlayer.uid,
                    ctrlHeroGuid = serverPlayer.ctrlHeroGuid
                };

                this.players.Add(player.uid, player);
            }

            //设置本地玩家
            var userDataStore = GameDataManager.Instance.UserStore;
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
            foreach (var serverEntity in battleInit.entityList)
            {
                BattleEntityManager.Instance.CreateViewEntityInfo(serverEntity);
            }

            //设置本地玩家控制的英雄
            this.localCtrlEntity = BattleEntityManager.Instance.FindEntity(this.localPlayer.ctrlHeroGuid);

            battleState = BattleState.Loading;

            //进入战斗状态
            CtrlManager.Instance.Enter<BattleCtrl>();
        }

        public Map GetLocalBattleMap()
        {
            return localBattleExecuter.GetMap();
        }

        public void CreateLocalBattle(int battleConfigId)
        {
            //填充数据

            //初始化本地战斗后台逻辑
            localBattleExecuter = new LocalBattleLogic_Executer();
            localBattleExecuter.Init();
            var battleLogic = localBattleExecuter.CreateLocalBattleLogic(battleConfigId);
            var battleClientArgs = GetBattleClientArgs(battleLogic);

            //填充客户端所需组件
            msgSender = new BattleClient_MsgSender_Local(battleLogic);
            msgReceiver = new BattleClient_MsgReceiver_Impl();

            ////客户端开启战斗
            //BattleManager.Instance.CreateBattle(battleClientArgs);

            ////启动本地战斗后台逻辑
            //localBattleExecuter.StartBattleLogic();

            GameMain.Instance.StartCoroutine(StartBattle(battleClientArgs));
        }

        IEnumerator StartBattle(BattleClient_CreateBattleArgs battleClientArgs)
        {
            yield return new WaitForSeconds(0.1f);

            //客户端开启战斗
            BattleManager.Instance.CreateBattle(battleClientArgs);

            yield return null;

            //启动本地战斗后台逻辑
            localBattleExecuter.StartBattleLogic();
        }


        public Battle_Client.BattleClient_CreateBattleArgs GetBattleClientArgs(Battle.Battle battle)
        {
            var _battle = battle;
            Battle_Client.BattleClient_CreateBattleArgs battleClientArgs = new BattleClient_CreateBattleArgs();
            battleClientArgs.guid = _battle.Guid;
            battleClientArgs.configId = _battle.tableId;
            battleClientArgs.roomId = _battle.RoomId;

            battleClientArgs.clientPlayers = new List<BattleClient_ClientPlayer>();

            var _battlePlayerList = _battle.GetAllPlayers();
            foreach (var _player in _battlePlayerList)
            {
                BattleClient_ClientPlayer player = new BattleClient_ClientPlayer();
                player.playerIndex = _player.playerIndex;
                player.team = _player.team;
                player.uid = _player.uid;
                player.ctrlHeroGuid = _player.ctrlHeroGuid;

                battleClientArgs.clientPlayers.Add(player);
            }

            //地图尺寸
            battleClientArgs.mapSizeX = _battle.GetMapSizeX();
            battleClientArgs.mapSizeZ = _battle.GetMapSizeZ();


            battleClientArgs.entityList = new List<BattleClientMsg_Entity>();
            var entities = _battle.GetAllEntities();
            foreach (var keyV in entities)
            {
                var _entity = keyV.Value;

                var entity = new BattleClientMsg_Entity();
                entity.guid = _entity.guid;
                entity.configId = _entity.configId;
                entity.playerIndex = _entity.playerIndex;

                entity.position = new UnityEngine.Vector3(_entity.position.x, _entity.position.y, _entity.position.z); ;

                //netEntity.MaxHp = (int)entity.MaxHealth;
                //netEntity.CurrHp = netEntity.MaxHp;

                //技能
                var skills = _entity.GetAllSkills();
                entity.skills = new List<BattleClientMsg_Skill>();
                foreach (var skillKV in skills)
                {
                    var skill = skillKV.Value;

                    BattleClientMsg_Skill skillInfo = new BattleClientMsg_Skill();
                    skillInfo.configId = skill.configId;
                    skillInfo.level = skill.level;
                    entity.skills.Add(skillInfo);
                }
                battleClientArgs.entityList.Add(entity);
            }



            return battleClientArgs;
        }


        //public void AllPlayerLoadFinish()
        //{
        //    EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);
        //}

        //public void StartBattle()
        //{
        //    EventDispatcher.Broadcast(EventIDs.OnBattleStart);

        //    //已经加载好的实体统一走一遍创建流程
        //    BattleEntityManager.Instance.NotifyCreateAllEntities();

        //    battleState = BattleState.Running;
        //}

        //public void CreateEntity(NetProto.BattleEntityProto serverEntity)
        //{
        //    var entity = BattleEntityManager.Instance.CreateEntity(serverEntity);
        //    EventDispatcher.Broadcast<BattleEntity>(EventIDs.OnCreateEntity, entity);
        //}



        ////logic 直接调用
        //public void EntityStartMove(BattleMsg_NotifyEntityMove entityMove)
        //{
        //    var guid = entityMove.Guid;
        //    var targetPos = entityMove.EndPos;
        //    var moveSpeed = entityMove.MoveSpeed;
        //    var entity = BattleEntityManager.Instance.FindEntity(guid);
        //    if (entity != null)
        //    {
        //        entity.StartMove(targetPos, moveSpeed);
        //    }
        //}

        ////net 层转换一次然后调用
        //public void EntityStartMove(scNotifyEntityMove notifyEntityMove)
        //{
        //    var guid = notifyEntityMove.Guid;
        //    var targetPos = BattleConvert.ConverToVector3(notifyEntityMove.EndPos);
        //    var moveSpeed = BattleConvert.GetValue(notifyEntityMove.MoveSpeed);

        //    BattleMsg_NotifyEntityMove msg = new BattleMsg_NotifyEntityMove()
        //    {
        //        Guid = guid,
        //        EndPos = targetPos,
        //        MoveSpeed = moveSpeed
        //    };

        //    EntityStartMove(msg);
        //}

        ////public void EntityStartMove(scNotifyEntityMove notifyEntityMove)
        ////{
        ////    var guid = notifyEntityMove.Guid;
        ////    var targetPos = BattleConvert.ConverToVector3(notifyEntityMove.EndPos);
        ////    var moveSpeed = BattleConvert.GetValue(notifyEntityMove.MoveSpeed);
        ////    var entity = BattleEntityManager.Instance.FindEntity(guid);
        ////    if (entity != null)
        ////    {
        ////        entity.StartMove(targetPos, moveSpeed);
        ////    }
        ////}

        //public void EntityStopMove(scNotifyEntityStopMove stop)
        //{
        //    var guid = stop.Guid;
        //    var endPos = BattleConvert.ConverToVector3(stop.EndPos);
        //    var entity = BattleEntityManager.Instance.FindEntity(guid);
        //    if (entity != null)
        //    {
        //        entity.StopMove(endPos);
        //    }
        //}

        //public void EntityUseSkill(scNotifyEntityUseSkill releaseSkill)
        //{
        //    var guid = releaseSkill.Guid;
        //    var entity = BattleEntityManager.Instance.FindEntity(guid);
        //    if (entity != null)
        //    {
        //        entity.ReleaseSkill();
        //    }

        //}

        //public void CreateSkillEffect(scNotifyCreateSkillEffect createSkillEffect)
        //{
        //    var guid = createSkillEffect.Guid;
        //    var resId = createSkillEffect.ResId;
        //    var pos = BattleConvert.ConverToVector3(createSkillEffect.Position);
        //    var followEntityGuid = createSkillEffect.FollowEntityGuid;
        //    BattleSkillEffectManager.Instance.CreateSkillEffect(guid, resId, pos, followEntityGuid);
        //}

        //public void SkillEffectStartMove(scNotifySkillEffectStartMove effectStartMove)
        //{
        //    var guid = effectStartMove.EffectGuid;
        //    var skillEffect = BattleSkillEffectManager.Instance.FindSkillEffect(guid);
        //    if (skillEffect != null)
        //    {
        //        var targetPos = BattleConvert.ConverToVector3(effectStartMove.TargetPos);
        //        var targetGuid = effectStartMove.TargetGuid;
        //        //var isFollow = effectStartMove.IsFollow;
        //        var moveSpeed = BattleConvert.GetValue(effectStartMove.MoveSpeed);
        //        skillEffect.StartMove(targetPos, targetGuid, moveSpeed);
        //    }
        //}

        //public void DestroySkillEffect(scNotifySkillEffectDestroy skillEffectDestroy)
        //{
        //    var effectGuid = skillEffectDestroy.EffectGuid;
        //    BattleSkillEffectManager.Instance.DestorySkillEffect(effectGuid);
        //}

        //internal void SyncEntityAttr(scNotifySyncEntityAttr sync)
        //{
        //    var entityGuid = sync.EntityGuid;
        //    var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
        //    if (entity != null)
        //    {
        //        entity.SyncAttr(sync.Attrs);
        //    }
        //}


        //internal void SyncEntityValue(scNotifySyncEntityValue sync)
        //{
        //    var entityGuid = sync.EntityGuid;
        //    var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
        //    if (entity != null)
        //    {
        //        entity.SyncValue(sync.Values);
        //    }
        //}

        //public void EntityDead(scNotifyEntityDead entityDead)
        //{
        //    var entityGuid = entityDead.EntityGuid;
        //    var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
        //    if (entity != null)
        //    {
        //        entity.Dead();
        //    }
        //}

        //public void PlayPlot(scNotifyPlayPlot playPlot)
        //{
        //    var name = playPlot.PlotName;

        //    BattleEntityManager.Instance.SetAllEntityShowState(false);
        //    CameraManager.Instance.GetCameraUI().SetUICameraShowState(false);

        //    PlotManager.Instance.StartPlot(name);
        //}


        //public void BattleEnd(scNotifyBattleEnd battleEnd)
        //{
        //    var isWin = 1 == battleEnd.IsWin;

        //    battleState = BattleState.End;

        //    EventDispatcher.Broadcast<bool>(EventIDs.OnBattleEnd, isWin);
        //}

        //internal void PlotEnd(scNotifyPlotEnd sync)
        //{
        //    BattleEntityManager.Instance.SetAllEntityShowState(true);
        //    CameraManager.Instance.GetCameraUI().SetUICameraShowState(true);

        //    PlotManager.Instance.ClosePlot();
        //}


        //internal void SetEntitiesShowState(scNotifySetEntityShowState sync)
        //{
        //    var entityGuids = sync.Guids.ToList();
        //    var isShow = sync.IsShow;
        //    BattleEntityManager.Instance.SetEntitiesShowState(entityGuids, isShow);
        //}

        ////get --------------------

        public void GetSkillByIndex()
        {

        }

        //

        public void Update(float timeDelta)
        {
            localBattleExecuter?.Update(timeDelta);
        }

        public void RemoveListener()
        {
            //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
        }

        public void Clear()
        {
            this.RemoveListener();
            localBattleExecuter = null;
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
}