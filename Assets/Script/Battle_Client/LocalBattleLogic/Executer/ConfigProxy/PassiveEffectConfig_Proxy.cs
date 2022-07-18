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
    public class PassiveEffectConfig_Proxy : IPassiveEffectConfig
    {
        Table.PassiveEffect tableConfig;
        List<int> triggerEffectList;

        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.PassiveEffect>(id);

            triggerEffectList = StringConvert.ToIntList(tableConfig.TriggerEffectList, ',');
        }

        public EffectTriggerTimeType TriggerTimeType => (EffectTriggerTimeType)tableConfig.TriggerTimeType;

        public EffectTriggerTargetType TriggerTargetType => (EffectTriggerTargetType)tableConfig.TriggerTargetType;

        public float TriggerChance => tableConfig.TriggerChance;

        public List<int> TriggerEffectList => triggerEffectList;

        public float TriggerCD => tableConfig.TriggerCD;

        public int TriggerEffectResId => tableConfig.TriggerEffectResId;

        public int Id => tableConfig.Id;



    }

}
