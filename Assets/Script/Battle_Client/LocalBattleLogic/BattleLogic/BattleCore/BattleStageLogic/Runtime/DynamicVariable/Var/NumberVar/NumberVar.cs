using System;

namespace Battle.BattleTrigger.Runtime
{
    public enum CalculateVarType
    {
        Plus = 0,
        Minus = 1,
        Multi = 2,
        Divide = 3
    }


    public class NumberVar : BaseVar
    {
       
        public void Parse(ITriggerDataNode nodeJsonData)
        {
            this.OnParse(nodeJsonData);
        }

        public virtual void OnParse(ITriggerDataNode nodeJsonData)
        {

        }

        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        //public static NumberVar ParseNumberValue(JsonData nodeJsonData)
        //{
        //    if (null == nodeJsonData)
        //    {
        //        return null;
        //    }

        //    NumberVar numberVar = null;

        //    if (!nodeJsonData.ContainsKey("__TYPE__"))
        //    {
        //        return null;
        //    }
        //    var nodeJsonType = nodeJsonData["__TYPE__"];
        //    var typeStr = nodeJsonType.ToString();
        //    var strs = typeStr.Split('.');
        //    var str = strs[strs.Length - 1];

        //    var fullName = NameSpaceName + "." + str;
        //    //_G.Log("ParseNumberValue fullName : " + fullName);
        //    var resultClassName = fullName;
        //    var type = Type.GetType(resultClassName);
        //    if (type != null)
        //    {
        //        if (type.IsSubclassOf(typeof(NumberVar)))
        //        {
        //            numberVar = Activator.CreateInstance(type) as NumberVar;
        //            numberVar.Parse(nodeJsonData);
        //        }
        //    }
        //    else
        //    {
        //        _G.LogError("the type of numberVar is not found : " + resultClassName);
        //    }

        //    return numberVar;
        //}

        public static NumberVar ParseNumberValue(ITriggerDataNode nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            NumberVar numberVar = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData.GetString("__TYPE__");
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //_G.Log("ParseNumberValue fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(NumberVar)))
                {
                    numberVar = Activator.CreateInstance(type) as NumberVar;
                    numberVar.Parse(nodeJsonData);
                }
            }
            else
            {
                _Battle_Log.LogError("the type of numberVar is not found : " + resultClassName);
            }

            return numberVar;
        }

        public virtual float Get(ActionContext context)
        {
            return 0;
        }
    }
}