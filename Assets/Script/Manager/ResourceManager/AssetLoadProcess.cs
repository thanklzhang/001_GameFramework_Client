
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetLoadProcess : BaseLoadProcess
{
    public Dictionary<string, AssetLoader> assetLoaderDic = new Dictionary<string, AssetLoader>();

    public override void AddLoader(BaseLoader loader)
    {
        Logx.Log("asset load  AddLoader ");
        var assetLoader = (AssetLoader)loader;

        //从缓存池中找到是否有相同 path 如果有则附加 finishCallback
        //没有的话加入到缓存池中
        AssetLoader currAssetLoader = null;
        if (!assetLoaderDic.TryGetValue(assetLoader.path, out currAssetLoader))
        {
            Logx.Log("asset load  AddLoader 1");

            assetLoaderDic.Add(assetLoader.path, assetLoader);
            //Logx.Logz("AssetLoadProcess AddLoader : no loader cache and start a new loader : " + assetLoader.path);
            base.AddLoader(loader);
        }
        else
        {
            Logx.Log("asset load  AddLoader 2");
            //Logx.Logz("AssetLoadProcess AddLoader : have loader cache " + assetLoader.path);
            currAssetLoader.finishLoadCallback += assetLoader.finishLoadCallback;
            currAssetLoader.refCount += 1;
        }
    }

    public override void OnLoadFinish(BaseLoader loader)
    {
        Logx.Log("asset load  OnLoadFinish ");
        var assetLoader = (AssetLoader)loader;
        //Logx.Logz("AssetLoadProcess OnLoadFinish : " + assetLoader.path);

        ////通知 正在准备的所有 loader 在这里指的是正在等依赖加载的 loader
        //for (int i = 0; i < this.preparingList.Count; i++)
        //{
        //    var prepareLoader = (AssetLoader)this.preparingList[i];
        //    prepareLoader.OnLoadOtherABFinish(assetBundleLoader.path);
        //}

        assetLoaderDic.Remove(assetLoader.path);
        base.OnLoadFinish(loader);
    }
}
