// using GameData;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// //全局 ctrl 游戏进程中一直存在 不受正常的 ctrl 管理
// public class GlobalCtrlPre : BaseCtrl_pre
// {
//     TipsUIPre _tipsUIPre;
//     TitleBarUIPre _titleUIPre;
//     SelectHeroUIPre _selectHeroUIPre;
//     public LoadingUIPre LoadingUIPre;
//
//     //TitleBarUI titleBarUI;
//     public override void OnInit()
//     {
//         //this.isParallel = false;
//     }
//
//     public override void OnStartLoad()
//     {
//         this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
//         {
//             new LoadUIRequest<TipsUIPre>() { selfFinishCallback = OnTipsUILoadFinish },
//             new LoadUIRequest<TitleBarUIPre>() { selfFinishCallback = OnTitleUILoadFinish },
//             new LoadUIRequest<SelectHeroUIPre>() { selfFinishCallback = OnSelectHeroUILoadFinish },
//             new LoadUIRequest<LoadingUIPre>() { selfFinishCallback = OnLoadingUILoadFinish }
//         });
//     }
//
//     public void OnTipsUILoadFinish(TipsUIPre tipsUIPre)
//     {
//         this._tipsUIPre = tipsUIPre;
//     }
//
//     public void OnTitleUILoadFinish(TitleBarUIPre titleUIPre)
//     {
//         this._titleUIPre = titleUIPre;
//     }
//
//     public void OnSelectHeroUILoadFinish(SelectHeroUIPre uiPre)
//     {
//         _selectHeroUIPre = uiPre;
//     }
//
//     public void OnLoadingUILoadFinish(LoadingUIPre uiPre)
//     {
//         LoadingUIPre = uiPre;
//         
//         LoadingUIPre.Hide();
//     }
//
//     public override void OnLoadFinish()
//     {
//         this._selectHeroUIPre.Hide();
//     }
//
//     public override void OnEnter(CtrlArgs args)
//     {
//     }
//
//     public override void OnActive()
//     {
//         _tipsUIPre.Show();
//         _titleUIPre.Show();
//         _selectHeroUIPre.Show();
//
//         _titleUIPre.clickCloseBtnAction += OnClickTitleUICloseBtn;
//
//         EventDispatcher.AddListener(EventIDs.OnRefreshBagData, OnRefreshBagData);
//     }
//
//     public void OnRefreshBagData()
//     {
//         if (currTitleBarId == TitleBarIds.Null)
//         {
//             return;
//         }
//
//         this.RefreshAll(currTitleBarId);
//     }
//
//     public void OnClickTitleUICloseBtn()
//     {
//         EventDispatcher.Broadcast(EventIDs.OnTitleBarClickCloseBtn);
//     }
//
//     public override void OnInactive()
//     {
//         _tipsUIPre.Hide();
//         _titleUIPre.Hide();
//         _selectHeroUIPre.Hide();
//
//         EventDispatcher.RemoveListener(EventIDs.OnRefreshBagData, OnRefreshBagData);
//         _titleUIPre.clickCloseBtnAction -= OnClickTitleUICloseBtn;
//     }
//
//     public override void OnExit()
//     {
//     }
//
//     public override void OnUpdate(float deltaTime)
//     {
//         _tipsUIPre.Update(deltaTime);
//         _titleUIPre.Update(deltaTime);
//     }
//
//     //------------------------------
//     public void ShowTips(string tipStr)
//     {
//         _tipsUIPre.Show();
//
//         _tipsUIPre.Refresh(new TipsUIArgs()
//         {
//             tipStr = tipStr
//         });
//     }
//
//     private TitleBarIds currTitleBarId;
//
//     public void ShowTitleBar(TitleBarIds titleBarId)
//     {
//         currTitleBarId = titleBarId;
//         RefreshAll(titleBarId);
//         _titleUIPre.Show();
//         
//     }
//
//     public void RefreshAll(TitleBarIds titleBarId)
//     {
//         currTitleBarId = titleBarId;
//         //给标题栏 ui 提供数据
//         TitleBarUIArgs titleArgs = new TitleBarUIArgs();
//         titleArgs.optionList = new List<TitleOptionUIData>();
//
//         var titleConfig = Table.TableManager.Instance.GetById<Table.TitleBar>((int)titleBarId);
//
//         if (null == titleConfig)
//         {
//             // Logx.LogWarning("the titleConfig is nil : configId : " + titleBarId);
//             _titleUIPre.Hide();
//             return;
//         }
//
//         //资源列表
//         if (!string.IsNullOrEmpty(titleConfig.ResList))
//         {
//             var strs = titleConfig.ResList.Split(',');
//             foreach (var str in strs)
//             {
//                 var resId = int.Parse(str);
//
//                 var bagStore = GameDataManager.Instance.BagStore;
//                 TitleOptionUIData optionData = new TitleOptionUIData();
//                 optionData.configId = resId;
//                 optionData.count = bagStore.GetCountByConfigId(optionData.configId);
//                 titleArgs.optionList.Add(optionData);
//             }
//         }
//
//         //标题名称
//         titleArgs.titleName = titleConfig.TitleName;
//         titleArgs.isShowCloseBtn = 1 == titleConfig.IsShowCloseBtn;
//         titleArgs.isShowBg = 1 == titleConfig.IsShowBg;
//         titleArgs.isShowLine = 1 == titleConfig.IsShowLine;
//
//        
//         _titleUIPre.Refresh(titleArgs);
//     }
//
//     public void HideTitleBar()
//     {
//         _titleUIPre.Hide();
//     }
//
//     //通用选英雄界面------
//
//     //这里可以改 CtrlManager ， 添加新 Ctrl 不会打断之前的 Ctrl 即可
//     public void ShowSelectHeroUI(SelectHeroUIArgs args)
//     {
//         this._selectHeroUIPre.Refresh(args);
//         this._selectHeroUIPre.Show();
//     }
//
//     public void HideSelectHeroUI()
//     {
//         this._selectHeroUIPre.Hide();
//     }
//
//     public void SelectHero(int guid)
//     {
//         this._selectHeroUIPre.SelectHero(guid);
//     }
//     //---------------
// }