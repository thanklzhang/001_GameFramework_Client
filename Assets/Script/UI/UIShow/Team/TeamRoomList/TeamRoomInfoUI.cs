using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;


public class TeamRoomInfoUI : BaseUI
{
    //this callback
    public Action event_onClickCloseBtn;
    public Action event_onClickStartBattleBtn;

    //single item callback
    public Action<int> event_onClickSinglePlayerReadyBtn;
    public Action<int> event_onClickSinglePlayerChangeHeroBtn;

    //ui component
    Button closeBtn;
    Button startBattleBtn;
    Transform playerRoot;

    Text roomNameText;
    Text stageNameText;


    //data
    List<TeamRoomPlayerUIData> roomPlayerDataList;
    List<TeamRoomPlayerUIShowObj> roomPlayerShowObjList;
    string roomName;
    string stageName;

    protected override void OnInit()
    {
        playerRoot = transform.Find("scroll/mask/content");
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        startBattleBtn = transform.Find("battleBtn").GetComponent<Button>();


        roomNameText = transform.Find("roomName").GetComponent<Text>();
        stageNameText = transform.Find("stageName").GetComponent<Text>();

        closeBtn.onClick.AddListener(() =>
        {
            event_onClickCloseBtn?.Invoke();
        });
        startBattleBtn.onClick.AddListener(() =>
        {
            event_onClickStartBattleBtn?.Invoke();
        });

        roomPlayerDataList = new List<TeamRoomPlayerUIData>();
        roomPlayerShowObjList = new List<TeamRoomPlayerUIShowObj>();
    }

    public override void Refresh(UIArgs args)
    {
        TeamRoomInfoUIArgs roomInfoArgs = (TeamRoomInfoUIArgs)args;

        roomPlayerDataList = roomInfoArgs.playerUIDataList;
        roomName = roomInfoArgs.roomName;
        stageName = roomInfoArgs.stageName;

        this.RefreshRoomInfo();
    }

    void RefreshRoomInfo()
    {
        //玩家列表
        UIListArgs<TeamRoomPlayerUIShowObj, TeamRoomInfoUI> args = new UIListArgs<TeamRoomPlayerUIShowObj, TeamRoomInfoUI>();
        args.dataList = roomPlayerDataList;
        args.showObjList = roomPlayerShowObjList;
        args.root = playerRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);

        //房间信息
        roomNameText.text = roomName;
        stageNameText.text = stageName;
    }

    public void OnClickSinglePlayerReadyBtn(int uid)
    {
        //实际上只能是自己
        event_onClickSinglePlayerReadyBtn?.Invoke(uid);
    }

    protected override void OnRelease()
    {
        event_onClickCloseBtn = null;
        event_onClickStartBattleBtn = null;
        event_onClickSinglePlayerReadyBtn = null;
        event_onClickSinglePlayerChangeHeroBtn = null;

        foreach (var item in roomPlayerShowObjList)
        {
            item.Release();
        }

        closeBtn.onClick.RemoveAllListeners();
        startBattleBtn.onClick.RemoveAllListeners();
    }
}

//-----------------------------------------

public class TeamRoomPlayerUIData
{
    public int uid;
    public string name;
    public string avatarURL;
    public int level;

    public bool isMaster;
    public bool isHasReady;
    public HeroCardUIData heroUIData;
    public bool isSelf;
}


public class TeamRoomInfoUIArgs : UIArgs
{
    public int id;
    public string stageName;
    public string roomName;
    public List<TeamRoomPlayerUIData> playerUIDataList;
}

public class TeamRoomPlayerUIShowObj : BaseUIShowObj<TeamRoomInfoUI>
{
    Text idText;
    Text nameText;
    Text levelText;

    RawImage playerAvatarImg;

    RawImage heroAvatarImg;
    Text heroLevelText;
    Text heroNameText;
    Button changeHeroBtn;

    Button readyBtn;
    Text readyBtnText;
    Text noReadyText;
    Text hasReadyText;


    Transform showRoot;
    Transform noShowRoot;

    TeamRoomPlayerUIData uiPlayerData;

    public override void OnInit()
    {
        showRoot = transform.Find("show");
        noShowRoot = transform.Find("noShow");

        idText = showRoot.Find("id").GetComponent<Text>();
        nameText = showRoot.Find("name").GetComponent<Text>();
        levelText = showRoot.Find("level").GetComponent<Text>();
        noReadyText = showRoot.Find("noReady").GetComponent<Text>();
        hasReadyText = showRoot.Find("hasReadyText").GetComponent<Text>();
        readyBtn = showRoot.Find("readyBtn").GetComponent<Button>();
        readyBtnText = readyBtn.transform.Find("Text").GetComponent<Text>();
        changeHeroBtn = showRoot.Find("changeHeroBtn").GetComponent<Button>();

        //avatarImg = transform.Find("joinBtn").GetComponent<RawImage>();

        heroAvatarImg = showRoot.Find("selectHeroIcon").GetComponent<RawImage>();
        heroLevelText = showRoot.Find("heroLevelText").GetComponent<Text>();
        heroNameText = showRoot.Find("heroNameText").GetComponent<Text>();

        readyBtn.onClick.AddListener(() =>
        {
            this.parentObj.OnClickSinglePlayerReadyBtn(this.uiPlayerData.uid);
        });

        changeHeroBtn.onClick.AddListener(() =>
        {
            this.parentObj.event_onClickSinglePlayerChangeHeroBtn(this.uiPlayerData.uid);
        });

    }

    public override void OnRefresh(object data, int index)
    {
        this.uiPlayerData = (TeamRoomPlayerUIData)data;
        //var teamStageId = this.uiPlayerData.teamStageId;
        //var currTeamStageTb = Table.TableManager.Instance.GetById<Table.TeamStage>(teamStageId);

        idText.text = "" + uiPlayerData.uid;
        nameText.text = uiPlayerData.name;
        levelText.text = "" + uiPlayerData.level;

        var heroConfigId = uiPlayerData.heroUIData.configId;
        var heroConfig = TableManager.Instance.GetById<Table.EntityInfo>(heroConfigId);
        heroLevelText.text = "" + uiPlayerData.heroUIData.level;
        heroNameText.text = "" + heroConfig.Name;

        var userStore = GameData.GameDataManager.Instance.UserStore;
        var isShowChangeHeroBtn = (int)userStore.Uid == this.uiPlayerData.uid;
        changeHeroBtn.gameObject.SetActive(isShowChangeHeroBtn);

        if (uiPlayerData.isSelf)
        {
            if (uiPlayerData.isHasReady)
            {
                noReadyText.gameObject.SetActive(false);
                hasReadyText.gameObject.SetActive(false);
                readyBtn.gameObject.SetActive(true);

                readyBtnText.text = "cancelReady";
            }
            else
            {
                noReadyText.gameObject.SetActive(false);
                hasReadyText.gameObject.SetActive(false);
                readyBtn.gameObject.SetActive(true);

                readyBtnText.text = "Ready";
            }
        }
        else
        {
            if (uiPlayerData.isHasReady)
            {
                noReadyText.gameObject.SetActive(false);
                hasReadyText.gameObject.SetActive(true);
                readyBtn.gameObject.SetActive(false);
            }
            else
            {
                noReadyText.gameObject.SetActive(true);
                hasReadyText.gameObject.SetActive(false);
                readyBtn.gameObject.SetActive(false);
            }
        }

    }

    public override void OnRelease()
    {
        readyBtn.onClick.RemoveAllListeners();
        changeHeroBtn.onClick.RemoveAllListeners();
    }
}

