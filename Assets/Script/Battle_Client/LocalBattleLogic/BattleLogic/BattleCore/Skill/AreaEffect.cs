using System.Collections.Generic;

namespace Battle
{
    //0 自己和所有友方单位
    //1 所有友方单位(不包括自己)
    //2 所有敌人
    //3 所有友方和敌人(不包括自己)
    //4 所有实体

    public enum SelectEntityType
    {
        MeAndFriend = 0,
        Friend = 1,
        AllEnemy = 2,
        FriendAndEnemy = 3,
        All = 4
    }

    public enum CenterType
    {
        SkillReleaser = 0,
        SkillTarget = 1
    }

    public enum StartPosType
    {
        Context = 0,
    }

    public enum AreaType
    {
        Circle = 0,
        Rectangle = 1
    }

    public enum StartPosShiftDirType
    {
        Null = 0,

        //释放技能者 到 技能目标(或技能目标点) 的方向
        FromReleaserToTarget = 1,
    }

    public class AreaEffect : SkillEffect
    {
        //public Table.AreaEffect tableConfig;
        public IAreaEffectConfig tableConfig;
        Vector3 centerPos;
        Battle battle;

        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = battle.ConfigManager.GetById<IAreaEffectConfig>(this.configId);
        }

        public override void OnStart()
        {
            base.OnStart();

            //_G.Log("AreaEffect OnStart");

            //tableConfig = TableManager.Instance.GetById<Table.AreaEffect>(this.configId);

            this.centerPos = context.fromSkill.releser.position;
            if (context.selectEntities.Count > 0)
            {
                centerPos = context.selectEntities[0].position;
            }

            // var centerType = tableConfig.CenterType;
            // if (centerType == CenterType.SkillReleaser)
            // {
            //     this.centerPos = context.fromSkill.releser.position;
            // }
            // else if (centerType == CenterType.SkillTarget)
            // {
            //     var centerTarget = this.battle.FindEntity(context.fromSkill.targetGuid);
            //     if (centerTarget != null)
            //     {
            //         this.centerPos = centerTarget.position;
            //     }
            // }

            var battle = this.context.battle;
            var allEntities = battle.GetAllEntities();
            var selectType = (SelectEntityType)tableConfig.SelectEntityType;
            var areaType = tableConfig.AreaType;
            var startPosType = (StartPosType)tableConfig.StartPosType;
            var startPosShiftType = (StartPosShiftDirType)tableConfig.StartPosShiftDirType;

            List<BattleEntity> selectEntities = new List<BattleEntity>();
            foreach (var item in allEntities)
            {
                var entity = item.Value;
                if (selectType == SelectEntityType.AllEnemy)
                {
                    if (entity.Team != this.context.fromSkill.releser.Team)
                    {
                        if (areaType == AreaType.Circle)
                        {
                            var sqrtDis = Vector3.SqrtDistance(this.centerPos, entity.position);

                            var range = tableConfig.RangeParam[0] / 1000.0f;
                            var calDis = range * range;
                            //_G.Log("AreaEffect OnStart : " + sqrtDis + " ? " + calDis);
                            if (sqrtDis <= calDis)
                            {
                                selectEntities.Add(entity);
                            }
                        }
                        else if (areaType == AreaType.Rectangle)
                        {
                            Rect rect = new Rect();
                            Vector3 dir = Vector3.right;
                            if (startPosShiftType == StartPosShiftDirType.FromReleaserToTarget)
                            {
                                var targetPos = context.fromSkill.targetPos;
                                if (context.fromSkill.targetGuid > 0)
                                {
                                    var targetEntity = battle.FindEntity(context.fromSkill.targetGuid);
                                    if (targetEntity != null)
                                    {
                                        targetPos = targetEntity.position;
                                    }
                                }

                                dir = (targetPos - context.fromSkill.releser.position);
                                dir = new Vector3(dir.x, 0, dir.z).normalized;
                            }

                            var width = tableConfig.RangeParam[0] / 1000.0f;
                            var height = tableConfig.RangeParam[1] / 1000.0f;

                            rect.center = centerPos.ToVector2ByXZ() +
                                          dir.ToVector2ByXZ() * (width / 2.0f + tableConfig.StartPosShiftDistance);
                            rect.width = width;
                            rect.height = height;

                            rect.widthDir = dir.ToVector2ByXZ().normalized;
                            rect.heightDir = (Vector3.Cross(rect.widthDir.ToVector3ByXZ(), Vector3.up).normalized)
                                .ToVector2ByXZ();

                            Circle circle = new Circle();
                            circle.radius = entity.collisionCircle / 3.0f;
                            circle.center = new Vector2(entity.position.x, entity.position.z);

                            var isCollision = CollisionTool.CheckBoxAndCircle(rect, circle);
                            if (isCollision)
                            {
                                selectEntities.Add(entity);
                            }
                        }
                    }
                }
            }

            //selectEntities
            //_G.Log("AreaEffect selectEntities count : " + selectEntities.Count);
            foreach (var entity in selectEntities)
            {
                SkillEffectContext context = new SkillEffectContext()
                {
                    selectEntities = new List<BattleEntity>() { entity },
                    battle = this.context.battle,
                    fromSkill = this.context.fromSkill
                };
                //触发下一步 effect
                TriggerEffect(context);
            }
        }

        //触发 effect
        public void TriggerEffect(SkillEffectContext context)
        {
            //_G.Log("battle", string.Format("AreaEffect effect of guid : {0} TriggerEffect", this.guid));

            //var effectIdStrs = tableConfig.EffectList.Split(',');
            //foreach (var item in effectIdStrs)
            //{
            //    var id = int.Parse(item);
            //    var battle = this.context.battle;
            //    battle.AddSkillEffect(id, context);
            //}


            foreach (var item in tableConfig.EffectList)
            {
                var id = item;
                var battle = this.context.battle;
                battle.AddSkillEffect(id, context);
            }
        }


        public override void OnUpdate(float timeDelta)
        {
        }
    }
}