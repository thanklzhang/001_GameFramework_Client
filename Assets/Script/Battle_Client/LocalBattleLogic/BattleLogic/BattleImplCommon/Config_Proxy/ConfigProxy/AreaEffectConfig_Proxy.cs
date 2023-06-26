using System;
using System.Collections.Generic;
using System.Linq;
using Table;

namespace Battle
{
    public class AreaEffectConfig_Proxy : IAreaEffectConfig
    {
        Table.AreaEffect tableConfig;
        List<int> effectList;
        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.AreaEffect>(id);

            effectList = StringConvert.ToIntList(tableConfig.EffectList, ',');
        }

        public CenterType CenterType => (CenterType)tableConfig.CenterType;

        public SelectEntityType SelectEntityType => (SelectEntityType)tableConfig.SelectEntityType;

        public float Range => tableConfig.Range;

        public int Id => tableConfig.Id;

        public List<int> EffectList => effectList;

        public int EffectResId => tableConfig.EffectResId;
    }

}
