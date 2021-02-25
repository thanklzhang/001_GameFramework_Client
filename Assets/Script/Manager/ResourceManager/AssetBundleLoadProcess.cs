
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetBundleLoadProcess : BaseLoadProcess
{
    public Dictionary<string, AssetBundleLoader> abLoaderDic = new Dictionary<string, AssetBundleLoader>();

    public override void AddLoader(BaseLoader loader)
    {
        var assetBundleLoader = (AssetBundleLoader)loader;

        //从缓存池中找到是否有相同 path 如果有则附加 finishCallback
        //没有的话加入到缓存池中


        base.AddLoader(loader);
    }

    public override void OnLoadFinish(BaseLoader loader)
    {
        var assetBundleLoader = (AssetBundleLoader)loader;
        //通知 正在准备的所有 loader 
        for (int i = 0; i < this.preparingList.Count; i++)
        {
            var prepareLoader = (AssetBundleLoader)this.preparingList[i];
            prepareLoader.OnLoadOtherABFinish(prepareLoader.path);
        }
       

        base.OnLoadFinish(loader);
    }
}
