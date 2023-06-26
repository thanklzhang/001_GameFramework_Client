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
    public class CalculateEffectConfig_Proxy : ICalculateEffectConfig
    {
        Table.CalculateEffect tableConfig;

        public List<CalculateAddedValueConfig> addedValueGroup;

        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.CalculateEffect>(id);

            addedValueGroup = new List<CalculateAddedValueConfig>();
            var valueGroupStr = tableConfig.AddedValueGroup.Split('|');
            if (!(1 == valueGroupStr.Length && string.IsNullOrEmpty(valueGroupStr[0])))
            {
                foreach (var option in valueGroupStr)
                {
                    var param = option.Split(',');

                    CalculateAddedValueConfig addedValue = new CalculateAddedValueConfig();
                    addedValue.valueType = (AddedValueType)int.Parse(param[0]);
                    addedValue.value = int.Parse(param[1]);
                    addedValue.effectDamageType = (EffectDamageType)int.Parse(param[2]);

                    addedValueGroup.Add(addedValue);
                }
            }
        }

        public string Name => tableConfig.Name;

        public EffectDamageType FinalEffectType => (EffectDamageType)tableConfig.FinalEffectType;

        public int EffectResId => tableConfig.EffectResId;

        public List<CalculateAddedValueConfig> AddedValueGroup => addedValueGroup;

        public int Id => tableConfig.Id;

        public EffectEntityTargetType EffectTargetType => (EffectEntityTargetType)tableConfig.EffectTargetType;

        public bool isEffectFollowTarget => 1 == tableConfig.IsEffectFollowTarget;
    }

}
