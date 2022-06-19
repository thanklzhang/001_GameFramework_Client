using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
using GameData;
using Battle_Client;

public class BattleNetHandler : NetHandler
{
    //public Action<scStartBattle> startBattleAction;
    //public Action<scEnterGame> enterGameResultAction;

    public Dictionary<ProtoIDs, Action<byte[]>> battleTransitionMsgDic;

    public override void Init()
    {
        battleTransitionMsgDic = new Dictionary<ProtoIDs, Action<byte[]>>();

        //战斗流程正常协议
        AddListener((int)ProtoIDs.NotifyCreateBattle, OnNotifyCreateBattle);
        AddListener((int)ProtoIDs.TransitionBattle2Player, OnTransitionBattle2Player);
        AddListener((int)ProtoIDs.NotifyBattleEnd, OnNotifyBattleEnd);

        //AddListener((int)ProtoIDs.EnterGame, OnEnterGame);

        //战斗转发协议 需要转一下
        AddBattleMsg(ProtoIDs.PlayerLoadProgress, this.OnPlayerLoadProgress);
        AddBattleMsg(ProtoIDs.NotifyAllPlayerLoadFinish, this.OnNotifyAllPlayerLoadFinish);
        AddBattleMsg(ProtoIDs.BattleReadyFinish, this.OnBattleReadyFinish);
        AddBattleMsg(ProtoIDs.NotifyBattleStart, this.OnNotifyBattleStart);
        AddBattleMsg(ProtoIDs.NotifyCreateEntities, this.OnNotifyCreateEntities);
        AddBattleMsg(ProtoIDs.NotifyEntityMove, this.OnNotifyEntityMove);
        AddBattleMsg(ProtoIDs.NotifyEntityStopMove, this.OnNotifyEntityStopMove);
        AddBattleMsg(ProtoIDs.NotifyEntityReleaseSkill, this.OnNotifyEntityUseSkill);
        AddBattleMsg(ProtoIDs.NotifyCreateSkillEffect, this.OnNotifyCreateSkillEffect);
        AddBattleMsg(ProtoIDs.NotifySkillEffectStartMove, this.OnNotifySkillEffectStartMove);
        AddBattleMsg(ProtoIDs.NotifySkillEffectDestroy, this.OnNotifySkillEffectDestroy);
        AddBattleMsg(ProtoIDs.NotifySyncEntityAttr, this.OnNotifySyncEntityAttr);
        AddBattleMsg(ProtoIDs.NotifySyncEntityValue, this.OnNotifySyncEntityValue);
        AddBattleMsg(ProtoIDs.NotifyEntityDead, this.OnNotifyEntityDead);
        AddBattleMsg(ProtoIDs.NotifyPlayPlot, this.OnNotifyPlayPlot);
        AddBattleMsg(ProtoIDs.ClientPlotEnd, this.OnClientPlotEnd);
        AddBattleMsg(ProtoIDs.NotifyPlotEnd, this.OnNotifyPlotEnd);
        AddBattleMsg(ProtoIDs.NotifySetEntityShowState, this.OnNotifySetEntityShowState);

    }


    void AddBattleMsg(ProtoIDs cmd, Action<byte[]> action)
    {
        battleTransitionMsgDic.Add(cmd, action);
    }


    //转发协议相关//////////////////////////////////////////

    //统一转发战斗消息
    public void TransitionBattleMsg(ProtoIDs cmd, IMessage msg)
    {
        Logx.Log("TransitionBattleMsg : send true cmd : " + cmd);
        csTransitionBattle tranBattleMsg = new csTransitionBattle();

        var myUid = GameDataManager.Instance.UserStore.Uid;
        ClientProtoHead head = new ClientProtoHead()
        {
            cmd = (ushort)cmd,
            uid = myUid
        };

        var clientData = ProtoMsgUtil.MakeClientMsgBytes(msg.ToByteArray(), head);

        tranBattleMsg.Cmd = (int)cmd;
        tranBattleMsg.Data = ByteString.CopyFrom(clientData);
        NetworkManager.Instance.SendMsg(ProtoIDs.TransitionBattle, tranBattleMsg.ToByteArray());
    }

