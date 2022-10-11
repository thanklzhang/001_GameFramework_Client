using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultUI : BaseUI
{
    public Action onClickConfirmBtn;

    //当前选中的关卡相关组件--
    Text winContent;
    Button confirmBtn;

    protected override void OnInit()
    {
        this.winContent = this.transform.Find("Panel/Result").GetComponent<Text>();
        this.confirmBtn = this.transform.Find("Panel/ConfirmBtn").GetComponent<Button>();

        confirmBtn.onClick.AddListener(() =>
        {
            onClickConfirmBtn?.Invoke();
        });
    }

    public override void Refresh(UIArgs args)
    {
        var resultArgs = (BattleResultUIArgs)args;
        var isWin = resultArgs.isWin;
        var showStr = isWin ? "you win" : "you fail";
        winContent.text = showStr;
    }

    protected override void OnRelease()
    {
        onClickConfirmBtn = null;
        this.confirmBtn.onClick.RemoveAllListeners();
    }
}

public class BattleResultUIArgs : UIArgs
{
    public bool isWin;
    public List<CommonItemUIArgs> uiItem;
}
