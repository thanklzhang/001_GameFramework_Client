using System.Collections.Generic;

namespace Battle
{
    public interface IProjectileEffectConfig : IConfig
    {
        bool IsFollow { get; }
        List<int> CollisionEffectList { get; }
        float Speed { get; }
        float LastTime { get; }
        int EffectResId { get; }
        bool IsThrough { get; }
        bool IsFlyMaxRange { get; }
        List<int> EndEffectList { get; }
        int EndRedirectCount { get; }
        int EndRedirectType { get; }
        string EndRedirectParam { get; }
        int EndRedirectLastTime { get; }
        int DeflectionAngle { get; }
    }

}
