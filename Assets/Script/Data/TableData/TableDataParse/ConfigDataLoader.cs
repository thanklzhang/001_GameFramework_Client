//using Config;

using FixedPointy;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
public class ConfigInfo
{
    public string name;
    public string json;
}
public class ConfigDataLoader
{
    private static ConfigDataLoader instance;

    public static ConfigDataLoader Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new ConfigDataLoader();
            }
            return instance;
        }

    }
    /// <summary>
    /// 用自动生成的加载类去读取 Config （目前没用）
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, System.Collections.IList> LoadFromFileByAutoClass()
    {
        TextAsset[] files = Resources.LoadAll<TextAsset>("Config/ConfigData");
        //LoadDatas<Enemy>(files[0].text);
        //LoadAllData(files);
        List<ConfigInfo> infoList = new List<ConfigInfo>();
        files.ToList().ForEach(file =>
        {
            ConfigInfo info = new ConfigInfo();
            info.name = file.name;
            info.json = file.text;
            infoList.Add(info);
        });

        return AutoConfigDataLoader.Instance.LoadAllData(infoList);
    }


    //-----------------------------以下是用动态泛型读取文件 在 ios 可能会出问题


    /// <summary>
    /// 根据动态泛型来一次性读取 Config 
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, System.Collections.IList> LoadFromFile()
    {
        TextAsset[] files = Resources.LoadAll<TextAsset>("Config/ConfigData");
        //LoadDatas<Enemy>(files[0].text);
        //LoadAllData(files);
        List<ConfigInfo> infoList = new List<ConfigInfo>();
        files.ToList().ForEach(file =>
        {
            ConfigInfo info = new ConfigInfo();
            info.name = file.name;
            info.json = file.text;
            infoList.Add(info);
        });

        return LoadAllData(infoList);
    }

    /// <summary>
    /// 根据 json 按字段赋值 反射出数据 为了 使 Fix 配合表
    /// </summary>
    /// <param name="infos"></param>
    /// <returns></returns>
    public Dictionary<string, System.Collections.IList> LoadAllDataByField(List<ConfigInfo> infos)
    {
        Dictionary<string, System.Collections.IList> configDic = new Dictionary<string, System.Collections.IList>();
        
        infos.ForEach(info =>
        {
            string typeName = "Config." + info.name;
            Type type = Type.GetType(typeName, true);
            Type listType = typeof(List<>).MakeGenericType(type);
            

            //建造每一个对象
            object list = Activator.CreateInstance(listType);
            List<JsonData> objs = JsonMapper.ToObject<List<JsonData>>(info.json);//{}

            objs.ForEach(obj =>
            {

                //定义 对象
                object currObject = Activator.CreateInstance(type);

                var fields = type.GetFields();
                fields.ToList().ForEach(f =>
                {
                    var jsonValue = obj[f.Name];

                    object value = jsonValue;
                    if (jsonValue.IsInt)
                    {
                        if (f.FieldType.Name.Contains("Fix"))
                        {
                            Fix fix = (int)jsonValue;
                        }
                        else
                        {
                            f.SetValue(currObject, (int)jsonValue);
                        }
                    }
                    else if (jsonValue.IsDouble)//f.FieldType.Name.Contains("Fix") && 
                    {
                        Fix fix = Fix.Ratio((int)(((double)jsonValue) * 100), 100);
                        f.SetValue(currObject, fix);
                    }
                    else if (jsonValue.IsString)
                    {
                        //string
                        f.SetValue(currObject, jsonValue.ToString());
                    }


                    var addMethod = listType.GetMethod("Add");
                    addMethod.Invoke(list, new object[] { currObject });
                });
            });

            configDic.Add(info.name, list as IList);

        });

        return configDic;
    }


    /// <summary>
    /// json ToObject 方法
    /// </summary>
    /// <param name="infos"></param>
    /// <returns></returns>
    public Dictionary<string, System.Collections.IList> LoadAllData(List<ConfigInfo> infos)
    {
        var curType = typeof(JsonMapper);
        //这个只是适用于一个泛型函数
        //var GenericMethod = curType
        //  .GetMethods()
        //  .Single(m => m.Name == "ToObject" && m.IsGenericMethodDefinition);

        Dictionary<string, System.Collections.IList> configDic = new Dictionary<string, System.Collections.IList>();
        var GenericMethod = curType
                     .GetMethods()
                     .Where(m => m.Name == "ToObject")
                     .Select(m => new
                     {
                         Method = m,
                         Params = m.GetParameters(),
                         Args = m.GetGenericArguments()
                     })
                     .Where(x => x.Params.Length == 1
                                 && x.Args.Length == 1
                                 && x.Params[0].ParameterType == typeof(string))
                     .Select(x => x.Method)
                     .First();


        infos.ForEach(info =>
        {
            string typeName = "Config." + info.name;
            Type type = Type.GetType(typeName, true);
            Type listType = typeof(List<>).MakeGenericType(type);

            //合并生成最终的函数
            MethodInfo curMethod = GenericMethod.MakeGenericMethod(listType);

            //执行函数
            //如果要执行的是静态函数，则第一个参数为null
            //第二个参数为参数值
            //即要调用的函数应该为 static fun<T>(string para1,int para2)
            var enemyList = curMethod.Invoke(null, new object[] { info.json }) as IList;

            configDic.Add(info.name, enemyList);
        });

        return configDic;
    }




}
