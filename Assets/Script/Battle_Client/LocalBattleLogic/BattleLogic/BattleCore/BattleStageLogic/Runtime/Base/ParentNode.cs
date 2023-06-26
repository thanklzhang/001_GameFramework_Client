namespace Battle.BattleTrigger.Runtime
{
    public class ParentNode : TriggerNode
    {
        //protected List<TriggerNode> childNodeList;

        public void ChildExecuteFinish(ActionContext context)
        {
            this.OnChildExecuteFinish(context);
        }

        public virtual void OnChildExecuteFinish(ActionContext context)
        {
            this.Finish(context);
        }
    }

}
