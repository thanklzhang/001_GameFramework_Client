//using System.Collections.Generic;

//namespace Battle
//{
//    //玩家 AI
//    //1 ：技能释放距离不够时候可以自动移动
//    //2 ：在攻击范围内可以自动普通攻击
//    //3 : 普通攻击可以一直追击目标 TODO:增加个巡逻范围 超过了就不追了
//    public class PlayerAI : BaseAI
//    {
//        protected SkillReleaseReadyInfo skillReleaseReadyInfo;

//        public override void OnInit()
//        {
//            skillReleaseReadyInfo = new SkillReleaseReadyInfo();
//            skillReleaseReadyInfo.Init(this.battle);
//        }

//        //玩家请求移动
//        public override void AskMoveToPos(Vector3 targetPos)
//        {
//            this.skillReleaseReadyInfo.Clear();
//            FindPathAndMoveToPos(targetPos);
//        }

//        public override void AskReleaseSkill(int skillId, int targetGuid, Vector3 targetPos)
//        {
//            var skill = entity.FindSkillByConfigId(skillId);
//            if (skill.isNormalAttack)
//            {
//                var targetEntity = this.battle.FindEntity(targetGuid);
//                if (null == targetEntity)
//                {
//                    //目标点已经找不到了 改为 idle 状态以便进行找下一个目标自动攻击
//                    this.skillReleaseReadyInfo.Clear();
//                    this.entity.ChangeToIdle();
//                    return;
//                }
//            }


//            var isInRange = this.entity.IsInSkillReleaseRange(skillId, targetGuid, targetPos);

//            if (!isInRange)
//            {
//                TryToMoveByReleaseSkill(skillId, targetGuid, targetPos);
//            }
//            else
//            {
//                entity.ReleaseSkill(skillId, targetGuid, targetPos);
//            }
//        }

//        public override void OnStartSkillEffect(Skill skill, int targetGuid, Vector3 targetPos)
//        {
//            //刷新准备技能信息
//            if (skill.isNormalAttack)
//            {
//                if (this.skillReleaseReadyInfo.IsHaveWillReleseSkill())
//                {
//                    if (skill.configId == this.skillReleaseReadyInfo.skillConfigId)
//                    {
//                        //普通攻击会一直攻击 所以不清理
//                    }
//                    else
//                    {
//                        //技能转换为普通攻击 在这里储存准备技能(普通攻击)信息
//                        TryToMoveByReleaseSkill(skill.configId, targetGuid, targetPos);
//                    }
//                }
//                else
//                {
//                    //从没有技能目标转换为开始普通攻击 在这里储存准备技能(普通攻击)信息
//                    TryToMoveByReleaseSkill(skill.configId, targetGuid, targetPos);
//                }
//            }
//            else
//            {
//                if (this.skillReleaseReadyInfo.IsHaveWillReleseSkill())
//                {
//                    this.skillReleaseReadyInfo.Clear();
//                }
//            }
//        }

//        public void TryToMoveByReleaseSkill(int configId, int targetGuid, Vector3 targetPos)
//        {
//            this.skillReleaseReadyInfo.SaveInfo(configId, targetGuid, targetPos);
//            Vector3 tPos;
//            if (skillReleaseReadyInfo.TryToGetSkillTargetPos(out tPos))
//            {
//                FindPathAndMoveToPos(tPos);
//            }
//            else
//            {
//                //目标点已经失去 改为 idle 状态以便进行自动攻击
//                //this.entity.ChangeToIdle();
//            }
//        }

//        public override void OnUpdate(float timeDelta)
//        {
//            //判断下是否有准备释放的技能
//            var isWillReleaseSkill = this.skillReleaseReadyInfo.IsHaveWillReleseSkill();
//            if (isWillReleaseSkill)
//            {
//                var sId = this.skillReleaseReadyInfo.skillConfigId;
//                var tGuid = this.skillReleaseReadyInfo.targetEntityGuid;
//                var tPos = this.skillReleaseReadyInfo.willReleaseSkillTargetPos;
//                AskReleaseSkill(sId, tGuid, tPos);
//            }
//            else
//            {
//                //在不打断玩家行为的情况下自动攻击
//                if (this.entity.EntityState == EntityState.Idle)
//                {
//                    //判断是否周围有在攻击范围内的的敌方单位
//                    BattleEntity nearestEntity = battle.FindNearestEnemyEntity(this.entity);

//                    if (nearestEntity != null)
//                    {
//                        var normalAttackSkill = this.entity.GetNormalAttackSkill();
//                        var sqrtDis = Vector3.SqrtDistance(nearestEntity.position, entity.position);
//                        var attackRange = normalAttackSkill.GetReleaseRange();
//                        if (sqrtDis <= attackRange * attackRange)
//                        {
//                            var targetEntity = nearestEntity;
//                            var normalSkillConfigId = this.entity.GetNormalAttackSkill().configId;
//                            //this.entity.ReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
//                            AskReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
//                        }
//                    }
//                }
//            }
//        }
//    }
//}


