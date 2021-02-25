
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetLoader : BaseLoader
{
    public string path;
    public Action<AssetCache> finishLoadCallback;

    public UnityEngine.Object asset;

    //加载中相同 ab 的请求引用数量
    public int refCount;

    public override void OnStart()
    {
        //加载 AB 
        refCount += 1;
    }

    internal override void OnLoadFinish()
    {
        //asset 加载完成
        AssetCache assetCache = new AssetCache();
        assetCache.path = path;
        assetCache.finishLoadCallback = finishLoadCallback;
        //abCache.ab = ab
        //finishLoadCallback?.Invoke(abCache);

        AssetManager.Instance.OnLoadAssetFinish(assetCache);

    }
}
