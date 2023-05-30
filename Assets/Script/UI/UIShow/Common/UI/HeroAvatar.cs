using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;


public class HeroAvatar
{
    public GameObject gameObject;
    public Transform transform;

    RawImage avatarImg;
    Text levelText;
    Text nameText;
    Button clickBtn;
    GameObject selectGo;

    public HeroCardUIData uiData;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        avatarImg = this.transform.Find("show/selectHeroIcon").GetComponent<RawImage>();
        nameText = this.transform.Find("show/heroNameText").GetComponent<Text>();
        levelText = this.transform.Find("show/heroLevelText").GetComponent<Text>();
        clickBtn = this.transform.Find("show/clickBtn").GetComponent<Button>();
        selectGo = this.transform.Find("show/border").gameObject;


    }


    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(HeroCardUIData uiData)
    {
        this.uiData = uiData;

        //avatarImg

        levelText.text = "" + this.uiData.level;

        var config = Table.TableManager.Instance.GetById<Table.EntityInfo>(this.uiData.configId);
        nameText.text = config.Name;

    }

    public void AddClickListener(Action<int> action)
    {
        clickBtn.onClick.AddListener(() =>
        {
            action?.Invoke(this.uiData.guid);
        });
    }

    public void SetSelectState(bool isShow)
    {
        selectGo.SetActive(isShow);
    }

    public void RemoveClickListener()
    {
        clickBtn.onClick.RemoveAllListeners();
    }

    public void Update()
    {

    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Release()
    {
        RemoveClickListener();
    }
}