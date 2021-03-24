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
        this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        UIManager.Instance.LoadUI<LobbyUI>((finishUI)=>
        {
            ui = finishUI;
            this.LoadFinish();
        });
    }

    public override void OnLoadFinish()
    {
        ui.onHeroListBtnClick += () =>
        {
            CtrlManager.Instance.Enter<HeroListCtrl>();
        };

        ui.onBattleBtnClick += () =>
        {
            CtrlManager.Instance.Enter<BattleCtrl>();
        };
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }

    
    public override void OnExit()
    {
        UIManager.Instance.ReleaseUI<LobbyUI>();
    }


}
