using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetBundleCache
{
    public string path;
    internal Action<AssetBundleCache> finishLoadCallback;
    internal AssetBundle assetBundle;
    private int refCount;

    //目前这个引用计数只会在 从 assetManager 加载 asset 的流程开始的时候开始计数 
    //其他情况暂不考虑
    public int RefCount
    {
        get => refCount;
        set
        {
            //Logx.Logz("ab : refCount : " + path + " : " + refCount + " -> " + value);
            refCount = value;
        }

    }
}

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    //assetBundle 的缓存
    Dictionary<string, AssetBundleCache> abCacheDic = new Dictionary<string, AssetBundleCache>();

    private AssetBundleManifest manifest;

    public void Init()
    {
        //Logx.Logzxy("AB", "abLog : init");
        var manifestAB = AssetBundle.LoadFromFile(Const.AssetBundlePath + "/" + "StreamingAssets");
        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //EventManager.AddListener<AssetBundleCache>((int)GameEvent.LoadABTaskFinish, this.OnLoadFinish);
    }

    public string[] GetDependPaths(string path)
    {
        var deps = manifest.GetDirectDependencies(path);
        return deps;
    }


    public void Load(string path, Action<AssetBundleCache> finishCallback, bool isSync)
    {
        if (isSync)
        {
            //LoadSync(path, finishCallback, fromRequest);
        }
        else
        {
            LoadAsync(path, finishCallback);
        }
    }


    public void LoadAsync(string path, Action<AssetBundleCache> finishCallback)
    {

        //Logx.Logzxy("AB", "LoadAsync : start load : " + path);

        var abCache = GetCacheByPath(path);
        if (abCache != null)
        {
            ////Logx.Logz("LoadAsync : have ab cache");
            //判断是否在 ab 缓存中 
            //有的话直接拿走
            //abCache.RefCount += 1;
            //ChageRefCount(abCache, 1);
            finishCallback?.Invoke(abCache);
        }
        else
        {
            ////Logx.Logz("LoadAsync : no ab cache");
            //可能没在缓存中 可能正在加载中 也可能第一次加载
            LoadTrueAssetBundle(path, finishCallback);
        }

    }

    public AssetBundleCache GetCacheByPath(string path)
    {
        AssetBundleCache abCache = null;
        abCacheDic.TryGetValue(path, out abCache);
        return abCache;
    }

    public void LoadTrueAssetBundle(string path, Action<AssetBundleCache> finishCallback)
    {

        var loader = new AssetBundleLoader();
        loader.path = path;
        loader.finishLoadCallback = finishCallback;

        //依赖
        var deps = GetDependPaths(path).ToList();
        for (int i = 0; i < deps.Count; i++)
        {
            var depPath = deps[i];
            Load(depPath, null, false);
        }
        Logx.Log("LoadTaskManager.Instance.StartAssetBundleLoader , loader.path : " + loader.path);
        //加载任务交给加载管理器去执行
        //Logx.Logz("AssetBundleManager : LoadTrueAssetBundle : start LoadTrueAssetBundle : " + path);
        LoadTaskManager.Instance.StartAssetBundleLoader(loader);


    }

    //有 AB 加载完成
    public void OnLoadFinish(AssetBundleCache ab)
    {
        //Logx.Logz("AssetBundleManager : OnLoadFinish : " + ab.path);
        //判断缓存中是否有 没有的话添加到缓存中
        AssetBundleCache abCache = null;
        if (!abCacheDic.TryGetValue(ab.path, out abCache))
        {
            abCacheDic.Add(ab.path, ab);
            abCache = abCacheDic[ab.path];
        }
        else
        {
            //abCache.RefCount += ab.RefCount;
            //这里可能有问题 如果这里增加了引用计数 那么当 asset 加载好之后 ab 这边又会增加引用计数
            //ChageRefCount(abCache, ab.RefCount);
        }

        //这个回调是加载开始的时候的时候传的回调 可以认为是相对的业务层传来的回调
        var finishCallback = ab.finishLoadCallback;
        finishCallback?.Invoke(abCache);
    }

    public void Release()
    {
        //EventManager.RemoveListener<AssetBundleCache>((int)GameEvent.LoadABTaskFinish, this.OnLoadFinish);
    }

    public void AddAssetBundleReference(string abPath)
    {
        AssetBundleCache abCache = null;
        if (abCacheDic.TryGetValue(abPath, out abCache))
        {
            //abCache.RefCount += 1; 
            ChageRefCount(abCache, 1);
        }
        else
        {
            //Logx.Logz("the abPath is not found : " + abPath);
        }
    }

    internal void ReduceAssetBundleReference(string abPath)
    {
        AssetBundleCache abCache = null;
        if (abCacheDic.TryGetValue(abPath, out abCache))
        {
            if (abCache.RefCount <= 0)
            {
                Logx.LogWarning("AB", "the refCount of abCache is 0 : " + abPath);
                return;
            }
            //abCache.RefCount -= 1;
            ChageRefCount(abCache, -1);
        }
        else
        {
            //Logx.Logz("the abPath is not found : " + abPath);
        }

    }

    public void ChageRefCount(AssetBundleCache cache, int value)
    {
        cache.RefCount += value;
        var deps = this.GetDependPaths(cache.path);
        for (int i = 0; i < deps.Length; i++)
        {
            var depPath = deps[i];
            if (this.abCacheDic.ContainsKey(depPath))
            {
                var currCache = this.abCacheDic[depPath];
                ChageRefCount(currCache, value);
            }
            else
            {
                //Logx.Logz("the abCacheDic doesnt contain this depPath : path and depPath are : " + cache.path + " " + depPath);
            }
        }
    }

    public void TrueReleaseAB(string abPath)
    {
        AssetBundleCache abCache = null;
        if (abCacheDic.TryGetValue(abPath, out abCache))
        {
            if (abCache.RefCount > 0)
            {
                Logx.LogWarning("AB", "the refCount of abCache is using(refCount > 0) : " + abPath);
                return;
            }

            var ab = abCache.assetBundle;
            ab.Unload(true);
            this.abCacheDic.Remove(abCache.path);
        }
    }
}
