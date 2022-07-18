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
    public class CollisionGroupEffectConfig_Proxy : ICollisionGroupEffectConfig
    {
        Table.CollisionGroupEffect tableConfig;
        List<int> skillEffectIds;

        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.CollisionGroupEffect>(id);
            skillEffectIds = StringConvert.ToIntList(tableConfig.SkillEffectIds, ',');
        }

        public int Id => tableConfig.Id;

        public List<int> SkillEffectIds => skillEffectIds;

        public CollisionGroupAffectType AffectType => (CollisionGroupAffectType)tableConfig.AffectType;

        public string AffectParam => tableConfig.AffectParam;

    }

}
