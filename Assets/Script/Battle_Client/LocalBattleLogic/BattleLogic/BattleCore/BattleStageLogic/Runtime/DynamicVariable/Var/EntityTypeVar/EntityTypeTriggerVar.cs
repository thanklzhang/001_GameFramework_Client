namespace Battle.BattleTrigger.Runtime
{
    public class EntityTypeTriggerVar : EntityTypeVar
    {
        public TriggerEntityType entityType;

        public override int Get(ActionContext context)
        {
            var entity = context.GetEntityByTriggerType(entityType);
            if (null == entity)
            {
                return 0;
            }
            return entity.configId;
        }

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            entityType = (TriggerEntityType)(int.Parse(nodeJsonData["entityType"].ToString()));
        }

    }
}