namespace Battle.BattleTrigger.Runtime
{
    public class IntFixedVar : NumberVar
    {
        public int value;

        public override float Get(ActionContext context)
        {
            return value;
        }

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            value = int.Parse(nodeJsonData["value"].ToString());
        }

    }
}