using System.Collections.Generic;

namespace Battle
{
    public enum MoveEndPosType
    {
        TargetEntityPos = 0,
        SkillTargetPos = 1,
        DirectionMaxDistancePos = 2

    }
    public interface IMoveEffectConfig : IConfig
    {
        float MoveSpeed { get; }
        List<int> StartEffectList { get; }
        List<int> EndRemoveEffectList { get; }
        bool IsThisEndForSkillEnd { get; }
        MoveEndPosType EndPosType {get;}
        float LastTime { get; }

    }

}
