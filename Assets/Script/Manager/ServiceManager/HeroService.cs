using System;
using System.Collections;
using System.Collections.Generic;
public class HeroService : BaseService
{
    /// <summary>
    /// service handle
    /// </summary>
    HeroServiceHandler handler = new HeroNetHandler();

    public override void Init()
    {
        handler.Init();
    }

    /// <summary>
    /// upgrade hero level
    /// </summary>
    /// <param name="heroId"></param>
    public void UpgradeHeroLevel(int heroId)
    {
        handler.SendUpgradeHeroLevel(heroId);
        
    }
}