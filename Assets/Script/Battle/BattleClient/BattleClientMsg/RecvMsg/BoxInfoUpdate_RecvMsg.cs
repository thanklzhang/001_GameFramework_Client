using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client
{
    public class BoxInfoUpdate_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as BoxInfoUpdate_RecvMsg_Arg;

            //目前开箱子只是自己开
            var hero = BattleManager.Instance.GetLocalCtrlHero();
            if (hero != null && arg.entityGuid == hero.guid)
            {
                hero.SetBoxList(arg.boxDic);
            }

        }
    }

    public class BoxInfoUpdate_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public Dictionary<RewardQuality,List<BattleClientMsg_BattleBox>> boxDic;
    }
}