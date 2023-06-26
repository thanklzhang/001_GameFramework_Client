using System;

namespace Battle.BattleTrigger.Runtime
{
    public enum Vector3CalculateVarType
    {
        Plus = 0,
        Minus = 1,
        //Multi = 2,
        //Divide = 3
    }

    public class Vector3Var : BaseVar
    {
        public virtual Vector3 Get(ActionContext context)
        {
            return Vector3.zero;
        }

        public void Parse(ITriggerDataNode nodeJsonData)
        {
            this.OnParse(nodeJsonData);
        }

        public virtual void OnParse(ITriggerDataNode nodeJsonData)
        {

        }


        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        public static Vector3Var ParseNumberValue(ITriggerDataNode nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            Vector3Var vector3Var = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //_G.Log("ParseVector3rValue fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(Vector3Var)))
                {
                    vector3Var = Activator.CreateInstance(type) as Vector3Var;
                    vector3Var.Parse(nodeJsonData);
                }
            }
            else
            {
                _Battle_Log.LogError("the type of vector3Var is not found : " + resultClassName);
            }

            return vector3Var;
        }

    }
}