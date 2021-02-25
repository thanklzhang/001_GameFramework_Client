
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using LitJson;

//public class AssetRequest
//{
//    public string path;
//    //public int sameRequestCount;
//    public List<Action<AssetRequest>> finishCallbacks = new List<Action<AssetRequest>>();
//    public UnityEngine.AssetBundleRequest finishProgress;
//    //public AssetBundleInfo assetBundleInfo;


//    public void DependFinishCallback(Action<AssetRequest> callback)
//    {
//        finishCallbacks.Add(callback);
//    }

//    public bool IsLoadFinish()
//    {
//        if (finishProgress.isDone)
//        {
//            return true;
//        }

//        return false;
//    }

//    internal void Update(float deltaTime)
//    {

//    }

//    internal void StartLoad(UnityEngine.AssetBundleRequest createAsset)
//    {
//        this.finishProgress = createAsset;
//    }

//    public bool isCanUse;
//    internal UnityEngine.Object assetObj;
//    internal AssetBundleRequest assetBundleReq;

//    private int refCount;

//    public int RefCount
//    {
//        get => refCount;
//        set
//        {
//            Logx.LogZxy("Asset", "change ref count : " + path + " : " + refCount + " => " + value);
//            refCount = value;
//        }
//    }

//    public bool isWillRelease;
//    public void Release()
//    {
//        isWillRelease = true;
//    }

//    public void Unload()
//    {
//        Resources.UnloadAsset(this.assetObj);
//        this.assetObj = null;
//    }

//    //-----------------------
//    //public class WaitABRequest
//    //{
//    //    public string path;
//    //    public List<Action<AssetRequest>> waitAbReqCallbacks = new List<Action<AssetRequest>>();

//    //    internal void DependWaitAbReq(Action<AssetRequest> finishCallback)
//    //    {
//    //        waitAbReqCallbacks.Add(finishCallback);
//    //    }
//    //}

//    //-----------------------

//    //public class AssetRequest
//    //{
//    //    public string path;
//    //    public object assetObj;
//    //    public int refCount;
//    //    public int RefCount
//    //    {
//    //        get
//    //        {
//    //            return refCount;
//    //        }

//    //        set
//    //        {
//    //            Logx.LogZxy("Asset", "change ref " + refCount + " -> " + value);
//    //            refCount = value;

//    //        }
//    //    }
//    //}
//}
//public class AssetManager : Singleton<AssetManager>
//{
//    //public List<WaitABRequest> waitABLoadingReqList = new List<WaitABRequest>();

//    public List<AssetRequest> waitLoadingReqs = new List<AssetRequest>();
//    public List<AssetRequest> onLoadingReqs = new List<AssetRequest>();

//    public Dictionary<string, AssetRequest> assetReqCacheDic = new Dictionary<string, AssetRequest>();

//    public Dictionary<string, string> assetToAbDic = new Dictionary<string, string>();


//    List<AssetRequest> willReleseAssetReqList = new List<AssetRequest>();
//    public void Init()
//    {

//        Logx.LogZxy("Asset", "init");
//        var assetFileStr = File.ReadAllText(Const.AppStreamingAssetPath + "/" + "AssetToAbFileData.json");
//        this.assetToAbDic = JsonMapper.ToObject<Dictionary<string, string>>(assetFileStr);

//    }
//    public void Load(string assetPath, Action<AssetRequest> finishCallback, bool isSync)
//    {
//        if (isSync)
//        {
//            //LoadSync(assetPath, finishCallback);
//        }
//        else
//        {
//            LoadAsync(assetPath, finishCallback);
//        }
//    }
//    //--------------------------------
//    public void LoadAsync(string assetPath, Action<AssetRequest> finishCallback)
//    {
//        if (!this.assetToAbDic.ContainsKey(assetPath))
//        {
//            Logx.LogErrorZxy("Asset", "LoadAsync : the asset doesnt exist in assetToAbDic : " + assetPath);
//            return;
//        }
//        if (this.assetReqCacheDic.ContainsKey(assetPath))
//        {
//            //已经有 asset 了 直接用
//            var assetReq = assetReqCacheDic[assetPath];

//            if (assetReq.isCanUse)
//            {
//                //已经加载完成 直接使用
//                Logx.LogZxy("Asset", "asset can use directly : " + assetPath);
//                this.OnLoadFinish(assetReq, assetReq.assetObj);
//            }
//            else
//            {
//                //正在加载 ab 
//                Logx.LogZxy("Asset", "have asset req cache , it is loading ab " + assetPath);
//                //var req = new AssetRequest();
//                //req.path = assetPath;
//                //req.finishCallbacks.Add(finishCallback);
//                //req.sameRequestCount = 1;
//                //this.OnLoadFinish(req, assetReq.assetObj);
//                assetReq.DependFinishCallback(finishCallback);
//            }

