using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : BaseUI
{
    public Action onCloseBtnClick;
    public Action onHeroListBtnClick;
    public Action onMainTaskBtnClick;

    Button closeBtn;
    Button heroListBtn;
    Button mainTaskBtn;

    protected override void OnInit()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        heroListBtn = this.transform.Find("heroListBtn").GetComponent<Button>();
        mainTaskBtn = this.transform.Find("mainTaskBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
        });

        heroListBtn.onClick.AddListener(() =>
        {
            onHeroListBtnClick?.Invoke();
        });

        mainTaskBtn.onClick.AddListener(() =>
        {
            onMainTaskBtnClick?.Invoke();
        });
    }
    protected override void OnRelease()
    {
        onCloseBtnClick = null;
        onHeroListBtnClick = null;
        onMainTaskBtnClick = null;
    }
}

