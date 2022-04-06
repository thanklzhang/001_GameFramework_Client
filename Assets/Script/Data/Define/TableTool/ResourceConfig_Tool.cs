using System.Collections.Generic;

namespace Table
{
    //资源类型
    public enum ResourceType
    {
        Null = 0,
        UI = 1,
        Picture = 2,
        Audio = 3,
        Effect = 4,
        Scene = 5,
        Model = 6
    }


    public static class ResourceConfig_Tool
    {
        public static bool IsGameObject(int resId)
        {
            var tb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(resId);
            var type = (ResourceType)tb.Type;

            if (type == ResourceType.UI || type == ResourceType.Effect || type == ResourceType.Model)
            {
                return true;
            }
            return false;
        }
    }
}