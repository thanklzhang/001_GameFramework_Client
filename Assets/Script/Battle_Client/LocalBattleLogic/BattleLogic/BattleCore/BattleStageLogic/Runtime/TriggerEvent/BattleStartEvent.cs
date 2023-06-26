namespace Battle.BattleTrigger.Runtime
{

    //战斗开始的时候
    public class BattleStartEvent : TriggerEvent
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
            this.battle.OnBattleStartAction += OnTriggerEvent;
        }

        public override void RemoveEvent()
        {
            this.battle.OnBattleStartAction -= OnTriggerEvent;
        }

        //private void OnBattleStartEvent(TriggerArg arg)
        //{
        //    //填充此时触发时候的上下文
        //    _G.Log("trigger BattleStartEvent event , start action");
        //    ExecuteAction();

        //}
    }
}
