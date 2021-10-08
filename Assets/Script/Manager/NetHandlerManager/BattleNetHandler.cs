using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
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

        //AddListener((int)ProtoIDs.EnterGame, OnEnterGame);

        //战斗转发协议 需要转一下
        AddBattleMsg(ProtoIDs.PlayerLoadProgress, this.OnPlayerLoadProgress);
        AddBattleMsg(ProtoIDs.NotifyAllPlayerLoadFinish, this.OnNotifyAllPlayerLoadFinish);
        AddBattleMsg(ProtoIDs.BattleReadyFinish, this.OnBattleReadyFinish);
        AddBattleMsg(ProtoIDs.NotifyBattleStart, this.OnNotifyBattleStart);
        AddBattleMsg(ProtoIDs.NotifyCreateEntities, this.OnNotifyCreateEntities);

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

        var myUid = GameDataManager.Instance.UserGameDataStore.Uid;
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
        GameDataManager.Instance.BattleGameDataStore.SetBattleInitData(battleInit.BattleInitArg);

        //EventDispatcher.Broadcast(EventIDs.OnCreateBattle);


        CtrlManager.Instance.Enter<BattleCtrl>();
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

        //先用数据层 之后考虑是否用 battleManager 流程
        var battleData = GameDataManager.Instance.BattleGameDataStore;
        battleData.IsAllPlayerLoadFinish = true;

        EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);

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
        EventDispatcher.Broadcast(EventIDs.OnBattleStart);
    }

    private void OnNotifyCreateEntities(byte[] byteData)
    {
        scNotifyCreateEntities create = scNotifyCreateEntities.Parser.ParseFrom(byteData);
        foreach (var item in create.BattleEntities)
        {
            var serverEntity = item;
            var entity = BattleEntityManager.Instance.CreateEntity(serverEntity.Guid, serverEntity.ConfigId);
            if (entity != null)
            {
                var v3 = BattleConvert.ConverToVector3(serverEntity.Position);
                entity.SetPosition(v3);
            }
        }
    }


    #region 玩家操作 

    //移动单位实体
    public void SendMoveEntity()
    {

    }

    public void OnMoveEntity()
    {

    }

    #endregion 

    #region 服务器发来的关键战斗事件

    //创建单位
    public void SendNotifyCreateEntities()
    {
        //BattleEntityManager.Instance.CreateEntity();
    }

    public void OnNotifyCreateEntities()
    {

    }


    #endregion


}

