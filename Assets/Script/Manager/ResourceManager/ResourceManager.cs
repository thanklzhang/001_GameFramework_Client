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

    public void GetObject<T>(int resId, Action<T> callback, bool isSync = false) where T : UnityEngine.Object
    {
        var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(resId);
        var fullPath = Const.buildPath + "/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;
        GetObject(fullPath, callback,isSync);
    }

    //get obj by pool (include : gameObject texture sprite material)
    public void GetObject<T>(string path, Action<T> callback, bool isSync = false) where T : UnityEngine.Object
    {
        ObjectPoolManager.Instance.GetObject<T>(path, (obj) =>
        {
            T getObj = null;
            if (!(obj is T))
            {
                //Debug.LogWarning(string.Format("GetObject : {0} is not the same as {1}", obj.GetType(), typeof(T)));

                //如果资源有 是有 sprite 的 texture ，那么会被视为 sprite ，所以这里进行 sprite 和 texture 的兼容
                if (typeof(T).IsSubclassOf(typeof(Texture)) && obj.GetType() == typeof(Sprite))
                {
                    getObj = ((Sprite)obj).texture as T;
                }
                else
                {
                    var tex = obj as Texture2D;
                    Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                    
                    getObj = sprite as T;

                }
            }
            else
            {
                getObj = obj as T;
            }

            callback?.Invoke(getObj);

        });
    }

    internal void ReturnObject<T>(int resId, T obj) where T : UnityEngine.Object
    {
        var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(resId);
        var fullPath = Const.buildPath + "/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;
        ReturnObject(fullPath,obj);
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
