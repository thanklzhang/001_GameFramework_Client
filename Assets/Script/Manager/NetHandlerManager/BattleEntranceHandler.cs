using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
public class BattleEntranceHandler : NetHandler
{
    public Action<scApplyHeroExamBattle> applyHeroExamBattleAction;
    //public Action<scEnterGame> enterGameResultAction;
    public override void Init()
    {

        AddListener((int)ProtoIDs.ApplyHeroExamBattle, OnApplyHeroExamBattle);
        //AddListener((int)ProtoIDs.EnterGame, OnEnterGame);

    }


    //申请英雄试炼战斗
    public void SendApplyHeroExamBattle(Action<scApplyHeroExamBattle> action)
    {
        csApplyHeroExamBattle appy = new csApplyHeroExamBattle();
        applyHeroExamBattleAction = action;
        NetworkManager.Instance.SendMsg(ProtoIDs.ApplyHeroExamBattle, appy.ToByteArray());
        //TransitionBattleMsg(ProtoIDs.ApplyHeroExamBattle, start);
    }

    public void OnApplyHeroExamBattle(MsgPack msgPack)
    {
        scApplyHeroExamBattle scApply = scApplyHeroExamBattle.Parser.ParseFrom(msgPack.data);

        applyHeroExamBattleAction?.Invoke(scApply);
        applyHeroExamBattleAction = null;

    }


}

