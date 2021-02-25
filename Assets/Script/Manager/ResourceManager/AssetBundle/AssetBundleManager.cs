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
}

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    //assetBundle 的缓存
    Dictionary<string, AssetBundleCache> abCacheDic = new Dictionary<string, AssetBundleCache>();

    private AssetBundleManifest manifest;

    public void Load()
    {
        Logx.LogZxy("AB", "abLog : init");
        var manifestAB = AssetBundle.LoadFromFile(Const.AssetBundlePath + "/" + "StreamingAssets");
        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        EventManager.AddListener<AssetBundleCache>((int)GameEvent.LoadABTaskFinish, this.OnLoadFinish);
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

        //如果没有 判断是否在加载中(包括等待依赖 ab) 在的话 
        //哪都没有 那么就开始加载 ab

        if (this.IsExistCache())
        {
            //判断是否在 ab 缓存中 
            //有的话直接拿走
        }
        //else if (this.IsLoadingDeps())
        //{
        //    //判断是否在等待所有依赖 ab 加载完) 有的话 附加 callback   
        //}
        //else if (this.IsLoadingSelf())
        //{
        //    //判断是否在加载自身 ab 中 有的话 附加 callback  
        //}
        else
        {
            //////////哪都没有 开始加载 自身 ab
            //可以直接加载
            LoadTrueAssetBundle(path, finishCallback);
        }

    }

    public bool IsExistCache()
    {
        return false;
    }

    public bool IsLoadingDeps()
    {
        return false;
    }

    public bool IsLoadingSelf()
    {
        return false;
    }

    public void LoadTrueAssetBundle(string path, Action<AssetBundleCache> finishCallback)
    {
        var loader = new AssetBundleLoader();
        loader.path = path;
        loader.finishLoadCallback = finishCallback;

        //...
        LoadTaskManager.Instance.AddAssetBundleLoader(loader);
    }

    public void OnLoadFinish(AssetBundleCache ab)
    {
        //判断缓存中是否有 没有的话添加到缓存中

        //cache 中 ref += refCount
        //这个回调是调用的时候传的回调
        ab.finishLoadCallback?.Invoke(ab);

    }


    public void Release()
    {
        EventManager.RemoveListener<AssetBundleCache>((int)GameEvent.LoadABTaskFinish, this.OnLoadFinish);
    }
}
