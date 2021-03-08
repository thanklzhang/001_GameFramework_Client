using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlArgs
{
    
}

public class BaseCtrl
{
    public virtual void Init()
    {

    }
    public virtual void Enter(CtrlArgs args)
    {

    }

    protected virtual void Exit()
    {

    }
}

public class CtrlManager : Singleton<CtrlManager>
{
    public Dictionary<Type, BaseCtrl> ctrlDic = new Dictionary<Type, BaseCtrl>();

    public void Init()
    {
        ctrlDic.Add(typeof(HeroListCtrl), new HeroListCtrl());
        ctrlDic.Add(typeof(HeroInfoCtrl), new HeroInfoCtrl());
    }

    public T GetUICtrl<T>() where T : BaseCtrl
    {
        var type = typeof(T);
        return (T)ctrlDic[type];
    }
}
