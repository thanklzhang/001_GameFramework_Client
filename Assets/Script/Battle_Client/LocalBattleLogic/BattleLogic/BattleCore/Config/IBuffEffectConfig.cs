using System.Collections.Generic;

namespace Battle
{
    public class BuffAddedValueConfig
    {
        public AddedValueType valueType;
        public int value;
        public int calculateTarget;
    }

    //叠层类型
    public enum AddLayerType
    {
        //替换
        Replace = 0,
        //叠加层数且叠加效果
        AddLayerAndEffect = 1,
        //叠加层数但是不叠加效果
        AddLayerWithoutEffect = 2,
    }

    //buff 后续效果的目标类型(通用)
    public enum BuffEffectTargetType
    {
        //技能释放着
        SkillReleaser = 0,
        //buff 目标者
        BuffTarget = 1,
    }

    public interface IBuffEffectConfig : IConfig
    {
        List<EntityAbnormalStateType> AbnormalStateTypeList { get; }
        float LastTime { get; }
        //BuffType Type { get; }
        List<EntityAttrType> AddedAttrGroup { get; }
        List<BuffAddedValueConfig> AddedValueGroup { get; }
        List<int> StartEffectList { get; }
        float IntervalTime { get; }
        List<int> IntervalEffectList { get; }
        // float ColliderRadius { get; }
        // List<int> EntityColliderEffectList { get; }
        // bool IsNoMove { get; }
        // bool IsNoReleaseSkill { get; }
        int EffectResId { get; }
        // EffectTargetType effectTargetType { get; }
        bool IsCanBeClear { get; }
        int MaxLayerCount { get; }
        AddLayerType AddLayerType { get; }
        bool IsMaxLayerRemove { get; }
        List<int> MaxLayerTriggerEffectList { get; }
        BuffEffectTargetType MaxLayerTriggerTargetType { get; }
        int IconResId { get; }

        List<int> EndRemoveEffectList { get; }


    }

}
