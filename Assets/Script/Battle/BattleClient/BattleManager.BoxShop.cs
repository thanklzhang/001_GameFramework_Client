using System.Collections.Generic;
using Battle;
using GameData;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    //宝箱商店
    public partial class BattleManager
    {
        private BoxShop boxShop;

        public void SetBoxShop(Dictionary<RewardQuality, BoxShopItem> shopItemDic)
        {
            boxShop.SetBoxShopItems(shopItemDic);
        }

        public void Buy()
        {
            boxShop.Buy();
        }

        public Dictionary<RewardQuality, BoxShopItem> GetAllBoxShopItem()
        {
            return this.boxShop.boxShopItemDic;
        }
    }
}