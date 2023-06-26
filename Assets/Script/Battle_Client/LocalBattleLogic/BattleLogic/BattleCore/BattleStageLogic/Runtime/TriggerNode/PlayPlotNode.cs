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
//    public class PlayPlotNode : ExecuteNode
//    {
//        public StringValue plotName;

//        public override void Parse(JsonData nodeJsonData)
//        {
//            var name = nodeJsonData["plotName"]["value"].ToString();
//            plotName = new StringValue() { value = name };
//            //nextNode = Trigger.ParseTriggerActionNode(nodeJsonData["nextNode"]);
//        }

//        public override void OnExecute(ActionContext context)
//        {
//            string str = string.Format("play plot : {0}", plotName.Get());

//            _G.Log("execute : PlayPlotNode " + str);

//            var battle = context.battle;

//            var name = plotName.Get();
//            battle.PlayPlot(name);

//        }
//    }

   
   
//}
