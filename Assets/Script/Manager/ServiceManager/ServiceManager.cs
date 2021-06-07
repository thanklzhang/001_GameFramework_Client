using System;
using System.Collections;
using System.Collections.Generic;


public class ServiceManager : Singleton<ServiceManager>
{
    public HeroService heroService = new HeroService();

    public void Init()
    {
        heroService.Init();
    }
}
