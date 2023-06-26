namespace Battle.BattleTrigger.Runtime
{


    public class LoopNode : ParentNode
    {
        public NumberVarField loopCount;
        public ExecuteGroup loopExecuteGroup;

        private int currCount;

        int targetLoopCount;
        public override void Parse(ITriggerDataNode nodeJsonData)
        {
            loopCount = NumberVarField.ParseNumberVarField(nodeJsonData["loopCount"]);
            var triggerReader = this.trigger.GetTriggerReader();
            loopExecuteGroup = triggerReader.ParseTriggerActionExecuteGroup(nodeJsonData["loopExecuteGroup"], trigger, this);
        }

        public override void OnReset()
        {
            this.currCount = 0;
        }

        public override void OnExecute(ActionContext context)
        {
            targetLoopCount = (int)loopCount.Get(context);
            this.StartExcuteLoopGroup(context);
        }

        public override void OnChildExecuteFinish(ActionContext context)
        {
            //_Battle_Log.Log("OnChildExecuteFinish : currCount : " + currCount);
            currCount += 1;
            if (currCount >= targetLoopCount)
            {
                this.Finish(context);
            }
            else
            {
                this.StartExcuteLoopGroup(context);
            }
        }

        void StartExcuteLoopGroup(ActionContext context)
        {
            loopExecuteGroup.StartExecute(context);
        }

        public override void OnUpdate(float deltaTime, ActionContext context)
        {
            loopExecuteGroup.Update(deltaTime, context);
        }


    }
}
