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
namespace Table
{
    public class TableInfo
    {
        public string name;
        public string json;
    }
    public class TableDataLoader : Singleton<TableDataLoader>
    {
        //-----------------------------以下是用动态泛型读取文件 在 ios 可能会出问题


        IEnumerator LoadFromAB(Action<List<TableInfo>> finishCallback)
        {
            List<TableInfo> infoList = new List<TableInfo>();

            var tablePath = "Table";
            var loadPath = Const.AssetBundlePath + "/" + Const.buildPath + "/" + tablePath;

            var files = System.IO.Directory.GetFiles(loadPath);
            foreach (var filePath in files)
            {
                bool isLoadFinish = false;
                string loadText = "";
                var ext = Path.GetExtension(filePath);
                if (ext == ".ab")
                {
                    var strIndex = filePath.IndexOf(Const.buildPath + "/" + tablePath);
                    var resultPath = filePath.Substring(strIndex).Replace("\\", "/").Replace(".ab", ".json").ToLower();
                    //Logx.Log("start load : resultPath :  " + resultPath);
                    ResourceManager.Instance.GetObject<TextAsset>(resultPath, (textAsset) =>
                    {
                        //Logx.Log("load text finish: " + textAsset.text);
                        loadText = textAsset.text;
                        isLoadFinish = true;
                    });

                    while (true)
                    {
                        yield return null;

                        if (isLoadFinish)
                        {
                            TableInfo info = new TableInfo();
                            info.name = Path.GetFileNameWithoutExtension(filePath);
                            info.json = loadText;
                            infoList.Add(info);
                            break;
                        }
                    }
                }
            }

            finishCallback?.Invoke(infoList);
        }

        /// <summary>
        /// 根据动态泛型来一次性读取 Config 
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadFromFile(Action<Dictionary<Type, IList>> finishCallback)
        {

            if (Const.isUseAB)
            {
                //AB 加载
                yield return LoadFromAB((list) =>
                {
                    var dic = LoadAllDataByField(list);
                    finishCallback?.Invoke(dic);
                });
            }
            else
            {
                //非 AB 加载 之后可能改成 AssetDatabase 的加载
                var loadPath = Application.dataPath + "/BuildRes/Table";
                string[] files = System.IO.Directory.GetFiles(loadPath);
                List<TableInfo> infoList = new List<TableInfo>();
                files.ToList().ForEach(file =>
                {
                    string jsonStr = FileOperate.GetTextFromFile(file);

                    TableInfo info = new TableInfo();
                    var ext = Path.GetExtension(file);
                    if (ext == ".json")
                    {
                        info.name = Path.GetFileNameWithoutExtension(file);
                        info.json = jsonStr;
                        infoList.Add(info);
                    }

                });

                //return LoadAllData(infoList);
                var dic = LoadAllDataByField(infoList);
                finishCallback?.Invoke(dic);
            }
        }

        public Dictionary<Type, IList> LoadFromFileByEditor()
        {
            //非 AB 加载 之后可能改成 AssetDatabase 的加载
            var loadPath = Application.dataPath + "/BuildRes/Table";
            string[] files = System.IO.Directory.GetFiles(loadPath);
            List<TableInfo> infoList = new List<TableInfo>();
            files.ToList().ForEach(file =>
            {
                string jsonStr = FileOperate.GetTextFromFile(file);

                TableInfo info = new TableInfo();
                var ext = Path.GetExtension(file);
                if (ext == ".json")
                {
                    info.name = Path.GetFileNameWithoutExtension(file);
                    info.json = jsonStr;
                    infoList.Add(info);
                }

            });

            //return LoadAllData(infoList);
            var dic = LoadAllDataByField(infoList);

            return dic;
        }


        /// <summary>
        /// 根据 json 按字段赋值 反射出数据 为了 使 Fix 配合表
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        Dictionary<Type, System.Collections.IList> LoadAllDataByField(List<TableInfo> infos)
        {
            Dictionary<Type, System.Collections.IList> configDic = new Dictionary<Type, System.Collections.IList>();

            infos.ForEach(info =>
            {
                string typeName = "Table." + info.name;
                try
                {
                    Type type = Type.GetType(typeName, true, true);
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
                }
                catch (TypeLoadException e)
                {
                    Logx.LogError("the type is not found : " + typeName);
                }
                finally
                {

                }


            });

            return configDic;
        }



    }
}