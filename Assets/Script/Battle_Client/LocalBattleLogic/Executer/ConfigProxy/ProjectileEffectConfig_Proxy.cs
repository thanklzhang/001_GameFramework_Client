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
    public class ProjectileEffectConfig_Proxy : IProjectileEffectConfig
    {
        Table.ProjectileEffect tableConfig;
        List<int> collisionEffectList;
        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.ProjectileEffect>(id);

            collisionEffectList = StringConvert.ToIntList(tableConfig.CollisionEffectList, ',');
        }
        public bool IsFollow => 1 == tableConfig.IsFollow;

        public List<int> CollisionEffectList => collisionEffectList;

        public int Id => tableConfig.Id;

        public float Speed => tableConfig.Speed;

        public float LastTime => tableConfig.LastTime;

        public int EffectResId => tableConfig.EffectResId;
    }

}
