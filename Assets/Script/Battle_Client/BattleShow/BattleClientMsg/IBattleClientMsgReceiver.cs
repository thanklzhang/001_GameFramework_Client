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

    //客户端战斗消息接收
    public interface IBattleClientMsgReceiver
    {
        //void On_EntityStartMove(IBattleClientMsg battleMsg);
        //void On_CreateBattle(BattleClientMsg_InitArg initArg);
        void On_AllPlayerLoadFinish();
        void On_StartBattle();
        void On_CreateEntities(List<BattleClientMsg_Entity> entity);
        //void On_EntityStartMove(int Guid, UnityEngine.Vector3 EndPos, UnityEngine.Vector3 uDir, float MoveSpeed);
        void On_EntityStartMoveByPath(int Guid, List<UnityEngine.Vector3> EndPos, float MoveSpeed);
        void On_EntityStopMove(int Guid, UnityEngine.Vector3 EndPos);
        void On_EntitySyncDir(int Guid, UnityEngine.Vector3 dir);
        void On_EntityUseSkill(int entityGuid,int skillConfig);
        void On_CreateSkillEffect(int skillGuid, int resId, UnityEngine.Vector3 pos, int followEntityGuid, bool isAutoDestroy);
        void On_SkillEffectStartMove(int EffectGuid, UnityEngine.Vector3 TargetPos,int TargetGuid, float moveSpeed);
        void On_DestroySkillEffect(int effectGuid);
        void On_SyncEntityAttr(int entityGuid, List<BattleClientMsg_BattleAttr> atts);
        void On_SyncEntityValue(int entityGuid, List<BattleClientMsg_BattleValue> values);
        void On_EntityDead(int entityGuid);
        void On_PlayPlot(string plotName);
        void On_PlotEnd();
        void On_SetEntitiesShowState(List<int> Guids, bool isShow);
        void On_BattleEnd(bool isWin);
    }


}