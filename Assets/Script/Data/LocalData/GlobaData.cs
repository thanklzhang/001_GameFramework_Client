using Assets.Script.Combat;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModelData
{


    public class GlobaData : Singleton<GlobaData>
    {
        public int localUdpPort;

        public CombatInitInfo combatInitInfo;


    }

    public class CombatInitInfo
    {
        public int currCombatId;
        public int localSeat;
        public List<CombatHeroInitInfo> combatHeroInfos;
    }

    public class CombatHeroInitInfo
    {
        public int seat;
        public int team;
        public int SN;

    }
}
