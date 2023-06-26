using System.Collections.Generic;

namespace Battle
{
    public interface ISkillTrackConfig : IConfig
    {
        SkillTrackStartTimeType StartTimeType { get; }
        SkillTrackEndTimeType EndTimeType { get; }
    }

}
