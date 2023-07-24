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
        private List<int> rangeParam;

        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.AreaEffect>(id);

            effectList = StringConvert.ToIntList(tableConfig.EffectList, ',');

            rangeParam = StringConvert.ToIntList(tableConfig.RangeParam, ',');
        }

        // public CenterType CenterType => (CenterType)tableConfig.CenterType;

        public SelectEntityType SelectEntityType => (SelectEntityType)tableConfig.SelectEntityType;

        public List<int> RangeParam => rangeParam;

        public int Id => tableConfig.Id;

        public List<int> EffectList => effectList;

        public int EffectResId => tableConfig.EffectResId;
        public AreaType AreaType => (AreaType)tableConfig.AreaType;

        public StartPosType StartPosType => (StartPosType)tableConfig.StartPosType;

        public StartPosShiftDirType StartPosShiftDirType => (StartPosShiftDirType)tableConfig.StartPosShiftDirType;

        public int StartPosShiftDistance => tableConfig.StartPosShiftDistance;
    }
}