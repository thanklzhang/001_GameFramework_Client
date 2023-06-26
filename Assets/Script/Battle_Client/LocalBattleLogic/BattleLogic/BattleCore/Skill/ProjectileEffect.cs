using System.Collections.Generic;

namespace Battle
{
    public enum RedirectType
    {
        //跟随施法单位
        FollowReleaserEntity = 1,
        //转向点到施法单位的方向
        RedirectPointToReleaserEntityDir = 2,
        //固定偏转角度
        FixedDeflectionAngle = 3,
    }

    public class ProjectileEffect : SkillEffect
    {
        public Vector3 position;

        public float triggerTimer;//每一段时间触发一次(默认会直接执行一次)
        public float lastTimer;

        public float speed;

        public Vector3 targetPos;
        public int targetGuid;


        public IProjectileEffectConfig tableConfig;

        Battle battle;

        Vector3 initDir;

        int currSuplusDirectCount;
        public bool isFollow;

        HashSet<int> colliderEntityGuidSet;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = battle.ConfigManager.GetById<IProjectileEffectConfig>(this.configId);
        }

        public override void OnStart()
        {
            base.OnStart();

            var battle = this.context.battle;

            this.position = this.context.fromSkill.releser.position;

            var startPos = position;

            lastTimer = this.tableConfig.LastTime / 1000.0f;

            //tableConfig = TableManager.Instance.GetById<Table.ProjectileEffect>(this.configId);
            isFollow = tableConfig.IsFollow;

            currSuplusDirectCount = this.tableConfig.EndRedirectCount;

            colliderEntityGuidSet = new HashSet<int>();

            EffectMoveArg effectrMoveArg = new EffectMoveArg()
            {
                effectGuid = this.guid,
                startPos = startPos,
                //targetPos = targetPos,
                targetGuid = targetGuid,
                isFollow = isFollow,
                moveSpeed = speed

            };

            if (!isFollow)
            {
                initDir = (targetPos - this.position).normalized;
                //计算偏转角度
                float angle = this.tableConfig.DeflectionAngle;

                Quaternion q = new Quaternion(0, angle * MathTool.Deg2Rad, 0);
                this.initDir = (q * this.initDir).normalized;
                this.initDir = new Vector3(this.initDir.x, 0, this.initDir.z);

                effectrMoveArg.targetPos = this.position + this.initDir * 1;


            }
            else
            {
                effectrMoveArg.targetPos = targetPos;
            }

            battle.OnSkillEffectStartMove(effectrMoveArg);

        }

        public override void OnUpdate(float timeDetla)
        {
            var battle = this.context.battle;
            lastTimer = lastTimer - battle.TimeDelta;

            if (lastTimer > 0)
            {
                //存活

                //判断碰撞--
                if (CheckCollision())
                {
                    //销毁了
                    return;
                }

                //处理移动--

                //是否跟随实体
                if (isFollow)
                {
                    //跟随某一个实体
                    var target = battle.FindEntity(targetGuid);
                    if (target != null)
                    {
                        var moveTargetPos = target.position;
                        var isReach = this.MoveToPos(moveTargetPos);
                        if (isReach)
                        {
                            //追到了目标实体
                            HandleEndRedirect();
                        }
                    }
                    else
                    {
                        _Battle_Log.LogError("the target is not found : " + targetGuid);
                        this.SetWillEndState();
                    }
                }
                else
                {
                    //判断是否按照最大飞行距离飞行
                    var isFlyMaxLength = this.tableConfig.IsFlyMaxRange;
                    if (isFlyMaxLength)
                    {
                        //按照最大飞行距离飞行 而不是到目标点就消失
                        var dir = initDir;
                        dir.y = 0;
                        MoveByDir(dir);
                    }
                    else
                    {
                        //到目标点就消失
                        var isReach = this.MoveToPos(targetPos);
                        if (isReach)
                        {
                            //到了目标点
                            HandleEndRedirect();
                        }
                    }
                }
            }
            else
            {
                HandleEndRedirect();
            }
        }


