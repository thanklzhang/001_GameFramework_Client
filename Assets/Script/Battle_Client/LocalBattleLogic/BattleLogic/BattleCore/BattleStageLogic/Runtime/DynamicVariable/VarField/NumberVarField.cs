using System;

namespace Battle.BattleTrigger.Runtime
{
    //public enum Number_GetValueFromType
    //{
    //    FloatFixed = 0,
    //    IntFixed = 1,
    //    CalculateExpression = 2,
    //    EntityAttr = 10,
    //}

    public class NumberVarField : BaseVarField
    {
        //Number_GetValueFromType getType;
        NumberVar numberVar;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            //getType = (Number_GetValueFromType)(int.Parse(nodeJsonData["getType"].ToString()));
            numberVar = NumberVar.ParseNumberValue(nodeJsonData.GetValueObj("numberVar"));
        }

        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        //public static NumberVarField ParseNumberVarField(JsonData nodeJsonData)
        //{
        //    if (null == nodeJsonData)
        //    {
        //        return null;
        //    }

        //    NumberVarField numberVarField = null;

        //    if (!nodeJsonData.ContainsKey("__TYPE__"))
        //    {
        //        return null;
        //    }
        //    var nodeJsonType = nodeJsonData["__TYPE__"];
        //    var typeStr = nodeJsonType.ToString();
        //    var strs = typeStr.Split('.');
        //    var str = strs[strs.Length - 1];

        //    var fullName = NameSpaceName + "." + str;
        //    //_G.Log("ParseNumberVarField fullName : " + fullName);
        //    var resultClassName = fullName;
        //    var type = Type.GetType(resultClassName);
        //    if (type != null)
        //    {
        //        if (type.IsSubclassOf(typeof(NumberVarField)) || type == typeof(NumberVarField))
        //        {
        //            numberVarField = Activator.CreateInstance(type) as NumberVarField;
        //            numberVarField.Parse(nodeJsonData);
        //        }
        //    }
        //    else
        //    {
        //        _G.LogError("the type of numberVarField is not found : " + resultClassName);
        //    }

        //    return numberVarField;
        //}

        public static NumberVarField ParseNumberVarField(ITriggerDataNode nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            NumberVarField numberVarField = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var typeStr = nodeJsonData.GetString("__TYPE__");
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //_G.Log("ParseNumberVarField fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(NumberVarField)) || type == typeof(NumberVarField))
                {
                    numberVarField = Activator.CreateInstance(type) as NumberVarField;
                    numberVarField.Parse(nodeJsonData);
                }
            }
            else
            {
                _Battle_Log.LogError("the type of numberVarField is not found : " + resultClassName);
            }

            return numberVarField;
        }

        public virtual float Get(ActionContext context)
        {
            return this.numberVar.Get(context);
        }

    }
}