//        }
//        else
//        {
//            //没有 asset 开始尝试加载 ab
//            Logx.LogZxy("Asset", "LoadAsync : no asset cache , start to load ab : " + assetPath);
//            //没有在加载相关 abB, 开始加载
//            var abPath = this.assetToAbDic[assetPath];

//            AssetRequest assetReq = new AssetRequest();
//            assetReq.path = assetPath;
//            assetReq.DependFinishCallback(finishCallback);

//            this.assetReqCacheDic.Add(assetPath, assetReq);


//            //WaitABRequest waitABReq = new WaitABRequest();
//            //waitABReq.path = assetPath;
//            //waitABReq.DependWaitAbReq(finishCallback);


//            //this.waitABLoadingReqList.Add(waitABReq);//assetPath,
//            //这里回调可以优化 拆出去
//            AssetBundleManager.Instance.Load(abPath, (abReq) =>
//            {
//                //Logx.LogZxy("Asset", "finish to load ab : " + assetPath);
//                //var callbacks = waitABReq.waitAbReqCallbacks;
//                //for (int i = 0; i < callbacks.Count; i++)
//                //{
//                //    var currCallback = callbacks[i];
//                //    this.OnLoadABFinishAsync(abReq, assetPath, currCallback);
//                //}

//                this.OnLoadABFinishAsync(abReq, assetReq);

//                //this.waitABLoadingReqList.Remove(waitABReq);//assetPath

//            }, false, null);
//        }
//    }

//    //-------------------------------------
//    public void OnLoadABFinishAsync(AssetBundleRequest abReq, AssetRequest assetReq)
//    {
//        Logx.LogZxy("Asset", "OnLoadABFinishAsync : " + abReq.path + " " + assetReq.path);
//        assetReq.assetBundleReq = abReq;
//        waitLoadingReqs.Add(assetReq);

//        //var waitLoadingReq = this.waitLoadingReqs.Find((assetReq) => assetReq.path == assetPath);
//        //if (waitLoadingReq != null)
//        //{
//        //    Logx.LogZxy("Asset", "in waitLoadingAsset queue : " + assetPath);
//        //    //在 等待加载队列中
//        //    waitLoadingReq.DependFinishCallback(finishCallback);
//        //}
//        //else
//        //{
//        //    var onLoadingReq = this.onLoadingReqs.Find((assetReq) => assetReq.path == assetPath);
//        //    if (onLoadingReq != null)
//        //    {
//        //        Logx.LogZxy("Asset", "in onLoadingAsset queue : " + assetPath);
//        //        //在 正在加载队列中
//        //        onLoadingReq.DependFinishCallback(finishCallback);
//        //    }
//        //    else
//        //    {
//        //        Logx.LogZxy("Asset", "dont have anyWhere , add to waitLoadingAsset queue : " + assetPath);
//        //        //哪都没有 放到等待加载队列中
//        //        var newReq = new AssetRequest();
//        //        newReq.path = assetPath;
//        //        newReq.sameRequestCount = 1;
//        //        newReq.DependFinishCallback(finishCallback);
//        //        newReq.assetBundleInfo = abInfo;

//        //        waitLoadingReqs.Add(newReq);
//        //    }
//        //}
//    }

//    public void OnLoadFinish(AssetRequest finishReq, UnityEngine.Object asset)
//    {
//        //Logx.LogZxy("Asset", "OnLoad asset finish : " + finishReq.path);
//        //AssetRequest assetReq = null;
//        //if (this.assetReqCacheDic.ContainsKey(finishReq.path))
//        //{
//        //    assetReq = this.assetReqCacheDic[finishReq.path];
//        //}
//        //else
//        //{
//        //    assetReq = new AssetRequest();
//        //    assetReq.path = finishReq.path;
//        //    assetReq.assetObj = asset;
//        //    this.assetReqCacheDic.Add(assetReq.path, assetReq);
//        //}


//        //assetReq.RefCount += finishReq.sameRequestCount;
//        //assetInfo.assetObj = finishReq.finishProgress.asset;
//        Logx.LogZxy("Asset", "OnLoadFinish : " + finishReq.path);
//        finishReq.assetObj = asset;
//        var callbacks = finishReq.finishCallbacks;
//        if (null != asset)
//        {
//            finishReq.isCanUse = true;
//        }
//        else
//        {
//            Logx.LogWarningZxy("Asset", "perharps the asset has been released : " + finishReq.path);
//        }
//        for (int i = 0; i < callbacks.Count; i++)
//        {
//            if (null != asset)
//            {
//                finishReq.RefCount += 1;
//            }
//            var callback = callbacks[i];
//            callback?.Invoke(finishReq);
//        }
//    }

