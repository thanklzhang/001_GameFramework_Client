using System.Collections.Generic;

namespace Battle
{
    public interface ISkillConfig : IConfig
    {
        string Name { get; }
        float BeforeTime { get; }
        float AfterTime { get; }
        SkillReleaseType SkillReleaseType { get; }
        List<int> EffectList { get; }
        float ReleaseRange { get; }
        float CdTime { get; }
        SkillReleaseTargeType SkillReleaseTargeType { get; }
        int ReleaserEffectResId { get; }
        SkillTargeType SkillTargetType { get; }
        bool IsPassive { get; }
        List<int> SkillTrackList { get; }

    }

}
