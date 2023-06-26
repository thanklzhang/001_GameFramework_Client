using System.Collections.Generic;

namespace Battle
{
    //技能释放目标类型
    public enum SkillReleaseTargeType
    {
        //无目标
        NoTarget = 0,
        //实体单位
        Entity = 1,
        //点
        Point = 2
    }

    public enum RelaseSkillState
    {
        ReadyRelease = 1,
        SkillBefore = 2,
        Releasing = 3,
        SkillAfter = 4,
        CD = 5
    }

    public enum SkillReleaseType
    {
        //有前摇后摇的正常技能
        NormalRelease = 0,
        ////瞬发技能
        //ImmediatelyRelease = 1,
        //持续技能
        LastRelease = 1
    }

    //施加技能目标类型
    public enum SkillTargeType
    {
        //无目标
        No = 0,
        //技能目标单位
        SkillTarget = 1,
        //技能释放着
        SkillReleaser = 2
    }

    //技能轨道的开始时机类型
    public enum SkillTrackStartTimeType
    {
        Null = 0,
        //技能前摇开始的时候
        SkillBeforeStart = 1,
        //技能开始了效果的时候
        SkillStartEffect = 2
    }

    //技能轨道的结束时机类型
    public enum SkillTrackEndTimeType
    {
        Null = 0,
        //技能完整流程结束时候(后摇结束时候)
        SkillFinishAllProcess = 1,
    }

    public class Skill
    {
        public int configId;
        public int level;

        public int targetGuid;
        public Vector3 targetPos;

        public BattleEntity releser;

        public ISkillConfig infoConfig;
        internal bool isNormalAttack;

        public RelaseSkillState state = RelaseSkillState.ReadyRelease;

        float currBeforeReleaseTimer = 0;
        float currAfterReleaseTimer = 0;
        float currCDTimer = 0;

        Battle battle;

        public void Init(BattleEntity releser)
        {
            //infoConfig = TableManager.Instance.GetById<Table.Skill>(configId);
            this.releser = releser;
            //skillEffectList = new List<SkillEffect>();
            this.battle = releser.GetBattle();
            this.infoConfig = battle.ConfigManager.GetById<ISkillConfig>(this.configId);

            //被动技能(其实可以判断出来 不过为了突出被动 增加了该字段)
            if (this.infoConfig.IsPassive)
            {
                foreach (var item in infoConfig.EffectList)
                {
                    var effectId = item;
                    SkillEffectContext context = new SkillEffectContext();
                    context.battle = this.GetBattle();
                    context.fromSkill = this;
                    battle.AddSkillEffect(effectId, context);
                }
            }
        }

        public bool Start(int targetGuid, Vector3 targetPos)
        {
            //_G.Log(string.Format("{0} release skill({1}) to target({2})", this.releser.guid,
            //    this.configId, targetGuid));

            this.targetGuid = targetGuid;
            this.targetPos = targetPos;

            var battle = releser.GetBattle();

            state = RelaseSkillState.SkillBefore;
            this.currBeforeReleaseTimer = GetSkillBeforeTotalTime();

            this.currCDTimer = GetCDMaxTime();

            NotifySkillTrackStart(SkillTrackStartTimeType.SkillBeforeStart);

            return true;
        }

        void NotifySkillTrackStart(SkillTrackStartTimeType startTimeType)
        {
            //根据 skill 获取到 track ， 并且根据触发时机来进行触发
            //type == startTimeType
            foreach (var skillTrackId in this.infoConfig.SkillTrackList)
            {
                if (skillTrackId > 0)
                {
                    var trackConfig = this.battle.ConfigManager.GetById<ISkillTrackConfig>(skillTrackId);
                    if (trackConfig.StartTimeType == startTimeType)
                    {
                        this.battle.OnNotifySkillTrackStart(this, skillTrackId);
                    }
                }

            }
        }

        float GetSkillBeforeTotalTime()
        {
            var scale = 1.0f;
            if (this.isNormalAttack)
            {
                var attackSpeed = this.releser.AttackSpeed;
                if (0 == attackSpeed)
                {
                    attackSpeed = 1;
                }
                scale = 1.0f / attackSpeed;
            }

            var rsult = this.infoConfig.BeforeTime * scale / 1000.0f;
            return rsult;
        }

