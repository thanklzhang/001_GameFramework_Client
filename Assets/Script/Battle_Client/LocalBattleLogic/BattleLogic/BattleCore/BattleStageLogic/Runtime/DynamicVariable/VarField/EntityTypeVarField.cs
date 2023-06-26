using System;

namespace Battle.BattleTrigger.Runtime
{
    //public enum EntityType_GetValueFromType
    //{
    //    //固定 configId 的类型
    //    EntityTypeByConfigId = 0,
    //    //触发单位的类型
    //    EntityTypeTrigger = 1
    //}

    public class EntityTypeVarField : BaseVarField
    {
        //Vector3_GetValueFromType getType;
        EntityTypeVar entityTypeVar;

        public override void OnParse(ITriggerDataNode nodeJsonData)
        {
            //getType = (Vector3_GetValueFromType)(int.Parse(nodeJsonData["getType"].ToString()));
            entityTypeVar = EntityTypeVar.ParseValue(nodeJsonData["entityTypeVar"]);
        }

        public static string NameSpaceName = "Battle.BattleTrigger.Runtime";

        public static EntityTypeVarField ParseVarField(ITriggerDataNode nodeJsonData)
        {
            if (null == nodeJsonData)
            {
                return null;
            }

            EntityTypeVarField entityTypeVarField = null;

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
                if (type.IsSubclassOf(typeof(EntityTypeVarField)) || type == typeof(EntityTypeVarField))
                {
                    entityTypeVarField = Activator.CreateInstance(type) as EntityTypeVarField;
                    entityTypeVarField.Parse(nodeJsonData);
                }
            }
            else
            {
                _Battle_Log.LogError("the type of entityTypeVarField is not found : " + resultClassName);
            }

            return entityTypeVarField;
        }

        public virtual int Get(ActionContext context)
        {
            return this.entityTypeVar.Get(context);
        }
    }
}