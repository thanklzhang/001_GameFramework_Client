using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigData
{
    public int id;
}
namespace Config
{
    public class ConfigManager : Singleton<ConfigManager>
    {
        //private static ConfigManager instance;

        //public static ConfigManager Instance
        //{
        //    get
        //    {
        //        if (null == instance)
        //        {
        //            instance = new ConfigManager();
        //            //instance.LoadConfig();
        //        }
        //        return instance;
        //    }

        //}
        //Dictionary<Type, List<ConfigData>> configDic = new Dictionary<Type, List<ConfigData>>();
        Dictionary<string, IList> configDic = new Dictionary<string, IList>();

        //public Dictionary<int, SkillEffectType> skillEffectDic;

        //public Dictionary<int, Config.SkillCalculateConfig> skillCalculateDic;
        //public Dictionary<int, Config.SkillProjectileConfig> skillProjectileDic;

        public void LoadConfig()
        {
            //这里是通过表来进行读取

            configDic = ConfigDataLoader.Instance.LoadFromFile();
            //configDic = ConfigDataLoader.Instance.LoadFromFileByAutoClass();


            //技能效果相关
            //skillEffectDic = new Dictionary<int, SkillEffectType>();

            //各种技能效果的字典
            //skillCalculateDic = GetDic<Config.SkillCalculateConfig>();
            //skillProjectileDic = GetDic<Config.SkillProjectileConfig>();
            //填充技能效果字典

            //foreach (var item in skillCalculateDic)
            //{
            //    skillEffectDic.Add(item.Key, SkillEffectType.Calulate);
            //}
            //foreach (var item in skillProjectileDic)
            //{
            //    skillEffectDic.Add(item.Key, SkillEffectType.Projectile);
            //}




        }

        //public SkillEffectType GetSkillEffectType(int SN)
        //{
        //    if (skillEffectDic.ContainsKey(SN))
        //    {
        //        return skillEffectDic[SN];
        //    }
        //    else
        //    {
        //        Debug.Log("the SN is not exist : " + SN);
        //        return SkillEffectType.Calulate;
        //    }
        //}

        public T GetById<T>(int id) where T : ConfigData
        {
            var data = (configDic[typeof(T).Name]).Cast<T>().Where(t => t.id == id);
            if (null == data || 0 == data.Count())
                throw new Exception("cant find the SN : " + id + " the type : " + data.GetType().ToString());
            //Debug.Log(data.First().GetType());
            return (T)(data.First());
        }


        public List<T> GetAll<T>() where T : ConfigData
        {
            var data = (configDic[typeof(T).Name]);
            return data.Cast<T>().ToList();//Select(d => (T)d).ToList();
        }

        public Dictionary<int, T> GetDic<T>() where T : ConfigData
        {

            return configDic[typeof(T).Name].Cast<T>().ToDictionary(d => d.id, obj => obj as T);
        }
    }

}
