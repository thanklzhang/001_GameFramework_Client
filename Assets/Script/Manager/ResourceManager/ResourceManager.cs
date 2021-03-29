using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;


public class ResourceManager : Singleton<ResourceManager>
{
    List<LoadObjectRequest> requestList = new List<LoadObjectRequest>();

    //load single
    public void LoadPrefab(string path, Action<GameObject> callback, bool isSync = false)
    {
        Action<UnityEngine.Object> finishCallback = (obj) =>
        {
            var gameObject = obj as GameObject;
            callback?.Invoke(gameObject);
        };
        AssetManager.Instance.Load(path, finishCallback, isSync);
    }

    //Get Obj by pool (include : gameObject texture sprite material)
    public void GetObject<T>(string path, Action<T> callback, bool isSync = false)
    {
        ObjectPoolManager.Instance.GetObject<T>(path, (obj) =>
        {
            callback?.Invoke(obj);
        });
    }

    internal void ReturnObject<T>(string path, T obj)
    {
        ObjectPoolManager.Instance.ReturnObject(path, obj);
    }

    //batch load asset
    public LoadObjectRequest LoadObjects(string[] pathList)
    {
        LoadObjectRequest req = new LoadAssetObjRequest();
        req.Start(pathList);
        requestList.Add(req);

        return req;
    }

    public void Update(float deltaTime)
    {
        for (int i = requestList.Count - 1; i >= 0; i--)
        {
            var req = requestList[i];
            if (req.CheckFinish())
            {
                req.Finish();
                requestList.Remove(req);
            }
        }
    }

}

