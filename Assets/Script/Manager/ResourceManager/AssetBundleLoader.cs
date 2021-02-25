
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetBundleLoader : BaseLoader
{
    public string path;
    public Action<AssetBundleCache> finishLoadCallback;

    public AssetBundle assetBundle;

    //加载中相同 ab 的请求引用数量
    public int refCount;

    public List<string> deps;

    AssetBundleCreateRequest abCreateReq;

    public override void OnStart()
    {
        //填充 deps

        refCount += 1;
    }

    public override void OnPrepareFinish()
    {

    }

    internal override void OnLoadFinish()
    {
        //ab 加载完成
        AssetBundleCache abCache = new AssetBundleCache();
        abCache.path = path;
        abCache.finishLoadCallback = finishLoadCallback;
        //abCache.ab = ab
        //finishLoadCallback?.Invoke(abCache);

        AssetBundleManager.Instance.OnLoadFinish(abCache);

    }

    internal void OnLoadOtherABFinish(string path)
    {
        //判断是否是当前 ab 所依赖的 ab
        //是的话 deps 移除该路径 相当于完成了一个加载依赖
    }

    public override bool IsPrepareFinish()
    {
        return 0 == deps.Count;
    }

    public override bool IsLoadFinish()
    {
        return abCreateReq.progress >= 1;
    }
}
