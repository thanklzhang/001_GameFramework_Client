
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public enum LoaderState
{
    //初始
    Null = 0,
    //正在准备中
    Prepare,
    //正在加载
    Loading,
    //加载完成
    Finish,
    //释放 下一个阶段会回到 Null
    Release,
}

public class BaseLoader
{
    //public string path;
    //public Action prepareFinishCallback;
    //public Action loadFinishCallback;

    public LoaderState loaderState = LoaderState.Null;

    public void Start()
    {
        loaderState = LoaderState.Prepare;
        this.OnStart();
    }

    public void PrepareFinish()
    {
        loaderState = LoaderState.Loading;
        this.OnPrepareFinish();
    }

    public void LoadFinish()
    {
        loaderState = LoaderState.Finish;
        this.OnLoadFinish();
    }

    public void Release()
    {
        loaderState = LoaderState.Release;
        this.OnRelease();
    }

    internal void Update(float timeDelta)
    {
        
    }

    public virtual void OnStart()
    {

    }
    public virtual void OnPrepareFinish()
    {
        //prepareFinishCallback?.Invoke();
    }

    internal virtual void OnLoadFinish()
    {
        //loadFinishCallback?.Invoke();
    }

    public virtual void OnRelease()
    {

    }

    public virtual bool IsPrepareFinish()
    {
        return false;
    }

    public virtual bool IsLoadFinish()
    {
        return false;
    }
}