    //统一接受战斗消息并解析为客户端需要的结构
    private void OnTransitionBattle2Player(MsgPack msgPack)
    {
        var trans = scTransitionBattle2Player.Parser.ParseFrom(msgPack.data);

        var clientTrueData = trans.Data;

        var clientMsg = ProtoMsgUtil.GetClientMsg(clientTrueData.ToByteArray());
        var clientCmd = (ProtoIDs)clientMsg.head.cmd;

        Logx.Log("OnTransitionBattle2Player , receive the true cmd : " + clientCmd);

        var battleMsgData = clientMsg.data;

        if (battleTransitionMsgDic.ContainsKey(clientCmd))
        {
            var action = battleTransitionMsgDic[clientCmd];
            action?.Invoke(battleMsgData);
        }
        else
        {
            Logx.LogWarning("the cmd is not found : " + clientCmd);
        }
    }

    public void OnNotifyCreateBattle(MsgPack msgPack)
    {
        Logx.Log("OnNotifyCreateBattle");
        var battleInit = NetProto.scNotifyCreateBattle.Parser.ParseFrom(msgPack.data);
        //GameDataManager.Instance.BattleGameDataStore.SetBattleInitData(battleInit.BattleInitArg);

        ////EventDispatcher.Broadcast(EventIDs.OnCreateBattle);


        //CtrlManager.Instance.Enter<BattleCtrl>();

        BattleManager.Instance.CreateBattle(battleInit);
    }

    //客户端真正操作的协议/////////////////////////////////////////////////////////////////////////


    //上报加载进度(目前算是 100 )
    public void SendPlayerLoadProgress(int progress)
    {
        csPlayerLoadProgress csProgress = new csPlayerLoadProgress();
        csProgress.Progress = progress;
        TransitionBattleMsg(ProtoIDs.PlayerLoadProgress, csProgress);
    }

    public void OnPlayerLoadProgress(byte[] byteData)
    {
        scPlayerLoadProgress progress = scPlayerLoadProgress.Parser.ParseFrom(byteData);
        Logx.Log("receive battle msg : PlayerLoadProgress");
    }

    //所有人都加载好了
    public void SendNotifyAllPlayerLoadFinish(Action action)
    {

    }

    public void OnNotifyAllPlayerLoadFinish(byte[] byteData)
    {
        Logx.Log("receive battle msg : NotifyAllPlayerLoadFinish");
        scNotifyAllPlayerLoadFinish allFinish = scNotifyAllPlayerLoadFinish.Parser.ParseFrom(byteData);

        ////先用数据层 之后考虑是否用 battleManager 流程
        //var battleData = GameDataManager.Instance.BattleGameDataStore;
        //battleData.IsAllPlayerLoadFinish = true;

        //EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);

        BattleManager.Instance.AllPlayerLoadFinish();

    }

    //战斗准备完成
    public void SendBattleReadyFinish(Action action)
    {
        csBattleReadyFinish ready = new csBattleReadyFinish();
        TransitionBattleMsg(ProtoIDs.BattleReadyFinish, ready);
    }

    public void OnBattleReadyFinish(byte[] byteData)
    {
        scBattleReadyFinish readyFinish = scBattleReadyFinish.Parser.ParseFrom(byteData);
    }

    //战斗正式开始
    public void SendNotifyBattleStart(Action action)
    {

    }

    public void OnNotifyBattleStart(byte[] byteData)
    {
        scNotifyBattleStart battleStart = scNotifyBattleStart.Parser.ParseFrom(byteData);
        //EventDispatcher.Broadcast(EventIDs.OnBattleStart);
        BattleManager.Instance.StartBattle();
    }



    private void OnNotifyCreateEntities(byte[] byteData)
    {
        scNotifyCreateEntities create = scNotifyCreateEntities.Parser.ParseFrom(byteData);
        foreach (var item in create.BattleEntities)
        {
            var serverEntity = item;
            BattleManager.Instance.CreateEntity(serverEntity);
        }
    }

    private void OnNotifyEntityMove(byte[] byteData)
    {
        scNotifyEntityMove notifyEntityMove = scNotifyEntityMove.Parser.ParseFrom(byteData);
        BattleManager.Instance.EntityStartMove(notifyEntityMove);
    }

    protected void OnNotifyEntityStopMove(byte[] byteData)
    {
        scNotifyEntityStopMove stop = scNotifyEntityStopMove.Parser.ParseFrom(byteData);
        BattleManager.Instance.EntityStopMove(stop);
    }

