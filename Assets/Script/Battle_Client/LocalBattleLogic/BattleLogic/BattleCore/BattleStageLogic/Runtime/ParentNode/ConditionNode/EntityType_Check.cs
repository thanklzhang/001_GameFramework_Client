namespace Battle.BattleTrigger.Runtime
{

    public class EntityType_Check : ConditionCheck
    {
        EntityTypeVarField aField;
        EqualCompareType op;
        EntityTypeVarField bField;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            aField = EntityTypeVarField.ParseVarField(nodeJsonData["aField"]);
            op = (EqualCompareType)(int.Parse(nodeJsonData["op"].ToString()));
            bField = EntityTypeVarField.ParseVarField(nodeJsonData["bField"]);
        }

        public override bool Check(ActionContext context)
        {
            bool result = false;
            switch (op)
            {
                case EqualCompareType.Equal:
                    result = aField.Get(context) == bField.Get(context);
                    break;
                case EqualCompareType.NotEqual:
                    result = aField.Get(context) != bField.Get(context);
                    break;
            }

            return result;
        }
    }
}