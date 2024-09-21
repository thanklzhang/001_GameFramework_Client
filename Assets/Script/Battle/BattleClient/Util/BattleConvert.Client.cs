using Google.Protobuf;
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
    }
}