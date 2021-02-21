using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    //assetBundle request 的缓存
    Dictionary<string, AssetBundleRequest> abReqCacheDic = new Dictionary<string, AssetBundleRequest>();

    List<AssetBundleRequest> willReleseABReq = new List<AssetBundleRequest>();

    //队列相关-------------
    //等待加载的 assetBundle 请求
    List<AssetBundleRequest> waitLoadABList = new List<AssetBundleRequest>();
    //正在加载中的 assetBundle 请求
    List<AssetBundleRequest> onLoadingABList = new List<AssetBundleRequest>();

    //同时加载的 ab 数量
    int maxLoadProcessCount = 3;
    private AssetBundleManifest manifest;

    public void Init()
    {
        Logx.LogZxy("AB", "abLog : init");
        var manifestAB = AssetBundle.LoadFromFile(Const.AssetBundlePath + "/" + "StreamingAssets");
        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

    }

    public string[] GetDependPaths(string path)
    {
        var deps = manifest.GetDirectDependencies(path);
        return deps;

    }

    public void Load(string path, Action<AssetBundleRequest> finishCallback, bool isSync, AssetBundleRequest fromRequest)//, AssetBundleRequest fromRequest
    {
        if (isSync)
        {
            LoadSync(path, finishCallback, fromRequest);
        }
        else
        {
            LoadAsync(path, finishCallback, fromRequest);
        }
    }


    public void LoadAsync(string path, Action<AssetBundleRequest> finishCallback, AssetBundleRequest formRequest)
    {

        Logx.LogZxy("AB", "LoadAsync : start load : " + path);

        //判断是否在 ab 请求 缓存中
        if (this.abReqCacheDic.ContainsKey(path))
        {
            var requestCache = this.abReqCacheDic[path];


            if (requestCache.isCanUseAB)
            {
                Logx.LogZxy("AB", "LoadAsync : the ab finish load , can use ");
                requestCache.DependFinishCallback(finishCallback);
                this.OnLoadFinish(requestCache, requestCache.assetBundle);
            }
            else
            {
                Logx.LogZxy("AB", "LoadAsync : the ab doesnt finish load , can not use , append");

                requestCache.DependRequestFrom(formRequest);
                requestCache.DependFinishCallback(finishCallback);
            }
        }
        else
        {
            //哪都没有 开始一个新的加载请求
            AssetBundleRequest request = null;
            request = new AssetBundleRequest();
            request.path = path;
            request.DependFinishCallback(finishCallback);
            request.DependRequestFrom(formRequest);


            abReqCacheDic.Add(path, request);

            //处理 ab 依赖
            var deps = GetDependPaths(path);
            if (deps != null && deps.Length > 0)
            {
                //有依赖
                request.AddLoadDepend(deps);
                for (int i = 0; i < deps.Length; ++i)
                {
                    var dependPath = deps[i];
                    //xxx.ab 取 xxx
                    var depPath = deps[i];//.Substring(0, index);
                    Logx.LogZxy("AB", "LoadAsync : load dep : " + path + " : dep : " + depPath);
                    Load(depPath, OnFinishLoadDependAsync, false, request);
                }
            }
            else
            {
                //没有依赖 本身 ab 进入待加载队列
                Logx.LogZxy("AB", "LoadAsync : dont have depends will load , add to waitQueue : " + path);
                this.waitLoadABList.Add(request);
            }
        }
    }


    public void LoadSync(string path, Action<AssetBundleRequest> finishCallback, AssetBundleRequest formRequest)
    {
        Logx.LogZxy("AB", "LoadSync : start load : " + path);

        //判断是否在 ab 请求 缓存中
        if (this.abReqCacheDic.ContainsKey(path))
        {
            var requestCache = this.abReqCacheDic[path];


            if (requestCache.isCanUseAB)
            {
                //ab 已经加载好了
                Logx.LogZxy("AB", "LoadSync : the ab finish load , can use : " + path);
                requestCache.DependFinishCallback(finishCallback);
                this.OnLoadFinish(requestCache, requestCache.assetBundle);
            }
            else
            {
                //ab 正在加载中
                if (!requestCache.isSyncLoad)
                {
                    Logx.LogZxyWarning("AB", "async to sync !!! not implement :" + path);
                    //异步转同步 此时要判断 等待加载队列 和 正在加载队列 和 正在等待依赖加载完的请求
                    //如果在的话 需要终止 并开始同步的 ab 加载 而且还要判断依赖 ab 的加载情况、

                    //没有意义 异步加载的时候为什么要同步加载？ 
                    //所以建议 都是同步 或 都是异步 或 在独立的没有交叉的两个时间段上加载

                    //如果真的有这种需求 自己弄吧
                }
                else
                {
                    //如果是同步 理论上就不会走到这里
                    Logx.LogZxyWarning("AB", "cant run hero , because it is sync");
                }
            }
        }
        else
        {
            //哪都没有 开始一个新的加载请求
            AssetBundleRequest request = null;
            request = new AssetBundleRequest();
            request.path = path;
            request.DependFinishCallback(finishCallback);
            request.DependRequestFrom(formRequest);


            abReqCacheDic.Add(path, request);

            //处理 ab 依赖
            var deps = GetDependPaths(path);
            if (deps != null && deps.Length > 0)
            {
                //有依赖
                request.AddLoadDepend(deps);
                for (int i = 0; i < deps.Length; ++i)
                {
                    var dependPath = deps[i];
                    var depPath = deps[i];
                    Logx.LogZxy("AB", "LoadSync : load dep : " + path + " : dep : " + depPath);
                    Load(depPath, OnFinishLoadDependSync, true, request);
                }
            }
            else
            {
                //没有依赖 直接加载本身 ab
                Logx.LogZxy("AB", "LoadSync : dont have depends will load , direct load : " + path);
                this.TrueLoadSync(request);
            }
        }
    }

    void ChangeRef(AssetBundleRequest abRequest, int value)
    {
        var preValue = abRequest.RefCount;

        if (abRequest.RefCount + value < 0)
        {
            Logx.LogZxyWarning("AB", "RefCount is will less than 0 : " + abRequest.path);
            return;
        }

        abRequest.RefCount += value;
        if (preValue > 0 && 0 == abRequest.RefCount)
        {
            //释放
            willReleseABReq.Add(abRequest);
            abRequest.Dispose();
            Logx.LogZxy("AB", "will Release AB : " + abRequest.path);
        }


        var deps = this.GetDependPaths(abRequest.path);
        for (int i = 0; i < deps.Length; i++)
        {
            var dep = deps[i];
            if (this.abReqCacheDic.ContainsKey(dep))
            {
                var abReqCache = this.abReqCacheDic[dep];
                ChangeRef(abReqCache, value);
            }
        }
    }


    void OnFinishLoadDependAsync(AssetBundleRequest abRequest)
    {
        Logx.LogZxy("AB", "OnFinishLoadDepend : finish dep : dependPath : " + abRequest.path);

        //加载依赖完成 找到相应的 request 进而找到被谁依赖
        var fromReqList = abRequest.fromRequests;
        for (int i = 0; i < fromReqList.Count; i++)
        {
            var fromReq = fromReqList[i];
            var isPreFinishAllDeps = fromReq.IsDependAllLoadFinish();
            fromReq.FinishLoadDepend(abRequest.path);
            if (!isPreFinishAllDeps && fromReq.IsDependAllLoadFinish())
            {
                this.waitLoadABList.Add(fromReqList[i]);
            }
        }
    }
    
    void OnFinishLoadDependSync(AssetBundleRequest abRequest)
    {
        Logx.LogZxy("AB", "OnFinishLoadDepend : finish dep : dependPath : " + abRequest.path);

        //加载依赖完成 找到相应的 request 进而找到被谁依赖
        var fromReqList = abRequest.fromRequests;
        for (int i = 0; i < fromReqList.Count; i++)
        {
            var fromReq = fromReqList[i];
            var isPreFinishAllDeps = fromReq.IsDependAllLoadFinish();
            fromReq.FinishLoadDepend(abRequest.path);

            if (!isPreFinishAllDeps && fromReq.IsDependAllLoadFinish())
            {
                TrueLoadSync(fromReq);
            }
        }
    }


    void OnLoadFinish(AssetBundleRequest req, AssetBundle assetBundle)
    {
        var callbackList = req.GetAllFinishCallback();

        req.FinishLoad(assetBundle);

        //每一个 finishCallback 算是一个引用
        for (int i = 0; i < callbackList.Count; i++)
        {
            var currCallback = callbackList[i];
            if (!req.isWillRelease)
            {
                //只有最外层的加载请求才能增加引用计数(里层通过外层来自动增加引用计数)
                if (0 == req.fromRequests.Count)
                {
                    ChangeRef(req, 1);
                }
            }
            else
            {
                Logx.LogZxyWarning("AB", "the assetBundle is null , perhaps has release in this frame : " + req.path);
            }

            currCallback?.Invoke(req);
        }
        req.ClearTempDataOnLoading();
    }

    public void Update(float timeDelta)
    {
        List<AssetBundleRequest> finishRequestList = new List<AssetBundleRequest>();
        //更新正常队列 先入队的先加载完成
        for (int i = 0; i < onLoadingABList.Count - 1; i++)
        {
            var loadingABReq = onLoadingABList[i];
            loadingABReq.Update(timeDelta);
        }

        for (int i = onLoadingABList.Count - 1; i >= 0; i--)
        {
            var loadingABReq = onLoadingABList[i];
            if (loadingABReq.CheckIsFinishLoadAsync())
            {
                var ab = loadingABReq.createRequest.assetBundle;
                Logx.LogZxy("AB", "Update : finish load : path : " + loadingABReq.path);

                this.OnLoadFinish(loadingABReq, ab);
                onLoadingABList.RemoveAt(i);

            }
        }

        //试着从等待加载队列中取能加载的请求
        var canUseProcessCount = this.maxLoadProcessCount - onLoadingABList.Count;
        if (canUseProcessCount < 0)
        {
            Logx.LogZxyWarning("AB", "Update : the (maxLoadProcessCount <= loadingABList.Count : " +
            this.maxLoadProcessCount + ") < " + onLoadingABList.Count);
            canUseProcessCount = 0;
        }

        //有可用的加载坑
        if (canUseProcessCount > 0)
        {
            // Debug.Log("zxy : abLog : update : have loadBench which can load : " + canUseProcessCount);
            //从等待加载序列中逐步加入可用加载坑
            for (int i = 0; i < canUseProcessCount; i++)
            {
                if (i >= waitLoadABList.Count)
                {
                    break;
                }
                else
                {
                    var waitLoadAB = waitLoadABList[0];
                    onLoadingABList.Add(waitLoadAB);
                    waitLoadABList.RemoveAt(0);
                    //真正开始加载
                    TrueLoadAsync(waitLoadAB);
                }
            }
        }

        //检测释放
        CheckRelease();
        willReleseABReq.Clear();
    }

    string GetLoadPath(string path)
    {
        return Const.AssetBundlePath + "/" + path;// + Const.ExtName;
    }
    
    void TrueLoadAsync(AssetBundleRequest req)
    {
        Logx.LogZxy("AB", "TrueLoadAsync : start to load truly : " + req.path);
        var path = GetLoadPath(req.path);

        var bundleCreateReq = AssetBundle.LoadFromFileAsync(path);
        req.StartLoadAsync(bundleCreateReq);
    }

    void TrueLoadSync(AssetBundleRequest req)
    {
        Logx.LogZxy("AB", "TrueLoadSync : start to load truly : " + req.path);
        var path = GetLoadPath(req.path);

        req.SetSyncLoadState();
        var assetBundle = AssetBundle.LoadFromFile(path);
        //req.assetBundle = assetBundle;
        OnLoadFinish(req, assetBundle);

    }

    void CheckRelease()
    {
        for (int i = this.willReleseABReq.Count - 1; i >= 0; i--)
        {
            var willReleaseABReq = this.willReleseABReq[i];
            willReleaseABReq.UnloadAB();
            this.abReqCacheDic.Remove(willReleaseABReq.path);
        }
    }
    
    public void Release(string path)
    {
        ReleaseAB(path);
    }

    void ReleaseAB(string path)
    {
        if (this.abReqCacheDic.ContainsKey(path))
        {
            var abReqCache = this.abReqCacheDic[path];
            ChangeRef(abReqCache, -1);
        }
        else
        {
            Logx.LogZxyWarning("AB", "ReleaseAB : the path is not found : " + path);
        }
    }
}
