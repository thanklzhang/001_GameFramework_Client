
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
            Debug.Log("zxy : asset : asset : change ref " + refCount + " -> " + value);
            refCount = value;

        }
    }
}

public class AssetManager : Singleton<AssetManager>
{
    public Dictionary<string, WaitABRequest> waitABLoadingReqDic = new Dictionary<string, WaitABRequest>();

    public List<AssetRequest> waitLoadingReqs = new List<AssetRequest>();
    public List<AssetRequest> onLoadingReqs = new List<AssetRequest>();

    public Dictionary<string, AssetInfo> assetCacheDic = new Dictionary<string, AssetInfo>();

    public Dictionary<string, string> assetToAbDic = new Dictionary<string, string>();

    public void Init()
    {

        Debug.Log("zxy : asset : init");
        var assetFileStr = File.ReadAllText(Const.AppStreamingAssetPath + "/" + "AssetToAbFileData.json");
        this.assetToAbDic = JsonMapper.ToObject<Dictionary<string, string>>(assetFileStr);

    }

    public void Load(string assetPath, Action<AssetInfo> finishCallback, bool isSync)
    {
        if (!this.assetCacheDic.ContainsKey(assetPath))
        {
            //没有 asset 开始尝试加载
            Debug.Log("zxy : asset : no asset cache : " + assetPath);
            //asset -> ab 对应表
            if (this.assetToAbDic.ContainsKey(assetPath))
            {
               
                var abPath = this.assetToAbDic[assetPath];
                if (this.waitABLoadingReqDic.ContainsKey(assetPath))
                {
                    Debug.Log("zxy : asset : in loading ab , add to wait ab : " + assetPath);
                    //正在处在加载相关 ab 中 
                    var waitABReq = this.waitABLoadingReqDic[assetPath];
                    waitABReq.DependWaitAbReq(finishCallback);
                }
                else
                {
                    Debug.Log("zxy : asset : no loading ab , start to load" + assetPath);
                    //没有在加载相关 abB, 开始加载
                    WaitABRequest waitABReq = new WaitABRequest();
                    waitABReq.path = assetPath;
                    waitABReq.DependWaitAbReq(finishCallback);
                    this.waitABLoadingReqDic.Add(assetPath, waitABReq);
                    AssetBundleManager.Instance.Load(abPath, (abInfo) =>
                     {
                         Debug.Log("zxy : asset : finish to load ab : " + assetPath);
                         var callbacks = waitABReq.waitAbReqCallbacks;
                         for (int i = 0; i < callbacks.Count; i++)
                         {
                             var currCallback = callbacks[i];
                             this.OnLoadABFinish(abInfo,assetPath, currCallback);
                         }
                       
                         this.waitABLoadingReqDic.Remove(assetPath);

                     }, isSync);
                }
            }
            else
            {
                Debug.LogError("zxy : asset : the asset doesnt exist in assetToAbDic : " + assetPath);
            }
        }
        else
        {
            Debug.Log("zxy : asset : asset exists : can use directly : " + assetPath);
            //已经有 asset 了 直接用
            var req = new AssetRequest();
            req.path = assetPath;
            req.finishCallbacks.Add(finishCallback);
            req.sameRequestCount = 1;
            this.OnLoadFinish(req);
        }

    }

    //-------------------------------------
    public void OnLoadABFinish(AssetBundleInfo abInfo, string assetPath, Action<AssetInfo> finishCallback)
    {
        var waitLoadingReq = this.waitLoadingReqs.Find((assetReq) => assetReq.path == assetPath);
        if (waitLoadingReq != null)
        {
            Debug.Log("zxy : asset : in waitLoadingAsset queue : " + assetPath);
            //在 等待加载队列中
            waitLoadingReq.DependReq(finishCallback);
        }
        else
        {
            var onLoadingReq = this.onLoadingReqs.Find((assetReq) => assetReq.path == assetPath);
            if (onLoadingReq != null)
            {
                Debug.Log("zxy : asset : in onLoadingAsset queue : " + assetPath);
                //在 正在加载队列中
                onLoadingReq.DependReq(finishCallback);
            }
            else
            {
                Debug.Log("zxy : asset : dont have anyWhere , add to waitLoadingAsset queue : " + assetPath);
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
            this.OnLoadFinish(finishReq);
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
                    Debug.Log("zxy : asset : have work bench , add to onLoadingAsset queue : " + waitTask.path);
                    this.onLoadingReqs.Add(waitTask);
                    TrueLoad(waitTask);
                    this.waitLoadingReqs.RemoveAt(0);
                }
                else
                {
                    break;
                }

            }
        }
    }


    void TrueLoad(AssetRequest req)
    {
        Debug.Log("zxy : asset : start to load asset : " + req.path);
        var ab = req.assetBundleInfo.assetBundle;
        var createAsset = ab.LoadAssetAsync(req.path); 
      
        req.StartLoad(createAsset);
    }


    public void OnLoadFinish(AssetRequest finishReq)
    {
        Debug.Log("zxy : asset : OnLoad asset finish : " + finishReq.path);
        var assetInfo = new AssetInfo();
        assetInfo.path = finishReq.path;
        assetInfo.RefCount += finishReq.sameRequestCount;
        assetInfo.assetObj = finishReq.finishProgress.asset;
        this.assetCacheDic.Add(assetInfo.path, assetInfo);
        var callbacks = finishReq.finishCallbacks;
        for (int i = 0; i < callbacks.Count; i++)
        {
            var callback = callbacks[i];
            callback?.Invoke(assetInfo);
        }
    }

    public void Relase(string path)
    {
        Debug.Log("zxy : asset : Relase : start to release : " + path);
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
                    Debug.LogError("zxy : asset : Relase : the assetPath is not in assetToAbDic : " + path);
                }
               



            }
        }
        else
        {
            Debug.LogWarning("zxy : asset : Relase : the path is not found : " + path);
        }
    }


}
