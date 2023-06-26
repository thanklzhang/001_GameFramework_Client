namespace Battle.BattleTrigger.Runtime
{
    public class Vector3CircleRandVar : Vector3Var
    {
        public Vector3VarField center;
        public NumberVarField radius;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            center = Vector3VarField.ParseVector3VarField(nodeJsonData["center"]);
            radius = NumberVarField.ParseNumberVarField(nodeJsonData["radius"]);
        }

        public override Vector3 Get(ActionContext context)
        {
            var battle = context.battle;

            //y 不考虑
            var x = battle.GetRandFloat(-radius.Get(context), radius.Get(context));
            var y = 0;//battle.GetRandFloat(-radius.Get(context), radius.Get(context));
            var z = battle.GetRandFloat(-radius.Get(context), radius.Get(context));

            Vector3 randV3 = center.Get(context) + new Vector3(x, y, z);

            return randV3;
        }

    }
}