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
public class TableInfo
{
    public string name;
    public string json;
}
public class TableDataLoader:Singleton<TableDataLoader>
{
    //-----------------------------以下是用动态泛型读取文件 在 ios 可能会出问题

    /// <summary>
    /// 根据动态泛型来一次性读取 Config 
    /// </summary>
    /// <returns></returns>
    public Dictionary<Type, System.Collections.IList> LoadFromFile()
    {

        string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/BuildRes/TableData");
      
        List<TableInfo> infoList = new List<TableInfo>();
        files.ToList().ForEach(file =>
        {

            // 从文件中读取并显示每行
            string line = FileOperate.GetTextFromFile(file);

            TableInfo info = new TableInfo();
            var ext = Path.GetExtension(file);
            if (ext == ".json")
            {
                info.name = Path.GetFileNameWithoutExtension(file);
                info.json = line;
                infoList.Add(info);
            }
           
        });

        //return LoadAllData(infoList);
        return LoadAllDataByField(infoList);
    }

    /// <summary>
    /// 根据 json 按字段赋值 反射出数据 为了 使 Fix 配合表
    /// </summary>
    /// <param name="infos"></param>
    /// <returns></returns>
    public Dictionary<Type, System.Collections.IList> LoadAllDataByField(List<TableInfo> infos)
    {
        Dictionary<Type, System.Collections.IList> configDic = new Dictionary<Type, System.Collections.IList>();
        
        infos.ForEach(info =>
        {
            string typeName = "Table." + info.name;
            Type type = Type.GetType(typeName, true);
            Type listType = typeof(List<>).MakeGenericType(type);
            

            //建造每一个对象
            object list = Activator.CreateInstance(listType);
            List<JsonData> objs = JsonMapper.ToObject<List<JsonData>>(info.json);//{}

            objs.ForEach(obj =>
            {

                //定义 对象
                object currObject = Activator.CreateInstance(type);

                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                fields.ToList().ForEach(f =>
                {
                    var jsonValue = obj[f.Name];

                    //object value = jsonValue;
                    //if (null == jsonValue)
                    //{
                    //    return;
                    //}
                    if (jsonValue.IsInt)
                    {
                        f.SetValue(currObject, (int)jsonValue);
                    }
                    else if (jsonValue.IsDouble)//f.FieldType.Name.Contains("Fix") && 
                    {
                        f.SetValue(currObject, jsonValue);
                    }
                    else if (jsonValue.IsString)
                    {
                        f.SetValue(currObject, jsonValue.ToString());
                    }

                    
                });

                var addMethod = listType.GetMethod("Add");
                addMethod.Invoke(list, new object[] { currObject });
            });

            configDic.Add(type, list as IList);

        });

        return configDic;
    }



}
