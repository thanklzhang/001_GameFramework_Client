using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;


//战斗宝箱界面 中 我的宝箱仓库界面
public partial class BattleBoxMainUI
{
   private Transform myBoxRoot;
   private Transform myBoxItemRoot;

   private Text coinText;

   private List<MyBoxItem> myBoxItemList;
   
   public void InitMyBox()
   {
      myBoxRoot = transform.Find("root/myBox");
      myBoxItemRoot = myBoxRoot.Find("itemRoot/mask/content");
      coinText = myBoxRoot.Find("coinCountText").GetComponent<Text>();
      myBoxItemList = new List<MyBoxItem>();
   }
   
   public void RefreshMyBoxUI()
   {
      //get data
      List<MyBoxGroup> dataList = new List<MyBoxGroup>();

      var localPlayer = BattleManager.Instance.GetLocalPlayer();
      var itemDic = localPlayer.GetAllMyBoxItems();
      foreach (var kv in itemDic)
      {
         var quality = kv.Key;
         var shopItem = kv.Value;

         dataList.Add(shopItem);
      }
      
      dataList.Sort((a, b) =>
      {
         return a.quality.CompareTo(b.quality);
      });


      //refresh show
      foreach (var boxShopItem in myBoxItemList)
      {
         boxShopItem.Release();
      }
      myBoxItemList.Clear();

      for (int i = 0; i < dataList.Count; i++)
      {
         var data = dataList[i];
         MyBoxItem showItem = null;
         GameObject go = null;
         if (i < myBoxItemRoot.childCount)
         {
            go = myBoxItemRoot.GetChild(i).gameObject;
         }
         else
         {
            go = GameObject.Instantiate(myBoxItemRoot.GetChild(0).gameObject,
               myBoxItemRoot, false);
         }

         showItem = new MyBoxItem();
         showItem.Init(go);
         showItem.RefreshUI(data);
            
         myBoxItemList.Add(showItem);
      }

      var coinCount = BattleManager.Instance.GetLocalPlayer().GetCoinCount();
      coinText.text = coinCount + "战银";
   }

   public void ReleaseMyBox()
   {
      

   }

}