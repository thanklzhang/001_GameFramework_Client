using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetProto;
public class LoginNetHandler : NetHandler
{
    public Action<scCheckLogin> loginResultAction;
    public Action<scEnterGame> enterGameResultAction;
    public override void Init()
    {

        AddListener((int)ProtoIDs.CheckLogin, OnCheckLogin);
        AddListener((int)ProtoIDs.EnterGame, OnEnterGame);

    }

    //check login

    public void SendCheckLogin(string account, string password, Action<scCheckLogin> action)
    {
        loginResultAction = action;
        csCheckLogin login = new csCheckLogin()
        {
            Account = account,
            Password = password
        };
        NetworkManager.Instance.SendMsg(ProtoIDs.CheckLogin, login.ToByteArray());

    }

    public void OnCheckLogin(MsgPack msgPack)
    {
        scCheckLogin check = scCheckLogin.Parser.ParseFrom(msgPack.data);

        loginResultAction?.Invoke(check);
        loginResultAction = null;

    }

    //enter game

    public void SendEnterGame(int uid, int token, Action<scEnterGame> action)
    {
        csEnterGame enter = new csEnterGame()
        {
            Uid = uid,
            Token = token
        };
        enterGameResultAction = action;
        NetworkManager.Instance.SendMsg(ProtoIDs.EnterGame, enter.ToByteArray());
    }

    public void OnEnterGame(MsgPack msgPack)
    {
        scEnterGame enterGame = scEnterGame.Parser.ParseFrom(msgPack.data);
        enterGameResultAction?.Invoke(enterGame);
        enterGameResultAction = null;
    }

}

