using System;
using System.Collections.Generic;

namespace Battle
{


    public class AIMgr
    {
        Battle battle;
        public Dictionary<int, BaseAI> aiDic;
        public void Init(Battle battle)
        {
            this.battle = battle;
            aiDic = new Dictionary<int, BaseAI>();
        }

        public void UseAIToEntity(BaseAI ai, BattleEntity entity)
        {
           // Logx.Log("add entity.guid : " + entity.guid);
            aiDic.Add(entity.guid, ai);
            ai.Init(entity);
        }

        public void Update(float timeDelta)
        {
            foreach (var item in aiDic)
            {
                var ai = item.Value;
                ai.Update(timeDelta);
            }
        }

        internal void OnEntityStopMove(int guid, Vector3 position)
        {
            if (aiDic.ContainsKey(guid))
            {
                var ai = aiDic[guid];
                ai.OnStopMove();
            }
            else
            {
                _Battle_Log.LogWarning("AIMgr : OnEntityStopMove : the guid is not found : " + guid);
            }
        }

        internal void OnMoveToCurrTargetPosFinish(int entityGuid)
        {
            if (aiDic.ContainsKey(entityGuid))
            {
                var ai = aiDic[entityGuid];
                ai.OnMoveToCurrTargetPosFinish();
            }
            else
            {
                _Battle_Log.LogWarning("AIMgr : OnMoveToCurrTargetPosFinish : the guid is not found : " + entityGuid);
            }
        }



        internal void OnEntityFinishSkillEffect(int entityGuid, Skill skill)
        {
            var skillConfigId = skill.configId;
            if (aiDic.ContainsKey(entityGuid))
            {
                var ai = aiDic[entityGuid];
                ai.OnFinishSkillEffect(skill);

            }
            else
            {
                _Battle_Log.LogWarning("AIMgr : OnEntityStartSkillEffect : the guid is not found : " + entityGuid);
            }
        }

        internal BaseAI FindAI(int guid)
        {
            BaseAI ai = null;
            if (aiDic.ContainsKey(guid))
            {
                ai = aiDic[guid];
            }
            else
            {
                _Battle_Log.LogWarning("AIMgr : FindAI : the guid is not found : " + guid);
            }
            return ai;
        }

        internal void RemoveAI(int guid)
        {
            var ai = FindAI(guid);
            if (this.aiDic.ContainsKey(guid))
            {
                this.aiDic[guid] = null;
                this.aiDic.Remove(guid);
            }
        }

        internal void OnEntityBeHurt(BattleEntity battleEntity, float resultDamage, Skill skill)
        {
            var ai = this.FindAI(battleEntity.guid);
            if (ai != null)
            {
                ai.OnBeHurt(resultDamage,skill);
            }
        }
    }

}


