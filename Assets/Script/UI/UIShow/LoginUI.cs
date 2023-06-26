using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : BaseUI
{
    public Action<string, string> onLoginBtnClick;
    public Action onChangeUserBtnClick;
    
    public Action<string, string> onLoginOrRegisterBtnClick;
    public Action onLoginOrRegisterCloseBtnClick;


    //登录主界面
    private Text userAccountText;
    private Button btn_login;
    Text stateText;
    private Button changeUserBtn;

    //登录/注册界面
    private GameObject loginRootObj;
    InputField accountInput;

    InputField passwordInput;

    // Text statePopText;
    Button loginRegisterConfirmBtn;
    private Button loginRegisterCloseBtn;

    protected override void OnInit()
    {
        //登录相关
        loginRootObj = this.transform.Find("loginRoot").gameObject;

        userAccountText = this.transform.Find("account/txt_name").GetComponent<Text>();
        btn_login = this.transform.Find("btn_login").GetComponent<Button>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();
        changeUserBtn = this.transform.Find("account/btn_change_user").GetComponent<Button>();

        //登录/注册界面
        accountInput = loginRootObj.transform.Find("accountInput").GetComponent<InputField>();
        passwordInput = loginRootObj.transform.Find("passwordInput").GetComponent<InputField>();

        loginRegisterConfirmBtn = loginRootObj.transform.Find("loginBtn").GetComponent<Button>();
        // statePopText = loginRootObj.transform.Find("statePopText").GetComponent<Text>();
        loginRegisterCloseBtn = loginRootObj.transform.Find("btn_close").GetComponent<Button>();
        


        btn_login.onClick.AddListener(() =>
        {
            //主界面点击登录按钮
            onLoginBtnClick?.Invoke("", "");
        });
        
        changeUserBtn.onClick.AddListener(() =>
        {
            onChangeUserBtnClick?.Invoke();
        });

        loginRegisterConfirmBtn.onClick.AddListener(() =>
        {
            //登录/注册界面点击登录按钮
            var account = accountInput.text;
            var password = passwordInput.text;
            onLoginOrRegisterBtnClick?.Invoke(account, password);
        });

        loginRegisterCloseBtn.onClick.AddListener(() =>
        {
            onLoginOrRegisterCloseBtnClick?.Invoke();
        });
        
      
        
        this.RefreshSaveLoginSuccessShow();
    }

    public void RefreshSaveLoginSuccessShow()
    {
        var preAccount = LocalDataTools.GetString("currAccount");
        var preAassword = LocalDataTools.GetString("currPassword");
        if (string.IsNullOrEmpty(preAccount))
        {
            preAccount = "----";
        }

        accountInput.text = preAccount;
        passwordInput.text = preAassword;

        userAccountText.text = preAccount;
    }

    public void SetStateText(string stateStr)
    {
        stateText.text = stateStr;
    }

    public void SetLoginRegisterUIShow(bool isShow)
    {
        loginRootObj.SetActive(isShow);
    }

    protected override void OnRelease()
    {
        onLoginBtnClick = null;
        onLoginOrRegisterBtnClick = null;
        onLoginOrRegisterCloseBtnClick = null;
        onChangeUserBtnClick = null;

        btn_login.onClick.RemoveAllListeners();
        loginRegisterConfirmBtn.onClick.RemoveAllListeners();
        loginRegisterCloseBtn.onClick.RemoveAllListeners();
        changeUserBtn.onClick.RemoveAllListeners();
    }
}