using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Table;

namespace Battle
{
    public class BuffEffectConfig_Proxy : IBuffEffectConfig
    {
        Table.BuffEffect tableConfig;
        List<int> startEffectList;

        List<int> intervalEffectList;

        // List<int> entityColliderEffectList;
        List<EntityAttrType> addedAttrGroup;
        List<BuffAddedValueConfig> addedValueGroup;
        List<int> maxLayerTriggerEffectList;
        List<EntityAbnormalStateType> abnormalStateTypeList;
        List<int> endRemoveEffectList;

        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.BuffEffect>(id);

            startEffectList = StringConvert.ToIntList(tableConfig.StartEffectList, ',');
            intervalEffectList = StringConvert.ToIntList(tableConfig.IntervalEffectList, ',');
            // entityColliderEffectList = StringConvert.ToIntList(tableConfig.EntityColliderEffectList, ',');
            addedAttrGroup = StringConvert.ToIntList(tableConfig.AddedAttrGroup, ',').Select((v) => (EntityAttrType) v)
                .ToList();
            maxLayerTriggerEffectList = StringConvert.ToIntList(tableConfig.MaxLayerTriggerEffectList, ',');
            abnormalStateTypeList = StringConvert.ToIntList(tableConfig.AbnormalStateTypeList, ',')
                .Select((v) => (EntityAbnormalStateType) v).ToList();

            addedValueGroup = new List<BuffAddedValueConfig>();
            var valueGroupStr = tableConfig.AddedValueGroup.Split('|');
            if (!(1 == valueGroupStr.Length && string.IsNullOrEmpty(valueGroupStr[0])))
            {
                foreach (var option in valueGroupStr)
                {
                    var param = option.Split(',');

                    BuffAddedValueConfig addedValue = new BuffAddedValueConfig();
                    addedValue.valueType = (AddedValueType) int.Parse(param[0]);
                    addedValue.value = int.Parse(param[1]);
                    addedValue.calculateTarget = int.Parse(param[2]);

                    addedValueGroup.Add(addedValue);
                }
            }

            endRemoveEffectList = StringConvert.ToIntList(tableConfig.EndRemoveEffectList);
        }

        public float LastTime => tableConfig.LastTime;

        //public BuffType Type => (BuffType)tableConfig.Type;

        public List<EntityAttrType> AddedAttrGroup => addedAttrGroup;

        public List<BuffAddedValueConfig> AddedValueGroup => addedValueGroup;

        public List<int> StartEffectList => startEffectList;

        public float IntervalTime => tableConfig.IntervalTime;

        public List<int> IntervalEffectList => intervalEffectList;

        // public float ColliderRadius => tableConfig.ColliderRadius;
        //
        // public List<int> EntityColliderEffectList => entityColliderEffectList;
        //
        // public bool IsNoMove => 1 == tableConfig.IsNoMove;
        //
        // public bool IsNoReleaseSkill => 1 == tableConfig.IsNoReleaseSkill;

        public int Id => tableConfig.Id;

        public int EffectResId => tableConfig.EffectResId;

        public EffectTargetType effectTargetType => (EffectTargetType) tableConfig.EffectTargetType;

        public bool IsCanBeClear => 1 == tableConfig.IsCanBeClear;

        public int MaxLayerCount => tableConfig.MaxLayerCount;

        public AddLayerType AddLayerType => (AddLayerType) tableConfig.AddLayerType;

        public bool IsMaxLayerRemove => 1 == tableConfig.IsMaxLayerRemove;

        public List<int> MaxLayerTriggerEffectList => maxLayerTriggerEffectList;

        public BuffEffectTargetType MaxLayerTriggerTargetType =>
            (BuffEffectTargetType) tableConfig.MaxLayerTriggerTargetType;

        public List<EntityAbnormalStateType> AbnormalStateTypeList => abnormalStateTypeList;

        public int IconResId => tableConfig.IconResId;

        public List<int> EndRemoveEffectList => endRemoveEffectList;
    }
}