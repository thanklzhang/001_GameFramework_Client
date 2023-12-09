using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class BaseSceneCtrl
{
    public string sceneName;
    public virtual void Init()
    {
        
    }
    //
    // private Action finishLoadAction;
    // public virtual void StartLoad()
    // {
    //     SceneLoadManager.Instance.Load(sceneName, LoadFinish);
    // }
    //
    // protected virtual void LoadFinish()
    // {
    //     SceneCtrlManager.Instance.LoadFinish();
    // }

    public virtual void StartLoad(Action action = null)
    {
        action?.Invoke();
    }

    public virtual void Enter()
    {
    }

    // public virtual void Active()
    // {
    // }
    //
    // public virtual void Inactive()
    // {
    // }
    //
    // public virtual void Release()
    // {
    //     
    // }

    public virtual void Exit(Action action)
    {
        action?.Invoke();
    }

}