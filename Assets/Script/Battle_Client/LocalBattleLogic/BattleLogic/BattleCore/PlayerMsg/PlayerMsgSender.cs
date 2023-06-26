using System.Collections.Generic;

namespace Battle
{
    public interface IPlayerMsgSender
    {
        void NotifyAll_AllPlayerLoadFinish();
        void NotifyAll_BattleStart();

        void NotifyAll_PlayPlot(string name);
        //void NotifyAll_EntityStartMove(int guid, Vector3 targetPos, Vector3 dir,float finalMoveSpeed);
        void NotifyAll_EntityStartMoveByPath(int guid, List<Vector3> pathList, float finalMoveSpeed);
        void NotifyAll_EntityStopMove(int guid, Vector3 position);
        void NotifyAll_SyncEntityDir(int guid, Vector3 dir);
        void NotifyAll_OnEntityReleaseSkill(int guid, int skillConfigId);
        void NotifyAll_CreateSkillEffect(CreateEffectInfo createEffectInfo);
        void NotifyAll_NotifyUpdateBuffInfo(BuffEffectInfo buffInfo);
        void NotifyAll_SkillEffectStartMove(EffectMoveArg effectrMoveArg);
        void NotifyAll_SkillEffectDestroy(int guid);
        void NotifyAll_CreateEntities(List<BattleEntity> entitiese);
        void NotifyAll_SyncEntityAttr(int guid, Dictionary<EntityAttrType, float> dic);
        void NotifyAll_SyncEntityCurrHealth(int guid, int hp, int fromEntityGuid);
        void NotifyAll_EntityAddBuff(int guid, BuffEffect buff);
        void NotifyAll_EntityDead(BattleEntity battleEntity);
        void NotifyAll_AllPlayerPlotEnd(string plotName);
        void NotifyAll_SetEntitiesShowState(List<int> entityGuids, bool isShow);
        void NotifyAll_NotifySkillInfoUpdate(Skill skill);
        void NotifyAll_NotifySkillTrackStart(Skill skill,int skillTrackId);
        void NotifyAll_NotifySkillTrackEnd(Skill skill, int skillTrackId);
        //void NotifyAll_BattleEnd(int winTeam);


        void SendMsgToClient(int uid, int cmd, byte[] bytes);
        void NotifyAllPlayerMsg(int cmd, byte[] bytes);
      
    }
}

