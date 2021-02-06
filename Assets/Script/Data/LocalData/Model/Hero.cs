using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Hero
{
    public int id;
    public Config.HeroInfo config;
    public int level;

    void Init()
    {
        //config = .....
    }

    //如果需要填充模拟数据 可以打开
    //public static Hero Create(int id, int level)
    //{
    //    Hero hero = new Hero()
    //    {
    //        id = id,
    //        level = level
    //    };

    //    hero.init();
    //    return hero;

    //}

    public static Hero Create(GC2CS.HeroInfo serverHero)
    {
        Hero hero = new Hero()
        {
            id = serverHero.Id,
            level = serverHero.Level,
            config = Config.ConfigManager.Instance.GetById<Config.HeroInfo>(serverHero.ConfigId)
        };

        hero.Init();
        return hero;

    }

}




