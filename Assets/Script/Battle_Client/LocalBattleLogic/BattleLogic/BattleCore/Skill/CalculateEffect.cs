using System.Collections.Generic;

namespace Battle
{
    //附加值类型
    public enum AddedValueType
    {
        //固定伤害值
        Fixed = 0,
        //物理攻击的千分比
        PhysicAttack_Permillage = 1,
        //魔法攻击的千分比
        MagicAttack_Permillage = 2,
        //生命值的千分比
        MaxHealth_Permillage = 3,

        //造成伤害的千分比
        HurtDamage_Permillage = 20
    }

    //效果伤害类型
    public enum EffectDamageType
    {
        Null = 0,
        Physic = 1,
        Magic = 2
    }

    //施加效果的实体类型(即将废弃)
    public enum EffectEntityTargetType
    {
        //选取的单位
        Selected = 0,
        //技能释放者
        SkillReleaser = 1,
        //技能目标者
        SkillTarget = 2
    }

    //效果附加值项
    public class CalculateEffectAddedOption
    {
        public AddedValueType addedValueType;
        public int value;
        public EffectDamageType effectDamageType;
    }


    //伤害计算
    public class DamageCalculate
    {
        public int damageSrcGuid;
        public List<CalculateEffectAddedOption> calculateOptionList;
        public EffectDamageType finalEffectDamageType;
    }



    public class CalculateEffect : SkillEffect
    {
        //public BattleEntity target;
        //simulate config
        public ICalculateEffectConfig tableConfig;
        DamageCalculate damageCalculate;
        public void SetDamageCalculate(DamageCalculate damageCal)
        {
            damageCalculate = damageCal;
        }
        internal int targetGuid;

        //
        Battle battle;
        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = battle.ConfigManager.GetById<ICalculateEffectConfig>(this.configId);

        }

        public override void OnStart()
        {
            this.TriggerEffect();
            this.SetWillEndState();
        }

        public void TriggerEffect()
        {
            //_G.Log(string.Format("CalculateEffect effect of guid : {0} TriggerEffect", this.guid));
            //根据 calculateList 开始计算最终值

            //TODO:
            //注意这里根据 EffectDamageType 不同要分开计算 
            //并且不同的 EffectAddedType 计算后 总共算一次结算伤害
            //也就是说：例如：造成 150 点 物理伤害 和 100 点魔法伤害
            //经过护甲计算后 目标造成了物理伤害和魔法伤害 但该攻击只算一次结算伤害(攻击特效)

            var battle = this.context.battle;
            var releaser = this.context.fromSkill.releser;




            float finalDamage = 0;
            foreach (var item in damageCalculate.calculateOptionList)
            {
                var damageOption = item;
                var type = damageOption.addedValueType;
                var value = damageOption.value;
                //先默认一次攻击只是一种伤害
                //var dd = damageOption.effectDamageType;
                if (type == AddedValueType.Fixed)
                {
                    finalDamage += value;
                }
                else if (type == AddedValueType.PhysicAttack_Permillage)
                {
                    finalDamage += releaser.Attack * value / 1000.0f;
                    //_G.Log("releaser.Attack : " + releaser.Attack);
                    //_G.Log("releaser.value : " + value);
                }
                //else if (type == AddedValueType.MagicAttackPercent)
                //{
                //    finalDamage += releaser.FinalAttak * value / 100;
                //}
                else if (type == AddedValueType.MaxHealth_Permillage)
                {
                    finalDamage += releaser.MaxHealth * value / 1000.0f;
                }
                else if (type == AddedValueType.HurtDamage_Permillage)
                {
                    finalDamage += context.damage * value / 1000.0f;
                }
            }


            //将伤害 施加在 实体 上
            var targets = new List<BattleEntity>();

            targets = this.context.selectEntities;
                
            // var effectEntityType = this.tableConfig.EffectTargetType;
            // if (effectEntityType == EffectEntityTargetType.Selected)
            // {
            //     targets = this.context.selectEntities;
            // }
            // else if (effectEntityType == EffectEntityTargetType.SkillReleaser)
            // {
            //     targets = new List<BattleEntity>() { releaser };
            // }
            // else if (effectEntityType == EffectEntityTargetType.SkillTarget)
            // {
            //     var skillTargetEntity = battle.FindEntity(this.context.fromSkill.targetGuid);
            //     if (skillTargetEntity != null)
            //     {
            //         targets = new List<BattleEntity>() { skillTargetEntity };
            //     }
            // }

            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];

                if (target != null)
                {
                    target.BeHurt((int)finalDamage, this.context.fromSkill);
                }
            }
        }

    }
}

