using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class BattleCtrl : BaseCtrl
{
    BattleUI ui;
    public override void OnInit()
    {
        this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        UIManager.Instance.LoadUI<BattleUI>((finishUI) =>
        {
            ui = finishUI;
            this.LoadFinish();
        });
    }

    public override void OnLoadFinish()
    {
        ui.onCloseBtnClick += () =>
        {
            CtrlManager.Instance.Exit<BattleCtrl>();
        };
    }

    public override void OnEnter()
    {
        ui.Show();
    }


    public override void OnExit()
    {
        //UIManager.Instance.FreezeUI();
        //ui.Freeze();
        ui.Hide();
    }

    public override void OnRelease()
    {
        UIManager.Instance.ReleaseUI<BattleUI>();
        //ui.Close();
    }
}
