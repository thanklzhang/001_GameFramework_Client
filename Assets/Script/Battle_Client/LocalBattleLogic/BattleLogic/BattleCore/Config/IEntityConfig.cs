using System.Collections.Generic;

namespace Battle
{
    public interface IEntityInfoConfig : IConfig
    {
        string Name { get; }
        List<int> SkillIds { get; }

        int BaseAttrId { get; }
        int LevelAttrId { get; }

        int Level { get; }
        int Star { get; }
    }

}
