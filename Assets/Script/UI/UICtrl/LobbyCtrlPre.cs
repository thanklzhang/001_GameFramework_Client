// using GameData;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// 
// using UnityEngine;
// using UnityEngine.UI;
//
// //英雄列表 ctrl
// public class LobbyCtrlPre : BaseCtrl_pre
// {
//     LobbyUIPre _uiPre;
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
//             new LoadUIRequest<LobbyUIPre>() { selfFinishCallback = OnUILoadFinish },
//             //new LoadUIRequest<TitleBarUI>(){selfFinishCallback = OnTitleUILoadFinish},
//         });
//     }
//
//     public void OnUILoadFinish(LobbyUIPre lobbyUIPre)
//     {
//         this._uiPre = lobbyUIPre;
//     }
//
//
//     //public void OnTitleUILoadFinish(TitleBarUI titleBarUI)
//     //{
//     //    this.titleBarUI = titleBarUI;
//     //}
//
//     public override void OnLoadFinish()
//     {
//         _uiPre.onClickHeroListBtn += OnClickHeroListBtn;
//         _uiPre.onClickMainTaskBtn += OnClickMainTaskBtn;
//         _uiPre.onClickTeamBtn += OnClickTeamBtn;
//     }
//
//     public void OnClickHeroListBtn()
//     {
//         CtrlManager.Instance.Enter<HeroListCtrlPre>();
//     }
//
//     public void OnClickMainTaskBtn()
//     {
//         CtrlManager.Instance.Enter<MainTaskCtrlPre>();
//     }
//
//     public void OnClickTeamBtn()
//     {
//         //var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
//         //net.SendGetTeamRoomList(() =>
//         //{
//         //    CtrlManager.Instance.Enter<TeamCtrl>();
//         //});
//
//         CtrlManager.Instance.Enter<TeamCtrlPre>();
//     }
//
//     public override void OnEnter(CtrlArgs args)
//     {
//         Logx.Log(LogxType.Game,"enter game lobby success");
//         
//         //play bgm
//         AudioManager.Instance.PlayBGM((int)ResIds.bgm_002);
//     }
//
//     public override void OnActive()
//     {
//         CtrlManager.Instance.ShowTitleBar(TitleBarIds.Lobby);
//
//
//       
//         
//         _uiPre.Show();
//         //titleBarUI.Show();
//
//         RefreshAll();
//     }
//
//     public void RefreshAll()
//     {
//         ////title
//         //RefreshTitleBarUI();
//
//
//         //lobby
//         var playerInfo = GameDataManager.Instance.UserStore.PlayerInfo;
//         LobbyUIArg arg = new LobbyUIArg()
//         {
//             playerName = playerInfo.name,
//             playerLevel = playerInfo.level,
//             avatarURL = playerInfo.avatarURL
//         };
//         this._uiPre.Refresh(arg);
//     }
//
//     //public void RefreshTitleBarUI()
//     //{ //给标题栏 ui 提供数据
//     //    TitleBarUIArgs titleArgs = new TitleBarUIArgs();
//     //    titleArgs.optionList = new List<TitleOptionUIData>();
//
//     //    //先就显示一个
//     //    var bagStore = GameDataManager.Instance.BagStore;
//     //    TitleOptionUIData optionData = new TitleOptionUIData();
//     //    optionData.configId = 22000001;
//     //    optionData.count = bagStore.GetCountByConfigId(optionData.configId);
//
//     //    titleArgs.optionList.Add(optionData);
//
//     //    titleBarUI.Refresh(titleArgs);
//
//     //}
//
//     public override void OnInactive()
//     {
//         _uiPre.Hide();
//         //titleBarUI.Hide();
//     }
//
//     public override void OnExit()
//     {
//         _uiPre.onClickHeroListBtn -= OnClickHeroListBtn;
//         _uiPre.onClickMainTaskBtn -= OnClickMainTaskBtn;
//         _uiPre.onClickTeamBtn -= OnClickTeamBtn;
//
//         //UIManager.Instance.ReleaseUI<LobbyUI>();
//     }
// }