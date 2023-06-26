namespace Battle.BattleTrigger.Runtime
{
    public class ActionContext
    {
        //战斗(考虑是不是要换成代理)
        public Battle battle;
        //攻击者
        public BattleEntity Attacker;
        //被攻击者
        public BattleEntity BeAttacker;
        //死亡者
        public BattleEntity DeadEntity;

        public BattleEntity GetEntityByTriggerType(TriggerEntityType triggerType)
        {
            BattleEntity entity = null;
            switch (triggerType)
            {
                case TriggerEntityType.Attacking_Entity:
                    entity = this.Attacker;
                    break;
                case TriggerEntityType.Be_Attacking_Entity:
                    entity = this.BeAttacker;
                    break;
                case TriggerEntityType.Dead_Entity:
                    entity = this.DeadEntity;
                    break;
                    //case TriggerEntityType.Releasing_Skill_Entity:
                    //    entity = this.Attacker;
                    //    break;
                    //case TriggerEntityType.Dead_Entity:
                    //    entity = this.Attacker;
                    //    break;
            }

            return entity;
        }
    }
}

