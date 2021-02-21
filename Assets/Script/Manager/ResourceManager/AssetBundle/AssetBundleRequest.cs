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
    public string path = "";

    private int refCount;
    public int RefCount
    {
        get => refCount;
        set
        {
            Logx.LogZxy("AB", "change ref count : " + path + " : " + refCount + " => " + value);
            refCount = value;
        }
    }
    List<Action<AssetBundleRequest>> finishCallbacks = new List<Action<AssetBundleRequest>>();

    public List<string> depends = new List<string>();

    public AssetBundleCreateRequest createRequest;
    public AssetBundle assetBundle;
    public void StartLoadAsync(AssetBundleCreateRequest createRequest)
    {
        this.createRequest = createRequest;
    }
    public void Update(float timeDelta)
    {

    }

    public void DependFinishCallback(Action<AssetBundleRequest> callback)
    {
        finishCallbacks.Add(callback);
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

    public bool isSyncLoad = false;
    public bool CheckIsFinishLoadAsync()
    {
        if (!this.IsDependAllLoadFinish())
        {
            Logx.LogZxyWarning("AB", "the ab doesnt finish load all deps");
            return false;
        }
        if (!isSyncLoad && null == this.createRequest)
        {
            Logx.LogZxyError("AB", "the createRequest is null : " + this.path);
            return false;
        }
        if (!isSyncLoad && this.createRequest.isDone)
        {
            return true;
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
            Logx.LogZxy("AB", "FinishLoadDepend : the depend doesnt exist ,perhaps has finish load : " + depend);
            return;
        }

        depends.Remove(depend);
    }

    public void AddLoadDepend(string[] deps)
    {
        depends = deps.ToList();
    }
    public bool isCanUseAB = false;

    internal void FinishLoad(AssetBundle assetBundle)
    {
        this.assetBundle = assetBundle;
        //if (!isSyncLoad)
        //{
        //    assetBundle = this.createRequest.assetBundle;
        //}
        //else
        //{
        //    //同步的话已经加载完 ab
        //}
        isCanUseAB = true;
    }

    public void ClearTempDataOnLoading()
    {
        finishCallbacks.Clear();
        fromRequests.Clear();
    }

    //将在下一帧释放
    public bool isWillRelease = false;
    public void Dispose()
    {
        isWillRelease = true;
    }



    internal void UnloadAB()
    {
        //isWillRelease = true;
        assetBundle.Unload(true);
        assetBundle = null;
    }

    internal void SetSyncLoadState()
    {
        this.isSyncLoad = true;
    }
}

