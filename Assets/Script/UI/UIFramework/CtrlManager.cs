﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlManager : Singleton<CtrlManager>
{
    public List<BaseCtrl> ctrlCacheList = new List<BaseCtrl>();
    //public Dictionary<Type, BaseCtrl> ctrlCacheDic = new Dictionary<Type, BaseCtrl>();
    public BaseCtrl currMainCtrl;
    public void Init()
    {
        //ctrlDic.Add(typeof(HeroListCtrl), new HeroListCtrl());
        //ctrlDic.Add(typeof(HeroInfoCtrl), new HeroInfoCtrl());
    }

    //public T GetUICtrl<T>() where T : BaseCtrl
    //{
    //    var type = typeof(T);
    //    return (T)ctrlDic[type];
    //}

    public void Enter<T>(CtrlArgs args = null) where T : BaseCtrl, new()
    {
        var findCtrl = FindCtrl<T>();
        if (findCtrl != null)
        {
            //已经存在 目前的方案是 ：直接把 ctrl 提到最前面
            ctrlCacheList.Remove(findCtrl);
            ctrlCacheList.Add(findCtrl);
            //这里应该是刷新 而不是再次进入
            findCtrl.ReEnter(args);
            if (!findCtrl.isParallel)
            {
                currMainCtrl = findCtrl;
            }

        }
        else
        {
            //没找到 开始一个新的 ctrl
            BaseCtrl newCtrl = new T();
            newCtrl.Init();

            ctrlCacheList.Add(newCtrl);

            if (!newCtrl.isParallel)
            {
                currMainCtrl = newCtrl;
            }

            //这里的回调需要 ctrl 自行在加载完之后调用 ctrl 中的 LoadFinish
            newCtrl.StartLoad(() =>
            {
                newCtrl.Enter(args);

                for (int i = ctrlCacheList.Count - 2; i >= 0; i--)
                {
                    BaseCtrl currCtrl = ctrlCacheList[i];

                    currCtrl.Exit();
                    if (!currCtrl.isParallel)
                    {
                        break;
                    }
                }
            });

         

        }

        //打开后再关闭上一组 ctrl 之后由于异步问题可能在回调中再关闭上一组 ctrl 这样防止中间的空档
        //
        //exit 上一个 主 ctrl 和之间的 并行 ctrl
        //先 exit 两个 主 ctrl 之间的 并行 ctrl

        //for (int i = ctrlCacheList.Count - 2; i >= 0; i--)
        //{
        //    BaseCtrl currCtrl = ctrlCacheList[i];
        //    currCtrl.Enter(null);
        //    if (!currCtrl.isParallel)
        //    {
        //        currMainCtrl = currCtrl;
        //    }
        //}

        //for (int i = 0; i < 10; i++)
        //{
        //    BaseCtrl currCtrl = null;
        //    if (currCtrl.isParallel)
        //    {
        //        currCtrl.Exit();
        //    }
        //}

        //BaseCtrl lastMainCtrl = null;
        //if (lastMainCtrl != null)
        //{
        //    lastMainCtrl.Exit();
        //}


    }

    public void Exit<T>() where T : BaseCtrl
    {
        var findCtrl = FindCtrl<T>();
        if (null == findCtrl)
        {
            Logx.LogzError("the findCtrl is null : " + typeof(T));
            return;
        }

        if (findCtrl.isParallel)
        {
            findCtrl.Exit();
            ctrlCacheList.Remove(findCtrl);
            findCtrl.Release();
        }
        else
        {
            //如果不是并行的 ctrl

            //需要判断是不是当前的主 ctrl 
            if (findCtrl == currMainCtrl)
            {
                //先打开上一组 ctrl 之后也改成异步回调之后再关闭
                currMainCtrl = null;
                for (int i = ctrlCacheList.Count - 2; i >= 0; i--)
                {
                    BaseCtrl currCtrl = ctrlCacheList[i];
                    currCtrl.Enter(null);
                    if (!currCtrl.isParallel)
                    {
                        currMainCtrl = currCtrl;
                    }
                }

                findCtrl.Exit();
                ctrlCacheList.Remove(findCtrl);
                findCtrl.Release();
            }
            else
            {
                Logx.LogzError("findCtrl == ctrl : " + typeof(T) + " != " + currMainCtrl?.GetType());
                return;
            }
        }
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

    //public BaseCtrl GetCurrMainCtrl()
    //{
    //    //for (int i = 0; i < ctrlCacheList.Count; i++)
    //    //{
    //    //    var ctrl = ctrlCacheList[i];
    //    //    if (ctrl.GetType() == typeof(T))
    //    //    {
    //    //        return ctrl;
    //    //    }
    //    //}
    //}
}