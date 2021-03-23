using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroListUI : BaseUI
{
    public Action onGoInfoUIBtnClick;
    public Action onCloseBtnClick;

    Button goInfoUIBtn;
    Button closeBtn;
    protected override void OnInit()
    {
        goInfoUIBtn = this.transform.Find("root/HeroCard/enterInfoBtn").GetComponent<Button>();
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();

        goInfoUIBtn.onClick.AddListener(() =>
        {
            onGoInfoUIBtnClick?.Invoke();
        });

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
        });
    }
}
