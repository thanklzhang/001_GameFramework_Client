using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
public class AssetBundleInfo
{
    public string name = "";
    public string path = "";

    public AssetBundle assetBundle;

    private int refCount = 0;
    public int RefCount
    {
        get
        {
            return refCount;
        }
        set
        {
            Logx.LogZxy("AB", "change ref : " + path + " " + refCount + " -> " + value);
            refCount = value;
        }
    }

}

public class AssetBundleRequest
{

    public string name = "";
    public string path = "";
    //public Action<AssetBundleInfo> finishCallbacks;

    public int sameRequestCount = 0;

    List<Action<AssetBundleInfo>> finishCallbacks = new List<Action<AssetBundleInfo>>();

    public List<string> depends = new List<string>();

    public AssetBundleCreateRequest createRequest;
    public void StartLoad(AssetBundleCreateRequest createRequest)
    {
        this.createRequest = createRequest;
    }
    public void Update(float timeDelta)
    {

    }

    public void DependRequest(Action<AssetBundleInfo> callback, bool isDepLoad)
    {
        //Debug.Log("zxy : DependRequest : " + this.path);
        if (!isDepLoad)
        {
            sameRequestCount += 1;
        }

        //Debug.Log("zxy : abInfo : change sameRequestCount : curr : " + this.path + " : " + sameRequestCount);
        if (callback != null)
        {
            finishCallbacks.Add(callback);
        }
        else
        {
            Logx.LogZxyWarning("AB", "DependRequest : the callback is null");
        }
    }

    public List<Action<AssetBundleInfo>> GetAllFinishCallback()
    {
        return finishCallbacks;
    }

    public bool IsDependAllLoadFinish()
    {
        return 0 == depends.Count;
    }

