// using GameData;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// //英雄列表 ctrl
// public class TeamCtrlPre : BaseCtrl_pre
// {
//     TeamRoomListUIPre _roomListUIPre;
//
//     TeamRoomInfoUIPre _roomInfoUIPre;
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
//             new LoadUIRequest<TeamRoomListUIPre>() { selfFinishCallback = OnRoomListUILoadFinish },
//             new LoadUIRequest<TeamRoomInfoUIPre>() { selfFinishCallback = OnRoomInfoUILoadFinish },
//         });
//     }
//
//     public void OnRoomListUILoadFinish(TeamRoomListUIPre roomListUIPre)
//     {
//         this._roomListUIPre = roomListUIPre;
//     }
//
//     public void OnRoomInfoUILoadFinish(TeamRoomInfoUIPre roomInfoUIPre)
//     {
//         this._roomInfoUIPre = roomInfoUIPre;
//     }
//
//     public override void OnLoadFinish()
//     {
//     }
//
//     public void OnRoomListUIClickCloseBtn()
//     {
//         CtrlManager.Instance.Exit<TeamCtrlPre>();
//     }
//
//     public void OnRoomListUIClickCreateBtn()
//     {
//         var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
//         net.SendCreateTeamRoom(() =>
//         {
//             var creatRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//             this.OnEnterRoomInfoUI(creatRoomData);
//         });
//     }
//
//     public void OnEnterRoomInfoUI(TeamRoomData creatRoomData)
//     {
//         CtrlManager.Instance.ShowTitleBar(TitleBarIds.TeamRoomInfo);
//         
//         this._roomListUIPre.Hide();
//         this._roomInfoUIPre.Show();
//
//         this.RefreshRoomInfoUI(creatRoomData);
//     }
//
//     public void RefreshRoomInfoUI()
//     {
//         var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//         this.RefreshRoomInfoUI(enterRoomData);
//     }
//
//     public void RefreshRoomInfoUI(TeamRoomData roomData)
//     {
//         List<TeamRoomData> roomList = GameDataManager.Instance.TeamStore.RoomList;
//
//         var selfUid = GameDataManager.Instance.UserStore.Uid;
//
//         TeamRoomInfoUIArgs args = new TeamRoomInfoUIArgs();
//         args.id = roomData.id;
//         //var teamStageTb = Table.TableManager.Instance.GetById<Table.TeamStage>(roomData.teamStageId);
//         args.stageConfigId = roomData.teamStageId;
//         args.roomName = roomData.roomName;
//         args.playerUIDataList = new List<TeamRoomPlayerUIData>();
//
//         foreach (var player in roomData.playerList)
//         {
//             TeamRoomPlayerUIData uiPlayer = ConvertToPlayerUIData(player);
//             args.playerUIDataList.Add(uiPlayer);
//         }
//
//         _roomInfoUIPre.Refresh(args);
//     }
//
//     public TeamRoomPlayerUIData ConvertToPlayerUIData(TeamRoomPlayerData player)
//     {
//         var selfUid = GameDataManager.Instance.UserStore.Uid;
//         TeamRoomPlayerUIData uiPlayer = new TeamRoomPlayerUIData()
//         {
//             uid = player.playerInfo.uid,
//             name = player.playerInfo.name,
//             level = player.playerInfo.level,
//             avatarURL = player.playerInfo.avatarURL,
//             heroUIData = HeroCardUIConvert.GetUIData(player.selectHeroData),
//             isHasReady = player.isHasReady,
//             isMaster = player.isMaster,
//             isSelf = player.playerInfo.uid == (int)selfUid
//         };
//         return uiPlayer;
//     }
//
//     public void OnClickSingleRoomJoinBtn(int roomId)
//     {
//         //send net
//         var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
//         net.SendEnterTeamRoom(roomId, () =>
//         {
//             var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//             this.OnEnterRoomInfoUI(enterRoomData);
//         });
//     }
//
//     public void OnRoomInfoUIClickCloseBtn()
//     {
//         // TODO : level room
//         //this.roomInfoUI.Hide();
//         //this.roomListUI.Show();
//         //RefreshRoomListUI();
//
//         var currRoom = GameDataManager.Instance.TeamStore.currEnterRoomData;
//         var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
//         net.SendLeaveTeamRoom(currRoom.id, () => { });
//     }
//
//     public void OnRoomInfoUIClickSinglePlayerReadyBtn(int uid)
//     {
//         //send net
//         var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
//         var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//         var player = enterRoomData.playerList.Find(p => p.playerInfo.uid == uid);
//         var roomId = enterRoomData.id;
//         var opReady = !player.isHasReady;
//         net.SendChangeReadyStateInTeamRoom(roomId, opReady, () =>
//         {
//             //enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//             //this.OnEnterRoomInfoUI(enterRoomData);
//         });
//     }
//
//     int currSelectHeroGuid;
//
//     public void OnRoomInfoUIClickSinglePlayerChangeHeroBtn(int uid)
//     {
//         if (uid == (int)GameData.GameDataManager.Instance.UserStore.Uid)
//         {
//             SelectHeroUIArgs args = new SelectHeroUIArgs();
//
//             args.heroCardUIDataList = new List<HeroCardUIData>();
//             var heroList = GameData.GameDataManager.Instance.HeroStore.HeroList;
//             var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//             var playerInfo = enterRoomData.playerList.Find(p => p.playerInfo.uid == uid);
//             for (int i = 0; i < heroList.Count; i++)
//             {
//                 var hero = heroList[i];
//
//                 HeroCardUIData uiData = HeroCardUIConvert.GetUIData(hero);
//                 args.heroCardUIDataList.Add(uiData);
//
//                 if (playerInfo.selectHeroData.guid == hero.guid)
//                 {
//                     currSelectHeroGuid = hero.guid;
//                 }
//             }
//
//             // //默认选择第一个
//             // if (0 == currSelectHeroGuid)
//             // {
//             //     currSelectHeroGuid = heroList[0].guid;
//             // }
//
//             args.event_ClickConfirmBtn = (curr) =>
//             {
//                 //send net
//                 CtrlManager.Instance.GlobalCtrlPre.HideSelectHeroUI();
//
//                 NetProto.csSelectUseHeroInTeamRoom csSelect = new NetProto.csSelectUseHeroInTeamRoom();
//                 var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//
//                 var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
//                 currSelectHeroGuid = curr;
//                 CtrlManager.Instance.GlobalCtrlPre.SelectHero(currSelectHeroGuid);
//                 
//                 net.SendSelectUseHeroInTeamRoom(enterRoomData.id, currSelectHeroGuid);
//             };
//
//             args.event_ClickOneHeroOption = (guid) =>
//             {
//                 currSelectHeroGuid = guid;
//                 CtrlManager.Instance.GlobalCtrlPre.SelectHero(currSelectHeroGuid);
//             };
//
//             args.currSelectHeroGuid = currSelectHeroGuid;
//
//             CtrlManager.Instance.GlobalCtrlPre.ShowSelectHeroUI(args);
//         }
//     }
//
//     public void OnRoomInfoUIClickStartBattleBtn()
//     {
//         var net = NetHandlerManager.Instance.GetHandler<BattleEntranceNetHandler>();
//         var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
//         var tamRoomId = enterRoomData.id;
//         net.ApplyTeamBattle(tamRoomId, () => { });
//     }
//
//     public override void OnEnter(CtrlArgs args)
//     {
//     }
//
//     public void SendNet(Action action)
//     {
//         var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
//         net.SendGetTeamRoomList(() => { action?.Invoke(); });
//     }
//
//     public void OnClickTitleCloseBtn()
//     {
//         if (_roomInfoUIPre.IsShow())
//         {
//             this.OnRoomInfoUIClickCloseBtn();
//         }
//         else
//         {
//             this.OnRoomListUIClickCloseBtn();
//         }
//     }
//
//     public override void OnActive()
//     {
//         CtrlManager.Instance.ShowTitleBar(TitleBarIds.TeamRoomList);
//
//
//         this._roomListUIPre.Show();
//         this._roomInfoUIPre.Hide();
//
//
//         _roomListUIPre.event_onClickCloseBtn += OnRoomListUIClickCloseBtn;
//         _roomListUIPre.event_onClickCreateBtn += OnRoomListUIClickCreateBtn;
//         _roomListUIPre.event_onClickSingleRoomJoinBtn += OnClickSingleRoomJoinBtn;
//
//         _roomInfoUIPre.event_onClickCloseBtn += OnRoomInfoUIClickCloseBtn;
//         _roomInfoUIPre.event_onClickSinglePlayerReadyBtn += OnRoomInfoUIClickSinglePlayerReadyBtn;
//         _roomInfoUIPre.event_onClickSinglePlayerChangeHeroBtn += OnRoomInfoUIClickSinglePlayerChangeHeroBtn;
//         _roomInfoUIPre.event_onClickStartBattleBtn += OnRoomInfoUIClickStartBattleBtn;
//
//         EventDispatcher.AddListener(EventIDs.OnTitleBarClickCloseBtn, OnClickTitleCloseBtn);
//         EventDispatcher.AddListener(EventIDs.OnPlayerChangeInfoInTeamRoom, OnPlayerChangeInfoInTamRoom);
//         EventDispatcher.AddListener<int>(EventIDs.OnPlayerLeaveTeamRoom, OnPlayerLeaveTeamRoom);
//
//         SendNet(() => { RefreshAll(); });
//     }
//
//     //有玩家改变状态
//     public void OnPlayerChangeInfoInTamRoom()
//     {
//         this.RefreshRoomInfoUI();
//     }
//
//     //有玩家离开
//     public void OnPlayerLeaveTeamRoom(int uid)
//     {
//         var selfUid = GameDataManager.Instance.UserStore.Uid;
//         if ((int)selfUid == uid)
//         {
//             // Logx.Log("room info : self leave room ");
//             //自己退出房间 或者被 t
//             GameDataManager.Instance.TeamStore.SetCurrEnterRoomData(null);
//             this._roomInfoUIPre.Hide();
//             this._roomListUIPre.Show();
//             CtrlManager.Instance.ShowTitleBar(TitleBarIds.TeamRoomList);
//             
//             SendNet(() => { RefreshAll(); });
//         }
//         else
//         {
//             // Logx.Log("room info : other player leave room , uid : " + uid);
//             //有人退出房间
//             RefreshRoomInfoUI();
//         }
//     }
//
//     public void RefreshAll()
//     {
//         this.RefreshRoomListUI();
//     }
//
//     public void RefreshRoomListUI()
//     {
//         TeamRoomListUIArgs args = new TeamRoomListUIArgs();
//         args.roomDataList = new List<TeamRoomUIData>();
//
//         List<TeamRoomData> roomList = GameDataManager.Instance.TeamStore.RoomList;
//         if (null == roomList)
//         {
//             roomList = new List<TeamRoomData>();
//         }
//
//         foreach (var roomData in roomList)
//         {
//             var teamStageTb = Table.TableManager.Instance.GetById<Table.TeamStage>(roomData.teamStageId);
//             TeamRoomUIData uiRoomData = new TeamRoomUIData()
//             {
//                 id = roomData.id,
//                 teamStageId = roomData.teamStageId,
//                 roomName = roomData.roomName,
//                 currPlayerCount = roomData.playerList.Count,
//                 totalPlayerCount = teamStageTb.MaxPlayerCount,
//                 
//             };
//             
//             //房主信息
//             var master = roomData.playerList.Find((pData) => { return pData.isMaster; });
//             uiRoomData.masterName = master.playerInfo.name;
//             uiRoomData.masterAvatarURL = master.playerInfo.avatarURL;
//             
//             args.roomDataList.Add(uiRoomData);
//         }
//
//         _roomListUIPre.Refresh(args);
//     }
//
//     public override void OnInactive()
//     {
//         _roomListUIPre.Hide();
//         _roomInfoUIPre.Hide();
//
//         _roomListUIPre.event_onClickCloseBtn -= OnRoomListUIClickCloseBtn;
//         _roomListUIPre.event_onClickCreateBtn -= OnRoomListUIClickCreateBtn;
//         _roomListUIPre.event_onClickSingleRoomJoinBtn -= OnClickSingleRoomJoinBtn;
//
//         _roomInfoUIPre.event_onClickCloseBtn -= OnRoomInfoUIClickCloseBtn;
//         _roomInfoUIPre.event_onClickSinglePlayerReadyBtn -= OnRoomInfoUIClickSinglePlayerReadyBtn;
//         _roomInfoUIPre.event_onClickSinglePlayerChangeHeroBtn -= OnRoomInfoUIClickSinglePlayerChangeHeroBtn;
//         _roomInfoUIPre.event_onClickStartBattleBtn -= OnRoomInfoUIClickStartBattleBtn;
//
//         EventDispatcher.RemoveListener(EventIDs.OnTitleBarClickCloseBtn, OnClickTitleCloseBtn);
//         EventDispatcher.RemoveListener(EventIDs.OnPlayerChangeInfoInTeamRoom, OnPlayerChangeInfoInTamRoom);
//         EventDispatcher.RemoveListener<int>(EventIDs.OnPlayerLeaveTeamRoom, OnPlayerLeaveTeamRoom);
//     }
//
//     public override void OnExit()
//     {
//         UIManager.Instance.UnloadUI<TeamRoomListUIPre>();
//         UIManager.Instance.UnloadUI<TeamRoomInfoUIPre>();
//     }
// }
//
// public enum TitleBarIds
// {
//     Null = 0,
//     Lobby = 1,
//     HeroList = 2,
//     TeamRoomList = 3,
//     TeamRoomInfo = 4
// }