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
    public class EntityAttrBaseConfig_Proxy : IEntityAttrBaseConfig
    {
        Table.EntityAttrBase tableConfig;

        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.EntityAttrBase>(id);
        }
        public int Attack => tableConfig.Attack;

        public int Defence => tableConfig.Defence;

        public int Health => tableConfig.Health;

        public int AttackSpeed => tableConfig.AttackSpeed;

        public int MoveSpeed => tableConfig.MoveSpeed;

        public int AttackRange => tableConfig.AttackRange;

        public int InputDamageRate => tableConfig.InputDamageRate;
        
        public int Id => tableConfig.Id;
        
       


    }

}
