using GameModelData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : BaseUI
{
    //public override void Init()
    //{
    //    this.resId = UIResIds.LobbyUI;
    //}

    //public override void LoadFinish(UIContext context)
    //{
    //    base.LoadFinish(context);


    //    openHeroListBtn = root.Find("heroListBtn").GetComponent<Button>();
    //    openMateBtn = root.Find("mateBtn").GetComponent<Button>();
    //    openHeroListBtn.onClick.AddListener(() =>
    //    {
    //        UIManager.Instance.ShowUI<HeroListUI>();
    //    });
    //    openHeroListBtn.gameObject.SetActive(false);

    //    openMateBtn.onClick.AddListener(() =>
    //    {
    //        UIManager.Instance.ShowUI<MateMainUI>();
    //    });
    //}

    //public override void Show()
    //{
    //    base.Show();
    //    EventManager.AddListener<bool>((int)GameEvent.ConnectGateServerResult, ConnectGateServerResult);

    //    //EventManager.AddListener<bool>((int)GameEvent.SyncPlayerBaseInfo, SyncPlayerBaseInfo);
    //}

    //public override void Hide()
    //{
    //    base.Hide();
    //    EventManager.RemoveListener<bool>((int)GameEvent.ConnectGateServerResult, ConnectGateServerResult);
    //    //EventManager.RemoveListener<bool>((int)GameEvent.SyncPlayerBaseInfo, SyncPlayerBaseInfo);
    //}

    //private void ConnectGateServerResult(bool isSuccess)
    //{
    //    if (isSuccess)
    //    {
    //        Debug.Log("connect gate server success");
    //        var lobbyNet = NetHandlerManager.Instance.GetHandler<LobbyNetHandler>();
    //        lobbyNet.AskEnterGameServer((enterIsSuccess) =>
    //        {
    //            if (enterIsSuccess)
    //            {
    //                Debug.Log("enter game success ");
    //                StartShowLobbyUI();
    //            }
    //            else
    //            {
    //                Debug.Log("enter game fail ");
    //            }

    //        });
    //    }
    //    else
    //    {
    //        Debug.Log("connect gate server fail ...");
    //    }
    //}

    //void StartShowLobbyUI()
    //{
    //    //连接 gate server 成功开始游戏 进行画面上的一些动画等
    //    Debug.Log("StartShowLobbyUI");

    //    openHeroListBtn.gameObject.SetActive(true);
    //}
    //Button openHeroListBtn;
    //Button openMateBtn;
}
