using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//登录 ctrl
public class LoginCtrl : BaseCtrl
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

    public void OnClickLoginBtn()
    {
        NetworkManager.Instance.ConnectToLoginServer();
    }

    public void OnLoginConnectResult()
    {
        //登录服务器登录并验证成功 请求大厅数据
        //NetworkManager.Instance.ConnectToGateServer();
    }

    public void OnGateConnectResult()
    {
        //前往大厅
        CtrlManager.Instance.Enter<LobbyCtrl>();
    }


    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }

    public override void OnActive()
    {

    }

    public override void OnInactive()
    {

    }

    public override void OnExit()
    {
        UIManager.Instance.ReleaseUI<HeroListUI>();
    }

}
