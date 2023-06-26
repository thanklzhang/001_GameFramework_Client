//namespace Battle
//{
//    public class CommonEntityAI : BaseAI
//    {

//        int willReleaseSkillId;
//        int skillTargetGuid;
//        Vector3 skillTargetPos;
//        float patrolRange = 10.0f;

//        //移动 cd ， 例如跟随敌人的时候需要实时跟随 但不能每帧都检测相对方向和距离 所以有个延迟时间
//        float moveCD = 0.5f;
//        float currMoveCDTimer;



//        public bool IsWillNormalAttack()
//        {
//            var skill = this.entity.FindSkillByConfigId(this.willReleaseSkillId);
//            return skill.isNormalAttack;
//        }

//        public override void AskMoveToPos(Vector3 targetPos)
//        {
//            if (currMoveCDTimer > 0)
//            {
//                //在 移动 CD 中
//                return;
//            }

//            currMoveCDTimer = moveCD;
//            this.entity.StartMoveToPos(targetPos);
//        }

//        public override void AskReleaseSkill(int skillId, int skillTargetGuid, Vector3 skillTargetPos)
//        {
//            //this.entity.ReleaseSkill(skillId, skillTargetGuid, skillTargetPos);
//            willReleaseSkillId = skillId;
//            this.skillTargetGuid = skillTargetGuid;
//            this.skillTargetPos = skillTargetPos;

//            var isInSkillRange = this.entity.IsInSkillReleaseRange(skillId, skillTargetGuid, skillTargetPos);
//            if (isInSkillRange)
//            {
//                //_G.Log("AskReleaseSkill : ReleaseSkill ");
//                this.entity.ReleaseSkill(skillId, skillTargetGuid, skillTargetPos);
//            }
//            else
//            {
//                //_G.Log("AskReleaseSkill : MoveToSkillTargetPos ");

//                Vector3 targetPos = GetSkillTargetPos();
//                this.AskMoveToPos(targetPos);
//            }

//        }

//        public void ClearSkillTarget()
//        {
//            willReleaseSkillId = 0;
//            skillTargetGuid = 0;
//            skillTargetPos = Vector3.zero;
//        }


//        public void AskChangeToIdle()
//        {

//            ClearSkillTarget();

//            this.entity.ChangeToIdle();
//        }

//        public void AskNormalAttack(int targetGuid)
//        {
//            var normalAttackSkill = this.entity.GetNormalAttackSkill();
//            if (null == normalAttackSkill)
//            {
//                _Battle_Log.LogWarning("the normalAttackSkill is null : by " + this.entity.infoConfig.Name);
//                return;
//            }

//            this.willReleaseSkillId = normalAttackSkill.configId;
//            this.skillTargetGuid = targetGuid;

//            //AskReleaseSkill(this.willReleaseSkillId, this.skillTargetGuid, skillTargetPos);
//            this.entity.AskReleaseSkill(this.willReleaseSkillId, this.skillTargetGuid, skillTargetPos);
//        }

//        public override void Update(float timeDelta)
//        {
//            //return;
//            var state = this.entity.EntityState;
//            var battle = this.entity.GetBattle();

//            //目前适用于自动巡逻普通攻击
//            if (state == EntityState.Idle)
//            {
//                if (willReleaseSkillId > 0)
//                {
//                    //有将要释放的技能

//                    //判断技能目标状态
//                    var attackTargetEntity = this.entity.GetBattle().FindEntity(skillTargetGuid);
//                    if (null == attackTargetEntity || attackTargetEntity.EntityState == EntityState.Dead)
//                    {
//                        this.ClearSkillTarget();
//                        return;
//                    }

//                    //判断目标是否在巡逻范围内
//                    Vector3 targetPos = GetSkillTargetPos();
//                    //判断是否在技能范围内
//                    var skill = this.entity.FindSkillByConfigId(willReleaseSkillId);


//                    var sqrtDis = Vector3.SqrtDistance(targetPos, this.entity.position);
//                    if (sqrtDis <= patrolRange * patrolRange)
//                    {
//                        //在巡逻范围内

//                        //判断是否在攻击范围内
//                        var isInReleaseRange = this.entity.IsInSkillReleaseRange(willReleaseSkillId, this.skillTargetGuid, this.skillTargetPos);

