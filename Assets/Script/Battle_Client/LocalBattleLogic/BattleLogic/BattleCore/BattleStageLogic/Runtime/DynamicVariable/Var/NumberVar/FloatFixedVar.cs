namespace Battle.BattleTrigger.Runtime
{
    public class FloatFixedVar : NumberVar
    {
        public float value;

        public override float Get(ActionContext context)
        {
            return value;
        }

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            value = float.Parse(nodeJsonData["value"].ToString());
        }

    }
}