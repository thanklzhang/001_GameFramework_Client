using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class HeroInfoCtrl : BaseCtrl
{
    HeroInfoUI ui;
    public override void OnInit()
    {
        this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        UIManager.Instance.LoadUI<HeroInfoUI>((finishUI) =>
        {
            ui = finishUI;
            this.LoadFinish();
        });
    }

    public override void OnLoadFinish()
    {
        ui.onBackClickEvent += () =>
        {
            CtrlManager.Instance.Exit<HeroInfoCtrl>();
        };
    }

    public override void OnEnter()
    {
        ui.Show();
    }


    public override void OnExit()
    {
        //UIManager.Instance.FreezeUI();
        ui.Freeze();
    }

    public override void OnRelease()
    {
        UIManager.Instance.ReleaseUI<HeroInfoUI>();
        //ui.Close();
    }
}
