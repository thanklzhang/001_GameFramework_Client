using System.Collections.Generic;

namespace Battle
{
    //public enum BuffType
    //{
    //    Null = 0,
    //    //击晕
    //    Stun = 1,
    //    //碰撞触发
    //    ColliderTrigger = 2,
    //    //改变属性
    //    ChangeAttr = 3,
    //    //间隔触发
    //    Interval = 4,
    //}

    public class BuffAttr
    {
        public int id;
        public EntityAttrType entityAttrType;
        public AddedValueType addedValueType;
        public int value;
        public int calculateTarget;
    }

    public enum EffectTargetType
    {
        //选取的实体
        SelectEntity = 0,

        //释放技能的实体
        SkillReleaser = 1,
    }

    public class BuffEffect : SkillEffect
    {
        //public Table.BuffEffect tableConfig;
        public IBuffEffectConfig tableConfig;

        float currLastTime;
        float intervaleTime;

        BattleEntity target;

        //BuffType buffType;
        Battle battle;

        int currStackCount = 1;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = battle.ConfigManager.GetById<IBuffEffectConfig>(this.configId);
        }

        public override void OnStart()
        {
            base.OnStart();

            ResetLastTimer();

            if (this.context.selectEntities.Count > 0)
            {
                target = this.context.selectEntities[0];
            }
            else
            {
                _Battle_Log.LogError(
                    "BuffEffect : the this.context.selectEntities of count is 0 : in buff , config : " +
                    tableConfig.Id);
            }

            if (null == target)
            {
                _Battle_Log.LogError("BuffEffect : the target is null , config : " + tableConfig.Id);
                return;
            }

            currStackCount = 1;

            target.AddBuff(this);

            //异常状态
            foreach (var stateType in this.tableConfig.AbnormalStateTypeList)
            {
                target.AddAbnormalState(stateType);
            }

            SkillEffectContext context = new SkillEffectContext()
            {
                selectEntities = new List<BattleEntity>() {target},
                battle = this.context.battle,
                fromSkill = this.context.fromSkill
            };
            //触发 start effect
            TriggerStartEffect(context);

            //ResetIntervaleTime();

            //间隔的第一下直接释放
            intervaleTime = 0;

            //属性改变
            AddAttrValue();
        }

        List<BuffAttr> buffAttrs = new List<BuffAttr>();

        public void ResetLastTimer()
        {
            currLastTime = tableConfig.LastTime / 1000.0f;
            if (currLastTime <= 0)
            {
                currLastTime = 999999999;
            }
        }

        public float GetMaxLastTime()
        {
            if (0 == this.tableConfig.LastTime)
            {
                return -1;
            }

            return tableConfig.LastTime / 1000.0f;
            ;
        }

        public float GetCurrLastTime()
        {
            return currLastTime;
        }

        public int GetCurrStackCount()
        {
            return this.currStackCount;
        }

        //相同 configId 叠加
        public void AddStack(int stack)
        {
            //叠层的时候刷新持续时间 也可以改为配置
            ResetLastTimer();

            if (this.currStackCount >= this.tableConfig.MaxLayerCount)
            {
                //触发满层效果
                foreach (var item in tableConfig.MaxLayerTriggerEffectList)
                {
                    var id = item;
                    var battle = this.context.battle;
                    this.context.selectEntities = new List<BattleEntity>();
                    if (this.tableConfig.MaxLayerTriggerTargetType == BuffEffectTargetType.BuffTarget)
                    {
                        this.context.selectEntities.Add(this.target);
                    }

                    battle.AddSkillEffect(id, context);
                }

                if (this.tableConfig.IsMaxLayerRemove)
                {
                    this.SetWillEndState();
                }
            }
            else
            {
                this.currStackCount = this.currStackCount + stack;

                AddAttrValue();
            }
        }

        public void AddAttrValue()
        {
            //这里先默认 n 层 就是 n 倍的属性效果
            if (buffAttrs != null && buffAttrs.Count > 0)
            {
                this.target.RemoveBuffAttrs(this.guid, buffAttrs);
            }

            buffAttrs?.Clear();

            for (int i = 0; i < tableConfig.AddedValueGroup.Count; i++)
            {
                BuffAttr option = new BuffAttr();
                EntityAttrType attrType = tableConfig.AddedAttrGroup[i];
                var addedValueConfig = tableConfig.AddedValueGroup[i];
                option.addedValueType = addedValueConfig.valueType;
                option.value = addedValueConfig.value * this.currStackCount;
                option.calculateTarget = addedValueConfig.calculateTarget;
                option.entityAttrType = attrType;

                buffAttrs.Add(option);
            }

            if (buffAttrs.Count > 0)
            {
                this.target.AddBuffAttrs(this.guid, buffAttrs);
            }
        }

        //触发 start effect
        public void TriggerStartEffect(SkillEffectContext context)
        {
            foreach (var item in tableConfig.StartEffectList)
            {
                var id = item;
                var battle = this.context.battle;
                battle.AddSkillEffect(id, context);
            }
        }


        public override void OnUpdate(float timeDelta)
        {
            //CheckCollider();

            CheckInterval(timeDelta);

            currLastTime -= timeDelta;
            if (currLastTime <= 0)
            {
                this.SetWillEndState();
            }
        }

        public void ResetIntervaleTime()
        {
            intervaleTime = tableConfig.IntervalTime / 1000.0f;
        }

        public void CheckInterval(float timeDelta)
        {
            if (tableConfig.LastTime <= 0)
            {
                return;
            }
            //if (null == tableConfig.IntervalEffectList || tableConfig.IntervalEffectList == "")
            //{
            //    return;
            //}

            if (0 == tableConfig.IntervalEffectList.Count)
            {
                return;
            }

            intervaleTime -= timeDelta;
            if (intervaleTime <= 0)
            {
                TriggerIntervalEffect();
                ResetIntervaleTime();
            }
        }

        public void TriggerIntervalEffect()
        {
            foreach (var item in tableConfig.IntervalEffectList)
            {
                var id = item;
                var battle = this.context.battle;
                battle.AddSkillEffect(id, context);
            }
        }

        public void ForceDelete()
        {
            this.SetWillEndState();
        }

        public override void OnEnd()
        {
            target.RemoveBuff(this);

            if (buffAttrs != null && buffAttrs.Count > 0)
            {
                this.target.RemoveBuffAttrs(this.guid, buffAttrs);
            }

            buffAttrs?.Clear();

            foreach (var stateType in this.tableConfig.AbnormalStateTypeList)
            {
                target.RemoveAbnormalState(stateType);
            }

            foreach (var item in tableConfig.EndRemoveEffectList)
            {
                var configId = item;
                //默认为技能释放者 之后拓展

                //检查 buff(这个可以直接用 entity 进行删除 buff)
                var releaser = this.context.fromSkill.releser;
                battle.DeleteBuffFromEntity(releaser.guid, configId);

                //检查 被动
                releaser.DeletePassiveSkill(configId);
            }
        }
    }
}