// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class CtrlManager : Singleton<CtrlManager>
// {
//     public List<BaseCtrl> ctrlCacheList = new List<BaseCtrl>();
//
//     public GlobalCtrlPre GlobalCtrlPre = new GlobalCtrlPre();
//
//     //public Dictionary<Type, BaseCtrl> ctrlCacheDic = new Dictionary<Type, BaseCtrl>();
//     public BaseCtrl CurrMainCtrlPre;
//     public void Init()
//     {
//         GlobalCtrlPre.Init();
//     }
//
//     public void Enter<T>(CtrlArgs args = null) where T : BaseCtrl, new()
//     {
//         var findCtrl = FindCtrl<T>();
//         if (findCtrl != null)
//         {
//             //已经存在 目前的方案是 ：直接把 ctrl 提到最前面
//             ctrlCacheList.Remove(findCtrl);
//             ctrlCacheList.Add(findCtrl);
//
//             findCtrl.Enter(args);
//             findCtrl.Active();
//
//             CurrMainCtrlPre = findCtrl;
//             //todo : gameObject 的层级提到最高
//         }
//         else
//         {
//             //没找到 开始一个新的 ctrl
//             BaseCtrl newCtrlPre = new T();
//             newCtrlPre.Init();
//
//             ctrlCacheList.Add(newCtrlPre);
//
//             CurrMainCtrlPre = newCtrlPre;
//
//             newCtrlPre.StartLoad(() =>
//             {
//                 newCtrlPre.Enter(args);
//                 newCtrlPre.Active();
//
//                 //上一个 ctrl
//                 var lastIndex = ctrlCacheList.Count - 2;
//                 if (lastIndex >= 0)
//                 {
//                     BaseCtrl currCtrlPre = ctrlCacheList[lastIndex];
//                     currCtrlPre.Inactive();
//                 }
//             });
//         }
//
//     }
//
//     public void Exit<T>() where T : BaseCtrl
//     {
//         var findCtrl = FindCtrl<T>();
//         if (findCtrl == CurrMainCtrlPre)
//         {
//             //先打开上一组 ctrl 之后也改成异步回调之后再关闭
//             CurrMainCtrlPre = null;
//             var nextIndex = ctrlCacheList.Count - 2;
//             findCtrl.Inactive();
//             findCtrl.Exit();
//             ctrlCacheList.Remove(findCtrl);
//
//             if (nextIndex >= 0)
//             {
//                 BaseCtrl currCtrlPre = ctrlCacheList[nextIndex];
//                 currCtrlPre.Active();
//                 CurrMainCtrlPre = currCtrlPre;
//             }
//
//         }
//         else
//         {
//             Logx.LogError("findCtrl == ctrl : " + typeof(T) + " != " + CurrMainCtrlPre?.GetType());
//             return;
//         }
//     }
//
//     public void Update(float deltaTime)
//     {
//         for (int i = 0; i < ctrlCacheList.Count; i++)
//         {
//             var ctrl = ctrlCacheList[i];
//             if (ctrl.state == CtrlState.Loading)
//             {
//                 if (ctrl.CheckLoadFinish())
//                 {
//                     ctrl.LoadFinish();
//                 }
//             }
//             if (ctrl.state == CtrlState.Active)
//             {
//                 ctrl.Update(deltaTime);
//             }
//         }
//
//         //global ctrl TODO:抽象出来 和上面走一套
//         if (this.GlobalCtrlPre.state == CtrlState.Loading)
//         {
//             if (GlobalCtrlPre.CheckLoadFinish())
//             {
//                 GlobalCtrlPre.LoadFinish();
//             }
//         }
//         if (GlobalCtrlPre.state == CtrlState.Active)
//         {
//             GlobalCtrlPre.Update(deltaTime);
//         }
//
//     }
//
//     public void LateUpdate(float deltaTime)
//     {
//         for (int i = 0; i < ctrlCacheList.Count; i++)
//         {
//             var ctrl = ctrlCacheList[i];
//             if (ctrl.state == CtrlState.Active)
//             {
//                 ctrl.LateUpdate(deltaTime);
//             }
//         }
//
//
//         //global ctrl TODO:抽象出来 和上面走一套
//         if (this.GlobalCtrlPre.state == CtrlState.Loading)
//         {
//             if (GlobalCtrlPre.state == CtrlState.Active)
//             {
//                 GlobalCtrlPre.LateUpdate(deltaTime);
//             }
//         }
//     }
//
//
//     public BaseCtrl FindCtrl<T>() where T : BaseCtrl
//     {
//         for (int i = 0; i < ctrlCacheList.Count; i++)
//         {
//             var ctrl = ctrlCacheList[i];
//             if (ctrl.GetType() == typeof(T))
//             {
//                 return ctrl;
//             }
//         }
//         return null;
//     }
//
//
//     //global ctrl---------------------------------------
//
//     public IEnumerator EnterGlobalCtrl()
//     {
//         bool isLoadFinish = false;
//         GlobalCtrlPre.StartLoad(() =>
//         {
//             GlobalCtrlPre.Enter(new CtrlArgs());
//             GlobalCtrlPre.Active();
//             isLoadFinish = true;
//         });
//
//         while (true)
//         {
//             yield return null;
//
//             if (isLoadFinish)
//             {
//                 break;
//             }
//         }
//     }
//
//     public void ExitGlobalCtrl()
//     {
//         GlobalCtrlPre.Inactive();
//         GlobalCtrlPre.Exit();
//     }
//
//     public void ShowTips(string str)
//     {
//         this.GlobalCtrlPre.ShowTips(str);
//     }
//
//     //TODO: 刷新标题栏的话可以在 ctrl 中用监听实现 ， 金币等资源变更了就刷新标题栏 待定
//     public void ShowTitleBar(TitleBarIds titleBarId)
//     {
//         this.GlobalCtrlPre.ShowTitleBar(titleBarId);
//     }
//
//     public void HideTitleBar()
//     {
//         this.GlobalCtrlPre.HideTitleBar();
//     }
//
//     //------------------------------------
// }
//
