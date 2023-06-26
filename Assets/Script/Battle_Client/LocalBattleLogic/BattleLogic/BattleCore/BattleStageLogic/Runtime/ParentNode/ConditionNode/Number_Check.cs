namespace Battle.BattleTrigger.Runtime
{

    public class Number_Check : ConditionCheck
    {
        NumberVarField aField;
        ConditionCompareType op;
        NumberVarField bField;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            aField = NumberVarField.ParseNumberVarField(nodeJsonData["aField"]);
            op = (ConditionCompareType)(int.Parse(nodeJsonData["op"].ToString()));
            bField = NumberVarField.ParseNumberVarField(nodeJsonData["bField"]);
        }

        public override bool Check(ActionContext context)
        {
            bool result = false;
            switch (op)
            {
                case ConditionCompareType.Equal:
                    result = aField.Get(context) == bField.Get(context);
                    break;
                case ConditionCompareType.NotEqual:
                    result = aField.Get(context) != bField.Get(context);
                    break;
                case ConditionCompareType.Less:
                    result = aField.Get(context) < bField.Get(context);
                    break;
                case ConditionCompareType.LessEqual:
                    result = aField.Get(context) <= bField.Get(context);
                    break;
                case ConditionCompareType.Greater:
                    result = aField.Get(context) > bField.Get(context);
                    break;
                case ConditionCompareType.GreaterEqual:
                    result = aField.Get(context) >= bField.Get(context);
                    break;
            }

            return result;
        }
    }
}