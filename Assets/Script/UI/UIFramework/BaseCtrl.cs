using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlArgs
{

}
////视图层
//public class BaseView
//{
//    //ui层
//    public BaseUI ui;
//    //场景层
//    //public BaseScene scene;
//    public void Show()
//    {
//        //ui 显示 并且提到最高层
//    }

//    public void Hide()
//    {

//    }
//}

public enum CtrlState
{
    Null = 1,
    Inited = 2,
    Loading = 3,
    Inactive = 4,
    Active = 5,
    Release = 6,

}

public class BaseCtrl
{
    //是否并行 并行是指不影响其他的 ctrl
    //public bool isParallel;
    Action finishCallback;

    //public BaseView view;
    public CtrlState state = CtrlState.Null;

    protected LoadResGroupRequest loadRequest;

    public void Init()
    {
        state = CtrlState.Inited;
        this.OnInit();
    }
    //生命周期-------------------------------------------------------------------------
    //供 ctrlManager 调用
    public void StartLoad(Action finishCallback)
    {
        this.finishCallback = finishCallback;
        state = CtrlState.Loading;
        this.OnStartLoad();
    }

    public virtual bool CheckLoadFinish()
    {
        if (null == loadRequest)
        {
            return true;
        }
        return loadRequest.CheckFinish();
    }

    public void LoadFinish()
    {
        state = CtrlState.Inactive;
        this.finishCallback?.Invoke();
        //this.finishCallback = null;
        this.OnLoadFinish();
    }

    public void Enter(CtrlArgs args)
    {
        this.OnEnter(args);
    }


    public void Active()
    {
        state = CtrlState.Active;
        this.OnActive();
    }

    public void Update(float deltaTime)
    {
        this.OnUpdate(deltaTime);
    }

    internal void Inactive()
    {
        state = CtrlState.Inactive;
        this.OnInactive();
    }

    public void Exit()
    {
        if (loadRequest != null)
        {
            loadRequest.Release();
        }
        state = CtrlState.Release;
        this.OnExit();
    }

    //public void Release()
    //{
    //    this.OnRelease();
    //}

    //可拓展-------------------------------------------------------------------------
    public virtual void OnInit()
    {

    }

    public virtual void OnStartLoad()
    {

    }

    public virtual void OnLoadFinish()
    {

    }

    public virtual void OnEnter(CtrlArgs args)
    {

    }

    public virtual void OnUpdate(float deltaTime)
    {

    }

    public virtual void OnActive()
    {

    }

    public virtual void OnInactive()
    {

    }

    public virtual void OnExit()
    {

    }



    //public virtual void OnRelease()
    //{

    //}
}
