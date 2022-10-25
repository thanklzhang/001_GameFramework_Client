
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;


//asset 资源缓存
public class AssetCache
{
    public string path;
    internal Action<AssetCache> finishLoadCallback;
    internal UnityEngine.Object asset;
    private int refCount;

    internal int RefCount
    {
        get => refCount;
        set
        {
            //Logx.Logz("asset : change ref : " + path + " : " + refCount + " -> " + value);
            refCount = value;
        }

    }
}

public class AssetManager : Singleton<AssetManager>
{

    public Dictionary<string, AssetCache> assetCacheDic = new Dictionary<string, AssetCache>();
    public Dictionary<string, string> assetToAbDic = new Dictionary<string, string>();

    public void Init()
    {

        //Logx.Logzxy("Asset", "init");

        //读取 asset 和 ab 对应关系表
        var assetFileStr = File.ReadAllText(Const.AppStreamingAssetPath + "/" + "AssetToAbFileData.json");
        this.assetToAbDic = JsonMapper.ToObject<Dictionary<string, string>>(assetFileStr);

        //EventManager.AddListener<AssetCache>((int)GameEvent.LoadAssetTaskFinish, this.OnLoadAssetFinish);

    }


    public string GetABPathByAssetPath(string assetPath)
    {
        string abPath = "";
        if (!assetToAbDic.TryGetValue(assetPath, out abPath))
        {
            Logx.LogWarning("AssetManager", "the abPath is not found by assetPath : " + assetPath);
        }
        return abPath;
    }

    public void Load(string assetPath, Action<UnityEngine.Object> finishCallback, bool isSync = false)
    {
        if (isSync)
        {
            //LoadSync(assetPath, finishCallback);
        }
        else
        {
            LoadAsync(assetPath, finishCallback);
        }
    }
    //--------------------------------
    public void LoadAsync(string assetPath, Action<UnityEngine.Object> finishCallback)
    {
        assetPath = assetPath.ToLower();
        if (Const.isUseAB && !this.assetToAbDic.ContainsKey(assetPath))
        {
            Logx.LogError("Asset", "LoadAsync : the asset doesnt exist in assetToAbDic : " + assetPath);
            return;
        }

        //判断缓存
        AssetCache assetCache = null;
        if (assetCacheDic.TryGetValue(assetPath, out assetCache))
        {
            assetCache.RefCount += 1;

            if (Const.isUseAB)
            {
                AddAssetBundleReferenceByAssetPath(assetPath);
            }

            finishCallback?.Invoke(assetCache.asset);
        }
        else
        {
            if (Const.isUseAB)
            {
                var abPath = this.assetToAbDic[assetPath];
                AssetBundleManager.Instance.Load(abPath, (abCache) =>
                {
                    //ab 加载完 需要检测下 asset 是否已经有了 因为有可能已经在别处先加载完
                    this.LoadAssetBundleFinish(abCache, assetPath, finishCallback);
                }, false);
            }
            else
            {
#if UNITY_EDITOR
                //var obj =UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                this.LoadAssetBundleFinish(null, assetPath, finishCallback);
#endif 

            }


        }

    }

    public void AddAssetBundleReferenceByAssetPath(string assetPath)
    {
        string abPath = "";
        if (!assetToAbDic.TryGetValue(assetPath, out abPath))
        {
            Logx.LogWarning("Asset", "AddAssetBundleReferenceByAssetPath : the abPath is not found by assetPath : " + assetPath);
            return;
        }
        AssetBundleManager.Instance.AddAssetBundleReference(abPath);
    }

    public void LoadAssetBundleFinish(AssetBundleCache abCache, string assetPath, Action<UnityEngine.Object> finishCallback)
    {
        AssetCache assetCache = null;
        if (assetCacheDic.TryGetValue(assetPath, out assetCache))
        {
            finishCallback?.Invoke(assetCache.asset);
            assetCache.RefCount += 1;
            if (Const.isUseAB)
            {
                AddAssetBundleReferenceByAssetPath(assetPath);
            }

        }
        else
        {
            if (Const.isUseAB)
            {
                var loader = new AssetLoader();
                loader.path = assetPath;
                loader.finishLoadCallback = (backAssetCache) =>
                {
                    //触发业务层回调
                    finishCallback?.Invoke(backAssetCache.asset);
                };
                LoadTaskManager.Instance.StartAssetLoader(loader);
            }
            else
            {
#if UNITY_EDITOR
                var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                AssetCache ac = new AssetCache()
                {
                    asset = obj,
                    finishLoadCallback = (backAssetCache) =>
                    {
                        //触发业务层回调
                        finishCallback?.Invoke(backAssetCache.asset);
                    },
                    path = assetPath
                };
                OnLoadAssetFinish(ac);


#endif
            }

        }
    }

    //外界加载完后会调用
    internal void OnLoadAssetFinish(AssetCache assetCache)
    {
        //加载完成 放到缓存中
        AssetCache currAssetCache = null;
        if (!assetCacheDic.TryGetValue(assetCache.path, out currAssetCache))
        {
            assetCacheDic.Add(assetCache.path, assetCache);
            //新的 cache
            currAssetCache = assetCacheDic[assetCache.path];
        }
        else
        {
            currAssetCache.RefCount += assetCache.RefCount;

            for (int i = 0; i < assetCache.RefCount; i++)
            {
                AddAssetBundleReferenceByAssetPath(currAssetCache.path);
            }
        }

        var callback = assetCache.finishLoadCallback;
        //会触发 LoadAssetBundleFinish 中的业务回调
        callback?.Invoke(currAssetCache);
    }

    //采用如下计数方式：
    //asset 计算自己的 并会改变 ab 计数
    //也就是说 多个 asset 的总量 等于 对应的 ab 计数
    //实际上只有 ab 计数也可以 但是为了之后 资源分析 等 在 asset 层也做一个计数
    public void Release(string assetPath)
    {
        AssetCache cache = null;
        if (!assetCacheDic.TryGetValue(assetPath, out cache))
        {
            Logx.LogWarning("AssetManager", "Release : the cache doesnt exist : " + assetPath);
            return;
        }

        if (cache.RefCount <= 0)
        {
            Logx.LogWarning("AssetManager", "Release : the refCount of cache is 0 : " + assetPath);
            return;
        }

        cache.RefCount -= 1;
        //if (0 == cache.RefCount)
        //{
        //    string abPath = "";
        //    if (!assetToAbDic.TryGetValue(assetPath, out abPath))
        //    {
        //        Logx.LogWarning("AssetManager", "Release : the abPath is not found by assetPath : " + assetPath);
        //        return;
        //    }

        //    AssetBundleManager.Instance.ReduceAssetBundleReference(abPath);
        //}

        string abPath = "";
        if (!assetToAbDic.TryGetValue(assetPath, out abPath))
        {
            Logx.LogWarning("AssetManager", "Release : the abPath is not found by assetPath : " + assetPath);
            return;
        }

        AssetBundleManager.Instance.ReduceAssetBundleReference(abPath);


    }

    //void Release()
    //{
    //    //EventManager.RemoveListener<AssetCache>((int)GameEvent.LoadAssetTaskFinish, this.OnLoadAssetFinish);
    //}


}
