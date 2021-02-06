//using Assets.Script.Combat;
//using FixedPointy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public enum SkillEffectType
//{
//    Calulate,
//    Projectile
//}

//public class SkillEffectLogicManager : Singleton<SkillEffectLogicManager>
//{
//    List<SkillEffectLogic> skillEffects = new List<SkillEffectLogic>();
//    List<SkillEffectLogic> willDestroyEffect = new List<SkillEffectLogic>();

//    List<SkillProjectile> projectiles = new List<SkillProjectile>();
//    List<SkillProjectile> willDestroyProjectiles = new List<SkillProjectile>();

//    public void Update()
//    {
//        //所有技能效果
//        willDestroyEffect.Clear();
//        for (int i = 0; i < skillEffects.Count; ++i)
//        {
//            var skill = skillEffects[i];
//            skill.Update();

//            if (skill.isWillDestroy)
//            {
//                willDestroyEffect.Add(skill);
//            }
//        }

//        DestroyEffects();

//        //投掷物
//        willDestroyProjectiles.Clear();
//        for (int i = 0; i < projectiles.Count; ++i)
//        {
//            var skill = projectiles[i];
//            skill.Update();

//            if (skill.isWillDestroy)
//            {
//                willDestroyProjectiles.Add(skill);
//            }
//        }

//        DestroyProjectiles();


//    }

//    public List<SkillEffectLogic> Create(List<int> skillEffectSNs, CombatLogicEntity releaser, CombatLogicEntity target, FixVec3 targetPos)
//    {
//        List<SkillEffectLogic> effects = new List<SkillEffectLogic>();
//        for (int i = 0; i < skillEffectSNs.Count; ++i)
//        {
//            var currSkillSN = skillEffectSNs[i];

//            var type = Config.ConfigManager.Instance.GetSkillEffectType(currSkillSN);//effect

//            SkillEffectLogic skill = null;
//            switch (type)
//            {
//                case SkillEffectType.Calulate:
//                    skill = new CalculateEffectLogic();
//                    skill.Init(currSkillSN, releaser, target, targetPos);
//                    break;
//                case SkillEffectType.Projectile:
//                    skill = new ProjectileEffectLogic();
//                    skill.Init(currSkillSN, releaser, target, targetPos);
//                    break;
//            }
//            effects.Add(skill);

//        }



//        //skill.Start();

//        return effects;

//    }

//    void DestroyEffects()
//    {
//        for (int i = willDestroyEffect.Count - 1; i >= 0; --i)
//        {
//            var skill = willDestroyEffect[i];
//            skillEffects.Remove(skill);
//        }
//    }

//    void DestroyProjectiles()
//    {
//        for (int i = willDestroyProjectiles.Count - 1; i >= 0; --i)
//        {
//            var skill = willDestroyProjectiles[i];
//            projectiles.Remove(skill);
//        }
//    }
//}

