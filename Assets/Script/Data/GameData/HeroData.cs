using DataModel;
using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroData : BaseData
{
    List<Hero> heroList;
    public override void Init()
    {
        base.Init();
        heroList = new List<Hero>();
    }

    //---------------------------------取数据------------------------

    public List<Hero> GetHeroList()
    {
        //test
        heroList = new List<Hero>();
        heroList.Add(new Hero()
        {
            id = 1,
            level = 12
        });
        heroList.Add(new Hero()
        {
            id = 2,
            level = 13
        });
        heroList.Add(new Hero()
        {
            id = 3,
            level = 13
        });
        heroList.Add(new Hero()
        {
            id = 4,
            level = 13
        });
        heroList.Add(new Hero()
        {
            id = 5,
            level = 23

        });
        heroList.Add(new Hero()
        {
            id = 6,
            level = 23

        });
        heroList.Add(new Hero()
        {
            id = 57,
            level = 23

        });
        return heroList;
    }

    public Hero GetHeroById(int id)
    {
        return null;
    }

    //---------------------------------刷新数据------------------------

    //从服务器刷新 多个英雄
    public void RefreshHeroList(List<GC2CS.HeroInfo> heroList)
    {

    }

    //从服务器刷新 单个英雄
    public void RefreshHero(Hero hero)
    {

    }

    void AddHero()
    {

    }

    void RemoveHero()
    {

    }

}



