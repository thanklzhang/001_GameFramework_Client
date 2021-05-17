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

        ui.onBattleBtnClick += OnClickBattleBtn;
    }

    public void OnClickHeroListBtn()
    {
        CtrlManager.Instance.Enter<HeroListCtrl>();
    }

    public void OnClickBattleBtn()
    {
        CtrlManager.Instance.Enter<BattleCtrl>();
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }

    public override void OnExit()
    {
        ui.onHeroListBtnClick -= OnClickHeroListBtn;
        ui.onBattleBtnClick -= OnClickBattleBtn;
        UIManager.Instance.ReleaseUI<LobbyUI>();
    }


}