        //碰撞检测
        //return 投掷物是否销毁
        public bool CheckCollision()
        {
            //有待优化
            var allEntities = battle.GetAllEntities();
            foreach (var item in allEntities)
            {
                var entity = item.Value;
                //先默认选择敌方
                if (entity.Team != this.context.fromSkill.releser.Team)
                {
                    var sqrtDis = Vector3.SqrtDistance(this.position, entity.position);
                    var dis = 1.5f;
                    var calDis = dis * dis;
                    if (sqrtDis <= calDis)
                    {
                        if (colliderEntityGuidSet.Contains(entity.guid))
                        {
                            //已经存在 不触发
                        }
                        else
                        {
                            //触发碰撞技能效果

                            //判断碰撞组(多个技能效果击中一个目标只生效一个)
                            bool isCanEffect = false;
                            var group = this.context.collisonGroupEffect;
                            if (group != null)
                            {
                                if (group.IsHasCollsion(entity.guid))
                                {
                                    isCanEffect = false;
                                }
                                else
                                {
                                    group.OnCollisionEntity(entity.guid);
                                    isCanEffect = true;
                                }
                            }
                            else
                            {
                                isCanEffect = true;
                            }

                            if (isCanEffect)
                            {
                                //填充此时的上下文 目前不考虑群招 
                                SkillEffectContext context = new SkillEffectContext()
                                {
                                    selectEntities = new List<BattleEntity>() { entity },
                                    battle = this.context.battle,
                                    fromSkill = this.context.fromSkill
                                };

                                this.TriggerCollisionEffect(context);

                                colliderEntityGuidSet.Add(entity.guid);

                                var isThrough = this.tableConfig.IsThrough;
                                if (isThrough)
                                {
                                    //穿透 继续飞行
                                }
                                else
                                {
                                    //飞行结束 判断转向 
                                    var isRedirect = HandleEndRedirect();
                                    if (!isRedirect)
                                    {
                                        //没有转向则已经销毁了
                                        return true;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return false;
        }

        //移动
        //return 是否到达目的地
        public bool MoveToPos(Vector3 targetPos)
        {

            var dir = (targetPos - this.position).normalized;
            var currFramePos = this.position;
            var nextFramePos = this.position + dir * speed * battle.TimeDelta;
            var dotValue = Vector3.Dot(nextFramePos - targetPos, targetPos - currFramePos);

            MoveByDir(dir);

            var isReach = dotValue >= 0;
            if (isReach)
            {
                this.position = targetPos;
            }

            return isReach;
        }

        public void MoveByDir(Vector3 dir)
        {
            this.position = this.position + dir * speed * battle.TimeDelta;
        }


        //碰到敌人触发 effect
        public void TriggerCollisionEffect(SkillEffectContext context)
        {
            //_Battle_Log.Log(string.Format("ProjectileEffect effect of guid : {0} TriggerCollisionEffect", this.guid));

            foreach (var item in tableConfig.CollisionEffectList)
            {
                var id = item;
                var battle = this.context.battle;
                battle.AddSkillEffect(id, context);
            }
        }

        //处理转向
        //retuturn 是否转向 如果不转向 那么直接销毁
        public bool HandleEndRedirect()
        {
            var isRerect = CheckEndRedirect();
            if (isRerect)
            {
                DoRedirect();
            }
            else
            {
                this.SetWillEndState();
            }

            return isRerect;
        }

        //判断结束的时候是否会转向
        public bool CheckEndRedirect()
        {
            return currSuplusDirectCount > 0;
        }

        //转向
        public void DoRedirect()
        {
            this.lastTimer = this.tableConfig.EndRedirectLastTime / 1000.0f;

            RedirectType directType = (RedirectType)this.tableConfig.EndRedirectType;
            if (directType == RedirectType.FollowReleaserEntity)
            {
                //转向为跟随施法单位
                this.isFollow = true;
                this.targetGuid = this.context.fromSkill.releser.guid;

                var tagetEntity = battle.FindEntity(targetGuid);
                var targetPos = Vector3.one;
                if (tagetEntity != null)
                {
                    targetPos = tagetEntity.position;
                }
                else
                {
                    this.SetWillEndState();
                }

                EffectMoveArg effectrMoveArg = new EffectMoveArg()
                {
                    effectGuid = this.guid,
                    startPos = this.position,
                    targetPos = targetPos,
                    targetGuid = targetGuid,
                    isFollow = isFollow,
                    moveSpeed = speed

                };

                battle.OnSkillEffectStartMove(effectrMoveArg);

            }

            currSuplusDirectCount = currSuplusDirectCount - 1;

            colliderEntityGuidSet?.Clear();

            this.context.collisonGroupEffect?.ClearCollisionCache(this.tableConfig.Id);


        }

        public override void OnEnd()
        {
            foreach (var item in tableConfig.EndEffectList)
            {
                var id = item;
                var battle = this.context.battle;

                battle.AddSkillEffect(id, context);
            }
        }
    }

}


