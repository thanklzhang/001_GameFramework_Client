namespace Battle.BattleTrigger.Runtime
{
    public enum TriggerState
    {
        Null = 0,
        Execute = 1,
        Finish = 2
    }

    public class Trigger
    {
        protected Battle battle;

        bool isClose = false;
        //触发的起因事件
        public TriggerEvent triggerEvent;
        //触发的动作执行
        public TriggerAction triggerAction;

        public TriggerState state = TriggerState.Null;

        public void Init(Battle battle)
        {
            this.battle = battle;
            triggerEvent.Init(this.battle, this);
        }

        public void RegisterEvent()
        {
            triggerEvent.RegisterEvent();
        }

        internal void ExecuteAction(ActionContext context)
        {
            //_Battle_Log.Log("trigger : ExecuteAction");
            state = TriggerState.Execute;
            this.triggerAction.StartExecute(context);

        }

        public void RemoveEvent()
        {
            this.triggerEvent.RemoveEvent();
        }

        public ActionContext GetCurrActionContext()
        {
            ActionContext context = new ActionContext();
            context.battle = battle;
            return context;
        }

        public ITriggerReader GetTriggerReader()
        {
            return this.battle.TriggerReader;
        }

        internal void SetBattle(Battle battle)
        {
            this.battle = battle;
        }

        //public void Close()
        //{

        //    this.RemoveEvent();
        //    state = TriggerState.Finish;
        //    //待manager 移除
        //}
        //---------------


        //Serialize and Deserialize

        //public static Trigger ParseFromJson(string json)
        //{
        //    Trigger trigger = new Trigger();

        //    var triggerNode = LitJson.JsonMapper.ToObject(json);
        //    var eventJsonData = triggerNode["triggerEvent"];
        //    var actionJsonData = triggerNode["executeGroup"];

        //    //事件
        //    var triggerEvent = ParseTriggerEvent(eventJsonData);

        //    trigger.triggerEvent = triggerEvent;

        //    //动作
        //    TriggerAction action = new TriggerAction();
        //    var executeGroup = ParseTriggerActionExecuteGroup(actionJsonData, trigger);
        //    executeGroup.SetTrigger(trigger);
        //    action.Init(executeGroup, trigger);

        //    trigger.triggerAction = action;

        //    return trigger;
        //}


        //public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        //public static TriggerEvent ParseTriggerEvent(LitJson.JsonData eventJsonData)
        //{
        //    TriggerEvent trigger = null;
        //    var eventJsonType = eventJsonData["__TYPE__"];
        //    var typeStr = eventJsonType.ToString();
        //    var strs = typeStr.Split('.');
        //    var str = strs[strs.Length - 1];

        //    var type = Type.GetType(NameSpaceName + "." + str);
        //    if (type != null)
        //    {
        //        if (type.IsSubclassOf(typeof(TriggerEvent)))
        //        {
        //            trigger = Activator.CreateInstance(type) as TriggerEvent;
        //            trigger.Parse(eventJsonData);
        //        }
        //        else
        //        {
        //            _G.LogError("the type is not subClass of TriggerEvent : " + type.ToString());
        //        }
        //    }
        //    else
        //    {
        //        _G.LogError("the type of event is not found : " + str);
        //    }

        //    return trigger;
        //}




        ////public static TriggerNode ParseTriggerActionNode(LitJson.JsonData nodeJsonData)
        ////{
        ////    if (null == nodeJsonData)
        ////    {
        ////        return null;
        ////    }
        ////    TriggerNode node = null;

        ////    if (!nodeJsonData.ContainsKey("__TYPE__"))
        ////    {
        ////        return null;
        ////    }
        ////    var nodeJsonType = nodeJsonData["__TYPE__"];
        ////    var typeStr = nodeJsonType.ToString();
        ////    var strs = typeStr.Split(".");
        ////    var str = strs[strs.Length - 1];

        ////    var fullName = NameSpaceName + "." + str;
        ////    _G.Log("ParseTriggerActionNode fullName : " + fullName);
        ////    var type = Type.GetType(fullName);
        ////    if (type != null)
        ////    {
        ////        if (type.IsSubclassOf(typeof(ConditionNode)))
        ////        {
        ////            node = Activator.CreateInstance(type) as ConditionNode;
        ////            node.Parse(nodeJsonData);
        ////        }
        ////        else if (type.IsSubclassOf(typeof(ExecuteNode)))
        ////        {
        ////            node = Activator.CreateInstance(type) as ExecuteNode;
        ////            node.Parse(nodeJsonData);
        ////        }
        ////        else
        ////        {
        ////            _G.LogError("the type is not subClass of ConditionNode or ExecuteNode : " + type.ToString());
        ////        }
        ////    }
        ////    else
        ////    {
        ////        _G.LogError("the type of node is not found : " + str);
        ////    }


        ////    return node;
        ////}

        //public static ExecuteGroup ParseTriggerActionExecuteGroup(LitJson.JsonData nodeJsonData, Trigger trigger,TriggerNode parent = null)
        //{
        //    ExecuteGroup group = new ExecuteGroup();

        //    var nodeList = ParseTriggerActionNodeList(nodeJsonData, group, trigger);
        //    group.SetActionNodeList(nodeList);
        //    group.SetTrigger(trigger);
        //    group.SetParent(parent);
        //    return group;
        //}

        ///// <summary>
        ///// 解析 action node list 
        ///// </summary>
        ///// <param name="nodeJsonData"></param>
        ///// <returns></returns>
        //public static List<TriggerNode> ParseTriggerActionNodeList(LitJson.JsonData nodeJsonData, ExecuteGroup executeGroup, Trigger trigger)
        //{
        //    List<TriggerNode> actionNodeList = new List<TriggerNode>();
        //    var isArray = nodeJsonData.IsArray;
        //    if (!isArray)
        //    {
        //        return null;
        //    }

        //    for (int i = 0; i < nodeJsonData.Count; i++)
        //    {
        //        var nodeJson = nodeJsonData[i];
        //        var triggerNode = ParseTriggerActionNode(nodeJson, trigger);
        //        if (null == triggerNode)
        //        {
        //            continue;
        //        }
        //        triggerNode.Init(executeGroup);
        //        actionNodeList.Add(triggerNode);
        //    }
        //    return actionNodeList;
        //}

        ///// <summary>
        ///// 解析 单个 action node
        ///// </summary>
        ///// <param name="nodeJsonData"></param>
        ///// <returns></returns>
        //public static TriggerNode ParseTriggerActionNode(LitJson.JsonData nodeJsonData,Trigger trigger)
        //{
        //    if (null == nodeJsonData)
        //    {
        //        return null;
        //    }
        //    TriggerNode node = null;

        //    if (!nodeJsonData.ContainsKey("__TYPE__"))
        //    {
        //        return null;
        //    }
        //    var nodeJsonType = nodeJsonData["__TYPE__"];
        //    var typeStr = nodeJsonType.ToString();
        //    var strs = typeStr.Split('.');
        //    var str = strs[strs.Length - 1];

        //    var fullName = NameSpaceName + "." + str;
        //    _G.Log("ParseTriggerActionNode fullName : " + fullName);
        //    var type = Type.GetType(fullName);
        //    if (type != null)
        //    {
        //        if (type.IsSubclassOf(typeof(ParentNode)))
        //        {
        //            node = Activator.CreateInstance(type) as ParentNode;
        //            node.SetTrigger(trigger);
        //            node.Parse(nodeJsonData);


        //        }
        //        else if (type.IsSubclassOf(typeof(ExecuteNode)))
        //        {
        //            node = Activator.CreateInstance(type) as ExecuteNode;
        //            node.SetTrigger(trigger);
        //            node.Parse(nodeJsonData);

        //        }
        //        else
        //        {
        //            _G.LogError("the type is not subClass of ParentNode or ExecuteNode : " + type.ToString());
        //        }
        //    }
        //    else
        //    {
        //        _G.LogError("the type of node is not found : " + str);
        //    }


        //    return node;
        //}

        public void Update(float deltaTime)
        {
            if (state == TriggerState.Execute)
            {
                //这个 context 待定
                ActionContext context = new ActionContext();
                context.battle = this.battle;
                triggerAction.Update(deltaTime, context);

            }

        }

        internal void Finish()
        {
            this.RemoveEvent();
            this.state = TriggerState.Finish;
        }
    }

}

