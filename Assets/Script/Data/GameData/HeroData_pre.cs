//using DataModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class HeroData : Singleton<HeroData>
//{
//    private List<Hero> heroList = new List<Hero>();

//    public void UpdateHeroList(List<GC2CS.HeroInfo> serverHeroList)
//    {
//        //heroList -- serverHeroList
//        foreach (var serverHero in serverHeroList)
//        {
//            var hero = GetHeroById(serverHero.Id);
//            if (hero != null)
//            {
//                UpdateHero(hero, serverHero);
//            }
//            else
//            {
//                AddHero(serverHero);
//            }
//        }

//    }

//    public List<Hero> GetHeroList()
//    {
//        return heroList;
//    }

//    public Hero GetHeroById(int id)
//    {
//        return heroList.Find(h => h.id == id);
//    }

//    public void AddHero(GC2CS.HeroInfo serverHero)
//    {
//        heroList.Add(Hero.Create(serverHero));
//    }
    
//    public void UpdateHero(Hero hero, GC2CS.HeroInfo serverHero)
//    {
//        hero.level = serverHero.Level;
//        //other info
//    }

//    public void DeleteHeroById(int id)
//    {
//        var hero = GetHeroById(id);
//        if (hero != null)
//        {
//            this.heroList.Remove(hero);
//        }


//    }


//}



