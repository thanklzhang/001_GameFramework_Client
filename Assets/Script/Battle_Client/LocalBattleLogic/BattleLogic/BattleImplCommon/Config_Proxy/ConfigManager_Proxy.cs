using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Table;
using System.Collections;

namespace Battle
{
    public class ConfigManager_Proxy : IConfigManager
    {
        public TableManager tableManager;

        Dictionary<Type, IList> typeToListConfigDic = new Dictionary<Type, IList>();
        Dictionary<Type, Dictionary<int, IConfig>> typeToDicConfigDic = new Dictionary<Type, Dictionary<int, IConfig>>();

        public void Init()
        {
            tableManager = TableManager.Instance;
            //tableManager.Init();

            LoadAllTableData("");
        }
        public static string NameSpaceName = "Battle";
        public void LoadAllTableData(string path)
        {
            //tableManager.LoadAllTableData(path);

            typeToListConfigDic = new Dictionary<Type, IList>();
            foreach (var item in tableManager.typeToListConfigDic)
            {
                var type = item.Key;
                var list = item.Value;

                var typeName = NameSpaceName + "." + type.Name;
                var proxyResultName = typeName + "Config_Proxy";
                
                var parentResultName = NameSpaceName + ".I" + type.Name + "Config";
                Type proxyType = Type.GetType(proxyResultName);
                Type parentType = Type.GetType(parentResultName);
                if (null == proxyType)
                {
                    Logx.LogWarning("the type of proxyType is not found : " + proxyResultName);
                    continue;
                }
                if (null == parentType)
                {
                    Logx.LogWarning("the type of parentType is not found : " + parentType);
                    continue;
                }
                List<IConfig> newList = new List<IConfig>();
                foreach (var data in list)
                {
                    var srcData = data as Table.BaseTable;
                    var newObj = Activator.CreateInstance(proxyType) as IConfig;
                    newObj.Init(srcData.Id);
                    newList.Add(newObj);
                }

                typeToDicConfigDic.Add(parentType, newList.ToDictionary((v)=>
                {
                    return v.Id;
                }));

                typeToListConfigDic.Add(parentType, newList);
            }


        }
        public T GetById<T>(int id) where T : IConfig
        {
            var type = typeof(T);
            if (typeToDicConfigDic.ContainsKey(type))
            {
                var dataDic = typeToDicConfigDic[type];
                if (dataDic.ContainsKey(id))
                {
                    var data = dataDic[id];
                    return (T)data;
                }
                else
                {
                    Logx.LogWarning("ConfigManager_Proxy", "the id is not found : " + id);
                }
            }
            else
            {
                Logx.LogWarning("ConfigManager_Proxy", "the type is not found : " + type);
            }
            return default(T);
        }


        public IList<T> GetList<T>()
        {
            var list = (typeToListConfigDic[typeof(T)]);
            return list.Cast<T>().ToList();
        }


        public Dictionary<int, T> GetDic<T>()
        {
            var type = typeof(T);
            if (typeToDicConfigDic.ContainsKey(type))
            {
                var configDic = typeToDicConfigDic[type];
                return configDic.ToDictionary(kv =>
                {
                    return kv.Key;
                }, (kv) =>
                {
                    return (T)kv.Value;
                });
            }
            return null;
        }

    }

}
