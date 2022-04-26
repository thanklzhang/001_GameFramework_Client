using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;


public class ResourceManager : Singleton<ResourceManager>
{
    List<LoadResGroupRequest> requestList = new List<LoadResGroupRequest>();

    //batch load asset
    public LoadResGroupRequest LoadObjects(List<LoadObjectRequest> requests)
    {
        LoadResGroupRequest req = new LoadResGroupRequest();
        req.Start(requests);
        requestList.Add(req);
        return req;
    }

    //get obj by pool (include : gameObject texture sprite material)
    public void GetObject<T>(string path, Action<T> callback, bool isSync = false) where T : UnityEngine.Object
    {
        ObjectPoolManager.Instance.GetObject<T>(path, (obj) =>
        {
            var getObj = obj as T;
            callback?.Invoke(getObj);
        });
    }

    internal void ReturnObject<T>(string path, T obj) where T : UnityEngine.Object
    {
        ObjectPoolManager.Instance.ReturnObject(path, obj);
    }

    public void Update(float deltaTime)
    {
        for (int i = requestList.Count - 1; i >= 0; i--)
        {
            var req = requestList[i];
            req.Update(deltaTime);
            if (req.CheckFinish())
            {
                req.Finish();
                requestList.Remove(req);
            }
        }
    }

#if UNITY_EDITOR
    public T GetObjectByEditor<T>(string path) where T : UnityEngine.Object
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
    }


    public T GetObjectByConfiIdEditor<T>(int configId) where T : UnityEngine.Object
    {
        var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(configId);
        var path = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;
        return GetObjectByEditor<T>(path);
    }
#endif
}
