using System.Collections.Generic;

namespace Battle_Client
{
    public class ClientPlayer
    {
        public int playerIndex;
        public int team;
        public int uid;
        public int ctrlHeroGuid;

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
