namespace Battle.BattleTrigger.Runtime
{
    //剧情结束的时候
    public class PlotStartEvent : TriggerEvent
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
            this.battle.OnPlotStartAction += OnTriggerEvent;
        }

        public override void RemoveEvent()
        {
            this.battle.OnPlotStartAction -= OnTriggerEvent;
        }
    }
}
