using System;

namespace Battle.BattleTrigger.Runtime
{
    public enum ConditionCheckType
    {
        Number_Check,
        Bool_Check,
    }

    public class ConditionCheck
    {
        public void Parse(ITriggerDataNode jsonData)
        {
            this.OnParse(jsonData);
        }

        public virtual void OnParse(ITriggerDataNode jsonData)
        {

        }

        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        public static ConditionCheck ParseConditionCheck(ITriggerDataNode nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            ConditionCheck conditionJudge = null;

            if (!nodeJsonData.ContainsKey("__TYPE__"))
            {
                return null;
            }
            var nodeJsonType = nodeJsonData["__TYPE__"];
            var typeStr = nodeJsonType.ToString();
            var strs = typeStr.Split('.');
            var str = strs[strs.Length - 1];

            var fullName = NameSpaceName + "." + str;
            //_Battle_Log.Log("ParseConditionJudge fullName : " + fullName);
            var resultClassName = fullName;
            var type = Type.GetType(resultClassName);
            if (type != null)
            {
                if (type.IsSubclassOf(typeof(ConditionCheck)))
                {
                    conditionJudge = Activator.CreateInstance(type) as ConditionCheck;
                    conditionJudge.Parse(nodeJsonData);
                }
            }
            else
            {
                _Battle_Log.LogError("the type of ConditionCheck is not found : " + resultClassName);
            }

            return conditionJudge;
        }

        public virtual bool Check(ActionContext context)
        {
            return false;
        }

    }
}