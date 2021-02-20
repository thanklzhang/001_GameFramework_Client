using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
public class AssetBundleRequest
{
    public string name = "";
    public string path = "";

    List<Action<AssetBundleRequest>> finishCallbacks = new List<Action<AssetBundleRequest>>();

    public List<string> depends = new List<string>();

    public AssetBundleCreateRequest createRequest;
    public void StartLoad(AssetBundleCreateRequest createRequest)
    {
        this.createRequest = createRequest;
    }
    public void Update(float timeDelta)
    {

    }

    public void DependFinishCallback(Action<AssetBundleRequest> callback)
    {
        if (callback != null)
        {
            finishCallbacks.Add(callback);
        }
        else
        {
            Logx.LogZxyWarning("AB", "DependRequest : the callback is null");
        }
    }

    public void DependRequests(List<Action<AssetBundleRequest>> callbacks, bool isDepLoad)
    {
        for (int i = 0; i < callbacks.Count; i++)
        {
            var callback = callbacks[i];
            DependFinishCallback(callback);
        }

    }

    public List<Action<AssetBundleRequest>> GetAllFinishCallback()
    {
        return finishCallbacks;
    }

    public bool IsDependAllLoadFinish()
    {
        return 0 == depends.Count;
    }

    public bool CheckIsFinishLoad()
    {
        if (this.IsDependAllLoadFinish())
        {
            if (null == this.createRequest)
            {
                Logx.LogZxyError("AB", "the createRequest is null : " + this.path);
                return false;
            }
            if (this.createRequest.isDone)
            {
                return true;
            }
        }

        return false;
    }
    public List<AssetBundleRequest> fromRequests = new List<AssetBundleRequest>();

    public void DependRequestFrom(AssetBundleRequest fromRequest)
    {
        if (fromRequest != null)
        {
            fromRequests.Add(fromRequest);
        }
    }

    public void FinishLoadDepend(string depend)
    {
        Logx.LogZxy("AB", "FinishLoadDepend : " + depend);
        if (!depends.Contains(depend))
        {
            Logx.LogZxyError("AB", "FinishLoadDepend : the depend doesnt exist : " + depend);
            return;
        }

        depends.Remove(depend);
    }

    public void AddLoadDepend(string[] deps)
    {
        depends = deps.ToList();
    }
    public bool isCanUseAB = false;

    internal void FinishLoad()
    {
        isCanUseAB = true;
        finishCallbacks.Clear();
    }
}

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    //assetBundle request 的缓存
    Dictionary<string, AssetBundleRequest> abReqCacheDic = new Dictionary<string, AssetBundleRequest>();

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
            //LoadSync(path, finishCallback, isDepLoad);
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
            //Logx.LogZxy("AB", "LoadAsync : have path in ab req cache " + path);

            var requestCache = this.abReqCacheDic[path];


            if (requestCache.isCanUseAB)
            {
                Logx.LogZxy("AB", "LoadAsync : the ab finish load , can use ");
                finishCallback?.Invoke(requestCache);
            }
            else
            {
                Logx.LogZxy("AB", "LoadAsync : the ab doesnt finish load , can not use ");

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

    void OnFinishLoadDependAsync(AssetBundleRequest abRequest)
    {
        Logx.LogZxy("AB", "OnFinishLoadDepend : finish dep : dependPath : " + abRequest.path);
        
        //加载依赖完成 找到相应的 request 进而找到被谁依赖
        var fromReqList = abRequest.fromRequests;
        for (int i = 0; i < fromReqList.Count; i++)
        {
            var fromReq = fromReqList[i];
            fromReq.FinishLoadDepend(abRequest.path);
            if (fromReq.IsDependAllLoadFinish())
            {
                this.waitLoadABList.Add(fromReqList[i]);
            }
        }
    }

    void OnLoadFinish(AssetBundleRequest req)//, AssetBundle ab
    {
        var callbackList = req.GetAllFinishCallback();
        for (int i = 0; i < callbackList.Count; i++)
        {
            var currCallback = callbackList[i];
            currCallback?.Invoke(req);
        }

        req.FinishLoad();
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
            if (loadingABReq.CheckIsFinishLoad())
            {
                var ab = loadingABReq.createRequest.assetBundle;
                Logx.LogZxy("AB", "Update : finish load : path : " + loadingABReq.path);

                this.OnLoadFinish(loadingABReq);
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
    }

    string GetLoadPath(string path)
    {
        return Const.AssetBundlePath + "/" + path;// + Const.ExtName;
    }

    void TrueLoadAsync(AssetBundleRequest req)
    {
        Logx.LogZxy("AB", "TrueLoad : start to load truly : " + req.path);
        var path = GetLoadPath(req.path);
        var bundleCreateReq = AssetBundle.LoadFromFileAsync(path);
        req.StartLoad(bundleCreateReq);
    }


}
