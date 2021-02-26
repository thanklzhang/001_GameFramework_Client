
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
        
        EventManager.AddListener<AssetCache>((int)GameEvent.LoadAssetTaskFinish, this.OnLoadAssetFinish);
        
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
        if (IsExistCache())
        {
            //如果有缓存 则直接拿走
        }
        else
        {
            if (IsLoading())
            {
                //在加载中的话 则附加 callback 等待加载完成触发
            }
            else
            {
                //哪都没有 开始加载 AB 
                LoadAssetBundle("", null, false);
            }
        }

    }

    public bool IsExistCache()
    {
        return false;
    }

    public bool IsLoading()
    {
        return false;
    }

    public void LoadAssetBundle(string path, Action<AssetBundleCache> finishCallback, bool isSync)
    {

        //调用 AssetBundle 加载
        AssetBundleManager.Instance.Load(path, finishCallback, isSync);
    }

    
    //public void OnLoadAssetFinish()
    //{
    //    //加载完成 放到缓存中

    //    //触发业务回调
    //}

    internal void OnLoadAssetFinish(AssetCache assetCache)
    {
        //加载完成 放到缓存中

        //触发业务回调
    }

    void Release()
    {
        EventManager.RemoveListener<AssetCache>((int)GameEvent.LoadAssetTaskFinish, this.OnLoadAssetFinish);
    }


}
