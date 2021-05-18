using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : BaseUI
{
    public Action onCloseBtnClick;

    Button closeBtn;
    protected override void OnInit()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
        });
    }

    protected override void OnRelease()
    {
        onCloseBtnClick = null;
    }

}
