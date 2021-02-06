//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FixedPointy;
//using NetCommon;
//using UnityEngine;

//public class CombatHero : CombatEntity
//{
  
//    public Config.HeroInfo config;

//    public static CombatHero Create(CombatHeroProto serHero)
//    {
//        var combatHero = new CombatHero();
//        combatHero.configId = serHero.ConfigId;
//        combatHero.guid = serHero.Guid;
//        combatHero.attack = serHero.FinalAttack;
//        combatHero.defence = serHero.FinalDefence;
//        combatHero.maxHealth = serHero.FinalHealth;
//        combatHero.actionSpeed = serHero.FinalActionSpeed;

//        combatHero.Init();

//        return combatHero;
//    }

//    void Init()
//    {
//        this.currHealth = this.maxHealth;
//        this.config = Config.ConfigManager.Instance.GetById<Config.HeroInfo>(this.configId);
//    }

//}





