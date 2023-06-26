//using Google.Protobuf;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using NetProto;
//using Table;
//using LitJson;

//namespace Battle.BattleTrigger.Runtime
//{
//    public class SetAllBattleEntityShowStateNode : ExecuteNode
//    {
//        public BoolValue isShow;
//        public override void Parse(JsonData nodeJsonData)
//        {
//            isShow = new BoolValue() { value = bool.Parse(nodeJsonData["isShow"]["value"].ToString()) };
//            //nextNode = Trigger.ParseTriggerActionNode(nodeJsonData["nextNode"]);
//        }

//        public override void OnExecute(ActionContext context)
//        {
//            string str = string.Format("SetAllBattleEntityShowStateNode isShow:{0} ", this.isShow);
//            _G.Log("execute : SetAllBattleEntityShowStateNode ");
//            var battle = context.battle;

//            var isShow = this.isShow.Get();
//            var allEntityGuids = battle.GetAllEntities().Select((e) => { return e.Key; }).ToList();
//            battle.SetEntitiesShowState(allEntityGuids, isShow);

//        }
//    }


//}
