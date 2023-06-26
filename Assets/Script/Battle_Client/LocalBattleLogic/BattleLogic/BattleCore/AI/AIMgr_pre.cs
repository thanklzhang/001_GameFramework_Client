//namespace Battle
//{
//    //public class BaseAI
//    //{
//    //    protected BattleEntity entity;
//    //    protected Battle battle;
//    //    public virtual void Update(float timeDelta) { }
//    //    public virtual void UseToEntity(BattleEntity entity) { }

//    //}

//    ////简单的 ai : 到了攻击范围遍追随攻击 如果超出攻击范围则继续寻找下一个在攻击距离的目标
//    //public class SimpleAI : BaseAI
//    //{
//    //    public BattleEntity findTarget;
//    //    public int attackSkillId;

//    //    public void Init()
//    //    {
//    //        var config = this.entity.infoConfig;
//    //        attackSkillId = int.Parse(config.SkillIds.Split(",")[1]);
//    //    }

//    //    public override void Update(float timeDelta)
//    //    {
//    //        //寻找范围内的敌人
//    //        //待优化
//    //        if (null == findTarget)
//    //        {
//    //            var entities = battle.GetAllEntities();
//    //            foreach (var item in entities)
//    //            {
//    //                var oppositeEntity = item.Value;
//    //                if (oppositeEntity.Team != this.entity.Team)
//    //                {
//    //                    var sqrtDis = Vector3.SqrtDistance(oppositeEntity.position, entity.position);
//    //                    var dis = 6.0f;
//    //                    var calDis = dis * dis;
//    //                    if (sqrtDis <= calDis)
//    //                    {
//    //                        findTarget = oppositeEntity;
//    //                        return;
//    //                    }
//    //                }

//    //            }
//    //        }
//    //        else
//    //        {
//    //            //找到目标

//    //            //判断目标是否在攻击范围内
//    //            var sqrtDis = Vector3.SqrtDistance(findTarget.position, entity.position);
//    //            var dis = 1.5f;
//    //            var calDis = dis * dis;
//    //            if (sqrtDis <= calDis)
//    //            {
//    //                if (entity.entityState == EntityState.Move)
//    //                {

//    //                }
//    //                //在攻击范围内 进行一次攻击(先写成 1 技能普通攻击)
//    //                PlayerAction aUseSkill = new UseSkillAction()
//    //                {
//    //                    releaserGuid = this.entity.guid,
//    //                    uid = 0,
//    //                    targetGuid = findTarget.guid,
//    //                    targetPos = findTarget.position,
//    //                    skillId = this.attackSkillId
//    //                };
//    //                battle.AddPlayerAction(aUseSkill);

//    //            }
//    //            else
//    //            {
//    //                //继续朝目标进行移动
//    //                PlayerAction aMove = new MoveAction()
//    //                {
//    //                    moveEntityGuid = this.entity.guid,
//    //                    uid = 0,
//    //                    targetPos = findTarget.position
//    //                };
//    //                battle.AddPlayerAction(aMove);


//    //            }
//    //        }
//    //    }
//    //}

//    public class AIMgr
//    {
//        //Battle battle;
//        //public Dictionary<int, BaseAI> aiDic;
//        //public void Init(Battle battle)
//        //{
//        //    this.battle = battle;
//        //    aiDic = new Dictionary<int, BaseAI>();
//        //}

//        //public void UseAIToEntity(BaseAI ai, BattleEntity entity)
//        //{

//        //}

//        //public void Update(float timeDelta)
//        //{
//        //    foreach (var item in aiDic)
//        //    {
//        //        var ai = item.Value;
//        //        ai.Update(timeDelta);
//        //    }
//        //}

//    }

//}


