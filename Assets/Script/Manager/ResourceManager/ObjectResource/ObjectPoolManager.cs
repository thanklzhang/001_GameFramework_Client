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
    UnityEngine.Object obj;

    bool isUsing;

    internal bool IsUsing()
    {
        return isUsing;
    }

    //internal UnityEngine.Object GetObj()
    //{
    //    return obj;
    //}

    internal void SetInfo(int id, UnityEngine.Object obj)
    {
        this.id = id;
        this.obj = obj;
    }

    internal UnityEngine.Object Use()
    {
        if (obj is GameObject)
        {
            var go = obj as GameObject;
            go.SetActive(true);
        }
        this.isUsing = true;
        return obj;
    }

    internal void Return()
    {
        if (obj is GameObject)
        {
            var go = obj as GameObject;
            go.SetActive(false);

        }
        this.isUsing = false;
    }

}
public class ObjectPool
{
    public UnityEngine.Object assetObj;
    public Dictionary<int, PoolObj> objectDic = new Dictionary<int, PoolObj>();
    bool isLoadingAsset = false;
    public string path;
    //Action<UnityEngine.Object> getObjCallbackList;
    List<Action<UnityEngine.Object>> getObjCallbackList = new List<Action<UnityEngine.Object>>();
    Type type;
    internal void GetObject<T>(Action<UnityEngine.Object> callback)
    {
        this.type = typeof(T);
        if (null == assetObj)
        {
            //这里这 loadAsset 一次 ，防止同时多次 LoadAsset 调用造成 ab 计数错误

            //不过如果都是走池子的话 那么 LoadAsset 多次实际上也没问题
            //因为如果在需要清理的时候 该类型的obj 都是未使用状态 即使 asset 引用 > 1 那么也可以减少到 0 并清理 objs
            if (isLoadingAsset)
            {
                //正在加载
                getObjCallbackList.Add(callback);
            }
            else
            {
                isLoadingAsset = true;
                getObjCallbackList.Add(callback);

                AssetManager.Instance.Load(path, (asset) =>
                {
                    this.OnAssetLoadFinish(asset);
                });
            }
        }
        else
        {
            //有 asset 直接取 obj
            var poolObj = GetCachePoolObj();
            var obj = poolObj.Use();
            callback?.Invoke(obj);
        }

    }

    public bool IsGameObject()
    {
        //TODO : 根据 path 来进行断定是否是 GameObject
        if (this.type == typeof(GameObject))
        {
            return true;
        }
        return false;
    }

    public PoolObj GetCachePoolObj()
    {
        PoolObj poolObj = null;
        var isGo = IsGameObject();

        if (isGo)
        {
            //有 asset 直接取 obj
            foreach (var item in objectDic)
            {
                if (item.Value.IsUsing())
                {
                    continue;
                }

                poolObj = item.Value;
                poolObj.Use();

                break;
            }
            if (null == poolObj)
            {
                //如果都在使用 那么创建新的 Object(目前没有上限 之后会有上限)
                poolObj = CreateNew();
            }
        }
        else
        {
            //由于是引用 所以第一个就是
            foreach (var item in objectDic)
            {
                poolObj = item.Value;
            }

            if (null == poolObj)
            {
                //如果都在使用 那么创建新的 Object(目前没有上限 之后会有上限)
                poolObj = CreateNew();
            }
        }
        return poolObj;
    }

    public PoolObj CreateNew()
    {
        //path
        UnityEngine.Object obj = null;
        var isGo = IsGameObject();

        PoolObj newPoolObj = null;
        if (isGo)
        {
            obj = GameObject.Instantiate(this.assetObj) as GameObject;
            newPoolObj = new PoolObj();
            newPoolObj.SetInfo(obj.GetInstanceID(), obj);
            //newPoolObj.id = obj.GetInstanceID();
            //newPoolObj.obj = obj;
            newPoolObj.Use();
            objectDic.Add(newPoolObj.id, newPoolObj);
        }
        else
        {
            obj = this.assetObj;
            if (!objectDic.ContainsKey(obj.GetInstanceID()))
            {
                newPoolObj = new PoolObj();
                newPoolObj.SetInfo(obj.GetInstanceID(), obj);
                //newPoolObj.id = obj.GetInstanceID();
                //newPoolObj.obj = obj;
                newPoolObj.Use();
                objectDic.Add(newPoolObj.id, newPoolObj);
            }
            else
            {
                newPoolObj = objectDic[obj.GetInstanceID()];
            }
        }

        return newPoolObj;
    }

    public void OnAssetLoadFinish(UnityEngine.Object asset)
    {
        this.assetObj = asset;
        for (int i = 0; i < getObjCallbackList.Count; i++)
        {
            var callback = getObjCallbackList[i];
            var poolObj = GetCachePoolObj();
            var obj = poolObj.Use();
            callback?.Invoke(obj);
        }
        getObjCallbackList.Clear();
    }

    internal void ReturnObject(UnityEngine.Object obj)
    {
        var isGo = obj is GameObject;
        var insId = obj.GetInstanceID();
        if (isGo)
        {
            if (objectDic.ContainsKey(insId))
            {
                var findObj = objectDic[insId];
                findObj.Return();

            }
            else
            {
                Logx.LogWarning("the insId is not found : " + insId);
            }
        }

    }
}
public class ObjectPoolGroup
{
    public Dictionary<string, ObjectPool> objectPoolDic = new Dictionary<string, ObjectPool>();

    internal void GetObject<T>(string path, Action<UnityEngine.Object> callback)
    {
        ObjectPool pool = null;
        if (objectPoolDic.ContainsKey(path))
        {
            pool = objectPoolDic[path];
        }
        else
        {
            pool = new ObjectPool();
            pool.path = path;
            objectPoolDic.Add(path, pool);
        }

        pool.GetObject<T>(callback);
    }

    public void ReturnObj(string path, UnityEngine.Object obj)
    {
        ObjectPool pool = null;
        if (objectPoolDic.ContainsKey(path))
        {
            pool = objectPoolDic[path];
            pool.ReturnObject(obj);
        }
        else
        {
            Logx.LogWarning("ObjectPoolGroup : GetObject : the path is not found : " + path);
        }


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

        group.GetObject<T>(path, callback);

    }

    public void ReturnObject<T>(string path, T obj) where T : UnityEngine.Object
    {
        var type = typeof(T);
        ObjectPoolGroup group = null;
        if (objectPoolGroupDic.ContainsKey(type))
        {
            group = objectPoolGroupDic[type];
            group.ReturnObj(path, obj);
        }
        else
        {
            Logx.LogWarning("the type is not found : " + type.ToString());
        }
    }

    public void Release()
    {
        //清除都没在使用的 poolObj
    }
}