using System.Collections.Generic;

namespace Battle
{
    public interface IConfigManager
    {
       void Init();
       void LoadAllTableData(string path);

       T GetById<T>(int id) where T:IConfig;
       IList<T> GetList<T>();
       Dictionary<int, T> GetDic<T>();


        //Dictionary<Type, IList> typeToListConfigDic = new Dictionary<Type, IList>();
        //Dictionary<Type, Dictionary<int, IConfig>> typeToDicConfigDic = new Dictionary<Type, Dictionary<int, IConfig>>();

        //public virtual void Load()
        //{

        //}

        //public T GetById<T>(int id) where T : IConfig
        //{
        //    var type = typeof(T);
        //    if (typeToDicConfigDic.ContainsKey(type))
        //    {
        //        var dataDic = typeToDicConfigDic[type];
        //        if (dataDic.ContainsKey(id))
        //        {
        //            var data = dataDic[id];
        //            return (T)data;
        //        }
        //        else
        //        {
        //            _G.LogError("ConfigManager.GetById", "the id is not found : " + id + " ,type : " + type.ToString());
        //        }
        //    }
        //    else
        //    {
        //        _G.LogError("ConfigManager.GetById", "the type is not found : " + type);
        //    }
        //    return null;
        //}


        //public List<T> GetList<T>() where T : IConfig
        //{
        //    var data = (typeToListConfigDic[typeof(T)]);
        //    return data.Cast<T>().ToList();//Select(d => (T)d).ToList();
        //}

        //public Dictionary<int, T> GetDic<T>() where T : IConfig
        //{
        //    var type = typeof(T);
        //    if (typeToDicConfigDic.ContainsKey(type))
        //    {
        //        var configDic = typeToDicConfigDic[type];
        //        return configDic.ToDictionary(kv =>
        //        {
        //            return kv.Key;
        //        }, (kv) =>
        //        {
        //            return (T)kv.Value;
        //        });
        //    }
        //    return null;
        //}
    }

}
