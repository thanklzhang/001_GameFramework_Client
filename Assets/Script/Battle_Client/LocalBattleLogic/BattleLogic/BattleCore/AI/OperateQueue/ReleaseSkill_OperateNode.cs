using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class ReleaseSkill_OperateNode : OperateNode
    {
        public ReleaseSkillBean releaseSkill;

        protected override void OnExecute()
        {
            this.entity.CheckAndReleaseSkill(releaseSkill.skillId, releaseSkill.targetGuid,
                releaseSkill.targetPos);
        }

        protected override void OnUpdate()
        {
            var isSuccess = this.entity.CheckAndReleaseSkill(releaseSkill.skillId, releaseSkill.targetGuid,
                releaseSkill.targetPos);
            if (!isSuccess)
            {
                
            }

            if (releaseSkill.targetGuid > 0)
            {
                //缺失目标后直接结束技能
                var findEntity = this.battle.FindEntity(releaseSkill.targetGuid);
                if (null == findEntity)
                {
                    this.operateModule.OnNodeExecuteFinish(releaseSkill.skillId);
                }           
            }

           
        }

        public override int GenKey()
        {
            return releaseSkill.skillId;
        }

        public override bool IsCanBeBreak()
        {
            var skill = this.entity.FindSkillByConfigId(releaseSkill.skillId);
            return skill.state == RelaseSkillState.ReadyRelease || skill.state == RelaseSkillState.CD;
        }
    }

    public class ReleaseSkillBean
    {
        public int skillId;
        public int targetGuid;
        public Vector3 targetPos;
    }



}
