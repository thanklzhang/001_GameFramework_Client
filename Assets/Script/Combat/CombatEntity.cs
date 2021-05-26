using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixedPointy;
using NetCommon;
using UnityEngine;

public class CombatEntity
{
    public int configId;
    public int guid;

    public int attack;
    public int defence;
    public int maxHealth;
    public int actionSpeed;

    public int currHealth;

    

    public GameObject gameObject;
    List<CombatSkill> skills;
    //public Config.HeroInfo config;

    internal void PlayAnimation(string animationName)
    {
        var aniComponent = gameObject.GetComponent<Animation>();
        aniComponent.Play(animationName);
    }

    public static CombatEntity Create(CombatHeroProto serHero)
    {
        var combatHero = new CombatEntity();
        combatHero.configId = serHero.ConfigId;
        combatHero.guid = serHero.Guid;
        combatHero.attack = serHero.FinalAttack;
        combatHero.defence = serHero.FinalDefence;
        combatHero.maxHealth = serHero.FinalHealth;
        combatHero.actionSpeed = serHero.FinalActionSpeed;

        combatHero.currHealth = combatHero.maxHealth;
        //combatHero.config = Config.ConfigManager.Instance.GetById<Config.HeroInfo>(combatHero.configId);

        return combatHero;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Init(GameObject obj)
    {
        this.gameObject = obj;
       
    }

    public void SetPosition(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    public void SetRotation(Quaternion quat)
    {
        gameObject.transform.rotation = quat;
    }

    public List<CombatSkill> GetSkills()
    {
        return skills;
    }

    internal void SetSkills(List<CombatSkill> skills)
    {
        this.skills = skills;
    }
}

