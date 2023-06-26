using System.Collections.Generic;

namespace Battle
{
    public interface ICollisionGroupEffectConfig : IConfig
    {
        List<int> SkillEffectIds { get; }
        CollisionGroupAffectType AffectType { get; }
        string AffectParam { get; }

    }

}
