using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlArgs
{

}
//视图层
public class BaseView
{
    //ui层
    public BaseUI ui;
    //场景层
    //public BaseScene scene;
    public void Show()
    {
        //ui 显示 并且提到最高层
    }

    public void Hide()
    {

    }
}

public class BaseCtrl
{
    //是否并行 并行是指不影响其他的 ctrl
    public bool isParallel;
    Action finishCallback;

    BaseView view;

    public void Init()
    {
        Logx.Logz("baseCtrl : init");
        this.OnInit();
    }

    public virtual void OnInit()
    {
        
    }

    //供 ctrlManager 调用
    public void StartLoad(Action finishCallback)
    {
        Logx.Logz("baseCtrl : StartLoad");
        this.finishCallback = finishCallback;
        this.OnStartLoad();
    }

    public virtual void OnStartLoad()
    {

    }

    //供子类在加载完成时调用
    public void LoadFinish()
    {
        Logx.Logz("baseCtrl : LoadFinish");
        this.finishCallback?.Invoke();
        this.finishCallback = null;
    }

    public void ReEnter(CtrlArgs args)
    {
        
    }

    public void Enter(CtrlArgs args)
    {
        Logx.Logz("baseCtrl : Enter");
        this.OnEnter();
        //view.Show();
    }

    public virtual void OnEnter()
    {

    }

    public void Exit()
    {
        Logx.Logz("baseCtrl : Exit");
        this.OnExit();
        //view.Hide();
    }
    public virtual void OnExit()
    {
        
    }

    public void Release()
    {
        Logx.Logz("baseCtrl : Release");
    }
}
