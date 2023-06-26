namespace Battle.BattleTrigger.Runtime
{
    public enum EntityPointType
    {
        Position = 1,
        HeadPos = 30,
    }

    public class EntityPointVar : Vector3Var
    {
        public TriggerEntityType entityType;
        public EntityPointType pointType;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            entityType = (TriggerEntityType)int.Parse(nodeJsonData["entityType"].ToString());
            pointType = (EntityPointType)int.Parse(nodeJsonData["pointType"].ToString());
        }

        public override Vector3 Get(ActionContext context)
        {
            Vector3 resultValue = Vector3.zero;

            var triggerEntity = context.GetEntityByTriggerType(entityType);
            if (triggerEntity != null)
            {
                resultValue = triggerEntity.GetPointByType(pointType);
            }

            return resultValue;
        }
    }
}