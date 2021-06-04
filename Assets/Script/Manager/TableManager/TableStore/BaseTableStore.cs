/*
 * generate by tool
*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using FixedPointy;
using System;

public class BaseTableStore<T> where T : Table.BaseTable
{
    private List<T> list = new List<T>();
    private Dictionary<int, T> dic = new Dictionary<int, T>();

    public Dictionary<int, T> Dic { get => dic; set => dic = value; }
    public List<T> List { get => list; set => list = value; }

    internal T GetById(int id)
    {
        return Dic[id];
    }

    public void Init()
    {

    }

    internal virtual void Load()
    {
        //加载表格数据

        

        //设置 List 和 Dic
    }
}
