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
            InitBoxShop();
        }

        public Dictionary<int, Currency> currencyDic =
            new Dictionary<int, Currency>();

        public int GetCurrencyCount(int itemId)
        {
            if (currencyDic.ContainsKey(itemId))
            {
                return currencyDic[itemId].count;
            }

            return 0;
        }

        public int GetCoinCount()
        {
            return GetCurrencyCount(Currency.CoinId);
        }
        
     
    }
}
