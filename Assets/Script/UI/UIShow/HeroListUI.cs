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
    Transform transform;

    Text levelText;
    Text nameText;
    GameObject unlockFlagObj;
    Action clickCallback;

    public HeroCardUIData uiData;

    public void Init(GameObject obj, Action clickCallback)
    {
        this.gameObject = obj;
        this.transform = this.gameObject.transform;

        this.clickCallback = clickCallback;

        levelText = this.transform.Find("level").GetComponent<Text>();
        nameText = this.transform.Find("name").GetComponent<Text>();
        unlockFlagObj = this.transform.Find("lockFlag").gameObject;
    }

    public void Refresh(HeroCardUIData uiData)
    {
        this.uiData = uiData;

        var id = this.uiData.id;
        var heroInfoTable = TableManager.Instance.HeroInfoStore.GetById(id);
        levelText.text = "" + this.uiData.level;
        nameText.text = "" + heroInfoTable.Name;
        unlockFlagObj.SetActive(!this.uiData.isUnlock);
    }

}

public class HeroListUI : BaseUI
{
    public Action onGoInfoUIBtnClick;
    public Action onCloseBtnClick;

    Button goInfoUIBtn;
    Button closeBtn;

    Transform heroListRoot;

    List<HeroCardUIData> cardDataList = new List<HeroCardUIData>();

    List<HeroCardShowObj> showDataObjList = new List<HeroCardShowObj>();
    protected override void OnInit()
    {
        goInfoUIBtn = this.transform.Find("root/HeroCard/enterInfoBtn").GetComponent<Button>();
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        heroListRoot = this.transform.Find("root");

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

        this.cardDataList = heroListArgs.cardList;

        this.RefreshHeroList();
    }

    void RefreshHeroList()
    {
        for (int i = 0; i < this.cardDataList.Count; i++)
        {
            var cardData = this.cardDataList[i];

            GameObject go = null;
            if (i < heroListRoot.childCount)
            {
                go = heroListRoot.GetChild(i).gameObject;
            }
            else
            {
                var tempObj = heroListRoot.GetChild(0).gameObject;
                go = GameObject.Instantiate(tempObj, heroListRoot);
            }

            HeroCardShowObj showObj = null;
            if (i < showDataObjList.Count)
            {
                showObj = showDataObjList[i];
            }
            else
            {
                showObj = new HeroCardShowObj();
                showDataObjList.Add(showObj);
                showObj.Init(go, null);
            }

            go.SetActive(true);
            showObj.Refresh(cardData);
        }

        for (int i = this.cardDataList.Count; i < heroListRoot.childCount; i++)
        {
            var obj = heroListRoot.GetChild(i).gameObject;
            obj.SetActive(false);
        }
    }

    protected override void OnRelease()
    {
        onGoInfoUIBtnClick = null;
        onCloseBtnClick = null;
    }
}
