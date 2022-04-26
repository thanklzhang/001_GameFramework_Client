using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class LobbyCtrl : BaseCtrl
{
    LobbyUI ui;
    TitleBarUI titleBarUI;
    public override void OnInit()
    {
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<LobbyUI>(){selfFinishCallback = OnUILoadFinish},
            new LoadUIRequest<TitleBarUI>(){selfFinishCallback = OnTitleUILoadFinish},
        });
    }

    public void OnUILoadFinish(LobbyUI lobbyUI)
    {
        this.ui = lobbyUI;
    }


    public void OnTitleUILoadFinish(TitleBarUI titleBarUI)
    {
        this.titleBarUI = titleBarUI;
    }

    public override void OnLoadFinish()
    {


        ui.onHeroListBtnClick += OnClickHeroListBtn;

        ui.onMainTaskBtnClick += OnClickMainTaskBtn;
    }

    public void OnClickHeroListBtn()
    {
        CtrlManager.Instance.Enter<HeroListCtrl>();
    }

    public void OnClickMainTaskBtn()
    {
        //开始主线
        //var battleEntranceNet = NetHandlerManager.Instance.GetHandler<BattleEntranceNetHandler>();

        //battleEntranceNet.SendApplyHeroExamBattle((xxx) =>
        //{
        //    //center 房间创建好了 等待战斗开始

        //    //this.OnStartBattle();
        //});

        CtrlManager.Instance.Enter<MainTaskCtrl>();

    }

    //void OnCreateBattle()
    //{
    //    //收到创建战斗消息
    //    CtrlManager.Instance.Enter<BattleCtrl>();
    //}



    public override void OnEnter(CtrlArgs args)
    {

    }

    public override void OnActive()
    {
        ui.Show();
        titleBarUI.Show();

        RefreshAll();
    }

    public void RefreshAll()
    {
        RefreshTitleBarUI();
    }

    public void RefreshTitleBarUI()
    {
        //给标题栏 ui 提供数据
        TitleBarUIArgs titleArgs = new TitleBarUIArgs();
        titleArgs.optionList = new List<TitleOptionUIData>();

        //先就显示一个
        var bagStore = GameDataManager.Instance.BagStore;
        TitleOptionUIData optionData = new TitleOptionUIData();
        optionData.configId = 22000001;
        optionData.count = bagStore.GetCountByConfigId(optionData.configId);

        titleArgs.optionList.Add(optionData);

        titleBarUI.Refresh(titleArgs);
    }

    public override void OnInactive()
    {
        ui.Hide();
        titleBarUI.Hide();
    }

    public override void OnExit()
    {
        ui.onHeroListBtnClick -= OnClickHeroListBtn;
        ui.onMainTaskBtnClick -= OnClickMainTaskBtn;
        //UIManager.Instance.ReleaseUI<LobbyUI>();
    }


}
