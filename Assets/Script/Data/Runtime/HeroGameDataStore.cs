﻿using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class HeroData
{
    public int id;
    public int level;
}

public class HeroGameDataStore : GameDataStore
{
    //public int flag;//待定
    List<HeroData> heroList = new List<HeroData>();
    Dictionary<int, HeroData> heroDic = new Dictionary<int, HeroData>();

    public List<HeroData> HeroList { get => heroList; set => heroList = value; }
    public Dictionary<int, HeroData> HeroDic { get => heroDic; }

    public void SetHeroDataList(List<HeroData> heroList)
    {
        //增删改查
        this.heroList = heroList;
        this.heroDic = this.heroList.ToDictionary(hero => hero.id);
    }

    void AddOneHero(HeroData heroData)
    {

    }

    public void UpdateOneHero(HeroData heroData)
    {
        //update
        var findData = GetDataById(heroData.id);
        if (findData != null)
        {
            findData.level = heroData.level;
            //idUnlock
        }
    }

    public HeroData GetDataById(int id)
    {
        if (HeroDic.ContainsKey(id))
        {
            return HeroDic[id];
        }
        else
        {
            return null;
        }

    }
}