using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class TeamCtrl : BaseCtrl
{
    TeamRoomListUI roomListUI;
    TeamRoomInfoUI roomInfoUI;
    //TitleBarUI titleBarUI;
    public override void OnInit()
    {
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<TeamRoomListUI>(){selfFinishCallback = OnRoomListUILoadFinish},
            new LoadUIRequest<TeamRoomInfoUI>(){selfFinishCallback = OnRoomInfoUILoadFinish},
        });
    }

    public void OnRoomListUILoadFinish(TeamRoomListUI roomListUI)
    {
        this.roomListUI = roomListUI;
    }

    public void OnRoomInfoUILoadFinish(TeamRoomInfoUI roomInfoUI)
    {
        this.roomInfoUI = roomInfoUI;
    }


    public override void OnLoadFinish()
    {

    }

    public void OnRoomListUIClickCloseBtn()
    {
        CtrlManager.Instance.Exit<TeamCtrl>();
    }

    public void OnRoomListUIClickCreateBtn()
    {
        var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
        net.SendCreateTeamRoom(() =>
        {

            var creatRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
            this.OnEnterRoomInfoUI(creatRoomData);
        });
    }

    public void OnEnterRoomInfoUI(TeamRoomData creatRoomData)
    {
        this.roomListUI.Hide();
        this.roomInfoUI.Show();

        this.RefreshRoomInfoUI(creatRoomData);
    }

    public void RefreshRoomInfoUI()
    {
        var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
        this.RefreshRoomInfoUI(enterRoomData);
    }

    public void RefreshRoomInfoUI(TeamRoomData roomData)
    {
        List<TeamRoomData> roomList = GameDataManager.Instance.TeamStore.RoomList;

        var selfUid = GameDataManager.Instance.UserStore.Uid;

        TeamRoomInfoUIArgs args = new TeamRoomInfoUIArgs();
        args.id = roomData.id;
        var teamStageTb = Table.TableManager.Instance.GetById<Table.TeamStage>(roomData.teamStageId);
        args.stageName = teamStageTb.Name;
        args.roomName = roomData.roomName;
        args.playerUIDataList = new List<TeamRoomPlayerUIData>();

        foreach (var player in roomData.playerList)
        {
            TeamRoomPlayerUIData uiPlayer = ConvertToPlayerUIData(player);
            args.playerUIDataList.Add(uiPlayer);
        }

        roomInfoUI.Refresh(args);
    }

    public TeamRoomPlayerUIData ConvertToPlayerUIData(TeamRoomPlayerData player)
    {
        var selfUid = GameDataManager.Instance.UserStore.Uid;
        TeamRoomPlayerUIData uiPlayer = new TeamRoomPlayerUIData()
        {
            uid = player.playerInfo.uid,
            name = player.playerInfo.name,
            level = player.playerInfo.level,
            avatarURL = player.playerInfo.avatarURL,
            selectHeroGuid = player.selectHeroGuid,
            isHasReady = player.isHasReady,
            isMaster = player.isMaster,
            isSelf = player.playerInfo.uid == (int)selfUid
        };
        return uiPlayer;
    }

    public void OnClickSingleRoomJoinBtn(int roomId)
    {
        //send net
        var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
        net.SendEnterTeamRoom(roomId, () =>
        {
            var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
            this.OnEnterRoomInfoUI(enterRoomData);
        });
    }

    public void OnRoomInfoUIClickCloseBtn()
    {
        // TODO : level room
        //this.roomInfoUI.Hide();
        //this.roomListUI.Show();
        //RefreshRoomListUI();

        var currRoom = GameDataManager.Instance.TeamStore.currEnterRoomData;
        var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
        net.SendLeaveTeamRoom(currRoom.id, () =>
         {

         });

    }
    public void OnRoomInfoUIClickSinglePlayerReadyBtn(int uid)
    {
        //send net
        var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
        var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
        var player = enterRoomData.playerList.Find(p => p.playerInfo.uid == uid);
        var roomId = enterRoomData.id;
        var opReady = !player.isHasReady;
        net.SendChangeReadyStateInTeamRoom(roomId, opReady, () =>
         {
             //enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
             //this.OnEnterRoomInfoUI(enterRoomData);

         });
    }
    public void OnRoomInfoUIClickStartBattleBtn()
    {
        var net = NetHandlerManager.Instance.GetHandler<BattleEntranceNetHandler>();
        var enterRoomData = GameDataManager.Instance.TeamStore.currEnterRoomData;
        var tamRoomId = enterRoomData.id;
        net.ApplyTeamBattle(tamRoomId, () =>
        {

        });
    }
    public override void OnEnter(CtrlArgs args)
    {

    }

    public void SendNet(Action action)
    {
        var net = NetHandlerManager.Instance.GetHandler<TeamNetHandler>();
        net.SendGetTeamRoomList(() =>
        {
            action?.Invoke();
        });
    }

    public override void OnActive()
    {

        this.roomListUI.Show();
        this.roomInfoUI.Hide();


        roomListUI.event_onClickCloseBtn += OnRoomListUIClickCloseBtn;
        roomListUI.event_onClickCreateBtn += OnRoomListUIClickCreateBtn;
        roomListUI.event_onClickSingleRoomJoinBtn += OnClickSingleRoomJoinBtn;

        roomInfoUI.event_onClickCloseBtn += OnRoomInfoUIClickCloseBtn;
        roomInfoUI.event_onClickSinglePlayerReadyBtn += OnRoomInfoUIClickSinglePlayerReadyBtn;
        roomInfoUI.event_onStartBattleeBtn += OnRoomInfoUIClickStartBattleBtn;


        EventDispatcher.AddListener(EventIDs.OnPlayerChangeInfoInTeamRoom, OnPlayerChangeInfoInTamRoom);
        EventDispatcher.AddListener<int>(EventIDs.OnPlayerLeaveTeamRoom, OnPlayerLeaveTeamRoom);

        SendNet(() =>
        {
            RefreshAll();
        });
        


    }

    //有玩家改变状态
    public void OnPlayerChangeInfoInTamRoom()
    {
        this.RefreshRoomInfoUI();
    }

    //有玩家离开
    public void OnPlayerLeaveTeamRoom(int uid)
    {
        var selfUid = GameDataManager.Instance.UserStore.Uid;
        if ((int)selfUid == uid)
        {
            Logx.Log("room info : self leave room ");
            //自己退出房间 或者被 t
            GameDataManager.Instance.TeamStore.SetCurrEnterRoomData(null);
            this.roomInfoUI.Hide();
            this.roomListUI.Show();

            SendNet(() =>
            {
                RefreshAll();
            });

        }
        else
        {
            Logx.Log("room info : other player leave room , uid : " + uid);
            //有人退出房间
            RefreshRoomInfoUI();
        }
    }

    public void RefreshAll()
    {
        this.RefreshRoomListUI();
    }

    public void RefreshRoomListUI()
    {
        TeamRoomListUIArgs args = new TeamRoomListUIArgs();
        args.roomDataList = new List<TeamRoomUIData>();

        List<TeamRoomData> roomList = GameDataManager.Instance.TeamStore.RoomList;
        if (null == roomList)
        {
            roomList = new List<TeamRoomData>();
        }
        foreach (var roomData in roomList)
        {
            var teamStageTb = Table.TableManager.Instance.GetById<Table.TeamStage>(roomData.teamStageId);
            TeamRoomUIData uiRoomData = new TeamRoomUIData()
            {
                id = roomData.id,
                teamStageId = roomData.teamStageId,
                roomName = roomData.roomName,
                currPlayerCount = roomData.playerList.Count,
                totalPlayerCount = teamStageTb.MaxPlayerCount
            };
            args.roomDataList.Add(uiRoomData);
        }

        roomListUI.Refresh(args);
    }

    public override void OnInactive()
    {
        roomListUI.Hide();
        roomInfoUI.Hide();

        roomListUI.event_onClickCloseBtn -= OnRoomListUIClickCloseBtn;
        roomListUI.event_onClickCreateBtn -= OnRoomListUIClickCreateBtn;
        roomListUI.event_onClickSingleRoomJoinBtn -= OnClickSingleRoomJoinBtn;

        roomInfoUI.event_onClickCloseBtn -= OnRoomInfoUIClickCloseBtn;
        roomInfoUI.event_onClickSinglePlayerReadyBtn -= OnRoomInfoUIClickSinglePlayerReadyBtn;
        roomInfoUI.event_onStartBattleeBtn -= OnRoomInfoUIClickStartBattleBtn;

        EventDispatcher.RemoveListener(EventIDs.OnPlayerChangeInfoInTeamRoom, OnPlayerChangeInfoInTamRoom);
        EventDispatcher.RemoveListener<int>(EventIDs.OnPlayerLeaveTeamRoom, OnPlayerLeaveTeamRoom);
    }

    public override void OnExit()
    {
        UIManager.Instance.ReleaseUI<TeamRoomListUI>();
        UIManager.Instance.ReleaseUI<TeamRoomInfoUI>();
    }


}
