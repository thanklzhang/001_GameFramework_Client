
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class LoadTaskManager : Singleton<LoadTaskManager>
{
    public LoadTaskProcess assetLoadTask;
    public LoadTaskProcess abLoadTask;

    public void Init()
    {
        assetLoadTask.SetFinishCallback(OnLoadABFinish);
        abLoadTask.SetFinishCallback(OnLoadAssetFinish);

    }

    public void Update(float timeDelta)
    {
        assetLoadTask.Update(timeDelta);
        abLoadTask.Update(timeDelta);
    }

    public void OnLoadABFinish(BaseLoader loader)
    {
        AssetBundleLoader abLoader = (AssetBundleLoader)loader;

        AssetBundleCache cache = new AssetBundleCache();
        cache.path = abLoader.path;
        cache.assetBundle = abLoader.assetBundle;
        cache.finishLoadCallback = abLoader.finishLoadCallback;

        EventManager.Broadcast((int)GameEvent.LoadABTaskFinish, cache);
    }
    public void OnLoadAssetFinish(BaseLoader loader)
    {

        AssetLoader assetLoader = (AssetLoader)loader;

        AssetCache cache = new AssetCache();
        cache.path = assetLoader.path;
        cache.asset = assetLoader.asset;
        cache.finishLoadCallback = assetLoader.finishLoadCallback;

        EventManager.Broadcast((int)GameEvent.LoadAssetTaskFinish, cache);
    }


    public void AddAssetLoader(BaseLoader loader)
    {
        assetLoadTask.AddLoader(loader);
    }

    public void AddAssetBundleLoader(BaseLoader loader)
    {
        abLoadTask.AddLoader(loader);
    }


}