    public void SendUseSkill(int skillId, int targetGuid, Vector3 targetPos)
    {
        csUseSkill useSkill = new csUseSkill();
        useSkill.Guid = 1;//每个玩家就一个实体 忽略
        useSkill.SkillId = skillId;
        useSkill.TargetGuid = targetGuid;
        useSkill.TargetPos = BattleConvert.ConvertToVector3Proto(targetPos);
        TransitionBattleMsg(ProtoIDs.UseSkill, useSkill);
    }

    private void OnNotifyEntityUseSkill(byte[] byteData)
    {
        scNotifyEntityUseSkill releaseSkill = scNotifyEntityUseSkill.Parser.ParseFrom(byteData);
        BattleManager.Instance.EntityUseSkill(releaseSkill);
    }

    public void SendClientPlotEnd()
    {
        csClientPlotEnd clientPlotEnd = new csClientPlotEnd();
        TransitionBattleMsg(ProtoIDs.ClientPlotEnd, clientPlotEnd);
    }

    private void OnClientPlotEnd(byte[] byteData)
    {
        
    }

    private void OnNotifyCreateSkillEffect(byte[] byteData)
    {
        scNotifyCreateSkillEffect skillEffect = scNotifyCreateSkillEffect.Parser.ParseFrom(byteData);
        BattleManager.Instance.CreateSkillEffect(skillEffect);
    }

    private void OnNotifySkillEffectStartMove(byte[] byteData)
    {
        scNotifySkillEffectStartMove effectStartMove = scNotifySkillEffectStartMove.Parser.ParseFrom(byteData);
        BattleManager.Instance.SkillEffectStartMove(effectStartMove);
    }

    private void OnNotifySkillEffectDestroy(byte[] byteData)
    {
        scNotifySkillEffectDestroy skillEffectDestroy = scNotifySkillEffectDestroy.Parser.ParseFrom(byteData);
        BattleManager.Instance.DestroySkillEffect(skillEffectDestroy);
    }

    private void OnNotifySyncEntityAttr(byte[] byteData)
    {
        scNotifySyncEntityAttr sync = scNotifySyncEntityAttr.Parser.ParseFrom(byteData);
        BattleManager.Instance.SyncEntityAttr(sync);
    }

    private void OnNotifySyncEntityValue(byte[] byteData)
    {
        scNotifySyncEntityValue sync = scNotifySyncEntityValue.Parser.ParseFrom(byteData);
        BattleManager.Instance.SyncEntityValue(sync);
    }

    private void OnNotifyEntityDead(byte[] byteData)
    {
        scNotifyEntityDead sync = scNotifyEntityDead.Parser.ParseFrom(byteData);
        BattleManager.Instance.EntityDead(sync);
    }

    private void OnNotifyPlayPlot(byte[] byteData)
    {
        scNotifyPlayPlot sync = scNotifyPlayPlot.Parser.ParseFrom(byteData);
        BattleManager.Instance.PlayPlot(sync);
    }

    private void OnNotifyBattleEnd(MsgPack msgPack)
    {
        scNotifyBattleEnd sync = scNotifyBattleEnd.Parser.ParseFrom(msgPack.data);
        BattleManager.Instance.BattleEnd(sync);
    }

    public void OnNotifyPlotEnd(byte[] byteData)
    {
        scNotifyPlotEnd sync = scNotifyPlotEnd.Parser.ParseFrom(byteData);
        BattleManager.Instance.PlotEnd(sync);
    }

    public void OnNotifySetEntityShowState(byte[] byteData)
    {
        scNotifySetEntityShowState sync = scNotifySetEntityShowState.Parser.ParseFrom(byteData);
        BattleManager.Instance.SetEntitiesShowState(sync);
    }

    #region 战斗中玩家操作 

    //移动单位实体
    public void SendMoveEntity(int entityGuid, Vector3 targetPos)
    {
        csMoveEntity move = new csMoveEntity()
        {
            Guid = entityGuid,
            TargetPos = BattleConvert.ConvertToVector3Proto(targetPos)
        };
        this.TransitionBattleMsg(ProtoIDs.MoveEntity, move);

    }

    public void OnMoveEntity()
    {

    }

    #endregion


}

