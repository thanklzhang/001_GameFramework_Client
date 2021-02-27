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
    public int refCount;
}

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    //assetBundle 的缓存
    Dictionary<string, AssetBundleCache> abCacheDic = new Dictionary<string, AssetBundleCache>();

    private AssetBundleManifest manifest;

    public void Init()
    {
        Logx.LogZxy("AB", "abLog : init");
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

        Logx.LogZxy("AB", "LoadAsync : start load : " + path);

        var abCache = GetCacheByPath(path);
        if (abCache != null)
        {
            //Logx.Logz("LoadAsync : have ab cache");
            //判断是否在 ab 缓存中 
            //有的话直接拿走
            abCache.refCount += 1;
            finishCallback?.Invoke(abCache);
        }
        else
        {
            //Logx.Logz("LoadAsync : no ab cache");
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

        //加载任务交给加载管理器去执行
        LoadTaskManager.Instance.StartAssetBundleLoader(loader);
        Logx.Logz("AssetBundleManager : LoadTrueAssetBundle : start LoadTrueAssetBundle : " + path);

    }

    //有 AB 加载完成
    public void OnLoadFinish(AssetBundleCache ab)
    {
        Logx.Logz("AssetBundleManager : OnLoadFinish : " + ab.path);
        //判断缓存中是否有 没有的话添加到缓存中
        AssetBundleCache abCache = null;
        if (!abCacheDic.TryGetValue(ab.path, out abCache))
        {
            abCacheDic.Add(ab.path, ab);
            abCache = abCacheDic[ab.path];
        }

        abCache.refCount += ab.refCount;

        //这个回调是加载开始的时候的时候传的回调 可以认为是相对的业务层传来的回调
        var finishCallback = abCache.finishLoadCallback;
        finishCallback?.Invoke(abCache);
    }

    public void Release()
    {
        //EventManager.RemoveListener<AssetBundleCache>((int)GameEvent.LoadABTaskFinish, this.OnLoadFinish);
    }
}
