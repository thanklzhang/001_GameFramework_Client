namespace Battle.BattleTrigger.Runtime
{
    public enum ConditionCompareType
    {
        Equal = 0,
        NotEqual = 1,
        Less = 2,
        LessEqual = 3,
        Greater = 4,
        GreaterEqual = 5
    }

    public enum EqualCompareType
    {
        Equal = 0,
        NotEqual = 1
    }

    public class ConditionNode : ParentNode
    {
        //public List<TriggerNode> aNextActionNodeList;
        //public List<TriggerNode> bNextActionNodeList;

        public ConditionCheck conditionCheck;
        public ExecuteGroup aExecuteGroup;
        public ExecuteGroup bExecuteGroup;

        public bool Check(ActionContext context)
        {
            return conditionCheck.Check(context);
        }

        public override void Parse(ITriggerDataNode nodeJsonData)
        {
            conditionCheck = ConditionCheck.ParseConditionCheck(nodeJsonData["check"]);

            var triggerReader = this.trigger.GetTriggerReader();
            //aExecuteGroup = Trigger.ParseTriggerActionExecuteGroup(nodeJsonData["aExecuteGroup"], trigger, this);
            //bExecuteGroup = Trigger.ParseTriggerActionExecuteGroup(nodeJsonData["bExecuteGroup"], trigger, this);
            aExecuteGroup = triggerReader.ParseTriggerActionExecuteGroup(nodeJsonData["aExecuteGroup"], trigger, this);
            bExecuteGroup = triggerReader.ParseTriggerActionExecuteGroup(nodeJsonData["bExecuteGroup"], trigger, this);

        }

        public override void OnExecute(ActionContext context)
        {
            var isReach = Check(context);
            if (isReach)
            {
                //_Battle_Log.Log("aExecuteGroup.StartExecute ：" + this.GetType());
                aExecuteGroup.StartExecute(context);
            }
            else
            {
                //_Battle_Log.Log("IntCheckNode : bExecuteGroup.StartExecute : " + this.GetType());
                bExecuteGroup.StartExecute(context);
            }
        }

        public override void OnUpdate(float deltaTime, ActionContext context)
        {
            var isReach = Check(context);
            if (isReach)
            {
                aExecuteGroup.Update(deltaTime, context);
            }
            else
            {
                bExecuteGroup.Update(deltaTime, context);
            }
        }
    }

}
