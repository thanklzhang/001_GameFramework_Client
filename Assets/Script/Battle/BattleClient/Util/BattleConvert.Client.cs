﻿using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Battle;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    public partial class BattleConvert
    {
        //战斗逻辑中货币 转换为 客户端战斗中的传输数据货币
        public static Dictionary<int, BattleClient_CurrencyItem> ConvertTo(
            Dictionary<int, BattleCurrencyItem> battleCurrencyItemDic)
        {
            var dic = new Dictionary<int, BattleClient_CurrencyItem>();

            foreach (var kv in battleCurrencyItemDic)
            {
                var _item = kv.Value;

                var item = new BattleClient_CurrencyItem();
                item.itemId = _item.itemConfigId;
                item.count = _item.count;
                dic.Add(item.itemId, item);
            }

            return dic;
        }

        //我的宝箱：战斗逻辑 -》 客户端战斗
        public static Dictionary<RewardQuality, MyBoxGroup> ConvertTo(
            Dictionary<RewardQuality, BattleClientMsg_MyBoxQualityGroup> _dic)
        {
            Dictionary<RewardQuality, MyBoxGroup> dic = new Dictionary<RewardQuality, MyBoxGroup>();
            foreach (var kv in _dic)
            {
                var _quality = kv.Key;
                var _item = kv.Value;

                var group = new MyBoxGroup();
                group.quality = _quality;
                group.count = _item.count;

                dic.Add(_quality, group);
            }

            return dic;
        }
        
        //宝箱商店转换：战斗逻辑 -》 客户端战斗
        public static Dictionary<RewardQuality, BoxShopItem> ConvertTo(
            Dictionary<RewardQuality,BattleBoxShopItem > _dic)
        {
            var dic = new Dictionary<RewardQuality, BoxShopItem>();
            foreach (var kv in _dic)
            {
                var _quality = kv.Key;
                var _item = kv.Value;

                var shopItem = new BoxShopItem();
                shopItem.configId = _item.configId;
                shopItem.canBuyCount = _item.GetCanBuyCount();
                shopItem.maxBuyCount = _item.GetMaxBuyCount();
                shopItem.costItemId = _item.costItemId;
                shopItem.costCount = _item.costCount;

                dic.Add(_quality, shopItem);
            }

            return dic;
        }
        
    }
}