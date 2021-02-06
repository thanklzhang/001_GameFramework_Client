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
            Debug.Log("zxy : abInfos : change ref : " + path + " " + refCount + " -> " + value);
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
        if (!isDepLoad)
        {
            sameRequestCount += 1;
        }
       
        Debug.Log("zxy : abInfo : change sameRequestCount : curr : " + this.path + " : " + sameRequestCount);
        if (callback != null)
        {
            finishCallbacks.Add(callback);
        }
        else
        {
            Debug.LogWarning("zxy : DependFinishCallback : the callback is null");
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
        if (!depends.Contains(depend))
        {
            Debug.LogWarning("zxy : the depend doesnt exist : " + depend);
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

    //正在等待依赖加载的 assetBundle 请求（本体 ab 这里只是储存验证一下 实际上不需要也可以）
    //依赖加载都加载完 本体 ab 将要进入等待加载队列中
    Dictionary<string, AssetBundleRequest> fromDependCacheDic = new Dictionary<string, AssetBundleRequest>();
    //等待加载的 assetBundle 请求
    List<AssetBundleRequest> waitLoadABList = new List<AssetBundleRequest>();
    //正在加载中的 assetBundle 请求
    List<AssetBundleRequest> loadingABList = new List<AssetBundleRequest>();
    //已经加载好的 assetBundle 信息
    Dictionary<string, AssetBundleInfo> abCacheDic = new Dictionary<string, AssetBundleInfo>();

    // AssetBundleManifest manifest;
    //Dictionary<string, AssetPakageInfo> assetPakageInfos;
    //同时加载的 ab 数量
    int maxLoadProcessCount = 3;
    private AssetBundleManifest manifest;
    //private string[] abdepends

    public string[] GetAllDependencies(string path)
    {
        var deps = manifest.GetAllDependencies(path);

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

    public void Init()
    {
        Debug.Log("zxy : abLog : init");
        var manifestAB = AssetBundle.LoadFromFile(Const.AssetBundlePath + "/" + "StreamingAssets");
        manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //var assetFileStr = File.ReadAllText(Const.AppStreamingAssetPath + "/" + "AssetFileData.json");
        //this.assetPakageInfos = JsonMapper.ToObject<Dictionary<string, AssetPakageInfo>>(assetFileStr);

        // this.assetPakageInfos.ToList().ForEach(dd =>
        // {
        //     Debug.Log(dd.Key);
        // });

    }

    public void ChangeRefCount(AssetBundleInfo abInfo, int value)
    {

        if (!abCacheDic.ContainsKey(abInfo.path))
        {
            return;
        }
        //递归还是循环都行 自己看着办 这里用递归
        
        var preRefCount = abInfo.RefCount;

        abInfo.RefCount += value;

        //引用减到 0 的时候释放 ab
        if (preRefCount > 0 && 0 == abInfo.RefCount)
        {
            Debug.Log("zxy : abInfo ref : refCount is 0 , unload : " + abInfo.path);
            abInfo.assetBundle.Unload(true);
            abCacheDic.Remove(abInfo.path);
        }

        var depends = this.GetAllDependencies(abInfo.path);
        for (int i = 0; i < depends.Length; i++)
        {
            var depPath = depends[i];
            AssetBundleInfo depABInfo = null;
            if (this.abCacheDic.ContainsKey(depPath))
            {
                Debug.Log("zxy : abInfo ref : dep , start to change ref : " + abInfo.path + " 's dep : " + depPath);
                depABInfo = this.abCacheDic[depPath];
                this.ChangeRefCount(depABInfo, value);
            }
            else
            {
                Debug.LogError("zxy : ChangeRefCount : the path is not found : fromDepPath : " + abInfo.path + " depPath : " + depPath);
            }
        }
    }

    public void Load(string path, Action<AssetBundleInfo> finishCallback, bool isAsync,bool isDepLoad = false)//, AssetBundleRequest fromRequest
    {
        Debug.Log("zxy : abLog : Load : " + path);
        if (this.abCacheDic.ContainsKey(path))
        {
            Debug.Log("zxy : abLog : have path : direct Load : " + path);
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
                Debug.Log("zxy : abLog : path in waitQueue : " + path);
                //在等待加载队列中
                waitReq.DependRequest(finishCallback, isDepLoad);
                //waitReq.finishCallbacks += finishCallback;
            }
            else
            {
                //是否在正在加载队列中
                AssetBundleRequest loadingReq = null;
                for (int i = 0; i < loadingABList.Count; i++)
                {
                    var abReq = loadingABList[i];
                    if (abReq.path == path)
                    {
                        loadingReq = abReq;
                        break;
                    }
                }

                if (loadingReq != null)
                {
                    Debug.Log("zxy : abLog : path in loadingQueue : " + path);
                    //在正在加载队列中
                    loadingReq.DependRequest(finishCallback, isDepLoad);

                }
                else
                {
                    //是否在等待依赖加载缓存中
                    if (this.fromDependCacheDic.ContainsKey(path))
                    {
                        Debug.Log("zxy : abLog : path in waitDepDic : " + path);
                        //在等待依赖加载缓存中
                        this.fromDependCacheDic[path].DependRequest(finishCallback, isDepLoad);
                    }
                    else
                    {
                        //哪都没有 开始新加载
                        Debug.Log("zxy : abLog : dont have any where , start new load : " + path);
                        AssetBundleRequest request = new AssetBundleRequest();
                        request.path = path;
                        request.DependRequest(finishCallback, isDepLoad);
                        this.fromDependCacheDic.Add(path, request);

                        //abTempCacheDic.Add(path, request);
                        //处理 ab 依赖
                        var deps = GetAllDependencies(path);// + Const.ExtName);
                        //Debug.Log("length of dep bundles : " + deps.Length);
                        if (deps != null && deps.Length > 0)
                        {
                            //有依赖
                            request.AddLoadDepend(deps);
                            for (int i = 0; i < deps.Length; ++i)
                            {
                                var dependPath = deps[i];
                                //xxx.ab 取 xxx
                                //var index = deps[i].IndexOf(Const.ExtName);
                                var depPath = deps[i];//.Substring(0, index);
                                Debug.Log("zxy : abLog : load dep : " + path + " : dep : " + depPath);
                                Load(depPath, (abInfo) =>
                                {
                                    this.OnFinishLoadDepend(request.path, depPath);
                                }, true,true);//, request
                            }
                        }
                        else
                        {
                            //没有依赖 本身 ab 进入待加载队列
                            Debug.Log("zxy : abLog : dont have depends will load , add to waitQueue : " + path);
                            this.waitLoadABList.Add(request);
                        }
                    }
                }
            }
        }
    }

    void OnFinishLoadDepend(string fromPath, string dependPath)
    {
        if (!this.fromDependCacheDic.ContainsKey(fromPath))
        {
            Debug.LogError("zxy : the fromPath doesnt exist : " + fromPath);
            return;
        }

        var fromRequest = this.fromDependCacheDic[fromPath];
        fromRequest.FinishLoadDepend(dependPath);
        if (fromRequest.IsDependAllLoadFinish())
        {
            Debug.Log("zxy : abLog : finish dep : fromPath : " + fromPath + " , dependPath : " + dependPath);
            //注意这里 可能在刚完成加载后 更改了 waiLoadABList 数据 注意
            this.waitLoadABList.Add(fromRequest);
            this.fromDependCacheDic.Remove(fromRequest.path);
        }
    }

    //ab 加载完成
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
        //req.finishCallbacks?.Invoke(abInfo);
    }

    public void Update(float timeDelta)
    {

        List<AssetBundleRequest> finishRequestList = new List<AssetBundleRequest>();
        //更新正常队列 先入队的先加载完成
        for (int i = 0; i < loadingABList.Count - 1; i++)
        {
            var loadingABReq = loadingABList[i];
            loadingABReq.Update(timeDelta);
        }


        for (int i = loadingABList.Count - 1; i >= 0; i--)
        {
            var loadingABReq = loadingABList[i];
            if (loadingABReq.IsFinishLoad())
            {
                var ab = loadingABReq.createRequest.assetBundle;
                if (abCacheDic.ContainsKey(loadingABReq.path))
                {
                    Debug.LogError("zxy : have cache , but will load finish again : " + loadingABReq.name);
                    return;
                }
                Debug.Log("zxy : abLog : update : finish load : path : " + loadingABReq.path);

                //var callbackList = loadingABReq.GetAllFinishCallback();
                //for (int j = 0; j < callbackList.Count; j++)
                //{

                //    var currCallback = callbackList[j];
                //    currCallback?.Invoke(abInfo);
                //}

                this.OnLoadFinish(loadingABReq, ab);
                loadingABList.RemoveAt(i);

            }
        }

        //试着从等待加载队列中取能加载的请求
        var canUseProcessCount = this.maxLoadProcessCount - loadingABList.Count;
        if (canUseProcessCount < 0)
        {
            Debug.LogWarning("zxy : the maxLoadProcessCount is less than loadingABList.Count : " +
            this.maxLoadProcessCount + " < " + loadingABList.Count);
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
                    loadingABList.Add(waitLoadAB);
                    waitLoadABList.RemoveAt(0);
                    //真正开始加载
                    TrueLoad(waitLoadAB);
                }
            }
        }
    }

    string GetLoadPath(string path)
    {
        return Const.AssetBundlePath + "/" + path;// + Const.ExtName;
    }

    void TrueLoad(AssetBundleRequest req)
    {
        Debug.Log("zxy : abLog : TrueLoad : " + req.path);
        var path = GetLoadPath(req.path);
        var bundleCreateReq = AssetBundle.LoadFromFileAsync(path);
        req.StartLoad(bundleCreateReq);
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
            //var packageInfo = assetPakageInfos[path];
            //var deps = packageInfo.dependencies;
            //for (int i = 0; i < deps.Count; i++)
            //{
            //    var dep = deps[i];
            //    ReleaseAB(dep);
            //}
        }
        else
        {
            Debug.LogWarning("zxy : the ab is not found in abCacheDic : " + path);
        }
    }

    public void ReleseAll()
    {

    }


}
