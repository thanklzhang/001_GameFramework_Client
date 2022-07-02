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
    public class EntityAttrLevelConfig_Proxy : IEntityAttrLevelConfig
    {
        Table.EntityAttrLevel tableConfig;

        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.EntityAttrLevel>(id);
        }

        public int TemplateId => tableConfig.TemplateId;

        public int Level => tableConfig.Level;

        public float Attack => tableConfig.Attack;

        public float Defence => tableConfig.Defence;

        public float Health => tableConfig.Health;

        public float AttackSpeed => tableConfig.AttackSpeed;

        public float MoveSpeed => tableConfig.MoveSpeed;

        public float AttackRange => tableConfig.AttackRange;

        public int Id => tableConfig.Id;


    }

}
