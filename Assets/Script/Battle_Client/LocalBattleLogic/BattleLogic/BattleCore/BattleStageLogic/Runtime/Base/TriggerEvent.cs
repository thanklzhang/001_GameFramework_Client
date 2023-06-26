namespace Battle.BattleTrigger.Runtime
{
    //public enum TriggerEventType
    //{
    //    Null = 0,
    //    TimePass = 1,//游戏经过多少秒
    //    TimePassLoop = 2,//每当游戏经过多少秒
    //    EntityByTypeDead = 101,//某个类型的单位死亡

    //}
    public class TriggerEvent
    {
        //public TriggerEventType type;

        protected Battle battle;
        Trigger trigger;

        ActionContext context;
        public virtual void Init(Battle battle, Trigger trigger)
        {
            this.battle = battle;
            this.trigger = trigger;
        }



        public virtual void Update(float timeDelta)
        {

        }

        public virtual bool IsExecuteAction(TriggerArg arg)
        {
            return true;
        }


        public virtual void Parse(ITriggerDataNode eventJsonData)
        {

        }

        public virtual void RegisterEvent()
        {

        }

        public virtual void RemoveEvent()
        {

        }

        public virtual void OnTriggerEvent(TriggerArg arg)
        {
            var isExecuteNext = IsExecuteAction(arg);
            if (isExecuteNext)
            {
                if (null == arg)
                {
                    arg = new TriggerArg();
                    arg.context = new ActionContext()
                    {
                        battle = this.battle
                    };
                }

                context = arg.context;
                ExecuteAction();
            }
        }

        protected ActionContext GetCurrActionContext()
        {
            //return this.trigger.GetCurrActionContext();
            return context;
        }

        protected void ExecuteAction()
        {
            this.OnExecuteAction_Pre();

            //_Battle_Log.Log(string.Format("trigger {0} event , start action", this.GetType().ToString()));
            //var context = this.trigger.GetCurrActionContext();
            this.trigger.ExecuteAction(context);

        }

        public virtual void OnExecuteAction_Pre()
        {

        }

    }
}
