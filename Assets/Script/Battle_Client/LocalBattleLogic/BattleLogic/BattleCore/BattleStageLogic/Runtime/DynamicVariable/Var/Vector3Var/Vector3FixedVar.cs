namespace Battle.BattleTrigger.Runtime
{
    public class Vector3FixedVar : Vector3Var
    {
        public Vector3 value;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            float x = float.Parse(nodeJsonData["x"].ToString());
            float y = float.Parse(nodeJsonData["y"].ToString());
            float z = float.Parse(nodeJsonData["z"].ToString());
            value = new Vector3(x, y, z);
        }

        public override Vector3 Get(ActionContext context)
        {
            return value;
        }

    }
}