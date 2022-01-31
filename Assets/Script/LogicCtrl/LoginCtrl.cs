using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//登录 ctrl
public class LoginCtrl : BaseCtrl
{
    LoginUI ui;

    public override void OnInit()
    {
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<LoginUI>(){selfFinishCallback = OnUILoadFinish},
        });
        //UIManager.Instance.LoadUI<HeroListUI>();

    }

    public void OnUILoadFinish(LoginUI heroListUI)
    {
        this.ui = heroListUI;
    }

    public override void OnLoadFinish()
    {
        ui.onLoginBtnClick += OnClickLoginBtn;
        ui.onRegisteBtnClick += OnRegisteBtnClick;

        ui.SetStateText("no login");
        ui.SetStateText("start to connect to login server ...");
        NetworkManager.Instance.ConnectToLoginServer((isSuccess) =>
        {

            if (isSuccess)
            {
                ui.SetStateText("connect to login server success !!!");

                Logx.Log("StartToLogin : login success");
            }
            else
            {
                Logx.Log("StartToLogin : login fail");
                ui.SetStateText("connect login server fail");
            }
        });

    }

    public void OnClickLoginBtn(string account, string password)
    {

        //account = "zxy";
        //password = "zhang425";
        //ui.SetStateText("start to connect to login server ...");
        var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();
        ui.SetStateText("start to check login ...");
        loginHandler.SendCheckLogin(account, password, (result) =>
        {
            if (0 == result.Err)
            {
                Logx.Log("StartToLogin : check login success !!!");
                ui.SetStateText("check login success !!!");
                this.StartToEnterGame(result);
                LocalDataTools.SetString("currAccount", account);
                LocalDataTools.SetString("currPassword", password);
            }
            else
            {
                Logx.Log("StartToLogin : check login fail");
                ui.SetStateText("check login fail");
            }

        });
    }

    public void OnRegisteBtnClick(string account, string password,string againPassword)
    {
        if (password != againPassword)
        {
            Logx.Log("regist fail , the password is not the same as againPassword");
            return;
        }

        var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();
        loginHandler.SendRegistAccount(account, password, (result) =>
        {
            if (0 == result.Err)
            {
                Logx.Log("regist success !!! " + result.Account);
            }
            else
            {
                Logx.Log("regist fail !!! " + result.Account);
            }

        });

    }

    void StartToEnterGame(NetProto.scCheckLogin result)
    {
        var ip = result.Ip;
        var port = result.Port;
        var uid = result.Uid;
        Logx.Log("get uid : " + uid);

        ui.SetStateText("start to connect gate server ...");

        NetworkManager.Instance.ConnectToGateServer(ip, port, (isSuccess) =>
        {
            if (isSuccess)
            {
                ui.SetStateText("connect gate server success !!!!");
                Logx.Log("connect gate server success !!!!");

                var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();

                ui.SetStateText("start to enter game ...");

                loginHandler.SendEnterGame(uid, "", (enterResult) =>
                {
                    if (0 == enterResult.Err)
                    {
                        Logx.Log("enter game success !!!");

                        ui.SetStateText("enter game success !!!");
                        OnGateConnectResult();
                    }
                    else
                    {
                        Logx.Log("enter game fail !!!");
                        ui.SetStateText("enter game fail !!!");
                    }
                    
                });
            }
            else
            {
                ui.SetStateText("connect gate server fail");

                Logx.Log("StartToEnterGame : fail");
            }
        });
    }

    //public void OnLoginConnectResult()
    //{
    //    //登录服务器登录并验证成功 请求大厅数据
    //    //NetworkManager.Instance.ConnectToGateServer();
    //}

    public void OnGateConnectResult()
    {
        Logx.Log("StartGame !!! enter lobby !!!");

        //前往大厅
        CtrlManager.Instance.Enter<LobbyCtrl>();
    }


    public override void OnEnter(CtrlArgs args)
    {
       
    }

    public override void OnActive()
    {
        ui.Show();
    }

    public override void OnInactive()
    {
        ui.Hide();
    }

    public override void OnExit()
    {
        //UIManager.Instance.ReleaseUI<LoginUI>();
    }

}
