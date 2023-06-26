using System.Collections.Generic;

namespace Battle
{
    //触发的时机类型
    public enum EffectTriggerTimeType
    {
        OnNormalAttack = 1,
        OnSkillRelease = 2,
        OnNomalAttackToOtherSuccess = 3,
        OnBeNomalAttackByOtherSuccess = 4,
        OnHurtToOther = 5,
        OnBeHurt = 6,
        OnCollisionEntity = 7,
    }

    //触发目标类型
    public enum EffectTriggerTargetType
    {
        NomralAttackEntity = 1,
        SkillReleaseEntity = 2,
        BeNomralAttackEntity = 3,
        BeSkillEffectEntity = 4,
        CollisionEntity = 5,

    }

    public interface IPassiveEffectConfig : IConfig
    {
        EffectTriggerTimeType TriggerTimeType { get; }
        EffectTriggerTargetType TriggerTargetType { get; }
        float TriggerChance { get; }
        List<int> TriggerEffectList { get; }
        float TriggerCD { get; }
        int TriggerEffectResId { get; }
        List<int> AfterTriggerRemoveEffectList { get; }


    }

}
