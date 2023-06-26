using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    //单位的属性
    public class AttrHelper
    {
        public static bool IsPermillage(EntityAttrType attrType)
        {
            if ((int) attrType >= 1000)
            {
                return true;
            }

            return false;
        }

        public static EntityAttrType GetPermillageTypeByFixedType(EntityAttrType attrType)
        {
            var type = (EntityAttrType) ((int) attrType + 1000);
            return type;
        }
    }
}