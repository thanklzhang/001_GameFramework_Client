namespace Battle.BattleTrigger.Runtime
{
    public class Vector3RectRandVar : Vector3Var
    {
        public Vector3VarField randMin;
        public Vector3VarField randMax;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            randMin = Vector3VarField.ParseVector3VarField(nodeJsonData["randMin"]);
            randMax = Vector3VarField.ParseVector3VarField(nodeJsonData["randMax"]);
        }

        public override Vector3 Get(ActionContext context)
        {
            var battle = context.battle;
            var min = randMin.Get(context);
            var max = randMin.Get(context);
            Vector3 randV3 = battle.GetRectRandVector3(min, max);

            return randV3;
        }

    }
}