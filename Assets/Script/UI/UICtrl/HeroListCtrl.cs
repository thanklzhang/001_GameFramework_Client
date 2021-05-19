using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroListCtrlArgs : CtrlArgs
{
    public Action yesAction;
    public Action noAction;
    public Action closeAction;
}

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

    public override void OnActive()
    {
        //组装数据
        TableManager.Instance.Init();
        var heroInfoTable = TableManager.Instance.Get<HeroInfoTable>();
        var heroTableList = heroInfoTable.GetByType();
        heroInfoTable.GetById<object>(111);
        heroInfoTable.GetList<object>();

        //将组装后的数据传递给 UI 层数据
        HeroListUIArgs uiArgs = ConvertToUIArgs();
        ui.Refresh(uiArgs);

    }

    public HeroListUIArgs ConvertToUIArgs()
    {
        HeroListUIArgs uiArgs = null;
        return uiArgs;
    }

    public override void OnExit()
    {
        ui.onCloseBtnClick -= OnClickCloseBtn;
        ui.onGoInfoUIBtnClick -= OnClickGoInfoUIBtn;

        UIManager.Instance.ReleaseUI<HeroListUI>();
    }

}
