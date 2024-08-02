using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using Table;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//选项
public class BattleBoxCell
{
    public GameObject gameObject;
    public Transform transform;

    public Image icon;
    public Text nameText;
    public Text describeText;
    public Button selectBtn;

    public int index = -1;

    private BattleBoxUI parentUI;

    public BattleClientMsg_BattleBoxSelection data;

    public void Init(GameObject gameObject, BattleBoxUI parentUI)
    {
        this.parentUI = parentUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        icon = transform.Find("iconBg/icon").GetComponent<Image>();
        nameText = transform.Find("name_text").GetComponent<Text>();
        describeText = transform.Find("content_text").GetComponent<Text>();
        selectBtn = transform.Find("selectBtn").GetComponent<Button>();

        selectBtn.onClick.AddListener(OnClickSelectBtn);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshUI(BattleClientMsg_BattleBoxSelection selectionDat,int index)
    {
        this.data = selectionDat;
        this.index = index;
        
        var configId = this.data.rewardConfigId;
        var rewardConfig = TableManager.Instance.GetById<Table.BattleReward>(configId);


        ResourceManager.Instance.GetObject<Sprite>(rewardConfig.IconResId,
            (sprite) => { icon.sprite = sprite; });
        nameText.text = rewardConfig.Name;
        describeText.text = rewardConfig.Describe;
    }

    public void OnClickSelectBtn()
    {
        this.parentUI.OnSelectSelection(this);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}