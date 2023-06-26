using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Table;
using LitJson;

namespace Battle.BattleTrigger.Runtime
{
    //游戏结束
    public class BattleEndNode : ExecuteNode
    {
        public NumberVarField winTeam;

        public override void Parse(ITriggerDataNode nodeJsonData)
        {
            winTeam = (NumberVarField.ParseNumberVarField(nodeJsonData["winTeam"]));
            //nextNode = Trigger.ParseTriggerActionNode(nodeJsonData["nextNode"]);
        }

        public override void OnExecute(ActionContext context)
        {
            //var bIsWin = 1 == this.isWin;
            //var str = "";
            //if (bIsWin)
            //{
            //    str = "you win";
            //}
            //else
            //{
            //    str = "you fail";
            //}
            //_Battle_Log.Log("execute : BattleEndNodeNode " + str);
            var battle = context.battle;

            var intWinTeam = (int)winTeam.Get(context);
            battle.BattleEnd(intWinTeam);

        }
    }
    //
}
