namespace Battle.BattleTrigger.Runtime
{
    public class EntityTypeByConfigIdVar : EntityTypeVar
    {
        public int configId;

        public override int Get(ActionContext context)
        {
            return configId;
        }

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            configId = int.Parse(nodeJsonData["configId"].ToString());
        }

    }
}