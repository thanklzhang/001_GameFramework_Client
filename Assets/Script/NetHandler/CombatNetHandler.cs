using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CombatNetHandler : NetHandler
{
    //public Action playerLoadCombatFinishAction;
    //public Action normalAttackAction;
    //public override void Init()
    //{
    //    base.Init();

    //    AddListener(ProtoMsgIds.GC2SS_PlayerLoadCombatFinish, RespPlayerLoadCombatFinish);
    //    AddListener(ProtoMsgIds.GC2SS_SyncCombatStart, RespSyncCombatStart);
    //    AddListener(ProtoMsgIds.GC2SS_SyncRoundActionStart, OnRespSyncRoundActionStart);
    //    AddListener(ProtoMsgIds.GC2SS_SyncTimelineAllSequence, OnRespSyncTimelineAllSequence);
    //    AddListener(ProtoMsgIds.GC2SS_SyncCombatEnd, RespCombatEnd);
    //}

    //public void ReqPlayerLoadCombatFinish(Action action = null)
    //{
    //    reqPlayerLoadCombatFinish req = new reqPlayerLoadCombatFinish();
    //    playerLoadCombatFinishAction = action;
    //    //send message
    //    this.SendMsgToSS(ProtoMsgIds.GC2SS_PlayerLoadCombatFinish, req);
    //}

    //public void RespPlayerLoadCombatFinish(byte[] data)
    //{
    //    respPlayerLoadCombatFinish finish = respPlayerLoadCombatFinish.Parser.ParseFrom(data);
    //    if (finish.Ret != ResultCode.Success)
    //    {
    //        Console.WriteLine("error : " + finish.Ret.ToString());
    //    }

    //    playerLoadCombatFinishAction?.Invoke();

    //}

    ////都加载好资源了 真正的开始战斗了
    //public void RespSyncCombatStart(byte[] data)
    //{

    //    SyncCombatStart combatStart = SyncCombatStart.Parser.ParseFrom(data);
    //    EventManager.Broadcast((int)GameEvent.CombatStart);
    //    //playerLoadCombatFinishAction?.Invoke();

    //}


    ////public void ReqSendNormalAttack(Action callback)
    ////{
    ////    normalAttackAction = callback;
    ////    reqNormalAttack normalAttack = new reqNormalAttack();
    ////    this.SendMsgToSS(ProtoMsgIds.GC2SS_NormalAttack, normalAttack);
    ////}

    ////public void RespNormalAttack()
    ////{
    ////    normalAttackAction?.Invoke();
    ////}


    //public void OnRespSyncRoundActionStart(byte[] data)
    //{
    //    SyncRoundActionStart roundAction = SyncRoundActionStart.Parser.ParseFrom(data);
    //    EventManager.Broadcast((int)GameEvent.CombatRoundActionStart, roundAction);
    //}

   

    //public void ReqPlayerFinishOperation(reqCombatPlayerOperation reqOp)
    //{
    //    SendMsgToSS(ProtoMsgIds.GC2SS_CombatPlayerOperation, reqOp);
    //}

    //public void OnRespSyncTimelineAllSequence(byte[] data)
    //{
    //    SyncTimelineSequenceProto proto = SyncTimelineSequenceProto.Parser.ParseFrom(data);
    //    //EventManager.Broadcast((int)GameEvent.CombatRoundShowStart, proto);
    //    CombatManager.Instance.OnSyncTimelineSequenceStart(proto);
    //}

    //public void RespCombatEnd(byte[] data)
    //{
    //    SyncCombatEnd end = SyncCombatEnd.Parser.ParseFrom(data);
    //    CombatResultData resultData = new CombatResultData()
    //    {
    //        isWin = 1 == end.IsWin,
    //        enemyAttack = end.EnemtyAttack
    //    };
    //    EventManager.Broadcast((int)GameEvent.CombatEnd, resultData);
    //}

}

