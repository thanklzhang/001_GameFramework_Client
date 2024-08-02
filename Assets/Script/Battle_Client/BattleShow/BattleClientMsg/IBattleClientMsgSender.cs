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
    public class ItemUseArg
    {
        public int itemIndex;
        public int releaserGuid;
        public int targetGuid;
        public UnityEngine.Vector3 targetPos;
    }

    public interface IBattleClientMsgSender
    {
        void Send_PlayerLoadProgress(int progress);//千分比
        void Send_BattleReadyFinish();
        void Send_ClientPlotEnd();
        void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos);
        void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos);
        void Send_UseItem(ItemUseArg itemUseArg );
        void Send_UseSkillItem(ItemUseArg itemUseArg);
        void Send_OpenBox();
        void Send_SelectBoxReward(int index);

    }
}