using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroListUI : BaseUI
{
    public Action onGoInfoUIBtnClickEvent;

    Button goInfoUIBtn;
    protected override void OnInit()
    {
        goInfoUIBtn = this.transform.Find("root/HeroCard/enterInfoBtn").GetComponent<Button>();

        goInfoUIBtn.onClick.AddListener(() =>
        {
            onGoInfoUIBtnClickEvent?.Invoke();
        });
    }
}
