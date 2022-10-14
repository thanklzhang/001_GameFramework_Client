using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlManager : Singleton<CtrlManager>
{
    public List<BaseCtrl> ctrlCacheList = new List<BaseCtrl>();

    public GlobalCtrl globalCtrl = new GlobalCtrl();

    //public Dictionary<Type, BaseCtrl> ctrlCacheDic = new Dictionary<Type, BaseCtrl>();
    public BaseCtrl currMainCtrl;
    public void Init()
    {
        globalCtrl.Init();
    }

    public void Enter<T>(CtrlArgs args = null) where T : BaseCtrl, new()
    {
        var findCtrl = FindCtrl<T>();
        if (findCtrl != null)
        {
            //已经存在 目前的方案是 ：直接把 ctrl 提到最前面
            ctrlCacheList.Remove(findCtrl);
            ctrlCacheList.Add(findCtrl);

            findCtrl.Enter(args);
            findCtrl.Active();

            currMainCtrl = findCtrl;
            //todo : gameObject 的层级提到最高
        }
        else
        {
            //没找到 开始一个新的 ctrl
            BaseCtrl newCtrl = new T();
            newCtrl.Init();

            ctrlCacheList.Add(newCtrl);

            currMainCtrl = newCtrl;

            //这里的回调需要 ctrl 自行在加载完之后调用 ctrl 中的 LoadFinish(之后流程会优化)
            newCtrl.StartLoad(() =>
            {
                newCtrl.Enter(args);
                newCtrl.Active();

                var nextIndex = ctrlCacheList.Count - 2;
                if (nextIndex >= 0)
                {
                    BaseCtrl currCtrl = ctrlCacheList[nextIndex];
                    currCtrl.Inactive();
                }
            });
        }

    }

    public void Exit<T>() where T : BaseCtrl
    {
        var findCtrl = FindCtrl<T>();
        if (findCtrl == currMainCtrl)
        {
            //先打开上一组 ctrl 之后也改成异步回调之后再关闭
            currMainCtrl = null;
            var nextIndex = ctrlCacheList.Count - 2;
            findCtrl.Inactive();
            findCtrl.Exit();
            ctrlCacheList.Remove(findCtrl);

            if (nextIndex >= 0)
            {
                BaseCtrl currCtrl = ctrlCacheList[nextIndex];
                currCtrl.Active();
                currMainCtrl = currCtrl;
            }

        }
        else
        {
            Logx.LogError("findCtrl == ctrl : " + typeof(T) + " != " + currMainCtrl?.GetType());
            return;
        }
    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < ctrlCacheList.Count; i++)
        {
            var ctrl = ctrlCacheList[i];
            if (ctrl.state == CtrlState.Loading)
            {
                if (ctrl.CheckLoadFinish())
                {
                    ctrl.LoadFinish();
                }
            }
            if (ctrl.state == CtrlState.Active)
            {
                ctrl.Update(deltaTime);
            }
        }

        //global ctrl TODO:抽象出来 和上面走一套
        if (this.globalCtrl.state == CtrlState.Loading)
        {
            if (globalCtrl.CheckLoadFinish())
            {
                globalCtrl.LoadFinish();
            }
        }
        if (globalCtrl.state == CtrlState.Active)
        {
            globalCtrl.Update(deltaTime);
        }

    }

    public void LateUpdate(float deltaTime)
    {
        for (int i = 0; i < ctrlCacheList.Count; i++)
        {
            var ctrl = ctrlCacheList[i];
            if (ctrl.state == CtrlState.Active)
            {
                ctrl.LateUpdate(deltaTime);
            }
        }


        //global ctrl TODO:抽象出来 和上面走一套
        if (this.globalCtrl.state == CtrlState.Loading)
        {
            if (globalCtrl.state == CtrlState.Active)
            {
                globalCtrl.LateUpdate(deltaTime);
            }
        }
    }

    public IEnumerator EnterGlobalCtrl()
    {
        bool isLoadFinish = false;
        globalCtrl.StartLoad(() =>
        {
            globalCtrl.Enter(new CtrlArgs());
            globalCtrl.Active();
            isLoadFinish = true;
        });

        while (true)
        {
            yield return null;

            if (isLoadFinish)
            {
                break;
            }
        }
    }

    public void ExitGlobalCtrl()
    {
        globalCtrl.Inactive();
        globalCtrl.Exit();
    }


    public BaseCtrl FindCtrl<T>() where T : BaseCtrl
    {
        for (int i = 0; i < ctrlCacheList.Count; i++)
        {
            var ctrl = ctrlCacheList[i];
            if (ctrl.GetType() == typeof(T))
            {
                return ctrl;
            }
        }
        return null;
    }

}
