using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//全局 ctrl 游戏进程中一直存在 不受正常的 ctrl 管理
public class GlobalCtrl : BaseCtrl
{
    TipsUI tipsUI;
    TitleBarUI titleUI;
    SelectHeroUI selectHeroUI;
    //TitleBarUI titleBarUI;
    public override void OnInit()
    {
        //this.isParallel = false;
    }

    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<TipsUI>(){selfFinishCallback = OnTipsUILoadFinish},
             new LoadUIRequest<TitleBarUI>(){selfFinishCallback = OnTitleUILoadFinish},
              new LoadUIRequest<SelectHeroUI>(){ selfFinishCallback = OnSelectHeroUILoadFinish }
        });
    }

    public void OnTipsUILoadFinish(TipsUI tipsUI)
    {
        this.tipsUI = tipsUI;

    }

    public void OnTitleUILoadFinish(TitleBarUI titleUI)
    {
        this.titleUI = titleUI;
    }

    public void OnSelectHeroUILoadFinish(SelectHeroUI ui)
    {
        selectHeroUI = ui;
    }

    public override void OnLoadFinish()
    {
        this.selectHeroUI.Hide();
    }

    public override void OnEnter(CtrlArgs args)
    {

    }

    public override void OnActive()
    {
        tipsUI.Show();
        titleUI.Show();
        selectHeroUI.Show();

        titleUI.clickCloseBtnAction += OnClickTitleUICloseBtn;

        EventDispatcher.AddListener(EventIDs.OnRefreshBagData, OnRefreshBagData);
    }

    public void OnRefreshBagData()
    {
        this.ShowTitleBar();
    }

    public void OnClickTitleUICloseBtn()
    {
        EventDispatcher.Broadcast(EventIDs.OnTitleBarClickCloseBtn);
    }

    public override void OnInactive()
    {
        tipsUI.Hide();
        titleUI.Hide();
        selectHeroUI.Hide();

        EventDispatcher.RemoveListener(EventIDs.OnRefreshBagData, OnRefreshBagData);
        titleUI.clickCloseBtnAction -= OnClickTitleUICloseBtn;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        tipsUI.Update(deltaTime);
        titleUI.Update(deltaTime);
    }

    //------------------------------
    public void ShowTips(string tipStr)
    {
        tipsUI.Show();

        tipsUI.Refresh(new TipsUIArgs()
        {
            tipStr = tipStr
        });
    }

    public void ShowTitleBar()
    {
        //给标题栏 ui 提供数据
        TitleBarUIArgs titleArgs = new TitleBarUIArgs();
        titleArgs.optionList = new List<TitleOptionUIData>();

        //先就显示一个
        var bagStore = GameDataManager.Instance.BagStore;
        TitleOptionUIData optionData = new TitleOptionUIData();
        optionData.configId = 22000001;
        optionData.count = bagStore.GetCountByConfigId(optionData.configId);

        titleArgs.optionList.Add(optionData);

        titleUI.Show();
        titleUI.Refresh(titleArgs);
    }

    public void HideTitleBar()
    {
        titleUI.Hide();
    }

    //通用选英雄界面------

    //这里可以改 CtrlManager ， 添加新 Ctrl 不会打断之前的 Ctrl 即可
    public void ShowSelectHeroUI(SelectHeroUIArgs args)
    {
        this.selectHeroUI.Refresh(args);
        this.selectHeroUI.Show();
    }

    public void HideSelectHeroUI()
    {
        this.selectHeroUI.Hide();
    }

    public void SelectHero(int guid)
    {
        this.selectHeroUI.SelectHero(guid);
    }
    //---------------

}
