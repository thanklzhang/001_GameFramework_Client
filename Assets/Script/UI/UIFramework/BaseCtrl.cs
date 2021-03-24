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

public class BaseCtrl
{
    //是否并行 并行是指不影响其他的 ctrl
    public bool isParallel;
    Action finishCallback;

    //public BaseView view;

    public void Init()
    {
        this.OnInit();
    }
    //生命周期-------------------------------------------------------------------------
    //供 ctrlManager 调用
    public void StartLoad(Action finishCallback)
    {
        this.finishCallback = finishCallback;
        this.OnStartLoad();
    }

    //供子类在加载完成时调用
    public void LoadFinish()
    {
        this.finishCallback?.Invoke();
        this.finishCallback = null;
        this.OnLoadFinish();
    }

    public void Enter(CtrlArgs args)
    {
        this.OnEnter(args);
    }


    public void Active()
    {
        this.OnActive();
    }

    internal void Inactive()
    {
        this.OnInactive();
    }

    public void Exit()
    {
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
