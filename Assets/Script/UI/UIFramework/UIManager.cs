using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : Singleton<UIManager>
{
    Transform uiRoot;
    internal void Init(Transform uiRoot)
    {
        this.uiRoot = uiRoot;
    }

    public Dictionary<Type, BaseUI> uiCacheDic = new Dictionary<Type, BaseUI>();


    internal void LoadUI<T>(Action<T> finishCallback) where T : BaseUI, new()
    {
        var type = typeof(T);
        if (uiCacheDic.ContainsKey(type))
        {
            var ui = (T)uiCacheDic[type];
            finishCallback?.Invoke(ui);
        }
        else
        {
            //Logx.LogzError("loadUI : the type is not found : " + type);
            //return null;

            var uiConfigInfo = UIConfigInfoDic.GetInfo<T>();
            //这里之后改成 resourceManager 加载的 go
            AssetManager.Instance.Load(uiConfigInfo.path, (prefab) =>
            {
                GameObject obj = GameObject.Instantiate(prefab as GameObject);
                obj.transform.SetParent(this.uiRoot, false);
                T t = new T();
                t.Init(obj, uiConfigInfo.path);
                uiCacheDic.Add(t.GetType(), t);
                finishCallback?.Invoke(t);
            }, false);
        }
    }

    public void ReleaseUI<T>()
    {
        var type = typeof(T);
        if (uiCacheDic.ContainsKey(type))
        {
            var ui = uiCacheDic[type];
            uiCacheDic.Remove(type);
            ui.Release();
        }
        else
        {
            Logx.LogzWarning("the ui is not exist in cache dic : " + type);
        }
    }
}