//    public void Update(float deltaTime)
//    {
//        //update
//        List<AssetRequest> finishReqs = new List<AssetRequest>();
//        for (int i = 0; i < this.onLoadingReqs.Count; i++)
//        {
//            var onLoadingReq = this.onLoadingReqs[i];
//            onLoadingReq.Update(deltaTime);
//            //此处可优化 比如用事件回调之类的
//            if (onLoadingReq.IsLoadFinish())
//            {
//                finishReqs.Add(onLoadingReq);
//            }
//        }

//        //移除 finish load 的请求
//        for (int i = 0; i < finishReqs.Count; i++)
//        {
//            var finishReq = finishReqs[i];
//            this.onLoadingReqs.Remove(finishReq);
//        }


//        //触发完成操作
//        for (int i = 0; i < finishReqs.Count; i++)
//        {
//            var finishReq = finishReqs[i];
//            this.OnLoadFinish(finishReq, finishReq.finishProgress.asset);
//        }

//        //判断同时加载的任务 看是否有坑
//        int maxLoadTaskCount = 3;
//        int surplusCount = maxLoadTaskCount - this.onLoadingReqs.Count;
//        if (surplusCount > 0)
//        {
//            //有坑 开始填加载坑

//            for (int i = 0; i < surplusCount; i++)
//            {
//                if (this.waitLoadingReqs.Count >= 1)
//                {

//                    var waitTask = this.waitLoadingReqs[0];
//                    //Logx.LogZxy("Asset", "have work bench , add to onLoadingAsset queue : " + waitTask.path);
//                    this.onLoadingReqs.Add(waitTask);
//                    TrueLoadAsync(waitTask);
//                    this.waitLoadingReqs.RemoveAt(0);
//                }
//                else
//                {
//                    break;
//                }
//            }
//        }
//        this.CheckRelease();
//        willReleseAssetReqList.Clear();

//    }

//    void TrueLoadAsync(AssetRequest req)
//    {
//        Logx.LogZxy("Asset", "TrueLoadAsync " + req.path);
//        var ab = req.assetBundleReq.assetBundle;
//        var createAsset = ab.LoadAssetAsync(req.path);

//        req.StartLoad(createAsset);
//    }

//    void CheckRelease()
//    {
//        for (int i = this.willReleseAssetReqList.Count - 1; i >= 0; i--)
//        {
//            var willReleaseAssetReq = this.willReleseAssetReqList[i];
//            willReleaseAssetReq.Unload();
//            this.assetReqCacheDic.Remove(willReleaseAssetReq.path);
//        }
//    }

//    public void Release(string path)
//    {
//        Logx.LogZxy("Asset", "Relase : start to release : " + path);
//        if (this.assetReqCacheDic.ContainsKey(path))
//        {
//            var assetReq = this.assetReqCacheDic[path];
//            assetReq.RefCount -= 1;
//            if (0 == assetReq.RefCount)
//            {
//                assetReq.Release();
//                //willReleseAssetReqList.Add(assetReq);

//                //this.assetReqCacheDic.Remove(path);
//                //Logx.LogZxy("Asset", assetReq.assetObj);
//                //if (assetReq.assetObj.GetType() != typeof(GameObject) &&
//                //    assetReq.assetObj.GetType() != typeof(Component))
//                //{
//                //    Resources.UnloadAsset(assetReq.assetObj);
//                //}

//                //assetReq.assetObj = null;

//                //检查同 ab 中其他的 asset 是否存在有正在使用的(ref > 0)
//                //目前先按照 1 个 ab 1 个 asset 策略
//                //if (assetToAbDic.ContainsKey(path))
//                //{
//                //    var abPath = assetToAbDic[path];
//                //    AssetBundleManager.Instance.Release(abPath);
//                //}
//                //else
//                //{
//                //    Logx.LogErrorZxy("Asset", "Relase : the assetPath is not in assetToAbDic : " + path);
//                //}
//            }
//            if (assetReq.RefCount < 0)
//            {
//                Logx.LogWarningZxy("Asset", "the assetReq.RefCount is less than 0");
//            }
//        }
//        else
//        {
//            Logx.LogWarningZxy("Asset", "Relase : the path is not found : " + path);
//        }
//    }

//}
