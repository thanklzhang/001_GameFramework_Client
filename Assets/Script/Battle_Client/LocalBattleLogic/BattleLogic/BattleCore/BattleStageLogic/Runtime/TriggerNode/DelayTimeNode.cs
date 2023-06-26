namespace Battle.BattleTrigger.Runtime
{
    public class DelayTimeNode : ExecuteNode
    {
        public NumberVarField delayTime;

        private float currTime;
        public override void Parse(ITriggerDataNode nodeJsonData)
        {
            //delayTime = new FloatValue() { value = float.Parse(nodeJsonData["delayTime"]["value"].ToString()) };
            delayTime = NumberVarField.ParseNumberVarField(nodeJsonData["delayTime"]);
        }

        public override void OnExecute(ActionContext context)
        {
            currTime = 0;
        }

        public override void OnUpdate(float deltaTime, ActionContext context)
        {
            currTime += deltaTime;
            if (currTime >= delayTime.Get(context))
            {
                ReachTime(context);
            }
        }

        void ReachTime(ActionContext context)
        {
            this.Finish(context);
        }
    }


}
