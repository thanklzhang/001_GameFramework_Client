using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeroListUIArgs : UIArgs
{
    public List<HeroCard> cardList;
}

public class HeroCard
{
    public int id;
    public int level;
    public int isUnlock;

    public void Refresh()
    {
        
    }
}

public class HeroListUI : BaseUI
{
    public Action onGoInfoUIBtnClick;
    public Action onCloseBtnClick;

    Button goInfoUIBtn;
    Button closeBtn;

    List<HeroCard> cardList = new List<HeroCard>();
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

    public override void Refresh(UIArgs args)
    {
        HeroListUIArgs heroListArgs = (HeroListUIArgs)args;

        this.cardList = heroListArgs.cardList;
        this.RefreshHeroList();
    }

    void RefreshHeroList()
    {
        for (int i = 0; i < this.cardList.Count; i++)
        {
            var card = this.cardList[i];
            card.Refresh();
        }
    }

    protected override void OnRelease()
    {
        onGoInfoUIBtnClick = null;
        onCloseBtnClick = null;
    }
}
