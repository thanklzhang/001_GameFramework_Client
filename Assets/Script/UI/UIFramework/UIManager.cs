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


    internal void GetUICache<T>(Action<T> finishCallback = null) where T : BaseUI, new()
    {
        var type = typeof(T);
        if (uiCacheDic.ContainsKey(type))
        {
            var ui = (T)uiCacheDic[type];
            finishCallback?.Invoke(ui);
        }
        else
        {
            ////Logx.LogError("loadUI : the type is not found : " + type);
            //return null;

            var uiConfigInfo = UIConfigInfoDic.GetInfo<T>();
            if (null == uiConfigInfo)
            {
                Logx.LogError("GetUICache : the type is not found : " + typeof(T));
                return;
            }
            ResourceManager.Instance.GetObject<GameObject>(uiConfigInfo.path, (gameObject) =>
            {
                gameObject.transform.SetParent(this.uiRoot, false);
                T t = new T();
                t.Init(gameObject, uiConfigInfo.path);
                uiCacheDic.Add(t.GetType(), t);
                finishCallback?.Invoke(t);
            });
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

            var uiConfigInfo = UIConfigInfoDic.GetInfo<T>();
            ResourceManager.Instance.ReturnObject<GameObject>(uiConfigInfo.path, ui.gameObject);
            //AssetManager.Instance.Release(uiConfigInfo.path);
        }
        else
        {
            //Logx.LogWarning("the ui is not exist in cache dic : " + type);
        }
    }

}
