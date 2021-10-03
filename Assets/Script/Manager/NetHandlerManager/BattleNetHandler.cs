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
    public override void Init()
    {

        AddListener((int)ProtoIDs.NotifyCreateBattle, OnNotifyCreateBattle);


        AddListener((int)ProtoIDs.TransitionBattle2Player, OnTransitionBattle2Player);
       
        //AddListener((int)ProtoIDs.EnterGame, OnEnterGame);

    }


    //转发协议相关//////////////////////////////////////////

    //统一转发战斗消息
    public void TransitionBattleMsg(ProtoIDs cmd, IMessage msg)
    {
        csTransitionBattle tranBattleMsg = new csTransitionBattle();
        tranBattleMsg.Cmd = (int)cmd;
        tranBattleMsg.Data = msg.ToByteString();
        NetworkManager.Instance.SendMsg(cmd, tranBattleMsg.ToByteArray());
    }

    //统一接受战斗消息并解析为客户端需要的结构
    private void OnTransitionBattle2Player(MsgPack msgPack)
    {
        var trans = csTransitionBattle.Parser.ParseFrom(msgPack.data);

        var cmd = (ProtoIDs)trans.Cmd;
        var data = trans.Data;

        //if (cmd == ProtoIDs.StartBattle)
        //{
        //    this.OnStartBattle(data);
        //}
    }

    //客户端真正操作的协议/////////////////////////////////////////////////////////////////////////

    public void OnNotifyCreateBattle(MsgPack msgPack)
    {
        Logx.Log("OnNotifyCreateBattle");
        var battleInit = NetProto.scNotifyCreateBattle.Parser.ParseFrom(msgPack.data);
        GameDataManager.Instance.BattleGameDataStore.SetBattleInitData(battleInit.BattleInitArg);

        //EventDispatcher.Broadcast(EventIDs.OnCreateBattle);


        CtrlManager.Instance.Enter<BattleCtrl>();
    }
    ////战斗开始
    //public void SendStartBattle(Action<scStartBattle> action)
    //{
    //    csStartBattle start = new csStartBattle();
    //    startBattleAction = action;

    //    TransitionBattleMsg(ProtoIDs.StartBattle, start);
    //}

    //public void OnStartBattle(ByteString data)
    //{
    //    scStartBattle startBattle = scStartBattle.Parser.ParseFrom(data);

    //    startBattleAction?.Invoke(startBattle);
    //    startBattleAction = null;

    //}

    //上报加载进度
    public void SendPlayerLoadProgress(Action action)
    {

    }

    public void OnPlayerLoadProgress()
    {

    }

    //所有人都加载好了
    public void SendNotifyAllPlayerLoadFinish(Action action)
    {

    }

    public void OnNotifyAllPlayerLoadFinish()
    {

    }

    //战斗准备完成
    public void SendBattleReadyFinish(Action action)
    {

    }

    public void OnBattleReadyFinish()
    {

    }

    //战斗流程正式开始
    public void SendNotifyBattleProcessStart(Action action)
    {

    }

    public void OnNotifyBattleProcessStart()
    {

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

    }

    public void OnNotifyCreateEntities()
    {

    }


    #endregion


}

