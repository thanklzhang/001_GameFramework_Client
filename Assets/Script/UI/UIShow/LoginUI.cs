using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : BaseUI
{
    //public override void Init()
    //{
    //    this.resId = UIResIds.LoginUI;
    //}

    //public override void LoadFinish(UIContext context)
    //{
    //    base.LoadFinish(context);

    //    loginBtn = root.Find("loginBtn").GetComponent<Button>();
    //    loginBtn.onClick.AddListener(() =>
    //    {
    //        //登录
    //        loginBtn.gameObject.SetActive(false);
    //        var loginNet = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();
    //        var rand = UnityEngine.Random.Range(1, 10000000.0f);
    //        loginNet.AskLogin(rand.ToString(), "425", (isSuccess) =>
    //         {
    //             if (isSuccess)
    //             {
    //                 Debug.Log("login success , start to connect gate server ...");
    //                 GameStateManager.Instance.ChangeState(GameState.Lobby);
    //             }
    //             else
    //             {
    //                 Debug.Log("login fail ... ");
    //                 loginBtn.gameObject.SetActive(true);
    //             }
    //         });

    //    });
    //    loginBtn.gameObject.SetActive(false);

    //}

    //public override void Show()
    //{
    //    base.Show();
    //    EventManager.AddListener<bool>((int)GameEvent.ConnectLoginServerResult, ConnectLoginServerResult);
    //}

    //public override void Hide()
    //{
    //    base.Hide();
    //    EventManager.RemoveListener<bool>((int)GameEvent.ConnectLoginServerResult, ConnectLoginServerResult);
    //}

    //private void ConnectLoginServerResult(bool isSuccess)
    //{
    //    if (isSuccess)
    //    {
    //        Debug.Log("show login success ui");
    //        loginBtn.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.Log("show login fail ui");
    //    }
    //}


    //Button goToLobbyBtn;
    //Button closeBtn;
    //Button loginBtn;

}
