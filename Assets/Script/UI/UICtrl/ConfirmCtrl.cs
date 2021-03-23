using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class ConfirmCtrl : BaseCtrl
{
    ConfirmUI ui;
    public override void OnInit()
    {
        this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        UIManager.Instance.LoadUI<ConfirmUI>((finishUI) =>
        {
            ui = finishUI;
            this.LoadFinish();
        });
    }

    public override void OnLoadFinish()
    {
        ui.onCloseClickEvent += () =>
        {
            CtrlManager.Instance.Exit<ConfirmCtrl>();
        };
        ui.onClickYesBtn += () =>
        {
            CtrlManager.Instance.Enter<HeroInfoCtrl>();
        };
        ui.onClickNoBtn += () =>
        {
            CtrlManager.Instance.Exit<ConfirmCtrl>();
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
        UIManager.Instance.ReleaseUI<ConfirmUI>();
    }
}
