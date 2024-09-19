using System;
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

        icon = this.transform.Find("").GetComponent<Image>();
        iconBg = this.transform.Find("").GetComponent<Image>();
        nameText = this.transform.Find("").GetComponent<Text>();
        countText = this.transform.Find("").GetComponent<Text>();
        costResText = this.transform.Find("").GetComponent<Text>();
        buyBtn = this.transform.Find("").GetComponent<Button>();

        buyBtn.onClick.AddListener(OnClickBuyBtn);
    }

    public void RefreshUI(BoxShopItem data)
    {
        this.data = data;
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
        var localPlayer = BattleManager.Instance.GetLocalPlayer();
        localPlayer.BuyBoxFromShop(data.quality,1);
    }

    public void Release()
    {
        buyBtn.onClick.RemoveAllListeners();
    }
}