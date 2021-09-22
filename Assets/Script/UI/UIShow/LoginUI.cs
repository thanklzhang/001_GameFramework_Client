using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : BaseUI
{
    public Action<string, string> onLoginBtnClick;

    Button loginBtn;
    InputField accountInput;
    InputField passwordInput;
    Text stateText;

    protected override void OnInit()
    {
        loginBtn = this.transform.Find("loginBtn").GetComponent<Button>();
        accountInput = this.transform.Find("accountInput").GetComponent<InputField>();
        passwordInput = this.transform.Find("passwordInput").GetComponent<InputField>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();

        loginBtn.onClick.AddListener(() =>
        {
            var account = accountInput.text;
            var password = passwordInput.text;
            onLoginBtnClick?.Invoke(account, password);
        });
    }

    public void SetStateText(string stateStr)
    {
        Loom.QueueOnMainThread(()=>
        {
            stateText.text = stateStr;
        });
       
    }

    protected override void OnRelease()
    {
        onLoginBtnClick = null;
    }
}

