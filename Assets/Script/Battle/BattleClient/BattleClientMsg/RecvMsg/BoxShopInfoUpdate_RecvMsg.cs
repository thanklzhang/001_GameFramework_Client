﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client
{
    public class BoxShopInfoUpdate_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as BoxShopInfoUpdate_RecvMsg_Arg;

            var player = BattleManager.Instance.GetLocalPlayer();
            if (player != null && arg.playerIndex == player.playerIndex)
            {
                player.SetBoxShopItemsData(arg.boxDic);
            }
        }
    }

    public class BoxShopInfoUpdate_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int playerIndex;
        public Dictionary<RewardQuality, BoxShopItem> boxDic;
    }
}