using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Table;
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
            new LoadUIRequest<LoginUI>() { selfFinishCallback = OnUILoadFinish },
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
        ui.onChangeUserBtnClick += OnChangeUserBtnClick;

        ui.onLoginOrRegisterBtnClick += OnLoginRegisterBtnClick;
        ui.onLoginOrRegisterCloseBtnClick += OnLoginOrRegisterCloseBtnClick;

        ui.SetStateText("没有登录");
        ui.SetLoginRegisterUIShow(false);

        //目前直接当作局域网进行连接
        // string ip = NetTool.GetHostIp();

        string ip = GameValue.LANServerIP;
        int port = 5556;
        OnclickConnectBtn(ip, port);
        
       
    }

    public void OnclickConnectBtn(string ip, int port)
    {
        var isLocal = Const.isLANServer;
        if (isLocal)
        {
            // ui.SetConnectTips("start to connect to login server ...");
            ui.SetStateText("开始连接登录服务器 ... " + ip + ":" + port);
            
            Logx.Log(LogxType.Game,"start to connect login server ...");
            
            port = 5556;
            NetworkManager.Instance.ConnectToLoginServer(ip, port, (isSuccess) =>
            {
                if (isSuccess)
                {
                    ui.SetStateText("连接登录服务器成功");
                    // ui.SetConnectTips("connect to login server success !!!");
                    Logx.Log(LogxType.Game," connect login server success");
                    // ui.SetConnectUIShow(false);
                }
                else
                {
                    ui.SetStateText("连接登录服务器失败");
                    // this.ui.SetConnectTips("connect login server fail");
                    Logx.Log(LogxType.Game," connect login server fail");
                }
            });
        }
        else
        {
            // this.ui.SetConnectTips("暂不支持远程服务器");
        }
    }

    public void OnClickLoginBtn(string account, string password)
    {
        var preAccount = LocalDataTools.GetString("currAccount");
        var prePassword = LocalDataTools.GetString("currPassword");
        bool isHaveAccount = !string.IsNullOrEmpty(preAccount);
        if (isHaveAccount)
        {
            //有账号就直接登录
            SendLoginNet(preAccount, prePassword);
        }
        else
        {
            //没有账号 弹出登录/注册界面
            var str = ("无账号,请注册账号");
            // Logx.Log(str);
            CtrlManager.Instance.globalCtrl.ShowTips(str);

            ui.SetLoginRegisterUIShow(true);
        }
    }

    private void OnChangeUserBtnClick()
    {
        this.ui.SetLoginRegisterUIShow(true);
    }

    private void SendLoginNet(string account, string password)
    {
        //account = "zxy";
        //password = "zhang425";
        //ui.SetStateText("start to connect to login server ...");
        var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();
        ui.SetStateText("开始登录 ...");
        
        Logx.Log(LogxType.Game," start login ...");
        
        // Logx.Log("account : " + account);
        // Logx.Log("password : " + password);
        loginHandler.SendCheckLogin(account, password, (result) =>
        {
            if (0 == result.Err)
            {
                Logx.Log(LogxType.Game," login success ...");
                ui.SetStateText("登录成功");
                CtrlManager.Instance.globalCtrl.ShowTips("登录成功");
                this.SaveLoginSuccessInfo(account, password);
                this.StartToEnterGame(result);
            }
            else
            {
                Logx.Log(LogxType.Game," login fail , err : " + result.Err);
                
                var tips = "未知错误";
                if (1 == result.Err)
                {
                    tips = "登录失败,该账号和密码不符合";
                }
                else if (2 == result.Err)
                {
                    tips = "注册失败,请联系管理员";
                }

                CtrlManager.Instance.globalCtrl.ShowTips(tips + " , 错误码：" + result.Err);
                ui.SetStateText("登录失败");
            }
        });
    }

    private void OnLoginRegisterBtnClick(string account, string password)
    {
        if (CheckLogin(account))
        {
            this.ui.SetLoginRegisterUIShow(false);
            SendLoginNet(account, password);
        }
        else
        {
            CtrlManager.Instance.globalCtrl.ShowTips("账号中不能有特殊符号");
        }
    }

    private bool CheckLogin(string account)
    {
        Regex regExp = new Regex("[ \\[ \\] \\^ \\-_*×――(^)$%~!＠@＃#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;/\'\"{}（）‘’“”-]");
        if (regExp.IsMatch(account))
        {
            return false;
        }

        return true;
    }

    protected void OnLoginOrRegisterCloseBtnClick()
    {
        this.ui.SetLoginRegisterUIShow(false);
    }

    public void SaveLoginSuccessInfo(string account, string password)
    {
        LocalDataTools.SetString("currAccount", account);
        LocalDataTools.SetString("currPassword", password);
    }


    void StartToEnterGame(NetProto.scCheckLogin result)
    {
        var ip = result.Ip;
        var port = result.Port;
        var uid = result.Uid;
        // Logx.Log("StartToEnterGame : get uid : " + uid);

        ui.SetStateText("开始连接网关服务器 ...");
        
        Logx.Log(LogxType.Game,"start to connect gate server ...");

        NetworkManager.Instance.ConnectToGateServer(ip, port, (isSuccess) =>
        {
            if (isSuccess)
            {
                ui.SetStateText("连接网关服务器成功");
                
                Logx.Log(LogxType.Game,"connect gate server success");
                
                var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();

                ui.SetStateText("开始进入游戏 ...");

                Logx.Log(LogxType.Game,"request enter game");
                loginHandler.SendEnterGame(uid, "", (enterResult) =>
                {
                    if (0 == enterResult.Err)
                    {
                        Logx.Log(LogxType.Game,"enter game success");
                        
                        ui.SetStateText("进入游戏成功");

                        CtrlManager.Instance.globalCtrl.ShowTips("进入游戏成功");

                        OnGateConnectResult();
                    }
                    else
                    {
                        ui.SetStateText("进入游戏失败");

                        Logx.Log(LogxType.Game,"enter game fail");
                        
                        CtrlManager.Instance.globalCtrl.ShowTips("进入游戏失败");
                    }
                });
            }
            else
            {
                ui.SetStateText("连接网关服务器失败");

                Logx.Log(LogxType.Game,"connect gate server fail");

                CtrlManager.Instance.globalCtrl.ShowTips("连接 网关服务器 失败");
            }
        });
    }

    public void OnGateConnectResult()
    {
        Logx.Log(LogxType.Game,"start enter game lobby ...");

        //前往大厅
        CtrlManager.Instance.Enter<LobbyCtrl>();
    }


    public override void OnEnter(CtrlArgs args)
    {
    }

    public override void OnActive()
    {
        CtrlManager.Instance.HideTitleBar();
        
        
        ui.Show();
        
        //play bgm
        AudioManager.Instance.PlayBGM((int)ResIds.bgm_001);
    }

    public override void OnInactive()
    {
        ui.Hide();
    }

    public override void OnExit()
    {
        ui.onLoginBtnClick -= OnClickLoginBtn;
        ui.onChangeUserBtnClick -= OnChangeUserBtnClick;

        ui.onLoginOrRegisterBtnClick -= OnLoginRegisterBtnClick;
        ui.onLoginOrRegisterCloseBtnClick -= OnLoginOrRegisterCloseBtnClick;
    }
}