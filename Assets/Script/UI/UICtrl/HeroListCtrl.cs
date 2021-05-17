using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class HeroListCtrl : BaseCtrl
{
    HeroListUI ui;


    public override void OnInit()
    {
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<HeroListUI>(){selfFinishCallback = OnUILoadFinish},
        });
        //UIManager.Instance.LoadUI<HeroListUI>();

    }

    public void OnUILoadFinish(HeroListUI heroListUI)
    {
        this.ui = heroListUI;
    }

    public override void OnLoadFinish()
    {
        ui.onCloseBtnClick += OnClickCloseBtn;
        ui.onGoInfoUIBtnClick += OnClickGoInfoUIBtn;
    }

    public void OnClickCloseBtn()
    {
        CtrlManager.Instance.Exit<HeroListCtrl>();
    }

    public void OnClickGoInfoUIBtn()
    {
        ConfirmCtrlArgs args = new ConfirmCtrlArgs();
        args.yesAction = () =>
        {
            CtrlManager.Instance.Enter<HeroInfoCtrl>();
        };

        CtrlManager.Instance.Enter<ConfirmCtrl>(args);
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }


    public override void OnExit()
    {
        ui.onCloseBtnClick -= OnClickCloseBtn;
        ui.onGoInfoUIBtnClick -= OnClickGoInfoUIBtn;

        UIManager.Instance.ReleaseUI<HeroListUI>();
    }

}
