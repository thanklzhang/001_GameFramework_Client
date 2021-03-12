using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoUI : BaseUI
{
    public Action onBackClickEvent;

    Button backBtn;
    protected override void OnInit()
    {
        backBtn = this.transform.Find("closeBtn").GetComponent<Button>();

        backBtn.onClick.AddListener(() =>
        {
            onBackClickEvent?.Invoke();
        });
    }
}

