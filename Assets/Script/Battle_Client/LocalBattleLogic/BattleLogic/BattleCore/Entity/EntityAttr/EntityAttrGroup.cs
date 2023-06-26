using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{

    //属性组
    public class EntityAttrGroup
    {
        //public int id;//唯一标识id 如 获得装备时候为 装备 id 移除的时候按照这个 id 移除

        Dictionary<EntityAttrType, EntityAttrOption> attrOptionDic = new Dictionary<EntityAttrType, EntityAttrOption>();
        //Dictionary<EntityAttrType, EntityAttrOption> permillageOptionDic = new Dictionary<EntityAttrType, EntityAttrOption>();

        public Dictionary<EntityAttrType, EntityAttrOption> GetAttrOptions()
        {
            return attrOptionDic;
        }

        public void AddAttr(EntityAttrType type, int id, float value)//, bool isPermillage
        {
            var dic = attrOptionDic;//GetOptionDic(isPermillage);

            EntityAttrOption attrOption = null;
            if (dic.ContainsKey(type))
            {
                attrOption = dic[type];
                attrOption.Add(id, value);
            }
            else
            {
                attrOption = new EntityAttrOption();
                attrOption.Init(type, id, value);
                dic[type] = attrOption;
                attrOption.Add(id, value);
            }


        }

        public void RemoveAttr(EntityAttrType type, int id)//, bool isPermillage
        {
            EntityAttrOption attrOption = null;
            var dic = attrOptionDic;// GetOptionDic(isPermillage);
            if (dic.ContainsKey(type))
            {
                attrOption = dic[type];
                attrOption.Remove(id);
            }
        }

        public float GetTotalValue(EntityAttrType type)//, bool isPermillage
        {
            var dic = attrOptionDic;// GetOptionDic(isPermillage);
            if (dic.ContainsKey(type))
            {
                var option = dic[type];
                return option.GetTotalValue();
            }

            return 0;
        }

        //public Dictionary<EntityAttrType, EntityAttrOption> GetOptionDic(bool isPermillage)
        //{
        //    Dictionary<EntityAttrType, EntityAttrOption> dic = null;
        //    if (!isPermillage)
        //    {
        //        dic = fixedOptionDic;
        //    }
        //    else
        //    {
        //        dic = permillageOptionDic;
        //    }

        //    return dic;
        //}
    }

}
