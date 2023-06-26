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

            var centerType = tableConfig.CenterType;
            if (centerType == CenterType.SkillReleaser)
            {
                this.centerPos = context.fromSkill.releser.position;
            }

            var battle = this.context.battle;
            var allEntities = battle.GetAllEntities();
            var selectType = (SelectEntityType)tableConfig.SelectEntityType;
            var range = tableConfig.Range / 1000.0f;
            List<BattleEntity> selectEntities = new List<BattleEntity>();
            foreach (var item in allEntities)
            {
                var entity = item.Value;
                if (selectType == SelectEntityType.AllEnemy)
                {
                    if (entity.Team != this.context.fromSkill.releser.Team)
                    {
                        var sqrtDis = Vector3.SqrtDistance(this.centerPos, entity.position);

                        var calDis = range * range;
                        //_G.Log("AreaEffect OnStart : " + sqrtDis + " ? " + calDis);
                        if (sqrtDis <= calDis)
                        {
                            selectEntities.Add(entity);
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


