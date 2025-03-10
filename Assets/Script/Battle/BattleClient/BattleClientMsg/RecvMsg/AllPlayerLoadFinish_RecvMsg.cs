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
    public class AllPlayerLoadFinish_RecvMsg : ClientRecvMsg
    {
        
        public override void Handle()
        {
            var arg = this.msgArg as AllPlayerLoadFinish_RecvMsg_Arg;
            EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);
        }
    }

    public class AllPlayerLoadFinish_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        
    }
}