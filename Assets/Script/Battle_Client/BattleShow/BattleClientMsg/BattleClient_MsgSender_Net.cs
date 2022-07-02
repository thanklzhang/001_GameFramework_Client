using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
namespace Battle_Client
{
    //战斗客户端消息发送器
    public class BattleClient_MsgSender_Net : IBattleClientMsgSender
    {
        public void Send_BattleReadyFinish()
        {
            throw new NotImplementedException();
        }

        public void Send_ClientPlotEnd()
        {
            throw new NotImplementedException();
        }

        public void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos)
        {
            throw new NotImplementedException();
        }

        public void Send_PlayerLoadProgress(int progress)
        {
            throw new NotImplementedException();
        }

        public void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos)
        {
            throw new NotImplementedException();
        }
    }
}