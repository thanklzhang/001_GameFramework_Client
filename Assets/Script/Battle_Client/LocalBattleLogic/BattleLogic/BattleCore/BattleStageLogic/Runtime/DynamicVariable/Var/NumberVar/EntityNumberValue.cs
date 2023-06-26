namespace Battle.BattleTrigger.Runtime
{
    public enum TriggerEntityType
    {
        Attacking_Entity = 1,
        Be_Attacking_Entity = 2,
        Releasing_Skill_Entity = 10,
        Dead_Entity = 25

    }

    public enum EntityAttrNumberType
    {
        Attack = 1,
        Defence = 2,
        CurrHealth = 3,
        MaxHealth = 4,
        AttackSpeed = 5,
        MoveSpeed = 6,
        AttackRange = 7,

        Level = 1000,
        Star = 1001
    }

    public class EntityAttrVar : NumberVar
    {
        public TriggerEntityType entityType;
        public EntityAttrNumberType attrType;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            entityType = (TriggerEntityType)int.Parse(nodeJsonData["entityType"].ToString());
            attrType = (EntityAttrNumberType)int.Parse(nodeJsonData["attrType"].ToString());
        }

        public override float Get(ActionContext context)
        {
            float resultValue = 0;

            var triggerEntity = context.GetEntityByTriggerType(entityType);
            if (triggerEntity != null)
            {
                resultValue = triggerEntity.GetEntityAtrrFinalValue(attrType);
            }

            return resultValue;
        }
    }
}