using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class HeroListCtrl : BaseCtrl
{
    HeroListUI ui;

    public override void Init()
    {

    }

    //请求相应的数据
    public void SendGetDataList(Action callback)
    {
        //var netHandler = NetHandlerManager.Instance.GetHandler<HeroListNetHandler>();
        ////请求服务器
        //netHandler.ReqServerHeroList(callback);

        //test
        callback?.Invoke();
    }

    ////收到服务器消息
    //public void OnGetDataList()
    //{

    //}

    public override void OpenUI()
    {
        //1 先获得数据然后再进界面 或者 2 先进界面在取数据 或者 3 两个同时
        //这里目前采用 1 
        SendGetDataList(() =>
        {
            ui = (HeroListUI)UIManager.Instance.OpenUI(UIName.HeroListUI);
            ui.openCallback += OnUIOpen;
        });
    }

    public void OnUIOpen()
    {
        this.CreateHeroListShow();
    }

    public void CreateHeroListShow()
    {
        var heroDataList = DataManager.Instance.heroData.GetHeroList();
        ui.CreateHeroList(heroDataList);

    }
    //升级
    public void SendUpgradeLevel()
    {
        //请求服务器
        //net.send(xxx,this.OnUpdateLevel);
    }
    public void OnUpdateLevel()
    {
        //收到服务器消息 net.send
        this.RefreshLevelShow();
    }
    public void RefreshLevelShow()
    {
        ui.RefreshLevelText(2);
    }


   
   

}
