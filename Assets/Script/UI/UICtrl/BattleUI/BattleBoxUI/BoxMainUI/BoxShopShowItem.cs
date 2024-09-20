﻿using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

public class BoxShopShowItem
{
    public GameObject gameObject;
    public Transform transform;

    public Image icon;
    public Image iconBg;
    public Text nameText;
    public Text countText;

    public Text costResText;
    public Button buyBtn;

    public BoxShopItem data;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        iconBg = this.transform.Find("iconBg").GetComponent<Image>();

        icon = iconBg.transform.Find("icon").GetComponent<Image>();
        nameText = this.transform.Find("name_text").GetComponent<Text>();
        countText = this.transform.Find("count_text").GetComponent<Text>();
        costResText = this.transform.Find("costText").GetComponent<Text>();
        buyBtn = this.transform.Find("buyBtn").GetComponent<Button>();

        buyBtn.onClick.AddListener(OnClickBuyBtn);
    }

    public void RefreshUI(BoxShopItem data)
    {
        this.data = data;

        var config = BattleConfigManager.Instance.
            GetById<IBattleBoxShopItem>(this.data.configId);
        var quality = (RewardQuality)config.Quality;
        iconBg.color = ColorDefine.GetColorByQuality((RewardQuality)config.Quality);
        int iconResId = config.IconResId;
        if (iconResId > 0)
        {
            ResourceManager.Instance.GetObject<Sprite>(iconResId, (sprite) =>
            {
                icon.sprite = sprite;
            });
        }

        nameText.text = config.Name;
        countText.text = this.data.canBuyCount + "/" + this.data.maxBuyCount;
        costResText.text = this.data.costCount + "战银";

    }

    void OnClickBuyBtn()
    {
        //检查货币是否够
        var player = BattleManager.Instance.GetLocalPlayer();
        var myCoin = player.GetCoinCount();
        // data.costItemId
        if (myCoin < data.costCount)
        {
            Logx.Log(LogxType.BattleBox, "BoxShop : OnClickBuyBtn : " +
                                         "coin not enough : myCoin : " + myCoin + "," +
                                         "needCostCount : " + data.costCount);
            return;
        }
        
        //send buy
        var config = BattleConfigManager.Instance.
            GetById<IBattleBoxShopItem>(this.data.configId);
        var localPlayer = BattleManager.Instance.GetLocalPlayer();
        localPlayer.BuyBoxFromShop((RewardQuality)config.Quality,1);
    }

    public void Release()
    {
        buyBtn.onClick.RemoveAllListeners();
    }
}