//                        if (isInReleaseRange)
//                        {
//                            //在攻击范围内

//                            //发起攻击
//                            //this.AskNormalAttack(skillTargetGuid);
//                            this.entity.ReleaseSkill(willReleaseSkillId, skillTargetGuid, skillTargetPos);
//                        }
//                        else
//                        {
//                            //不在攻击范围内 开始跑向目标点
//                            currMoveCDTimer = 0;
//                            this.AskMoveToPos(targetPos);
//                        }
//                    }
//                    else
//                    {
//                        //此时目标点已经不在出巡逻范围了 取消跟踪目标 开始闲置
//                        this.AskChangeToIdle();
//                    }
//                }
//                else
//                {
//                    //正在闲置

//                    //寻找巡逻范围内最近的敌人
//                    BattleEntity minDisEntity = null;
//                    float minDis = 9999999;
//                    var allEntities = battle.GetAllEntities();
//                    foreach (var item in allEntities)
//                    {
//                        var currEntity = item.Value;
//                        var sqrtDis = Vector3.SqrtDistance(currEntity.position, this.entity.position);

//                        if (sqrtDis <= minDis && currEntity.Team != this.entity.Team)
//                        {
//                            minDis = sqrtDis;
//                            minDisEntity = currEntity;
//                        }

//                    }

//                    if (minDisEntity != null)
//                    {
//                        if (minDis <= patrolRange * patrolRange)
//                        {
//                            //找到巡逻范围中最近的敌人

//                            var normalAttackSkill = this.entity.GetNormalAttackSkill();
//                            var isInRange = this.entity.IsInSkillReleaseRange(normalAttackSkill.configId, minDisEntity.guid, Vector3.zero);

//                            if (isInRange)
//                            {
//                                //在攻击距离内

//                                //发起攻击
//                                this.AskNormalAttack(minDisEntity.guid);
//                            }
//                            else
//                            {
//                                //不在攻击距离 开始跑向他
//                                currMoveCDTimer = 0;
//                                this.AskMoveToPos(minDisEntity.position);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //没有找到 继续闲置
//                    }
//                }
//            }
//            else if (state == EntityState.Move)
//            {
//                if (currMoveCDTimer > 0)
//                {
//                    currMoveCDTimer -= timeDelta;
//                }


//                if (willReleaseSkillId > 0)
//                {
//                    //有将要释放的技能

//                    //判断目标是否在巡逻范围内
//                    Vector3 targetPos = this.GetSkillTargetPos();

//                    var sqrtDis = Vector3.SqrtDistance(targetPos, this.entity.position);
//                    if (sqrtDis <= patrolRange * patrolRange)
//                    {
//                        //在巡逻范围内

//                        //判断是否在攻击范围内
//                        var isInReleaseRange = this.entity.IsInSkillReleaseRange(this.willReleaseSkillId, this.skillTargetGuid, this.skillTargetPos);

//                        if (isInReleaseRange)
//                        {
//                            //在攻击范围内

//                            //发起攻击
//                            this.AskNormalAttack(skillTargetGuid);
//                        }
//                        else
//                        {
//                            //不在攻击范围内 开始跑向目标点
//                            this.AskMoveToPos(targetPos);
//                        }
//                    }
//                    else
//                    {
//                        //此时目标点已经不在出巡逻范围了 取消跟踪目标 开始闲置
//                        this.AskChangeToIdle();
//                    }
//                }
//                else
//                {
//                    //正在闲置着走(只有移动到目标点的行为)
//                }
//            }
//            else if (state == EntityState.UseSkill)
//            {

//            }
//            else if (state == EntityState.Dead)
//            {

//            }

//        }

//        public Vector3 GetSkillTargetPos()
//        {
//            Vector3 targetPos = Vector3.zero;
//            if (skillTargetGuid > 0)
//            {
//                //目标是单位
//                var attackTargetEntity = this.entity.GetBattle().FindEntity(skillTargetGuid);

//                targetPos = attackTargetEntity.position;
//            }
//            else
//            {
//                //目前是普通攻击 所以不会走这里
//                //目标是点
//                targetPos = skillTargetPos;

//            }
//            return targetPos;
//        }

//    }
//}


