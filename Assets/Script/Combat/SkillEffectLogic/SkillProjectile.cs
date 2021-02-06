//using Assets.Script.Combat;
//using FixedPointy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public class SkillProjectile
//{
//    public bool isWillDestroy;
//    public FixVec3 posistion;
//    public FixVec3 dir;
//    public ProjectileEffectLogic projectileEffect;
//    public CombatLogicEntity target;

//    public void Init(ProjectileEffectLogic projectileEffect)
//    {
//        this.projectileEffect = projectileEffect;
//    }
//    Fix limit = Fix.Ratio(1, 2);
//    Fix lifeTimer;

//    List<int> colliderGuids = new List<int>();

//    public void Update()
//    {
//        var projectileSendType = (ProjectileSendType)projectileEffect.config.projectileSendType;
//        if (projectileSendType == ProjectileSendType.Follow)
//        {
//            dir = (this.target.position - this.posistion).Normalize();
//            this.posistion += Const.timeDelta * dir * (Fix)projectileEffect.config.flySpeed;
//            if ((this.target.position - this.posistion).GetMagnitude() < limit)
//            {
//                //碰撞上了 开始计算伤害(之后可能会加碰撞半径)
//                CollideCalculate(this.target);
//                this.isWillDestroy = true;
//            }
//        }
//        if (projectileSendType == ProjectileSendType.Direct)
//        {
//            this.posistion += Const.timeDelta * dir * projectileEffect.config.flySpeed;
//            Fix lifeTime = projectileEffect.config.lifeTime;
//            //是否到达投掷物存活时间
//            if (lifeTimer >= lifeTime)
//            {
//                this.isWillDestroy = true;
//            }

//            var entities = CombatLogicEntityManager.Instance.GetEntities();
//            var targetUnitType = (SkillUnitTargetType)projectileEffect.config.targetType;
//            for (int i = entities.Count - 1; i >= 0; --i)
//            {
//                var currEntity = entities[i];
//                var dis = currEntity.position - this.posistion;
//                if (targetUnitType == SkillUnitTargetType.Enemy)
//                {
//                    if (currEntity.team != projectileEffect.releaser.team)
//                    {
//                        if (dis.GetMagnitude() <= limit)
//                        {
//                            if (!colliderGuids.Contains(currEntity.guid))
//                            {
//                                colliderGuids.Add(currEntity.guid);

//                                //碰撞上了 开始计算伤害(之后可能会加碰撞半径)
//                                CollideCalculate(currEntity);

//                                //是否穿透
//                                if (projectileEffect.config.isThrough)
//                                {

//                                }
//                                else
//                                {
//                                    this.isWillDestroy = true;
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//        }

//        lifeTimer += Const.timeDelta;
//    }

//    public void CollideCalculate(CombatLogicEntity entity)
//    {
//        var nextSkills = projectileEffect.config.nextSkillEffectSNs.Split(',').Select(s =>
//        {
//            int sn = int.Parse(s);
//            return sn;
//        }).ToList();

//        SkillEffectLogicManager.Instance.Create(nextSkills, this.projectileEffect.releaser, entity, entity.position);
//    }


//}

