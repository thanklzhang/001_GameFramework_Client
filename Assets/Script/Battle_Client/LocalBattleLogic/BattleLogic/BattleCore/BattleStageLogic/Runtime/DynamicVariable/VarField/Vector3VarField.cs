using System;

namespace Battle.BattleTrigger.Runtime
{
    ////后台应该不需要这个
    //public enum Vector3_GetValueFromType
    //{
    //    Vector3Fixed = 0,
    //    Vector3CalculateExpression = 2,
    //    EntityPoint = 10,
    //}

    public class Vector3VarField : BaseVarField
    {
        //Vector3_GetValueFromType getType;
        Vector3Var vector3Var;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            //getType = (Vector3_GetValueFromType)(int.Parse(nodeJsonData["getType"].ToString()));
            vector3Var = Vector3Var.ParseNumberValue(nodeJsonData["vector3Var"]);
        }

        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        
        public static Vector3VarField ParseVector3VarField(ITriggerDataNode nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            Vector3VarField vector3VarField = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //_G.Log("ParseVector3VarField fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(Vector3VarField)) || type == typeof(Vector3VarField))
                {
                    vector3VarField = Activator.CreateInstance(type) as Vector3VarField;
                    vector3VarField.Parse(nodeJsonData);
                }
            }
            else
            {
                _Battle_Log.LogError("the type of vector3VarField is not found : " + resultClassName);
            }

            return vector3VarField;
        }

        public virtual Vector3 Get(ActionContext context)
        {
            return this.vector3Var.Get(context);
        }
    }
}