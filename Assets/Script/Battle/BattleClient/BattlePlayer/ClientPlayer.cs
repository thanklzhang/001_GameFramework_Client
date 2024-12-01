using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public partial class ClientPlayer
    {
        public int playerIndex;
        public int team;
        public int uid;
        public int ctrlHeroGuid;

        public void Init()
        {
            InitCurrency();
            InitBoxShop();
            InitMyBox();
            InitBattleReward();
        }

      
    }
}
