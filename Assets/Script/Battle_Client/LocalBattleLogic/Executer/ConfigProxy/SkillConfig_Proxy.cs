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
    public class SkillConfig_Proxy : ISkillConfig
    {
        Table.Skill tableConfig;
        List<int> effectList;
        public void Init(int id)
        {
            tableConfig = Table.TableManager.Instance.GetById<Table.Skill>(id);

            effectList = StringConvert.ToIntList(tableConfig.EffectList, ',');
        }

        public string Name => tableConfig.Name;

        public float BeforeTime => tableConfig.BeforeTime;

        public float AfterTime => tableConfig.AfterTime;

        public SkillReleaseType SkillReleaseType => (SkillReleaseType)tableConfig.SkillReleaseType;

        public List<int> EffectList => effectList;

        public float ReleaseRange => tableConfig.ReleaseRange;

        public float CdTime => tableConfig.CdTime;

        public SkillReleaseTargeType SkillReleaseTargeType => (SkillReleaseTargeType)tableConfig.SkillReleaseTargeType;

        public int Id => tableConfig.Id;
    }

}
