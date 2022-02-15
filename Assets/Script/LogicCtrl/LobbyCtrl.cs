using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class LobbyCtrl : BaseCtrl
{
    LobbyUI ui;
    public override void OnInit()
    {
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<LobbyUI>(){selfFinishCallback = OnUILoadFinish},
        });
    }

    public void OnUILoadFinish(LobbyUI lobbyUI)
    {
        this.ui = lobbyUI;
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
    }

    public override void OnInactive()
    {
        ui.Hide();
    }

    public override void OnExit()
    {
        ui.onHeroListBtnClick -= OnClickHeroListBtn;
        ui.onMainTaskBtnClick -= OnClickMainTaskBtn;
        //UIManager.Instance.ReleaseUI<LobbyUI>();
    }


}
