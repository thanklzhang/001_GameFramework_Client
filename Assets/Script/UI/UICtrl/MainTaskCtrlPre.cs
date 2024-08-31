// using System;
// using System.Collections;
// using System.Collections.Generic;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
// //public class HeroListCtrlArgs : CtrlArgs
// //{
// //    public Action yesAction;
// //    public Action noAction;
// //    public Action closeAction;
// //}
//
// //主线任务 ctrl
// public class MainTaskCtrlPre : BaseCtrl_pre
// {
//     MainTaskUIPre _mainUIPre;
//     MainTaskStageUIPre _stageUIPre;
//
//
//     int currSelectChapterId;
//     int currSelectStageId;
//
//     public override void OnInit()
//     {
//         //this.isParallel = false;
//     }
//     public override void OnStartLoad()
//     {
//         this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
//         {
//             new LoadUIRequest<MainTaskUIPre>(){selfFinishCallback = OnMainUILoadFinish},
//             new LoadUIRequest<MainTaskStageUIPre>(){selfFinishCallback = OnStageUILoadFinish},
//         });
//         //UIManager.Instance.LoadUI<HeroListUI>();
//
//     }
//
//     public void OnMainUILoadFinish(MainTaskUIPre mainUIPre)
//     {
//         this._mainUIPre = mainUIPre;
//     }
//
//     public void OnStageUILoadFinish(MainTaskStageUIPre stageUIPre)
//     {
//         this._stageUIPre = stageUIPre;
//     }
//
//     public override void OnLoadFinish()
//     {
//
//     }
//
//     public void OnClickCloseBtn()
//     {
//         CtrlManager.Instance.Exit<MainTaskCtrlPre>();
//     }
//
//     public void OnClickStageCloseBtn()
//     {
//         this.currSelectChapterId = 0;
//         this.currSelectStageId = 0;
//
//         _stageUIPre.Hide();
//     }
//
//     public override void OnEnter(CtrlArgs args)
//     {
//
//     }
//
//     public void OnClickTitleCloseBtn()
//     {
//         if (_stageUIPre.IsShow())
//         {
//             this.OnClickStageCloseBtn();
//         }
//         else
//         {
//             this.OnClickCloseBtn();
//         }
//     }
//
//     public override void OnActive()
//     {
//         //待定
//         CtrlManager.Instance.ShowTitleBar(0);
//
//         EventDispatcher.AddListener(EventIDs.OnTitleBarClickCloseBtn, OnClickTitleCloseBtn);
//         EventDispatcher.AddListener(EventIDs.OnRefreshAllMainTaskData, OnRefreshAllMainTaskData);
//
//         _mainUIPre.Show();
//         _stageUIPre.Hide();
//         _mainUIPre.onClickCloseBtn += OnClickCloseBtn;
//         _mainUIPre.onClickChapterBtn += OnClickChapterBtn;
//         _mainUIPre.onClickReceiveBtn += OnClickChapterReceiveBtn;
//
//         _stageUIPre.onClickCloseBtn += OnClickStageCloseBtn;
//         _stageUIPre.onClickStageBtn += OnClickStageBtn;
//         _stageUIPre.onClickStartBtn += onClickStageStartBtn;
//         _stageUIPre.onClickReceiveBtn += onClickStageReceiveBtn;
//
//         //EventDispatcher.AddListener<HeroData>(EventIDs.OnUpgradeHeroLevel, OnUpgradeHeroLevel);
//
//         currSelectChapterId = 0;
//         currSelectStageId = 0;
//
//         RefreshAll();
//
//     }
//
//     public void RefreshAll()
//     {
//         RefreshChapterUI();
//         RefreshStageUI();
//
//     }
//
//     public void RefreshChapterUI()
//     {
//         MainTaskUIArgs uiArgs = ConvertToMainTaskUIArgs();
//         _mainUIPre.Refresh(uiArgs);
//
//     }
//
//     public override void OnInactive()
//     {
//         EventDispatcher.RemoveListener(EventIDs.OnTitleBarClickCloseBtn, OnClickTitleCloseBtn);
//         EventDispatcher.RemoveListener(EventIDs.OnRefreshAllMainTaskData, OnRefreshAllMainTaskData);
//
//         _mainUIPre.Hide();
//         _stageUIPre.Hide();
//
//         _mainUIPre.onClickCloseBtn -= OnClickCloseBtn;
//         _mainUIPre.onClickChapterBtn -= OnClickChapterBtn;
//         _mainUIPre.onClickReceiveBtn -= OnClickChapterReceiveBtn;
//
//         _stageUIPre.onClickCloseBtn -= OnClickStageCloseBtn;
//         _stageUIPre.onClickStageBtn -= OnClickStageBtn;
//         _stageUIPre.onClickStartBtn -= onClickStageStartBtn;
//         _stageUIPre.onClickReceiveBtn -= onClickStageReceiveBtn;
//
//         //EventDispatcher.RemoveListener<HeroData>(EventIDs.OnUpgradeHeroLevel, OnUpgradeHeroLevel);
//     }
//
//     //主线 chapter ui 参数
//     public MainTaskUIArgs ConvertToMainTaskUIArgs()
//     {
//         var mainTaskDataStore = GameData.GameDataManager.Instance.MainTaskStore;
//         MainTaskUIArgs uiArgs = new MainTaskUIArgs();
//
//
//         uiArgs.currChapterId = mainTaskDataStore.GetCurrChapterId();
//         uiArgs.chapterUIDataList = new List<MainTaskChapterUIData>();
//
//         var allTbData = ConfigManager.Instance.GetList<Config.MainTaskChapter>();
//
//         foreach (var tbData in allTbData)
//         {
//             MainTaskChapterUIData uiData = new MainTaskChapterUIData();
//             uiData.chapterId = tbData.Id;
//             var chapterData = mainTaskDataStore.GetChapterById(tbData.Id);
//             if (chapterData != null)
//             {
//                 uiData.state = chapterData.state;
//             }
//             else
//             {
//                 uiData.state = MainTaskChapterState.Lock;
//             }
//
//             uiArgs.chapterUIDataList.Add(uiData);
//         }
//
//         return uiArgs;
//     }
//
//     //点击了一个章节按钮
//     //chapterId 这里应该是 guid 这样保证只有一个变量 否则一旦 chapterId 一样就得另加变量了
//     public void OnClickChapterBtn(int chapterId)
//     {
//         currSelectChapterId = chapterId;
//         _stageUIPre.Show();
//         RefreshStageUI();
//
//         _stageUIPre.OnClickStageBtn(this.currSelectStageId);
//
//     }
//
//
//     public void OnClickChapterReceiveBtn(int chapterId)
//     {
//         var netHandler = NetHandlerManager.Instance.GetHandler<MainTaskNetHandler>();
//         netHandler.SendReceiveRward(1, chapterId, null);
//     }
//
//     ////////////
//
//     //点击 stageUI 中的一项 stage
//     public void OnClickStageBtn(int stageId)
//     {
//         this.currSelectStageId = stageId;
//         RefreshStageUI();
//
//         //var mainTaskDataStore = GameData.GameDataManager.Instance.MainTaskStore;
//         //var currStageId = mainTaskDataStore.GetCurrStageIdByChapter(chapterId);
//         //stageUI.OnClickStageBtn(currStageId);
//
//     }
//
//     public void onClickStageStartBtn(int stageId)
//     {
//         //var netHandler = NetHandlerManager.Instance.GetHandler<MainTaskNetHandler>();
//         ////netHandler.SendFinishStage(this.currSelectChapterId, stageId, null);
//         //netHandler.SendApplyMainTaskBattle(this.currSelectChapterId, stageId, null);
//
//         var net = NetHandlerManager.Instance.GetHandler<BattleEntranceNetHandler>();
//         net.ApplyMainTaskBattle(this.currSelectChapterId, stageId, () =>
//          {
//
//          });
//
//
//     }
//
//     public void onClickStageReceiveBtn(int stageId)
//     {
//         var netHandler = NetHandlerManager.Instance.GetHandler<MainTaskNetHandler>();
//         netHandler.SendReceiveRward(0, stageId, null);
//     }
//
//     public void RefreshStageUI()
//     {
//         var chapterId = currSelectChapterId;
//         if (0 == chapterId)
//         {
//             return;
//         }
//
//         var mainTaskDataStore = GameData.GameDataManager.Instance.MainTaskStore;
//
//         if (0 == this.currSelectStageId)
//         {
//             //第一次选择
//             this.currSelectStageId = mainTaskDataStore.GetCurrStageIdByChapter(chapterId);
//             if (0 == this.currSelectStageId)
//             {
//                 //没有找到 那么选择第一个
//                 var chapterData = mainTaskDataStore.GetChapterById(chapterId);
//                 this.currSelectStageId = chapterData.GetStageList()[0].id;
//             }
//
//         }
//         MainTaskStageUIArgs uiArgs = ConvertToMainTaskStageIUArgs(chapterId, this.currSelectStageId);
//         _stageUIPre.Refresh(uiArgs);
//
//     }
//
//     //主线 stage ui 参数
//     public MainTaskStageUIArgs ConvertToMainTaskStageIUArgs(int chapterId, int currSelectId)
//     {
//         var mainTaskDataStore = GameData.GameDataManager.Instance.MainTaskStore;
//         MainTaskStageUIArgs uiArgs = new MainTaskStageUIArgs();
//
//         var stageList = mainTaskDataStore.GetStageListByChapterId(chapterId);
//
//         uiArgs.chapterId = chapterId;
//         uiArgs.currStageId = currSelectId;
//         //uiArgs.currStageId = mainTaskDataStore.GetCurrStageIdByChapter(chapterId);
//         uiArgs.stageUIDataList = new List<MainTaskStageUIData>();
//
//         var stageIds = MainTaskChapter_Tool.GetStageIdsByChapterId(chapterId);
//         foreach (var stageId in stageIds)
//         {
//             MainTaskStageUIData uiData = new MainTaskStageUIData();
//             uiData.stageId = stageId;
//             var stageData = mainTaskDataStore.GetStageData(chapterId, stageId);
//             if (stageData != null)
//             {
//                 uiData.state = stageData.state;
//             }
//             else
//             {
//                 uiData.state = MainTaskStageState.Lock;
//             }
//
//             uiArgs.stageUIDataList.Add(uiData);
//         }
//
//         return uiArgs;
//     }
//
//
//     public void OnRefreshAllMainTaskData()
//     {
//         RefreshAll();
//     }
//
//     public override void OnExit()
//     {
//         UIManager.Instance.UnloadUI<MainTaskUIPre>();
//         UIManager.Instance.UnloadUI<MainTaskStageUIPre>();
//     }
//
// }
