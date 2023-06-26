using System.Collections.Generic;

namespace Battle
{
    public interface IAreaEffectConfig : IConfig
    {
        CenterType CenterType { get; }
        SelectEntityType SelectEntityType { get; }
        float Range { get; }
        List<int> EffectList { get; }
        int EffectResId {get;}
    }

}