    public bool IsFinishLoad()
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
    public AssetBundleRequest fromRequest;

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
}

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    //等待加载的 assetBundle 请求
    List<AssetBundleRequest> waitLoadABList = new List<AssetBundleRequest>();
    //正在加载中的 assetBundle 请求
    List<AssetBundleRequest> onLoadingABList = new List<AssetBundleRequest>();
    //已经加载好的 assetBundle 信息
    Dictionary<string, AssetBundleInfo> abCacheDic = new Dictionary<string, AssetBundleInfo>();

    //正在等待依赖加载的 assetBundle 请求（本体 ab 这里方便之后储存验证一下 实际上不需要也可以)
    List<AssetBundleRequest> fromDependCacheList = new List<AssetBundleRequest>();

    // AssetBundleManifest manifest;
    //Dictionary<string, AssetPakageInfo> assetPakageInfos;
    //同时加载的 ab 数量
    int maxLoadProcessCount = 3;
    private AssetBundleManifest manifest;
    //private string[] abdepends

    public void Init()
    {
        Logx.LogZxy("AB", "abLog : init");
        var manifestAB = AssetBundle.LoadFromFile(Const.AssetBundlePath + "/" + "StreamingAssets");
        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //var assetFileStr = File.ReadAllText(Const.AppStreamingAssetPath + "/" + "AssetFileData.json");
        //this.assetPakageInfos = JsonMapper.ToObject<Dictionary<string, AssetPakageInfo>>(assetFileStr);

        // this.assetPakageInfos.ToList().ForEach(dd =>
        // {
        //     Debug.Log(dd.Key);
        // });

    }

    public string[] GetDependPaths(string path)
    {
        var deps = manifest.GetDirectDependencies(path);
        deps.ToList().ForEach((str) =>
        {
            Logx.LogZxy("AB", "path : " + path + " dep : " + str);
        } );
        return deps;
        //if (assetPakageInfos.ContainsKey(path))
        //{
        //    var assetPackageInfo = assetPakageInfos[path];
        //    return assetPackageInfo.dependencies.ToArray();
        //}
        //else
        //{
        //    return new string[] { };
        //}

    }

    public void Load(string path, Action<AssetBundleInfo> finishCallback, bool isSync, bool isDepLoad = false)//, AssetBundleRequest fromRequest
    {
        if (isSync)
        {
            LoadSync(path, finishCallback, isDepLoad);

        }
        else
        {
            LoadAsync(path, finishCallback, isDepLoad);
        }


    }

    public AssetBundle TrueLoadSync(AssetBundleRequest req)
    {
        Logx.LogZxy("AB", "abLog : TrueLoadSync : " + req.path);
        var path = GetLoadPath(req.path);
        var assetBundle = AssetBundle.LoadFromFile(path);
        //req.StartLoad(bundleCreateReq);
        return assetBundle;
    }

    public void LoadSync(string path, Action<AssetBundleInfo> finishCallback, bool isDepLoad = false)
    {
        if (this.abCacheDic.ContainsKey(path))
        {
            Logx.LogZxy("AB", "LoadSync : have path : direct Load (perhaps about async to sync): " + path);
            //已经有 ab 了 直接完成
            var abCache = this.abCacheDic[path];
            this.ChangeRefCount(abCache, 1);
            finishCallback?.Invoke(abCache);
        }
        else
        {
            //是否在等待队列中
            AssetBundleRequest waitReq = null;
            for (int i = 0; i < waitLoadABList.Count; i++)
            {
                var abReq = waitLoadABList[i];
                if (abReq.path == path)
                {
                    waitReq = abReq;

                    break;
                }
            }
            if (waitReq != null)
            {
                Logx.LogZxy("AB", " LoadSync : path in waitQueue and async to sync : " + path);
                //在等待加载队列中
                //异步转同步 直接完成
                waitReq.DependRequest(finishCallback, isDepLoad);

                this.waitLoadABList.Remove(waitReq);

                var assetBundle = TrueLoadSync(waitReq);
                this.OnLoadFinish(waitReq, assetBundle);


            }
            else
            {
                //是否在正在加载队列中
                AssetBundleRequest loadingReq = null;
                for (int i = 0; i < onLoadingABList.Count; i++)
                {
                    var abReq = onLoadingABList[i];
                    if (abReq.path == path)
                    {
                        loadingReq = abReq;
                        break;
                    }
                }

                if (loadingReq != null)
                {
                    Logx.LogZxy("AB", "LoadSync : path in loadingQueue (didnt test!!!) : onLoadingReq async to sync : " + path);
                    //在正在加载队列中
                    //异步转同步 直接完成

                    loadingReq.DependRequest(finishCallback, isDepLoad);

                    this.onLoadingABList.Remove(loadingReq);

                    loadingReq.createRequest.assetBundle.Unload(true);

                    var assetBundle = TrueLoadSync(loadingReq);
                    this.OnLoadFinish(waitReq, assetBundle);


                }
                else
                {

                    //加到依赖加载缓存中
                    Logx.LogZxy("AB", "LoadSync : dont have any where , start new load : " + path);
                    AssetBundleRequest request = new AssetBundleRequest();
                    request.path = path;
                    request.DependRequest(finishCallback, isDepLoad);
                    this.fromDependCacheList.Add(request);

                    //处理 ab 依赖
                    var deps = GetDependPaths(path);// + Const.ExtName);
                                                    //Debug.Log("length of dep bundles : " + deps.Length);
                    if (deps != null && deps.Length > 0)
                    {
                        //有依赖
                        request.AddLoadDepend(deps);
                        for (int i = 0; i < deps.Length; ++i)
                        {
                            var dependPath = deps[i];
                            //xxx.ab 取 xxx
                            var depPath = deps[i];//.Substring(0, index);
                            Logx.LogZxy("AB", "LoadSync : load dep : " + path + " : dep : " + depPath);
                            Load(depPath, (abInfo) =>
                            {
                                this.OnFinishLoadDependSync(depPath, request);
                            }, true, true);//, request
                        }
                    }
                    else
                    {
                        //没有依赖 本身 ab 同步转异步完成
                        Logx.LogZxy("AB", "LoadSync : dont have depends will load and async to sync : " + path);

                        var assetBundle = TrueLoadSync(request);
                        this.OnLoadFinish(request, assetBundle);
                    }
                }
            }
        }
    }

    void OnFinishLoadDependSync(string dependPath, AssetBundleRequest fromRequest)
    {

        fromRequest.FinishLoadDepend(dependPath);
        if (fromRequest.IsDependAllLoadFinish())
        {
            //同步转异步 直接完成

            Logx.LogZxy("AB", "OnFinishLoadDependSync : finish dep : async to sync : dependPath : " + dependPath + " , fromPath : " + fromRequest.path);

            var assetBundle = TrueLoadSync(fromRequest);
            this.OnLoadFinish(fromRequest, assetBundle);

            this.fromDependCacheList.Remove(fromRequest);



        }
    }

    public void LoadAsync(string path, Action<AssetBundleInfo> finishCallback, bool isDepLoad = false)
    {
        Logx.LogZxy("AB", "start a new Load request : " + path);
        if (this.abCacheDic.ContainsKey(path))
        {
            Logx.LogZxy("AB", "Load : have path : direct Load : " + path);
            //已经有 ab 了 直接完成
            var abCache = this.abCacheDic[path];
            this.ChangeRefCount(abCache, 1);
            finishCallback?.Invoke(abCache);
        }
        else
        {
            //是否在等待队列中
            AssetBundleRequest waitReq = null;
            for (int i = 0; i < waitLoadABList.Count; i++)
            {
                var abReq = waitLoadABList[i];
                if (abReq.path == path)
                {
                    waitReq = abReq;

                    break;
                }
            }
            if (waitReq != null)
            {
                Logx.LogZxy("AB", "Load : path in waitQueue : " + path);
                //在等待加载队列中
                waitReq.DependRequest(finishCallback, isDepLoad);
                //waitReq.finishCallbacks += finishCallback;
            }
            else
            {
                //是否在正在加载队列中
                AssetBundleRequest loadingReq = null;
                for (int i = 0; i < onLoadingABList.Count; i++)
                {
                    var abReq = onLoadingABList[i];
                    if (abReq.path == path)
                    {
                        loadingReq = abReq;
                        break;
                    }
                }

                if (loadingReq != null)
                {
                    Logx.LogZxy("AB", "Load : path in loadingQueue : " + path);
                    //在正在加载队列中
                    loadingReq.DependRequest(finishCallback, isDepLoad);

                }
                else
                {
                    //加到依赖加载缓存中
                    AssetBundleRequest request = new AssetBundleRequest();
                    request.path = path;
                    request.DependRequest(finishCallback, isDepLoad);
                    this.fromDependCacheList.Add(request);


                    //处理 ab 依赖
                    var deps = GetDependPaths(path);// + Const.ExtName);
                                                    //Debug.Log("length of dep bundles : " + deps.Length);
                    if (deps != null && deps.Length > 0)
                    {
                        //有依赖
                        request.AddLoadDepend(deps);
                        for (int i = 0; i < deps.Length; ++i)
                        {
                            var dependPath = deps[i];
                            //xxx.ab 取 xxx
                            var depPath = deps[i];//.Substring(0, index);
                            Logx.LogZxy("AB", "Load : load dep : " + path + " : dep : " + depPath);
                            Load(depPath, (abInfo) =>
                            {
                                this.OnFinishLoadDependAsync(depPath, request);
                            }, false, true);//, request
                        }
                    }
                    else
                    {
                        //没有依赖 本身 ab 进入待加载队列
                        Logx.LogZxy("AB", "Load : dont have depends will load , add to waitQueue : " + path);
                        this.waitLoadABList.Add(request);
                    }

                }
            }
        }


    }

    void OnFinishLoadDependAsync(string dependPath, AssetBundleRequest fromRequest)
    {

        fromRequest.FinishLoadDepend(dependPath);
        if (fromRequest.IsDependAllLoadFinish())
        {
            Logx.LogZxy("AB", "OnFinishLoadDepend : finish dep : dependPath : " + dependPath + " , fromPath : " + fromRequest.path);

            this.waitLoadABList.Add(fromRequest);
            this.fromDependCacheList.Remove(fromRequest);
        }
    }

    void OnLoadFinish(AssetBundleRequest req, AssetBundle ab)
    {
        AssetBundleInfo abInfo = null;
        if (abCacheDic.ContainsKey(req.path))
        {
            abInfo = abCacheDic[req.path];
            this.ChangeRefCount(abInfo, req.sameRequestCount);
        }
        else
        {
            abInfo = new AssetBundleInfo();
            abInfo.name = req.name;
            abInfo.path = req.path;
            abInfo.assetBundle = ab;
            abCacheDic.Add(abInfo.path, abInfo);
            this.ChangeRefCount(abInfo, req.sameRequestCount);

        }


        var callbackList = req.GetAllFinishCallback();
        for (int i = 0; i < callbackList.Count; i++)
        {
            var currCallback = callbackList[i];
            currCallback?.Invoke(abInfo);


        }
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
            if (loadingABReq.IsFinishLoad())
            {
                var ab = loadingABReq.createRequest.assetBundle;
                if (abCacheDic.ContainsKey(loadingABReq.path))
                {
                    Logx.LogZxyError("AB", "Update : have cache , but will load finish again : " + loadingABReq.name);
                    return;
                }
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
    }

    string GetLoadPath(string path)
    {
        return Const.AssetBundlePath + "/" + path;// + Const.ExtName;
    }

    void TrueLoadAsync(AssetBundleRequest req)
    {
        //可能已经完成 在正常情况下可能是异步转同步导致
        if (this.abCacheDic.ContainsKey(req.path))
        {
            Logx.LogZxy("AB", "TrueLoad : the ab already finished , direct to finish: " + req.path);
            onLoadingABList.Remove(req);
            var ab = this.abCacheDic[req.path];
            this.OnLoadFinish(req, ab.assetBundle);
            return;
        }


        Logx.LogZxy("AB", "TrueLoad : start to load truly : " + req.path);
        var path = GetLoadPath(req.path);
        var bundleCreateReq = AssetBundle.LoadFromFileAsync(path);
        req.StartLoad(bundleCreateReq);
    }

    public void ChangeRefCount(AssetBundleInfo abInfo, int value)
    {

        if (!abCacheDic.ContainsKey(abInfo.path))
        {
            return;
        }
        //递归还是循环都行 自己看着办 这里用递归
        Logx.LogZxy("AB", "ChangeRefCount : check " + abInfo.path);
        var preRefCount = abInfo.RefCount;

        abInfo.RefCount += value;

        //引用减到 0 的时候释放 ab
        if (preRefCount > 0 && 0 == abInfo.RefCount)
        {
            Logx.LogZxy("AB", "ChangeRefCount : refCount is 0 , unload :  : " + abInfo.path);
            abInfo.assetBundle.Unload(true);
            abCacheDic.Remove(abInfo.path);
        }

        var depends = this.GetDependPaths(abInfo.path);
        for (int i = 0; i < depends.Length; i++)
        {
            var depPath = depends[i];
            AssetBundleInfo depABInfo = null;
            if (this.abCacheDic.ContainsKey(depPath))
            {
                //Logx.LogZxy("AB","abInfo ref : dep , start to change ref : " + abInfo.path + " 's dep : " + depPath);
                depABInfo = this.abCacheDic[depPath];
                Logx.LogZxy("AB", "will ChangeRefCount  " + depPath + " from " + abInfo.path);
                this.ChangeRefCount(depABInfo, value);

            }
            else
            {
                Logx.LogZxyError("AB", "ChangeRefCount : the path is not found : fromDepPath : " + abInfo.path + " depPath : " + depPath);
            }
        }
    }

    public void Release(string path)
    {
        ReleaseAB(path);
    }

    void ReleaseAB(string path)
    {
        if (abCacheDic.ContainsKey(path))
        {
            var abInfo = abCacheDic[path];
            ChangeRefCount(abInfo, -1);
        }
        else
        {
            Logx.LogZxyWarning("AB", "Release the ab is not found in abCacheDic : " + path);
        }
    }

    public void ReleseAll()
    {

    }


}
