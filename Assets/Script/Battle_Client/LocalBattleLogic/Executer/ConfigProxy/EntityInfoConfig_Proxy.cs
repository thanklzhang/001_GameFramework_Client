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
    public class EntityInfoConfig_Proxy : IEntityInfoConfig
    {
        Table.EntityInfo tableConfig;
        List<int> skillIds;
        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.EntityInfo>(id);

            skillIds = StringConvert.ToIntList(tableConfig.SkillIds, ',');
        }

        public string Name => tableConfig.Name;

        public List<int> SkillIds => skillIds;

        public int BaseAttrId => tableConfig.BaseAttrId;

        public int Id => tableConfig.Id;

        public int LevelAttrId => tableConfig.LevelAttrId;
    }

}
