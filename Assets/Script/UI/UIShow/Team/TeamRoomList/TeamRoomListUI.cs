using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;


public class TeamRoomListUI : BaseUI
{
    //this callback
    public Action event_onClickCloseBtn;
    public Action event_onClickCreateBtn;

    //single item callback
    public Action<int> event_onClickSingleRoomJoinBtn;

    //ui component
    Button closeBtn;
    Transform roomRoot;
    Button createBtn;

    //data
    public List<TeamRoomUIData> roomDataList;
    public List<TeamRoomUIShowObj> roomShowObjList;

    protected override void OnInit()
    {
        roomRoot = transform.Find("scroll/mask/content");
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        createBtn = transform.Find("createBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(() =>
        {
            event_onClickCloseBtn?.Invoke();
        });
        createBtn.onClick.AddListener(() =>
        {
            event_onClickCreateBtn?.Invoke();
        });

        roomDataList = new List<TeamRoomUIData>();
        roomShowObjList = new List<TeamRoomUIShowObj>();
    }

    public override void Refresh(UIArgs args)
    {
        TeamRoomListUIArgs teamArgs = (TeamRoomListUIArgs)args;
        roomDataList = teamArgs.roomDataList;
        this.RefreshRoomList();
    }

    void RefreshRoomList()
    {
        //列表
        UIListArgs<TeamRoomUIShowObj, TeamRoomListUI> args = new UIListArgs<TeamRoomUIShowObj, TeamRoomListUI>();
        args.dataList = roomDataList;
        args.showObjList = roomShowObjList;
        args.root = roomRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);
    }

    public void OnClickSingleRoomJoinBtn(int roomId)
    {
        event_onClickSingleRoomJoinBtn?.Invoke(roomId);
    }

    protected override void OnRelease()
    {
        event_onClickCloseBtn = null;
        event_onClickCreateBtn = null;
        event_onClickSingleRoomJoinBtn = null;

        foreach (var item in roomShowObjList)
        {
            item.Release();
        }

        closeBtn.onClick.RemoveAllListeners();
        createBtn.onClick.RemoveAllListeners();
    }
}

//-----------------------------------------

public class TeamRoomUIData
{
    public int id;
    public int teamStageId;
    public string roomName;
    public int currPlayerCount;
    public int totalPlayerCount;
}


public class TeamRoomListUIArgs : UIArgs
{
    public List<TeamRoomUIData> roomDataList;
}

public class TeamRoomUIShowObj : BaseUIShowObj<TeamRoomListUI>
{
    Text idText;
    Text roomNameText;
    Text stageNameText;
    Text roomPlayerCountText;

    Button joinBtn;

    TeamRoomUIData uiData;

    public override void OnInit()
    {
        idText = transform.Find("id").GetComponent<Text>();
        roomNameText = transform.Find("name").GetComponent<Text>();
        stageNameText = transform.Find("stageName").GetComponent<Text>();
        roomPlayerCountText = transform.Find("playerCount").GetComponent<Text>();

        joinBtn = transform.Find("joinBtn").GetComponent<Button>();

        joinBtn.onClick.AddListener(() =>
        {
            this.parentObj.OnClickSingleRoomJoinBtn(this.uiData.id);
        });
    }

    public override void OnRefresh(object data, int index)
    {
        this.uiData = (TeamRoomUIData)data;
        var teamStageId = this.uiData.teamStageId;
        var currTeamStageTb = Table.TableManager.Instance.GetById<Table.TeamStage>(teamStageId);

        idText.text = "" + uiData.id;
        roomNameText.text = uiData.roomName;
        stageNameText.text = currTeamStageTb.Name;
        roomPlayerCountText.text = uiData.currPlayerCount + "/" + uiData.totalPlayerCount;
    }

    public override void OnRelease()
    {
        joinBtn.onClick.RemoveAllListeners();
    }
}

