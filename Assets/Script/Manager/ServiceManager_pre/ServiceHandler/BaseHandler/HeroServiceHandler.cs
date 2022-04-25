using GameData;
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
        //GameDataManager.Instance.HeroStore.UpdateOneHero(heroData);
        //EventDispatcher.Broadcast(EventIDs.OnUpgradeHeroLevel, heroData);
    }
}
