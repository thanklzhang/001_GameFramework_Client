using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class AssetBundleLoader : BaseLoader
{
    public string path;
    public Action<AssetBundleCache> finishLoadCallback;

    public AssetBundle assetBundle;

    //加载中相同 ab 的请求引用数量
    public int refCount;

    public List<string> deps;

    AssetBundleCreateRequest abCreateReq;

    public override void OnStart()
    {
        ////Logx.Logz("AssetBundleLoader OnStart : " + this.path);
        refCount += 1;
        //填充 deps
        //这里应该抽出来

        deps = AssetBundleManager.Instance.GetDependPaths(this.path).ToList();
        // Logx.LogError("AB", "--dep start : " + path);
        // foreach (var VARIABLE in deps)
        // {
        //     Logx.LogError("AB", "dep :            " + VARIABLE);
        // }
        //
        // Logx.LogError("AB", "--dep end : " + path);
    }

    public override void OnPrepareFinish()
    {
        //Logx.Log("ab loader OnPrepareFinish : " + this.path);
        ////Logx.Logz("AssetBundleLoader OnPrepareFinish : " + this.path);
        //准备好了 开始加载
        if (this.path.Contains("img_skill_001.ab"))
        {
            Logx.LogWarning("img_skill_001.ab OnPrepareFinish");
        }

        var resultPath = GetABLoadPath(this.path);
        //Logx.Log("resultPath : " + resultPath);
        abCreateReq = AssetBundle.LoadFromFileAsync(resultPath);
        //Logx.Log("abCreateReq : " + abCreateReq);
    }

    internal override void OnLoadFinish()
    {
       // Logx.Log("ab loader OnLoadFinish : " + path);

        ////Logx.Logz("AssetBundleLoader OnLoadFinish : " + this.path);
        //ab 加载完成
        AssetBundleCache abCache = new AssetBundleCache();
        abCache.path = path;
        abCache.finishLoadCallback = finishLoadCallback;
        abCache.RefCount = this.refCount;
        abCache.assetBundle = abCreateReq.assetBundle;
        //abCache.ab = ab
        //finishLoadCallback?.Invoke(abCache);

        
        if (path.Contains("img_skill_001"))
        {
            Logx.LogWarning("img_skill_001.ab abloader OnLoadFinish");
        }
        
        
        AssetBundleManager.Instance.OnLoadFinish(abCache);
    }

    internal void OnLoadOtherABFinish(string path)
    {
        //判断是否是当前 ab 所依赖的 ab
        //是的话 deps 移除该路径 相当于完成了一个加载依赖
        if (deps.Contains(path))
        {
            deps.Remove(path);
        }

    }

    public override bool IsPrepareFinish()
    {
        for (int i = deps.Count - 1; i >= 0; i--)
        {
            var dep = deps[i];
            var abCache = AssetBundleManager.Instance.GetCacheByPath(dep);
            if (abCache != null)
            {
                deps.RemoveAt(i);
            }
        }

        return 0 == deps.Count;
    }

    public override bool IsLoadFinish()
    {
        if (null == abCreateReq)
        {
            return false;
        }

        //Logx.Log("abCreateReq progress : " + abCreateReq.progress);
        return abCreateReq.progress >= 1;
    }

    string GetABLoadPath(string path)
    {
        return Const.AssetBundlePath + "/" + path; // + Const.ExtName;
    }

    public override string GetPath()
    {
        return this.path;
    }
}