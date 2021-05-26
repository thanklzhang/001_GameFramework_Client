using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeroListUIArgs : UIArgs
{
    public List<HeroCardUIData> cardList;
}

public class HeroCardUIData
{
    public int id;
    public int level;
    public bool isUnlock;

}

public class HeroCardShowObj
{
    GameObject gameObject;

    Text levelText;
    Text nameText;
    GameObject unlockFlagObj;
    Action clickCallback;

    public HeroCardUIData uiData;

    public void Init(GameObject obj, Action clickCallback)
    {
        this.gameObject = obj;
        this.clickCallback = clickCallback;

    }

    public void Refresh(HeroCardUIData uiData)
    {
        this.uiData = uiData;
        levelText.text = "";
        nameText.text = "";
        unlockFlagObj.SetActive(this.uiData.isUnlock);
    }

}

public class HeroListUI : BaseUI
{
    public Action onGoInfoUIBtnClick;
    public Action onCloseBtnClick;

    Button goInfoUIBtn;
    Button closeBtn;

    List<HeroCardUIData> cardList = new List<HeroCardUIData>();

    List<HeroCardShowObj> showObjList = new List<HeroCardShowObj>();
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

        this.InitHeroList();

        this.RefreshHeroList();
    }

    void InitHeroList()
    {
        showObjList = new List<HeroCardShowObj>();
        for (int i = 0; i < this.cardList.Count; i++)
        {
            var showObj = new HeroCardShowObj();
            showObjList.Add(showObj);
        }
    }

    void RefreshHeroList()
    {
        for (int i = 0; i < this.showObjList.Count; i++)
        {
            var cardShow = this.showObjList[i];
            var card = this.cardList[i];
            cardShow.Refresh(card);
        }
    }

    protected override void OnRelease()
    {
        onGoInfoUIBtnClick = null;
        onCloseBtnClick = null;
    }
}
