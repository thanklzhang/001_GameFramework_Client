//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Assets.Script.Combat;
//using FixedPointy;

////这个是代表一个飞行效果类型 而不是单一飞行物的类型
//public enum ProjectileSendType
//{
//    Follow,
//    Direct,
//    NoDirNoTarget//这里其实和 direct 一样 

//}

//public enum SkillEffectTargetType
//{
//    Position,
//    Unit,
//    NoTarget
//}

//public enum SkillUnitTargetType
//{
//    Friend,
//    Enemy
//}

//public class ProjectileEffectLogic : SkillEffectLogic
//{
//    public Config.SkillProjectileConfig config;
//    List<SkillProjectile> projectiles = new List<SkillProjectile>();
//    public Fix currWaveTimer;
//    public Fix currProjectileWave;
//    FixVec3 sendInitPos;
//    public override void Init(int skillEffectSN, CombatLogicEntity releaser, CombatLogicEntity target, FixVec3 targetPos)
//    {
//        config = Config.ConfigManager.Instance.GetBySN<Config.SkillProjectileConfig>(skillEffectSN);
//        base.Init(skillEffectSN, releaser, target, targetPos);
//        sendInitPos = releaser.position;
//        SendProjectiles();
//    }

//    /// <summary>
//    /// 发射一波投掷物
//    /// </summary>
//    void SendProjectiles()
//    {
//        var targetType = (SkillEffectTargetType)config.targetType;
//        var projectileSendType = (ProjectileSendType)config.projectileSendType;

//        projectiles = new List<SkillProjectile>();
//        for (int i = 0; i < config.projectileNum; ++i)
//        {
//            SkillProjectile pro = new SkillProjectile()
//            {
//                projectileEffect = this,
//                posistion = this.posistion,
//            };

//            //发射方式
//            if (projectileSendType == ProjectileSendType.Follow)
//            {
//                pro.target = this.target;
//            }


//            if (projectileSendType == ProjectileSendType.Direct)
//            {
//                var baseDir = (targetPos - sendInitPos).Normalize();//这里先按照释放技能的时候按照自身作为释放点开始释放 也就是是人物移动不影响释放 之后可能增加类型的不同而不同

//                Fix yAngle = (i - config.projectileCount / 2) * (Fix)(config.intervalAngle);
//                FixVec3 matrix = new FixVec3(0, yAngle, 0);
//                var rotation = FixTrans3.MakeRotation(matrix);
//                var currDir = rotation * baseDir;

//                pro.dir = currDir;
//            }


//            //目标类型
//            if (targetType == SkillEffectTargetType.Position)
//            {

//            }

//            projectiles.Add(pro);
//        }

//        currProjectileWave += 1;
//    }

//    public override void Start()
//    {
//        base.Start();
//    }

//    public override void Update()
//    {
//        base.Update();

//        if (currProjectileWave < config.projectileCount)
//        {
//            currWaveTimer += Const.timeDelta;
//            if (currWaveTimer >= (Fix)config.projectileIntervalTime)
//            {
//                currWaveTimer = 0;
//                SendProjectiles();
//                if (currProjectileWave >= config.projectileCount)
//                {
//                    DestroySelf();
//                }
//            }
//        }


//    }

//}

