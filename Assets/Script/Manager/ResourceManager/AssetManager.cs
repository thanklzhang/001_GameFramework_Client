
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetRequest
{
    public string path;
    public int sameRequestCount;
    public List<Action<AssetInfo>> finishCallbacks = new List<Action<AssetInfo>>();
    public UnityEngine.AssetBundleRequest finishProgress;
    public AssetBundleInfo assetBundleInfo;


    public void DependReq(Action<AssetInfo> callback)
    {
        finishCallbacks.Add(callback);
    }

    public bool IsLoadFinish()
    {
        if (finishProgress.isDone)
        {
            return true;
        }

        return false;
    }

    internal void Update(float deltaTime)
    {

    }

    internal void StartLoad(UnityEngine.AssetBundleRequest createAsset)
    {
        this.finishProgress = createAsset;
    }
}

//-----------------------
public class WaitABRequest
{
    public string path;
    public List<Action<AssetInfo>> waitAbReqCallbacks = new List<Action<AssetInfo>>();

    internal void DependWaitAbReq(Action<AssetInfo> finishCallback)
    {
        waitAbReqCallbacks.Add(finishCallback);
    }
}

//-----------------------

public class AssetInfo
{
    public string path;
    public object assetObj;
    public int refCount;
    public int RefCount
    {
        get
        {
            return refCount;
        }

        set
        {
            Logx.LogZxy("Asset", "change ref " + refCount + " -> " + value);
            refCount = value;

        }
    }
}

public class AssetManager : Singleton<AssetManager>
{
    public List<WaitABRequest> waitABLoadingReqList = new List<WaitABRequest>();

    public List<AssetRequest> waitLoadingReqs = new List<AssetRequest>();
    public List<AssetRequest> onLoadingReqs = new List<AssetRequest>();

    public Dictionary<string, AssetInfo> assetCacheDic = new Dictionary<string, AssetInfo>();

    public Dictionary<string, string> assetToAbDic = new Dictionary<string, string>();

    public void Init()
    {

        Logx.LogZxy("Asset", "init");
        var assetFileStr = File.ReadAllText(Const.AppStreamingAssetPath + "/" + "AssetToAbFileData.json");
        this.assetToAbDic = JsonMapper.ToObject<Dictionary<string, string>>(assetFileStr);

    }
    public void Load(string assetPath, Action<AssetInfo> finishCallback, bool isSync)
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
    public void LoadAsync(string assetPath, Action<AssetInfo> finishCallback)
    {
        if (!this.assetToAbDic.ContainsKey(assetPath))
        {
            Debug.LogError("LoadAsync : the asset doesnt exist in assetToAbDic : " + assetPath);
            return;
        }
        if (!this.assetCacheDic.ContainsKey(assetPath))
        {
            //没有 asset 开始尝试加载 ab
            Logx.LogZxy("Asset", "LoadAsync : no asset cache : " + assetPath);

            //没有在加载相关 abB, 开始加载
            var abPath = this.assetToAbDic[assetPath];
            WaitABRequest waitABReq = new WaitABRequest();
            waitABReq.path = assetPath;
            waitABReq.DependWaitAbReq(finishCallback);
            this.waitABLoadingReqList.Add(waitABReq);//assetPath,
                                                     //这里回调可以优化 拆出去
            AssetBundleManager.Instance.Load(abPath, (abInfo) =>
            {
                Logx.LogZxy("Asset", "finish to load ab : " + assetPath);
                var callbacks = waitABReq.waitAbReqCallbacks;
                for (int i = 0; i < callbacks.Count; i++)
                {
                    var currCallback = callbacks[i];
                    this.OnLoadABFinishAsync(abInfo, assetPath, currCallback);
                }

                this.waitABLoadingReqList.Remove(waitABReq);//assetPath

            }, false);

        }
        else
        {
            Logx.LogZxy("Asset", "asset exists : can use directly : " + assetPath);
            //已经有 asset 了 直接用
            var assetInfo = assetCacheDic[assetPath];
            var req = new AssetRequest();
            req.path = assetPath;
            req.finishCallbacks.Add(finishCallback);
            req.sameRequestCount = 1;
            this.OnLoadFinish(req, assetInfo.assetObj);
        }
    }

