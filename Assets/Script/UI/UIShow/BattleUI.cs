using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : BaseUI
{
    public Action onCloseBtnClick;
    public Action onReadyStartBtnClick;

    Button closeBtn;
    Button readyStartBtn;

    public Text stateText;
    protected override void OnInit()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        readyStartBtn = this.transform.Find("readyStartBtn").GetComponent<Button>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
        });
        readyStartBtn.onClick.AddListener(() =>
        {
            onReadyStartBtnClick?.Invoke();
        });
    }

    public void SetReadyBattleBtnShowState(bool isShow)
    {
        readyStartBtn.gameObject.SetActive(isShow);
    }

    public void SetStateText(string stateStr)
    {
        stateText.text = stateStr;
    }


    protected override void OnRelease()
    {
        onCloseBtnClick = null;
        onReadyStartBtnClick = null;
    }

}
