using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;


public class SelectHeroUI : BaseUI
{
    Action event_ClickConfirmBtn;
    Action<int> event_ClickOneHeroOption;

    Button confirmBtn;

    Transform heroRoot;

    Button clickBtn;
    public List<HeroCardUIData> heroCardUIDataList;
    public List<HeroAvatar> heroShowObjList;

    int currSelectHeroGuid;
    protected override void OnInit()
    {
        heroRoot = this.transform.Find("scroll/mask/content");
        confirmBtn = this.transform.Find("confirmBtn").GetComponent<Button>();
      

        confirmBtn.onClick.AddListener(() =>
        {
            event_ClickConfirmBtn?.Invoke();
        });

        heroShowObjList = new List<HeroAvatar>();

    }

    public override void Refresh(UIArgs args)
    {
        SelectHeroUIArgs selectHeroUIArgs = (SelectHeroUIArgs)args;
        this.heroCardUIDataList = selectHeroUIArgs.heroCardUIDataList;
        this.event_ClickConfirmBtn = selectHeroUIArgs.event_ClickConfirmBtn;
        this.event_ClickOneHeroOption = selectHeroUIArgs.event_ClickOneHeroOption;

        currSelectHeroGuid = selectHeroUIArgs.currSelectHeroGuid;

        this.RefresInfo();
    }

    void RefresInfo()
    {
        for (int i = 0; i < this.heroCardUIDataList.Count; i++)
        {
            var data = this.heroCardUIDataList[i];
            GameObject go = null;
            HeroAvatar avater = null;
            if (i < this.heroRoot.childCount)
            {
                go = this.heroRoot.GetChild(i).gameObject;
            }
            else
            {
                var prefab = this.heroRoot.GetChild(0).gameObject;
                go = GameObject.Instantiate(prefab, this.heroRoot, false);
            }

            if (i < heroShowObjList.Count)
            {
                avater = heroShowObjList[i];
            }
            else
            {
                avater = new HeroAvatar();
                //理论上不会出现加一个还是空的的情况 
                heroShowObjList.Add(avater);

                avater.Init(go);
                avater.AddClickListener(event_ClickOneHeroOption);
            }

            avater.Refresh(data);
            avater.Show();
        }

        for (int i = this.heroRoot.childCount - 1; i >= this.heroCardUIDataList.Count; i--)
        {
            var go = this.heroRoot.GetChild(i).gameObject;

            if (i < heroShowObjList.Count)
            {
                heroShowObjList[i].Hide();
                heroShowObjList[i].Release();
                heroShowObjList.RemoveAt(i);
            }
            else
            {
                go.SetActive(false);
            }
        }


        SelectHero(currSelectHeroGuid);
    }

    public void SelectHero(int guid)
    {
        currSelectHeroGuid = guid;
        foreach (var showObj in heroShowObjList)
        {
            if (showObj.uiData.guid == guid)
            {
                showObj.SetSelectState(true);
            }
            else
            {
                showObj.SetSelectState(false);
            }
        }
    }

    protected override void OnRelease()
    {
        event_ClickConfirmBtn = null;
        this.confirmBtn.onClick.RemoveAllListeners();

        foreach (var item in heroShowObjList)
        {
            item.Release();
        }

        heroShowObjList = null;
    }
}

//-----------------------------------------


public class SelectHeroUIArgs : UIArgs
{
    public List<HeroCardUIData> heroCardUIDataList;
    public Action event_ClickConfirmBtn;
    public Action<int> event_ClickOneHeroOption;
    public int currSelectHeroGuid;
}
