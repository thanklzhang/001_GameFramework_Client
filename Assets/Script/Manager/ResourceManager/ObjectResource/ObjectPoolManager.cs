using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
public class PoolObj
{
    public int id;
    public UnityEngine.Object obj;
}
public class ObjectPool
{
    public Dictionary<int, PoolObj> objectPoolDic = new Dictionary<int, PoolObj>();

}
public class ObjectPoolGroup
{
    public Dictionary<string, ObjectPool> objectPoolDic = new Dictionary<string, ObjectPool>();

    internal void GetObject(string path, Action<UnityEngine.Object> callback)
    {

    }
}
public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    //gameObject texture sprite material
    public Dictionary<Type, ObjectPoolGroup> objectPoolGroupDic = new Dictionary<Type, ObjectPoolGroup>();

    public void GetObject<T>(string path, Action<UnityEngine.Object> callback)
    {
        var type = typeof(T);
        ObjectPoolGroup group = null;
        if (objectPoolGroupDic.ContainsKey(type))
        {
            group = objectPoolGroupDic[type];
        }
        else
        {
            group = new ObjectPoolGroup();
            objectPoolGroupDic.Add(type, group);
        }

        group.GetObject(path, callback);

    }

    public void ReturnObject<T>(string path, T obj)
    {

    }
}


