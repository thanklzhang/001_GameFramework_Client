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

    public override void Enter(CtrlArgs args)
    {
        //1 先获得数据然后再进界面 或者 2 先进界面在取数据 或者 3 两个同时
        //这里目前采用 2 
        ui = (HeroListUI)UIManager.Instance.OpenUI(UIName.HeroListUI);
        ui.openCallback += OnUIOpen;
        ui.onClickEnterHeroInfoBtnEvent += this.OnClickEnterHeroInfoBtn;
        ui.onClickUpdateHeroBtnEvent += this.OnCliCkUpdateHeroBtn;
    }

    public void OnUIOpen()
    {
        this.CreateHeroListShow();
    }

    //显示英雄列表流程------------------------------------------------
    public void CreateHeroListShow()
    {
        SendGetDataList(() =>
        {
            var heroDataList = DataManager.Instance.heroData.GetHeroList();
            ui.CreateHeroList(heroDataList);

        });
    }
    //-----------------------------------------------------------------


    private void OnClickEnterHeroInfoBtn(int heroId)
    {
        GameFunction.Instance.EnterHeroInfo(heroId);
    }


    //升级英雄流程------------------------------------------------
    //点击了升级按钮
    private void OnCliCkUpdateHeroBtn(int heroId)
    {
        SendUpgradeLevel(heroId);
    }

    //升级
    public void SendUpgradeLevel(int heroId)
    {
        //请求服务器
        //net.send(xxx,this.OnUpdateLevel);
        level += 1;

        //test
        OnUpdateLevel(heroId);
    }
    public void OnUpdateLevel(int heroId)
    {
        //收到服务器消息 net.send
        ui.RefreshLevelData(heroId, level);
    }
    //test
    int level = 1;
  
    //----------------------------------------------------------------

    protected override void Exit()
    {
        ui.openCallback += OnUIOpen;
        ui.onClickUpdateHeroBtnEvent += this.OnCliCkUpdateHeroBtn;
    }

}
