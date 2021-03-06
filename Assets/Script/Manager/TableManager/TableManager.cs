/*
 * generate by tool
*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using FixedPointy;
using System;
using System.IO;

public class TableManager : Singleton<TableManager>
{
    Dictionary<Type, IList> typeToListConfigDic = new Dictionary<Type, IList>();
    Dictionary<Type, Dictionary<int, Table.BaseTable>> typeToDicConfigDic = new Dictionary<Type, Dictionary<int, Table.BaseTable>>();

    internal void Init()
    {
        //HeroInfoStore.Init();
    }

    public void LoadAllTableData()
    {
        typeToListConfigDic = TableDataLoader.Instance.LoadFromFile();

        foreach (var configKV in typeToListConfigDic)
        {
            var iDic = new Dictionary<int, Table.BaseTable>();
            foreach (var tableData in configKV.Value)
            {
                Table.BaseTable convertTableData = tableData as Table.BaseTable;
                iDic.Add(convertTableData.Id, convertTableData);
            }

            typeToDicConfigDic.Add(configKV.Key, iDic);
        }

    }


    public T GetById<T>(int id) where T : Table.BaseTable
    {
        var type = typeof(T);
        if (typeToDicConfigDic.ContainsKey(type))
        {
            var dataDic = typeToDicConfigDic[type];
            if (dataDic.ContainsKey(id))
            {
                var data = dataDic[id];
                return data as T;
            }
            else
            {
                Logx.LogError("Table", "the id is not found : " + id);
            }
        }
        else
        {
            Logx.LogError("Table", "the type is not found : " + type);
        }
        return null;
    }


    public List<T> GetList<T>() where T : Table.BaseTable
    {
        var data = (typeToListConfigDic[typeof(T)]);
        return data.Cast<T>().ToList();//Select(d => (T)d).ToList();
    }

    public Dictionary<int, T> GetDic<T>() where T : Table.BaseTable
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
                return kv as T;
            });
        }
        return null;
    }
}