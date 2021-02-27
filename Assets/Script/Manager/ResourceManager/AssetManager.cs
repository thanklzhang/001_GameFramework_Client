
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
    internal int refCount;
}

public class AssetManager : Singleton<AssetManager>
{

    public Dictionary<string, AssetCache> assetCacheDic = new Dictionary<string, AssetCache>();
    public Dictionary<string, string> assetToAbDic = new Dictionary<string, string>();

    public void Init()
    {

        Logx.LogZxy("Asset", "init");

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
            Logx.LogWarningZxy("AssetManager", "the abPath is not found by assetPath : " + assetPath);
        }
        return abPath;
    }

    public void Load(string assetPath, Action<UnityEngine.Object> finishCallback, bool isSync)
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
        if (!this.assetToAbDic.ContainsKey(assetPath))
        {
            Logx.LogErrorZxy("Asset", "LoadAsync : the asset doesnt exist in assetToAbDic : " + assetPath);
            return;
        }

        //判断缓存
        AssetCache assetCache = null;
        if (assetCacheDic.TryGetValue(assetPath, out assetCache))
        {
            assetCache.refCount += 1;
            finishCallback?.Invoke(assetCache.asset);
        }
        else
        {
            var abPath = this.assetToAbDic[assetPath];
            AssetBundleManager.Instance.Load(abPath, (abCache) =>
            {
                //ab 加载完 需要检测下 asset 是否已经有了 因为有可能已经在别处先加载完
                this.LoadAssetBundleFinish(abCache, assetPath, finishCallback);
            }, false);

        }

    }

    public void LoadAssetBundleFinish(AssetBundleCache abCache, string assetPath, Action<UnityEngine.Object> finishCallback)
    {
        AssetCache assetCache = null;
        if (assetCacheDic.TryGetValue(assetPath, out assetCache))
        {
            finishCallback?.Invoke(assetCache.asset);
        }
        else
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
    }
    
    internal void OnLoadAssetFinish(AssetCache assetCache)
    {
        //加载完成 放到缓存中
        AssetCache currAssetCache = null;
        if (!assetCacheDic.TryGetValue(assetCache.path, out currAssetCache))
        {
            assetCacheDic.Add(assetCache.path, assetCache);
            currAssetCache = assetCacheDic[assetCache.path];
        }

        var callback = assetCache.finishLoadCallback;
        callback?.Invoke(currAssetCache);
    }

    void Release()
    {
        //EventManager.RemoveListener<AssetCache>((int)GameEvent.LoadAssetTaskFinish, this.OnLoadAssetFinish);
    }


}
