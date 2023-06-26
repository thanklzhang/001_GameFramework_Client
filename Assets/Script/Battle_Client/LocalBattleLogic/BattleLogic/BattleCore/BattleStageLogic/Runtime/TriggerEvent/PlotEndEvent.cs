namespace Battle.BattleTrigger.Runtime
{
    //剧情结束的时候
    public class PlotEndEvent : TriggerEvent
    {
        public override void Update(float timeDelta)
        {

        }

        public override bool IsExecuteAction(TriggerArg arg)
        {
            return true;
        }

        public override void RegisterEvent()
        {
            this.battle.OnPlotEndAction += OnTriggerEvent;
        }

        public override void RemoveEvent()
        {
            this.battle.OnPlotEndAction -= OnTriggerEvent;
        }
    }
}
