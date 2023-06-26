namespace Battle.BattleTrigger.Runtime
{
    public enum EntityEventType
    {
        Attack = 1,
        BeAttack = 2,
        ReleasingSkill = 10,
        Dead = 20
    }


    //某个单位死亡的时候
    public class EntityEventEvent : TriggerEvent
    {
        public EntityEventType entityEventType;

        public override void Parse(ITriggerDataNode eventJsonData)
        {
            entityEventType = (EntityEventType)(int.Parse(eventJsonData["entityEventType"].ToString()));
        }

        public override void Update(float timeDelta)
        {

        }

        public override bool IsExecuteAction(TriggerArg arg)
        {
            var eArg = arg as EventEntityEventArg;

            var currTriggerType = eArg.entityEventType;

            //var currDeadConfigId = deadArg.entity.configId;
            ////if (entityConfigId == currDeadConfigId)
            ////{
            ////    //填充此时触发时候的上下文
            ////    _G.Log("trigger entityDeadByType event , start action");

            ////}

            //return entityConfigId == currDeadConfigId;
        
            var isTrigger = currTriggerType == entityEventType;
            return isTrigger;
        }

        public override void RegisterEvent()
        {
            this.battle.OnEntityEventAction += OnTriggerEvent;
        }

        public override void RemoveEvent()
        {
            this.battle.OnEntityEventAction -= OnTriggerEvent;
        }

        //private void OnEntityByTypeDeadEvent(TriggerArg arg)
        //{
        //    var deadArg = arg as EventEntityByTypeDeadArg;

        //    var currDeadConfigId = deadArg.entity.configId;
        //    if (entityConfigId == currDeadConfigId)
        //    {
        //        //填充此时触发时候的上下文
        //        _G.Log("trigger entityDeadByType event , start action");
        //        ExecuteAction();
        //    }

        //}
    }

}
