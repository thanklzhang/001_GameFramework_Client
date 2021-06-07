using System;
using System.Collections;
using System.Collections.Generic;

public abstract class HeroServiceHandler : BaseHandler
{
    //send
    public abstract void SendUpgradeHeroLevel(int heroId); 

    //receive
    protected void OnUpgradeHeroLevel(HeroData heroData)
    {
        GameDataManager.Instance.HeroGameDataStore.UpdateOneHero(heroData);
        EventDispatcher.Broadcast(EventIDs.OnUpgradeHeroLevel, heroData);
    }
}
