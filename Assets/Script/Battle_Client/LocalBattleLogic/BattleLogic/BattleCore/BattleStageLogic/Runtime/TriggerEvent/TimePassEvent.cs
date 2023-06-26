namespace Battle.BattleTrigger.Runtime
{

    //游戏逝去一段时间的时候
    public class TimePassEvent : TriggerEvent
    {
        public NumberVarField targetTime;
        bool isActive = true;

        //float currTime;

        public override void Parse(ITriggerDataNode nodeJsonData)
        {
            targetTime = NumberVarField.ParseNumberVarField(nodeJsonData["targetTime"]);
        }

        public override void Update(float timeDelta)
        {
            //currTime += timeDelta;
        }

        public override bool IsExecuteAction(TriggerArg arg)
        {
            var timePassArg = arg as EventTimePassArg;
            var action = this.GetCurrActionContext();
            return isActive && timePassArg.currTime >= this.targetTime.Get(action);
        }

        public override void RegisterEvent()
        {
            this.battle.OnTimePassAction += OnTriggerEvent;
        }

        public override void RemoveEvent()
        {
            this.battle.OnTimePassAction -= OnTriggerEvent;
        }

        //执行一次就不能在执行了
        public override void OnExecuteAction_Pre()
        {
            isActive = false;
        }

        //private void OnTimePassEvent(TriggerArg arg)
        //{
        //    var timePassArg = arg as EventTimePassArg;
        //    if (timePassArg.currTime >= this.targetTime)
        //    {
        //        //填充此时触发时候的上下文
        //        Console.WriteLine("trigger timePass event , start action");

        //        ExecuteAction();
        //    }
        //}
    }
}
