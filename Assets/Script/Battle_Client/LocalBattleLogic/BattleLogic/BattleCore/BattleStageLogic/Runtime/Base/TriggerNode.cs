namespace Battle.BattleTrigger.Runtime
{
    public enum TriggerNodeState
    {
        Null = 0,
        Execute = 1,
        Finish = 2
    }

    public class TriggerNode
    {
        //List<TriggerNode> childList;
        protected Trigger trigger;
        protected ExecuteGroup executeGroup;

        public TriggerNodeState state = TriggerNodeState.Null;

        //父子层级 0 - n
        //public int layer;

        //internal void SetExecuteGroup(ExecuteGroup executeGroup)
        //{
        //    this.executeGroup = executeGroup;
        //}

        public int executeIndex;

        public virtual void Parse(ITriggerDataNode nodeJsonData)
        {

        }

        public void Init(ExecuteGroup executeGroup)
        {
            this.executeGroup = executeGroup;
            this.OnInit(executeGroup);
        }

        public virtual void OnInit(ExecuteGroup executeGroup)
        {
            
        }

        //Trigger trigger;

        public void Reset()
        {
            this.OnReset();
        }

        public virtual void OnReset()
        {
            
        }

        //主动执行
        public void Execute(ActionContext context, int index)
        {
            state = TriggerNodeState.Execute;
            this.executeIndex = index;
            this.Reset();
            this.OnExecute(context);
        }

        public virtual void OnExecute(ActionContext context)
        {

        }

        //主动结束
        public void Finish(ActionContext context)
        {
            if (null == executeGroup)
            {
                _Battle_Log.LogError("the executeGroup is null");
                return;
            }

            state = TriggerNodeState.Finish;
            this.Reset();
            executeGroup.OnNodeFinish(executeIndex, context);
        }

        public void Update(float deltaTime, ActionContext context)
        {
            this.OnUpdate(deltaTime, context);
        }

        public virtual void OnUpdate(float deltaTime, ActionContext context)
        {

        }

        internal void SetTrigger(Trigger trigger)
        {
            this.trigger = trigger;
        }


        //protected void ParseTriggerActionNode(JsonData nodeJsonData)
        //{
        //    Trigger.ParseTriggerActionNode(nodeJsonData);
        //}
    }
}
