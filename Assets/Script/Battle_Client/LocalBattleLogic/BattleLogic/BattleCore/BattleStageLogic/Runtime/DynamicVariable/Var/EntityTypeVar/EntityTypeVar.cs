using System;

namespace Battle.BattleTrigger.Runtime
{
    public class EntityTypeVar : BaseVar
    {

        public void Parse(ITriggerDataNode nodeJsonData)
        {
            this.OnParse(nodeJsonData);
        }

        public virtual void OnParse(ITriggerDataNode nodeJsonData)
        {

        }

        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";
        public static EntityTypeVar ParseValue(ITriggerDataNode nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            EntityTypeVar entityTypeVar = null;

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
                if (type.IsSubclassOf(typeof(EntityTypeVar)))
                {
                    entityTypeVar = Activator.CreateInstance(type) as EntityTypeVar;
                    entityTypeVar.Parse(nodeJsonData);
                }
            }
            else
            {
                _Battle_Log.LogError("the type of entityTypeVar is not found : " + resultClassName);
            }

            return entityTypeVar;
        }

        public virtual int Get(ActionContext context)
        {
            return 0;
        }
    }
}