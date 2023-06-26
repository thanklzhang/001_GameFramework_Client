using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Battle.BattleTrigger.Runtime;

namespace Battle
{
    //释放技能的准备信息(一般在距离不够释放的时候临时储存)
    public class SkillReleaseReadyInfo
    {
        public int skillConfigId;
        public int targetEntityGuid;
        public Vector3 willReleaseSkillTargetPos;

        bool isHave;
        Battle battle;
        public void Init(Battle battle)
        {
            this.battle = battle;
        }

        public void SaveInfo(int skillConfigId, int targetEntityGuid, Vector3 skillTargetPos)
        {
            this.skillConfigId = skillConfigId;
            this.targetEntityGuid = targetEntityGuid;
            this.willReleaseSkillTargetPos = skillTargetPos;
            isHave = true;
        }

        public bool TryToGetSkillTargetPos(out Vector3 outpOs)
        {
            outpOs = Vector3.one;
            if (this.skillConfigId > 0)
            {
                if (this.targetEntityGuid > 0)
                {
                    var entity = this.battle.FindEntity(this.targetEntityGuid);
                    if (entity != null)
                    {
                        outpOs = entity.position;
                        return true;
                    }
                }
                else
                {
                    outpOs = this.willReleaseSkillTargetPos;
                    return true;
                }
            }

            return false;
        }

        public bool IsHaveWillReleseSkill()
        {
            return isHave;
        }

        public void Clear()
        {
            this.skillConfigId = 0;
            this.targetEntityGuid = 0;
            willReleaseSkillTargetPos = Vector3.one;
            isHave = false;
        }

    }
}
