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
    public class BattleClient_MsgSender_Remote : IBattleClientMsgSender
    {
        public void Send_PlayerLoadProgress(int progress)
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            netHandle.SendPlayerLoadProgress(progress);
        }

        public void Send_BattleReadyFinish()
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            netHandle.SendBattleReadyFinish(null);
        }

        public void Send_ClientPlotEnd()
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            netHandle.SendClientPlotEnd();
        }

        public void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos)
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            netHandle.SendMoveEntity(guid, targetPos);
        }

        public void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos)
        {
            var netHandle = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            netHandle.SendUseSkill(skillId, targetGuid, targetPos);
        }
    }
}