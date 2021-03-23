using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : BaseUI
{
    public Action onCloseBtnClick;
    public Action onHeroListBtnClick;
    public Action onBattleBtnClick;

    Button closeBtn;
    Button heroListBtn;
    Button battleBtn;

    protected override void OnInit()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        heroListBtn = this.transform.Find("heroListBtn").GetComponent<Button>();
        battleBtn = this.transform.Find("battleBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
        });

        heroListBtn.onClick.AddListener(() =>
        {
            onHeroListBtnClick?.Invoke();
        });

        battleBtn.onClick.AddListener(() =>
        {
            onBattleBtnClick?.Invoke();
        });
    }
}

