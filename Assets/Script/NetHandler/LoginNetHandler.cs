using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoginNetHandler : NetHandler
{
    public Action<bool> loginResultAction;
    public override void Init()
    {
        base.Init();

        AddListener(ProtoMsgIds.GC2LS_AskLogin, LoginResult);

    }

    public void AskLogin(string account, string password, Action<bool> action)
    {
        //send message
        GC2LS.reqAskLogin ask = new GC2LS.reqAskLogin();
        ask.Account = account;
        ask.Password = password;
        this.loginResultAction = action;
        this.SendMsgToLS(ProtoMsgIds.GC2LS_AskLogin, ask);

    }

    public void LoginResult(byte[] bytes)
    {
        //var resp = GC2LS.respAskLogin.Parser.ParseFrom(bytes);

        ////refresh data
        //LoginData.Instance.SetData(resp);
        //UserData.Instance.SetAccount(resp.UserAccount, resp.Token);
        //this.loginResultAction?.Invoke(resp.IsSuccess);
        //this.loginResultAction = null;

    }
}