        float GetSkillAfterTotalTime()
        {
            var scale = 1.0f;
            if (this.isNormalAttack)
            {
                var attackSpeed = this.releser.AttackSpeed;
                if (0 == attackSpeed)
                {
                    attackSpeed = 1;
                }
                scale = 1.0f / attackSpeed;
            }

            var rsult = this.infoConfig.AfterTime * scale / 1000.0f;
            return rsult;
        }

        public void Update(float timeDelta)
        {
            var releaseType = (SkillReleaseType)this.infoConfig.SkillReleaseType;

            //CD
            if (state == RelaseSkillState.CD)
            {
                currCDTimer -= timeDelta;
                if (currCDTimer <= 0)
                {
                    //currCDTimer = this.GetCDMaxTime();

                    this.battle.OnSkillInfoUpdate(this);
                    this.state = RelaseSkillState.ReadyRelease;


                    return;
                }
            }

            //技能前摇等先都算成一个时间段
            //if (state == RelaseSkillState.SkillBefore || state == RelaseSkillState.Releasing ||
            //    state == RelaseSkillState.SkillAfter)
            //{
            //    currBeforeReleaseTimer -= timeDelta;
            //    if (currBeforeReleaseTimer <= 0)
            //    {
            //        this.state = RelaseSkillState.CD;
            //        currBeforeReleaseTimer = this.infoConfig.LastTime / 1000.0f;
            //        this.releser.OnSkillReleaseEnd(this);
            //        return;
            //    }
            //}

            if (state == RelaseSkillState.SkillBefore)
            {
                currBeforeReleaseTimer -= timeDelta;
                if (currBeforeReleaseTimer <= 0)
                {
                    //前摇结束 释放技能效果
                    this.StartReleaseSkillEffect();

                    this.state = RelaseSkillState.Releasing;

                    //currBeforeReleaseTimer = GetSkillBeforeTotalTime();
                    var isLastSkill = releaseType == SkillReleaseType.LastRelease;

                    if (isLastSkill)
                    {
                        //等待持续技能效果结束
                    }
                    else
                    {
                        this.battle.OnSkillInfoUpdate(this);
                    }

                    return;
                }
            }

            if (state == RelaseSkillState.Releasing)
            {
                //判断是否是持续技能 如果是持续技能 那么需要 effect 来进行结束该 releasing 状态

                var isLastSkill = releaseType == SkillReleaseType.LastRelease;
                if (isLastSkill)
                {
                    //等待持续技能效果结束
                }
                else
                {
                    OnChageToSkillAfter();
                }

            }

            if (state == RelaseSkillState.SkillAfter)
            {
                currAfterReleaseTimer -= timeDelta;
                if (currAfterReleaseTimer <= 0)
                {


                    //currAfterReleaseTimer = this.GetSkillAfterTotalTime();
                    this.FinishSkillRelease();



                    this.currCDTimer = GetCDTimerTotalTime();


                    return;
                }
            }

        }
        //已经释放了技能效果
        public void OnFinishSkillEffect()
        {
            var entityGuid = this.releser.guid;
            var skill = this;
            this.battle.OnEntityFinishSkillEffect(entityGuid, skill);
        }

        public void StartReleaseSkillEffect()
        {
            //前摇结束 释放技能
            foreach (var item in infoConfig.EffectList)
            {
                var id = item;
                SkillEffectContext context = new SkillEffectContext();
                context.battle = this.GetBattle();
                context.fromSkill = this;
                context.selectEntities = new List<BattleEntity>();

                if (this.infoConfig.SkillTargetType == SkillTargeType.SkillTarget)
                {
                    var targetEntity = this.GetBattle().FindEntity(this.targetGuid);
                    if (targetEntity != null)
                    {
                        context.selectEntities.Add(targetEntity);
                    }
                }
                else if (this.infoConfig.SkillTargetType == SkillTargeType.SkillReleaser)
                {
                    context.selectEntities.Add(this.releser);
                }


                var battle = releser.GetBattle();
                battle.AddSkillEffect(id, context);
            }


        }

