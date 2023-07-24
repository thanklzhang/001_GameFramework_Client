using System.Collections.Generic;

namespace Battle
{
    public interface IAreaEffectConfig : IConfig
    {
        //CenterType CenterType { get; }
        SelectEntityType SelectEntityType { get; }

        //float Range { get; }
        List<int> RangeParam { get; }
        List<int> EffectList { get; }
        int EffectResId { get; }


        AreaType AreaType { get; }

        StartPosType StartPosType { get; }

        StartPosShiftDirType StartPosShiftDirType { get; }

        int StartPosShiftDistance { get; }
    }
}