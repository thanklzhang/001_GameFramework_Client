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
    public class SkillTrackConfig_Proxy : ISkillTrackConfig
    {
        Table.SkillTrack tableConfig;
        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.SkillTrack>(id);
        }

        public SkillTrackStartTimeType StartTimeType => (SkillTrackStartTimeType)tableConfig.StartTimeType;

        public SkillTrackEndTimeType EndTimeType => (SkillTrackEndTimeType)tableConfig.EndTimeType;

        public int Id => tableConfig.Id;
    }

}
