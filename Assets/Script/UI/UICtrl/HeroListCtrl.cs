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
        //组装数据并传递给 UI 层数据
        HeroListUIArgs uiArgs = ConvertToUIArgs();
        ui.Refresh(uiArgs);
    }

    public HeroListUIArgs ConvertToUIArgs()
    {
        var heroTbStore = TableManager.Instance.HeroInfoStore;
        var heroDataStore = GameDataManager.Instance.HeroGameDataStore;
        var heroList = heroTbStore.List;
        HeroListUIArgs uiArgs = new HeroListUIArgs();
        uiArgs.cardList = new List<HeroCardUIData>();
        foreach (var item in heroList)
        {
            var hero = item;
            var uiData = new HeroCardUIData();
            uiData.id = hero.Id;
            var serverHeroData = heroDataStore.GetDataById(hero.Id);
            if (serverHeroData != null)
            {
                uiData.level = serverHeroData.level;
                uiData.isUnlock = true;
            }

            uiArgs.cardList.Add(uiData);
        }

        return uiArgs;
    }

    public override void OnExit()
    {
        ui.onCloseBtnClick -= OnClickCloseBtn;
        ui.onGoInfoUIBtnClick -= OnClickGoInfoUIBtn;

        UIManager.Instance.ReleaseUI<HeroListUI>();
    }

}
