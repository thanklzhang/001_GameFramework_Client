
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;

public class AssetLoader : BaseLoader
{
    public string path;
    public Action<AssetCache> finishLoadCallback;

    public UnityEngine.Object asset;

    public AssetBundleRequest assetRequest;

    string abPath;

    //加载中相同 ab 的请求引用数量
    public int refCount;

    AssetBundle assetBundle;

    public override void OnStart()
    {
        //加载 AB 
        refCount += 1;

        abPath = AssetManager.Instance.GetABPathByAssetPath(path);
    }

    public override void OnPrepareFinish()
    {
        var assetBundleCache = AssetBundleManager.Instance.GetCacheByPath(abPath);
        if (null == assetBundleCache)
        {
            Logx.LogError("AssetLoader", "the assetBundleCache is null : " + abPath);
            return;
        }

        if (null == assetBundleCache.assetBundle)
        {
            Logx.LogError("AssetLoader", "the assetBundleCache.assetBundle is null : " + abPath);
            return;
        }
        var ab = assetBundleCache.assetBundle;
        assetRequest = ab.LoadAssetAsync(path);
    }

    internal override void OnLoadFinish()
    {
        //asset 加载完成
        AssetCache assetCache = new AssetCache();
        assetCache.path = path;
        assetCache.finishLoadCallback = finishLoadCallback;
        assetCache.RefCount = refCount;
        assetCache.asset = assetRequest.asset;

        //abCache.ab = ab
        //finishLoadCallback?.Invoke(abCache);

        AssetManager.Instance.OnLoadAssetFinish(assetCache);

    }

    public override bool IsPrepareFinish()
    {
        var assetBundle = AssetBundleManager.Instance.GetCacheByPath(abPath);
        return assetBundle != null;
    }

    public override bool IsLoadFinish()
    {
        if (null == assetRequest)
        {
            return false;
        }
        return assetRequest.progress >= 1;

    }
}