        //改为 after 状态
        public void OnChageToSkillAfter()
        {
            //_Battle_Log.Log(string.Format("entity{0} OnChageToSkillAfter", this.releser.infoConfig.Name));
            this.state = RelaseSkillState.SkillAfter;

            currAfterReleaseTimer = GetSkillAfterTotalTime();
            //currAfterReleaseTimer = this.infoConfig.AfterTime / 1000.0f;

        }
        //TODO : Break
        public void Break()
        {
            FinishSkillRelease();
        }

        public void FinishSkillRelease()
        {
            //this.releser.ChangeToIdle();

            this.OnFinishSkillEffect();
            //this.releser.ChangeToIdle();

            if (this.state == RelaseSkillState.CD)
            {
                return;
            }
            this.state = RelaseSkillState.CD;

            var releaseType = (SkillReleaseType)this.infoConfig.SkillReleaseType;
            if (releaseType == SkillReleaseType.LastRelease)
            {
                this.battle.OnSkillInfoUpdate(this);
            }

            //track
            NotifySkillTrackEnd(SkillTrackEndTimeType.SkillFinishAllProcess);

            this.releser.OnSkillReleaseEnd(this);
        }

        void NotifySkillTrackEnd(SkillTrackEndTimeType endTimeType)
        {
            //根据 skill 获取到 track ， 并且根据触发时机来进行触发
            foreach (var skillTrackId in this.infoConfig.SkillTrackList)
            {
                if (skillTrackId > 0)
                {
                    var trackConfig = this.battle.ConfigManager.GetById<ISkillTrackConfig>(skillTrackId);
                    if (trackConfig.EndTimeType == endTimeType)
                    {
                        this.battle.OnNotifySkillTrackEnd(this, skillTrackId);
                    }
                }

            }
        }

        internal Battle GetBattle()
        {
            return releser.GetBattle();
        }

        public float GetReleaseRange()
        {
            if (this.isNormalAttack)
            {
                return this.releser.AttackRange;
            }
            else
            {
                return this.infoConfig.ReleaseRange / 1000.0f;
            }
        }

        //CD 计时器总时间 （总时间 - 前摇 - 后摇） ， 供逻辑用
        float GetCDTimerTotalTime()
        {
            var currCDTimer = 0.0f;
            if (this.isNormalAttack)
            {
                if (0 == this.releser.AttackSpeed)
                {
                    return 0.01f;
                }

                //普通攻击是要用 总 cd 时间 减去 前摇和后摇 保证真实的攻击次数和面板数据对应
                var cdMax = 1.0f / this.releser.AttackSpeed;
                var resultCD = cdMax - this.GetSkillBeforeTotalTime() - this.GetSkillAfterTotalTime();
                if (resultCD < 0)
                {
                    resultCD = 0;
                }
                currCDTimer = resultCD;
            }
            else
            {
                currCDTimer = this.infoConfig.CdTime / 1000.0f - this.GetSkillBeforeTotalTime() - this.GetSkillAfterTotalTime();

            }
            return currCDTimer;
        }

        //CD 时长 （总时间 - 前摇） , 不是真正的 CD 时间 ， 因为包括了后摇的时间 ， 这里待定
        public float GetCDMaxTime()
        {
            if (this.isNormalAttack)
            {
                if (0 == this.releser.AttackSpeed)
                {
                    return 0.01f;
                }

                //普通攻击是要用 总 cd 时间 减去 前摇和后摇 保证真实的攻击次数和面板数据对应
                var cdMax = 1.0f / this.releser.AttackSpeed;
                var resultCD = cdMax - this.GetSkillBeforeTotalTime();
                if (resultCD < 0)
                {
                    resultCD = 0;
                }
                return resultCD;
            }
            else
            {
                //技能只减去前摇
                var resultCD = this.infoConfig.CdTime / 1000.0f - this.GetSkillBeforeTotalTime();
                return resultCD;
            }
        }


        public float GetCurrCDTimer()
        {
            return this.currCDTimer;
        }

        public bool IsReadyRelease()
        {
            return this.state == RelaseSkillState.ReadyRelease;
        }
    }
}


