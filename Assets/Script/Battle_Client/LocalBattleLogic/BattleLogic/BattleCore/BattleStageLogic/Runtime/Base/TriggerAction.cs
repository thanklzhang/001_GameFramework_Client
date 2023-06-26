namespace Battle.BattleTrigger.Runtime
{
    public class TriggerAction
    {
        Trigger trigger;
        //public TriggerNode triggerRoot;
        ExecuteGroup executeGroup;
        //public List<TriggerNode> actionNodeList;

        //public int currExecuteIndex = -1;


        //internal void Execute(ActionContext context)
        //{
        //    //triggerRoot.Execute(context);
        //    currExecuteIndex = 0;
        //    var node = actionNodeList[currExecuteIndex];
        //    node.Execute(context);
        //}

        //public void OnNodeFinish(int index, ActionContext context)
        //{
        //    if (currExecuteIndex == index)
        //    {
        //        currExecuteIndex += 1;
        //        var node = actionNodeList[currExecuteIndex];
        //        node.Execute(context);
        //    }
        //}

        public void Init(ExecuteGroup executeGroup, Trigger trigger)
        {
            this.trigger = trigger;
            this.executeGroup = executeGroup;
        }

        public void StartExecute(ActionContext context)
        {
            this.executeGroup.StartExecute(context);
        }

        internal void Update(float deltaTime, ActionContext context)
        {
            this.executeGroup.Update(deltaTime, context);
        }
    }
}
