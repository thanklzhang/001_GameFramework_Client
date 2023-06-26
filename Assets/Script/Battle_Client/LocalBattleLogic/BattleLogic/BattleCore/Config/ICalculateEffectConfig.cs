using System.Collections.Generic;

namespace Battle
{

    public class CalculateAddedValueConfig
    {
        public AddedValueType valueType;
        public int value;
        public EffectDamageType effectDamageType;
    }

    public interface ICalculateEffectConfig : IConfig
    {
        string Name { get; }
        EffectDamageType FinalEffectType { get; }
        int EffectResId { get; }
        List<CalculateAddedValueConfig> AddedValueGroup { get; }
        EffectEntityTargetType EffectTargetType { get; }
        bool isEffectFollowTarget { get; }
    }

}
