namespace Battle.BattleTrigger.Runtime
{
    public class Vector3CalculateExpressionVar : Vector3Var
    {
        Vector3VarField aField;
        public Vector3CalculateVarType op;
        Vector3VarField bField;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            aField = Vector3VarField.ParseVector3VarField(nodeJsonData["aField"]);
            op = (Vector3CalculateVarType)int.Parse(nodeJsonData["op"].ToString());
            bField = Vector3VarField.ParseVector3VarField(nodeJsonData["bField"]);
        }

        public override Vector3 Get(ActionContext context)
        {
            Vector3 result = Vector3.zero;
            if (op == Vector3CalculateVarType.Plus)
            {
                result = aField.Get(context) + bField.Get(context);
            }
            else if (op == Vector3CalculateVarType.Minus)
            {
                result = aField.Get(context) - bField.Get(context);
            }

            return result;
        }

    }
}