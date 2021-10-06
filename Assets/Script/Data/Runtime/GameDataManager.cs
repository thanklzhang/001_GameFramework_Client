using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class GameDataStore
{
    public void Init()
    {

    }
}

public class GameDataManager : Singleton<GameDataManager>
{


    HeroGameDataStore heroGameDataStore = new HeroGameDataStore();
    public HeroGameDataStore HeroGameDataStore { get => heroGameDataStore; }

    BattleGameDataStore battleGameDataStore = new BattleGameDataStore();
    public BattleGameDataStore BattleGameDataStore { get => battleGameDataStore; }

    UserGameDataStore userGameDataStore = new UserGameDataStore();
    public UserGameDataStore UserGameDataStore { get => userGameDataStore; }

    internal void Init()
    {
        HeroGameDataStore.Init();
        BattleGameDataStore.Init();
        UserGameDataStore.Init();
    }

}
