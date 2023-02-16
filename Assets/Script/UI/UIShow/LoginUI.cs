using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : BaseUI
{
    public Action<string, string> onLoginBtnClick;
    public Action<string, string, string> onRegisteBtnClick;
    public Action<string, int> onClickConnectBtn;

    Button loginConfirmBtn;
    Button registConfirmBtn;
    InputField accountInput;
    InputField passwordInput;
    Text stateText;
    GameObject loginRootObj;
    GameObject registRootObj;
    InputField registAccountInput;
    InputField registPasswordInput;
    InputField registPasswordAgainInput;

    Transform connectRoot;
    Transform remoteServerRoot;
    Transform localServerRoot;
    public InputField localServerIpInput;
    public InputField localServerPortInput;
    Button connectBtn;
    public Text connectTipsText;

    Button loginOptionBtn;
    Button registOptionBtn;

    protected override void OnInit()
    {
        //选项卡
        loginOptionBtn = this.transform.Find("loginOption").GetComponent<Button>();
        registOptionBtn = this.transform.Find("registOption").GetComponent<Button>();

        //登录相关
        loginRootObj = this.transform.Find("loginRoot").gameObject;

        accountInput = loginRootObj.transform.Find("accountInput").GetComponent<InputField>();
        passwordInput = loginRootObj.transform.Find("passwordInput").GetComponent<InputField>();

        loginConfirmBtn = loginRootObj.transform.Find("loginBtn").GetComponent<Button>();

        //注册账号相关
        registRootObj = this.transform.Find("registRoot").gameObject;
        registAccountInput = registRootObj.transform.Find("accountInput").GetComponent<InputField>();
        registPasswordInput = registRootObj.transform.Find("passwordInput").GetComponent<InputField>();
        registPasswordAgainInput = registRootObj.transform.Find("passwordAgainInput").GetComponent<InputField>();


        registConfirmBtn = registRootObj.transform.Find("registBtn").GetComponent<Button>();

        //连接相关
        connectRoot = transform.Find("connect");
        remoteServerRoot = connectRoot.Find("remoteRoot");
        localServerRoot = connectRoot.Find("localRoot");
        localServerIpInput = localServerRoot.Find("Ip").GetComponent<InputField>();
        localServerPortInput = localServerRoot.Find("port").GetComponent<InputField>();
        connectBtn = connectRoot.Find("connectBtn").GetComponent<Button>();
        connectTipsText = connectRoot.Find("tips").GetComponent<Text>();
        connectBtn.onClick.AddListener(() =>
        {
            var ip = localServerIpInput.text;
            var port = int.Parse(localServerPortInput.text);
            onClickConnectBtn?.Invoke(ip, port);
        });
        this.localServerIpInput.text = NetTool.GetHostIp();// "192.168.3.13";
        this.localServerPortInput.text = "5556";
    

        //------------------------------

        stateText = this.transform.Find("stateText").GetComponent<Text>();

        loginOptionBtn.onClick.AddListener(() =>
        {
            this.SwitchToLoginView();
        });

        registOptionBtn.onClick.AddListener(() =>
        {
            this.SwitchToRegisterView();
        });

        loginConfirmBtn.onClick.AddListener(() =>
        {
            var account = accountInput.text;
            var password = passwordInput.text;
            onLoginBtnClick?.Invoke(account, password);
        });
        registConfirmBtn.onClick.AddListener(() =>
        {
            var account = registAccountInput.text;
            var password = registPasswordInput.text;
            var againPassword = registPasswordAgainInput.text;
            onRegisteBtnClick?.Invoke(account, password, againPassword);
        });

        this.RefreshSaveLoginSuccessShow();




    }

    internal void SetConnectUIShow(bool isShow)
    {
        connectRoot.gameObject.SetActive(isShow);
    }

    public void SetConnectTips(string str)
    {
        this.connectTipsText.text = str;
    }

    public void SwitchToLoginView()
    {
        loginRootObj.SetActive(true);
        registRootObj.SetActive(false);
    }

    public void SwitchToRegisterView()
    {
        loginRootObj.SetActive(false);
        registRootObj.SetActive(true);
    }

    public void RefreshSaveLoginSuccessShow()
    {
        var preAccount = LocalDataTools.GetString("currAccount");
        var preAassword = LocalDataTools.GetString("currPassword");
        accountInput.text = preAccount;
        passwordInput.text = preAassword;
    }

    public void SetStateText(string stateStr)
    {
        stateText.text = stateStr;
    }

    protected override void OnRelease()
    {
        onLoginBtnClick = null;

        connectBtn.onClick.RemoveAllListeners();
        this.onClickConnectBtn = null;
    }
}

