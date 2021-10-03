using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    HeroGameDataStore heroGameDataStore = new HeroGameDataStore();
    public HeroGameDataStore HeroGameDataStore { get => heroGameDataStore; }

    BattleGameDataStore battleGameDataStore = new BattleGameDataStore();
    public BattleGameDataStore BattleGameDataStore { get => battleGameDataStore; }

    internal void Init()
    {
        HeroGameDataStore.Init();
        BattleGameDataStore.Init();
    }

}
