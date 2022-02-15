using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameData
{
    public class GameDataStore
    {
        public void Init()
        {

        }
    }

    public class GameDataManager : Singleton<GameDataManager>
    {
        HeroGameDataStore heroStore = new HeroGameDataStore();
        public HeroGameDataStore HeroStore { get => heroStore; }

        BattleGameDataStore battleStore = new BattleGameDataStore();
        public BattleGameDataStore BattleStore { get => battleStore; }

        UserGameDataStore userStore = new UserGameDataStore();
        public UserGameDataStore UserStore { get => userStore; }

        MainTaskGameDataStore mainTaskStore = new MainTaskGameDataStore();
        public MainTaskGameDataStore MainTaskStore { get => mainTaskStore; }

        internal void Init()
        {
            HeroStore.Init();
            BattleStore.Init();
            UserStore.Init();
            MainTaskStore.Init();
        }

    }
}

