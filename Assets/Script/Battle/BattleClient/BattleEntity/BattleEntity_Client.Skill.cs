using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    //技能相关
    public partial class BattleEntity_Client
    {
        List<BattleSkillInfo> skills;
        internal void SetSkillList(List<BattleSkillInfo> skills)
        {
            this.skills = skills;
        }
        public List<BattleSkillInfo> GetSkills()
        {
            return skills;
        }

        
        public BattleSkillInfo FindSkill(int skillConfigId)
        {
            var skill = this.skills.Find((s) => { return s.configId == skillConfigId; });

            return skill;
        }

        
        internal void UpdateSkillInfo(int skillConfigId, float currCDTime, float maxCDTime)
        {
            var skill = this.FindSkill(skillConfigId);
            if (skill != null)
            {
                skill?.UpdateInfo(currCDTime, maxCDTime);
            }
            else
            {
                skill = new BattleSkillInfo();
                skill.configId = skillConfigId;
                skill.releaserGuid = this.guid;
                skill.level = 1;

                this.skills.Add(skill);

                skill?.UpdateInfo(currCDTime, maxCDTime);
            }
        }

        
        internal void ReleaseSkill(int skillConfigId)
        {
            var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillConfigId);
            var normalAttackSkill = GetNormalAttackSkill();
            if (normalAttackSkill != null && normalAttackSkill.configId == skillConfig.Id)
            {
                //普通攻击
                var attackSpeed = this.attr.attackSpeed;
                var aniScale = attackSpeed * (skillConfig.AnimationSpeedScale / 1000.0f);

                PlayAnimation("attack", aniScale);
            }
            else
            {
                //技能
                var aniScale = skillConfig.AnimationSpeedScale / 1000.0f;
                PlayAnimation("attack", aniScale);
            }
            //play animation
            //Logx.Log(this.guid + " release skill : " + skillConfigId);
        }

        public BattleSkillInfo GetNormalAttackSkill()
        {
            if (this.skills.Count > 0)
            {
                return this.skills[0];
            }

            return null;
        }

        internal int GetSkillIdByIndex(int index)
        {
            if (skills.Count > 0)
            {
                return skills[index].configId;
            }
            else
            {
                Logx.LogWarning("the count of skills is 0 : index : " + index);
                return -1;
            }
        }


    }
    
    
    public class BattleSkillInfo
    {
        public int releaserGuid;
        public int configId;
        public int level;
        public float maxCDTime;
        public float currCDTime;

        internal void UpdateInfo(float currCDTime, float maxCDTime)
        {
            this.currCDTime = currCDTime;
            this.maxCDTime = maxCDTime;

            EventDispatcher.Broadcast(EventIDs.OnSkillInfoUpdate, releaserGuid, this);
        }
    }

}