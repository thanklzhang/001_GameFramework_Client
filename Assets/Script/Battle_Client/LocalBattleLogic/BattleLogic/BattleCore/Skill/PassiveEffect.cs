using System.Collections.Generic;
using Battle.BattleTrigger.Runtime;

namespace Battle
{
    public class PassiveEffect : SkillEffect
    {
        //public Table.BuffEffect tableConfig;
        public IPassiveEffectConfig tableConfig;

        //BuffType buffType;
        Battle battle;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = battle.ConfigManager.GetById<IPassiveEffectConfig>(this.configId);
        }

        public override void OnStart()
        {
            var entities = context.selectEntities;
            foreach (var item in entities)
            {
                item.AddPassiveEffect(this);

                if (tableConfig.TriggerTimeType == EffectTriggerTimeType.OnCollisionEntity)
                {
                    //如果已经碰撞了 那么触发一次 enter 碰撞
                    var guids = battle.collisionMgr.GetCollisionEntityGuids(item.guid);
                    if (guids != null)
                    {
                        foreach (var guid in guids)
                        {
                            var entity = this.battle.FindEntity(guid);
                            if (entity != null && entity.EntityState != EntityState.Dead)
                            {
                                this.OnCollisionEntity(entity);
                            }
                        }
                    }
                }
            }
        }

        public override void OnUpdate(float timeDelta)
        {
        }

        public override void OnEnd()
        {
            var entities = context.selectEntities;
            foreach (var item in entities)
            {
                item.RemovePassiveEffect(this);
            }
        }

        //检查触发几率
        public bool CheckChance()
        {
            var chance = tableConfig.TriggerChance;
            var randInt = battle.GetRandInt(1, 1000);
            return randInt <= chance;
        }

        //当成功给别人伤害
        public void OnHurtToOtherSuccess(BattleEntity other, int resultDamage, Skill skill)
        {
        }

        public void ForceDelete()
        {
            this.SetWillEndState();
        }

        //普通攻击别人命中时
        public void OnNormalAttackToOtherSuccess(BattleEntity other, float resultDamage, Skill skill)
        {
            if (tableConfig.TriggerTimeType == EffectTriggerTimeType.OnNomalAttackToOtherSuccess)
            {
                if (!CheckChance())
                {
                    return;
                }

                // foreach (var item in tableConfig.TriggerEffectList)
                // {
                //     var effectId = item;
                //     SkillEffectContext context = new SkillEffectContext();
                //     context.battle = this.battle;
                //     context.fromSkill = this.context.fromSkill;
                //     context.selectEntities = new List<BattleEntity>();
                //     context.damage = resultDamage;
                //     if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.NomralAttackEntity)
                //     {
                //         context.selectEntities.Add(this.context.fromSkill.releser);
                //     }
                //     else if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.BeNomralAttackEntity)
                //     {
                //         context.selectEntities.Add(other);
                //     }
                //
                //     battle.AddSkillEffect(effectId, context);
                // }

                AddSkillEffect(other,resultDamage,skill);
                
                AfterTrigger();
            }
        }

        public void AddSkillEffect(BattleEntity other, float resultDamage,Skill skill)
        {
            foreach (var item in tableConfig.TriggerEffectList)
            {
                var effectId = item;
                SkillEffectContext context = new SkillEffectContext();
                context.battle = this.battle;
                context.fromSkill = this.context.fromSkill;
                context.selectEntities = new List<BattleEntity>();
                context.damage = resultDamage;
                if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.NomralAttackEntity)
                {
                    context.selectEntities.Add(this.context.fromSkill.releser);
                }
                else if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.BeNomralAttackEntity)
                {
                    context.selectEntities.Add(other);
                }
                else if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.CollisionEntity)
                {
                    context.selectEntities.Add(other);
                }

                battle.AddSkillEffect(effectId, context);
            }
        }

        //之后需要合并
        //当普通攻击释放出来的时候(前摇过了的那个时刻)
        public void OnNormalAttackStartEffect(BattleEntity other)
        {
            if (tableConfig.TriggerTimeType == EffectTriggerTimeType.OnNormalAttack)
            {
                if (!CheckChance())
                {
                    return;
                }

                // foreach (var item in tableConfig.TriggerEffectList)
                // {
                //     var effectId = item;
                //     SkillEffectContext context = new SkillEffectContext();
                //     context.battle = this.battle;
                //     context.fromSkill = this.context.fromSkill;
                //     context.selectEntities = new List<BattleEntity>();
                //     // context.damage = resultDamage;
                //     if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.NomralAttackEntity)
                //     {
                //         context.selectEntities.Add(this.context.fromSkill.releser);
                //     }
                //     else if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.BeNomralAttackEntity)
                //     {
                //         context.selectEntities.Add(other);
                //     }
                //
                //     battle.AddSkillEffect(effectId, context);
                // }
                AddSkillEffect(other,0,null);

                AfterTrigger();
            }
        }

        //被别人普通攻击命中时
        public void OnBeNormalAttackByOtherSuccess(BattleEntity other, float resultDamage, Skill skill)
        {
            
        }

        //之后需要合并
        public void OnCollisionEntity(BattleEntity other)
        {
            if (tableConfig.TriggerTimeType == EffectTriggerTimeType.OnCollisionEntity)
            {
                if (!CheckChance())
                {
                    return;
                }

                AddSkillEffect(other,0,null);
                
                // foreach (var item in tableConfig.TriggerEffectList)
                // {
                //     var effectId = item;
                //     SkillEffectContext context = new SkillEffectContext();
                //     context.battle = this.battle;
                //     context.fromSkill = this.context.fromSkill;
                //     context.selectEntities = new List<BattleEntity>();
                //
                //     if (this.tableConfig.TriggerTargetType == EffectTriggerTargetType.CollisionEntity)
                //     {
                //         context.selectEntities.Add(other);
                //     }
                //
                //     battle.AddSkillEffect(effectId, context);
                // }

                AfterTrigger();
            }
        }

        public void AfterTrigger()
        {
            foreach (var item in tableConfig.AfterTriggerRemoveEffectList)
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