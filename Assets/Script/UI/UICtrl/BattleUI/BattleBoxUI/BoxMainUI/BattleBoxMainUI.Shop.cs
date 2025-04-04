﻿using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

//战斗宝箱界面 中 商店页面
public partial class BattleBoxMainUI
{
    public Transform shopRoot;
    public Transform shopItemRoot;

    private List<BoxShopShowItem> shopItemList;

    public void InitShop()
    {
        shopRoot = transform.Find("root/shop");
        shopItemRoot = shopRoot.Find("shopItemRoot/mask/content");
        shopItemList = new List<BoxShopShowItem>();
    }

    public void RefreshShopUI()
    {
        //get data
        List<BoxShopItem> dataList = new List<BoxShopItem>();

        var localPlayer = BattleManager.Instance.GetLocalPlayer();
        var shopItemDic = localPlayer.GetAllBoxShopItems();
        foreach (var kv in shopItemDic)
        {
            var quality = kv.Key;
            var shopItem = kv.Value;

            dataList.Add(shopItem);
        }


        //refresh show
        foreach (var shopShowItem in shopItemList)
        {
            shopShowItem.Release();
        }
        
        shopItemList.Clear();

        for (int i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            BoxShopShowItem showItem = null;
            GameObject go = null;
            if (i < shopItemRoot.childCount)
            {
                go = shopItemRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(shopItemRoot.GetChild(0).gameObject,
                    shopItemRoot, false);
            }

            showItem = new BoxShopShowItem();
            showItem.Init(go);
            showItem.RefreshUI(data);
            
            shopItemList.Add(showItem);
        }
    }
}