using System;
using System.Collections;
using System.Collections.Generic;
using Table;
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

    public void OnClickOneHeroUpgradeLevelBtn(int heroId)
    {
        ServiceManager.Instance.heroService.UpgradeHeroLevel(heroId);
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }

    public override void OnActive()
    {
        ui.onCloseBtnClick += OnClickCloseBtn;
        ui.onGoInfoUIBtnClick += OnClickGoInfoUIBtn;
        ui.onClickOneHeroUpgradeLevelBtn += OnClickOneHeroUpgradeLevelBtn;

        EventDispatcher.AddListener<HeroData>(EventIDs.OnUpgradeHeroLevel, OnUpgradeHeroLevel);


        //组装数据并传递给 UI 层数据
        HeroListUIArgs uiArgs = ConvertToUIArgs();
        ui.Refresh(uiArgs);
    }

    public override void OnInactive()
    {
        ui.onCloseBtnClick -= OnClickCloseBtn;
        ui.onGoInfoUIBtnClick -= OnClickGoInfoUIBtn;
        ui.onClickOneHeroUpgradeLevelBtn -= OnClickOneHeroUpgradeLevelBtn;

        EventDispatcher.RemoveListener<HeroData>(EventIDs.OnUpgradeHeroLevel, OnUpgradeHeroLevel);
    }


    public void OnUpgradeHeroLevel(HeroData heroData)
    {
        //目前不用传来的 直接取值即可
        var nowHeroData = GameDataManager.Instance.HeroGameDataStore.GetDataById(heroData.id);
        ui.RefreshOneHero(ConvertToUIHeroData(nowHeroData));

    }

    public HeroCardUIData ConvertToUIHeroData(HeroData heroData)
    {
        HeroCardUIData uiData = new HeroCardUIData();
        uiData.id = heroData.id;
        uiData.level = heroData.level;
        var heroDataStore = GameDataManager.Instance.HeroGameDataStore;
        var serverHeroData = heroDataStore.GetDataById(heroData.id);
        if (serverHeroData != null)
        {
            uiData.isUnlock = true;
        }
        return uiData;
    }

    public HeroListUIArgs ConvertToUIArgs()
    {
        var heroDataStore = GameDataManager.Instance.HeroGameDataStore;
        var heroTbList = TableManager.Instance.GetList<Table.HeroInfo>();
        HeroListUIArgs uiArgs = new HeroListUIArgs();
        uiArgs.cardList = new List<HeroCardUIData>();
        foreach (var item in heroTbList)
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
     

        UIManager.Instance.ReleaseUI<HeroListUI>();
    }

}
