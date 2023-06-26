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
    public class MoveEffectConfig_Proxy : IMoveEffectConfig
    {
        Table.MoveEffect tableConfig;

        List<int> startEffectList;
        List<int> endRemoveEffectList;
        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.MoveEffect>(id);

            startEffectList = StringConvert.ToIntList(tableConfig.StartEffectList, ',');
            endRemoveEffectList = StringConvert.ToIntList(tableConfig.EndRemoveEffectList, ',');
        }
        public float MoveSpeed => tableConfig.MoveSpeed;

        public List<int> StartEffectList => startEffectList;

        public List<int> EndRemoveEffectList => endRemoveEffectList;

        public bool IsThisEndForSkillEnd => 1 == tableConfig.IsThisEndForSkillEnd;

        public int Id => tableConfig.Id;

        public MoveEndPosType EndPosType => (MoveEndPosType)tableConfig.EndPosType;

        public float LastTime => tableConfig.LastTime;
    }

}
