namespace Battle.BattleTrigger.Runtime
{
    public class CalculateExpressionVar : NumberVar
    {
        NumberVarField aField;
        public CalculateVarType op;
        NumberVarField bField;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            aField = NumberVarField.ParseNumberVarField(nodeJsonData["aField"]);
            op = (CalculateVarType)int.Parse(nodeJsonData["op"].ToString());
            bField = NumberVarField.ParseNumberVarField(nodeJsonData["bField"]);
        }

        public override float Get(ActionContext context)
        {
            float result = 0.0f;
            if (op == CalculateVarType.Plus)
            {
                result = aField.Get(context) + bField.Get(context);
            }
            else if (op == CalculateVarType.Minus)
            {
                result = aField.Get(context) - bField.Get(context);
            }
            else if (op == CalculateVarType.Multi)
            {
                result = aField.Get(context) - bField.Get(context);
            }
            else if (op == CalculateVarType.Divide)
            {
                result = aField.Get(context) - bField.Get(context);
            }

            return result;
        }

    }
}