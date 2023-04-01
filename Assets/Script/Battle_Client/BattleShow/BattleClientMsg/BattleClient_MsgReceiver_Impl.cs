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

    //战斗客户端消息接收器
    public class BattleClient_MsgReceiver_Impl : IBattleClientMsgReceiver
    {
        public void On_AllPlayerLoadFinish()
        {
            EventDispatcher.Broadcast(EventIDs.OnAllPlayerLoadFinish);
        }

        public void On_StartBattle()
        {
            EventDispatcher.Broadcast(EventIDs.OnBattleStart);

            //已经加载好的实体统一走一遍创建流程
            BattleEntityManager.Instance.NotifyCreateAllEntities();

            BattleManager.Instance.BattleState = Battle_Client.BattleState.Running;
        }

        public void On_CreateEntities(List<BattleClientMsg_Entity> entityList)
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                var msgEntity = entityList[i];
                var entity = BattleEntityManager.Instance.CreateEntity(msgEntity);
                EventDispatcher.Broadcast<BattleEntity>(EventIDs.OnCreateEntity, entity);
            }

        }

        public void On_CreateSkillEffect(CreateEffectInfo createEffectInfo)
        {
            //var lastTime = lastTimeInt / 1000.0f;
            BattleSkillEffectManager.Instance.CreateSkillEffect(createEffectInfo);
        }

        public void On_DestroySkillEffect(int effectGuid)
        {
            BattleSkillEffectManager.Instance.DestorySkillEffect(effectGuid);
        }

        public void On_EntityDead(int entityGuid)
        {
            var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
            if (entity != null)
            {
                entity.Dead();
            }
        }

        //public void On_EntityStartMove(int Guid, UnityEngine.Vector3 EndPos, UnityEngine.Vector3 dir, float MoveSpeed)
        //{
        //    var guid = Guid;
        //    var targetPos = EndPos;
        //    var moveSpeed = MoveSpeed;
        //    var entity = BattleEntityManager.Instance.FindEntity(guid);
        //    if (entity != null)
        //    {
        //        entity.StartMove(targetPos, dir, moveSpeed);
        //    }
        //}


        public void On_EntityStartMoveByPath(int Guid, List<UnityEngine.Vector3> EndPos, float MoveSpeed)
        {
            var guid = Guid;
            var targetPos = EndPos;
            var moveSpeed = MoveSpeed;
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                entity.StartMoveByPath(EndPos, moveSpeed);
            }
        }
        
        public void On_EntityStopMove(int Guid, UnityEngine.Vector3 EndPos)
        {
            var guid = Guid;
            var endPos = EndPos;
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                entity.StopMove(endPos);
            }
        }


        public void On_EntitySyncDir(int guid, UnityEngine.Vector3 dir)
        {
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                entity.SetToward(dir);
            }
        }

        public void On_EntityUseSkill(int entityGuid, int skillConfig)
        {
            var guid = entityGuid;
            var entity = BattleEntityManager.Instance.FindEntity(guid);
            if (entity != null)
            {
                entity.ReleaseSkill(skillConfig);
            }
        }

        public void On_PlayPlot(string plotName)
        {
            var name = plotName;

            BattleEntityManager.Instance.SetAllEntityShowState(false);
            CameraManager.Instance.GetCameraUI().SetUICameraShowState(false);

            PlotManager.Instance.StartPlot(name);
        }

        public void On_PlotEnd()
        {
            BattleEntityManager.Instance.SetAllEntityShowState(true);
            CameraManager.Instance.GetCameraUI().SetUICameraShowState(true);

            PlotManager.Instance.ClosePlot();
        }

        public void On_SetEntitiesShowState(List<int> Guids, bool isShow)
        {
            var entityGuids = Guids;
            BattleEntityManager.Instance.SetEntitiesShowState(isShow, entityGuids);
        }

        public void On_SkillEffectStartMove(int EffectGuid, UnityEngine.Vector3 TargetPos, int TargetGuid, float moveSpeed)
        {
            var guid = EffectGuid;
            var skillEffect = BattleSkillEffectManager.Instance.FindSkillEffect(guid);
            if (skillEffect != null)
            {
                skillEffect.StartMove(TargetPos, TargetGuid, moveSpeed);
            }
        }


        public void On_SyncEntityAttr(int entityGuid, List<BattleClientMsg_BattleAttr> atts)
        {
            var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
            if (entity != null)
            {
                entity.SyncAttr(atts);
            }
        }

        public void On_SyncEntityValue(int entityGuid, List<BattleClientMsg_BattleValue> values)
        {
            var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
            if (entity != null)
            {
                entity.SyncValue(values);
            }
        }

        public void On_BattleEnd(bool isWin)
        {
            //BattleManager.Instance.BattleState = BattleState.End;

            //EventDispatcher.Broadcast<bool>(EventIDs.OnBattleEnd, isWin);
        }

    }
}