    //-------------------------------------
    public void OnLoadABFinishAsync(AssetBundleInfo abInfo, string assetPath, Action<AssetInfo> finishCallback)
    {
        var waitLoadingReq = this.waitLoadingReqs.Find((assetReq) => assetReq.path == assetPath);
        if (waitLoadingReq != null)
        {
            Logx.LogZxy("Asset", "in waitLoadingAsset queue : " + assetPath);
            //在 等待加载队列中
            waitLoadingReq.DependReq(finishCallback);
        }
        else
        {
            var onLoadingReq = this.onLoadingReqs.Find((assetReq) => assetReq.path == assetPath);
            if (onLoadingReq != null)
            {
                Logx.LogZxy("Asset", "in onLoadingAsset queue : " + assetPath);
                //在 正在加载队列中
                onLoadingReq.DependReq(finishCallback);
            }
            else
            {
                Logx.LogZxy("Asset", "dont have anyWhere , add to waitLoadingAsset queue : " + assetPath);
                //哪都没有 放到等待加载队列中
                var newReq = new AssetRequest();
                newReq.path = assetPath;
                newReq.sameRequestCount = 1;
                newReq.DependReq(finishCallback);
                newReq.assetBundleInfo = abInfo;

                waitLoadingReqs.Add(newReq);
            }
        }
    }

    public void OnLoadFinish(AssetRequest finishReq, object asset)
    {
        Logx.LogZxy("Asset", "OnLoad asset finish : " + finishReq.path);
        AssetInfo assetInfo = null;
        if (this.assetCacheDic.ContainsKey(finishReq.path))
        {
            assetInfo = this.assetCacheDic[finishReq.path];
        }
        else
        {
            assetInfo = new AssetInfo();
            assetInfo.path = finishReq.path;
            assetInfo.assetObj = asset;
            this.assetCacheDic.Add(assetInfo.path, assetInfo);
        }


        assetInfo.RefCount += finishReq.sameRequestCount;
        //assetInfo.assetObj = finishReq.finishProgress.asset;
        var callbacks = finishReq.finishCallbacks;
        for (int i = 0; i < callbacks.Count; i++)
        {
            var callback = callbacks[i];
            callback?.Invoke(assetInfo);
        }
    }

    public void Update(float deltaTime)
    {
        //update
        List<AssetRequest> finishReqs = new List<AssetRequest>();
        for (int i = 0; i < this.onLoadingReqs.Count; i++)
        {
            var onLoadingReq = this.onLoadingReqs[i];
            onLoadingReq.Update(deltaTime);
            //此处可优化 比如用事件回调之类的
            if (onLoadingReq.IsLoadFinish())
            {
                finishReqs.Add(onLoadingReq);
            }
        }

        //移除 finish load 的请求
        for (int i = 0; i < finishReqs.Count; i++)
        {
            var finishReq = finishReqs[i];
            this.onLoadingReqs.Remove(finishReq);
        }


        //触发完成操作
        for (int i = 0; i < finishReqs.Count; i++)
        {
            var finishReq = finishReqs[i];
            this.OnLoadFinish(finishReq, finishReq.finishProgress.asset);
        }

        //判断同时加载的任务 看是否有坑
        int maxLoadTaskCount = 3;
        int surplusCount = maxLoadTaskCount - this.onLoadingReqs.Count;
        if (surplusCount > 0)
        {
            //有坑 开始填加载坑

            for (int i = 0; i < surplusCount; i++)
            {
                if (this.waitLoadingReqs.Count >= 1)
                {

                    var waitTask = this.waitLoadingReqs[0];
                    Logx.LogZxy("Asset", "have work bench , add to onLoadingAsset queue : " + waitTask.path);
                    this.onLoadingReqs.Add(waitTask);
                    TrueLoadAsync(waitTask);
                    this.waitLoadingReqs.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
        }
    }

    void TrueLoadAsync(AssetRequest req)
    {
        Logx.LogZxy("Asset", "start to load " + req.path);
        var ab = req.assetBundleInfo.assetBundle;
        var createAsset = ab.LoadAssetAsync(req.path);

        req.StartLoad(createAsset);
    }

   

    public void Release(string path)
    {
        Logx.LogZxy("Asset", "Relase : start to release : " + path);
        if (this.assetCacheDic.ContainsKey(path))
        {
            var assetInfo = this.assetCacheDic[path];
            assetInfo.RefCount -= 1;
            if (0 == assetInfo.RefCount)
            {
                this.assetCacheDic.Remove(path);

                //检查同 ab 中其他的 asset 是否存在有正在使用的(ref > 0)
                //目前先按照 1 个 ab 1 个 asset 策略
                if (assetToAbDic.ContainsKey(path))
                {
                    var abPath = assetToAbDic[path];
                    AssetBundleManager.Instance.Release(abPath);
                }
                else
                {
                    Logx.LogZxyError("Asset", "Relase : the assetPath is not in assetToAbDic : " + path);
                }
            }
        }
        else
        {
            Logx.LogZxyWarning("Asset", "Relase : the path is not found : " + path);
        }
    }

}
