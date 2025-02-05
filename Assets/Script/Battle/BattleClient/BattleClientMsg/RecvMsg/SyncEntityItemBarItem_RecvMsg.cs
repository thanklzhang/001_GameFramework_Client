using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client
{
    public class SyncEntityItemBarItem_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SyncEntityItemBarItem_RecvMsg_Arg;
            // var player = BattleManager.Instance.GetLocalPlayer();
            var entity = BattleEntityManager.Instance.FindEntity(arg.entityGuid);
            if (entity != null)
            {
                entity.UpdateItemBarItem(arg.index,arg.itemData);
            }
            // if (player != null && arg.playerIndex == player.playerIndex)
            // {
            //     player.SyncWarehouseItem(arg.itemData, arg.index);
            // }
        }
    }

    public class SyncEntityItemBarItem_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int entityGuid;
        public BattleItemData_Client itemData;
        public int index;
    }
}