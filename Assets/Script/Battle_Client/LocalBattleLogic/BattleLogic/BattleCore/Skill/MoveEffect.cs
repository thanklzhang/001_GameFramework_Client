using System.Collections.Generic;

namespace Battle
{
    public class MoveEffect : SkillEffect
    {

        public IMoveEffectConfig tableConfig;
        Battle battle;
        Vector3 moveTargetPos;
        public override void OnInit()
        {
            battle = this.context.battle;
            tableConfig = battle.ConfigManager.GetById<IMoveEffectConfig>(this.configId);
        }

        public override void OnStart()
        {
            base.OnStart();

            //_Battle_Log.Log("MoveEffect OnStart");

            //tableConfig = TableManager.Instance.GetById<Table.MoveEffect>(this.configId);

            var battle = this.context.battle;
            //默认为技能释放者
            var releaser = this.context.fromSkill.releser;
            SkillEffectContext context = new SkillEffectContext()
            {
                selectEntities = new List<BattleEntity>() { releaser },
                battle = battle,
                fromSkill = this.context.fromSkill
            };

            TriggerStartEffect(context);

            //默认是技能目标点
            moveTargetPos = this.context.fromSkill.targetPos;
            //默认是技能释放者(可拓展)
            var entity = this.context.fromSkill.releser;
            var speed = this.tableConfig.MoveSpeed / 1000.0f;
            var lastTime = this.tableConfig.LastTime / 1000.0f;
            var dir = (moveTargetPos - releaser.position).normalized;

            if (this.tableConfig.EndPosType == MoveEndPosType.DirectionMaxDistancePos)
            {
                moveTargetPos = entity.position + dir * (speed * lastTime);
                moveTargetPos.y = 0;
            }

            battle.OnEntityStartMoveByOnePos(entity.guid, moveTargetPos, speed);

        }

        //触发开始effect
        public void TriggerStartEffect(SkillEffectContext context)
        {
            //_G.Log("battle", string.Format("AreaEffect effect of guid : {0} TriggerEffect", this.guid));

            //var effectIdStrs = tableConfig.StartEffectList.Split(',');
            //foreach (var item in effectIdStrs)
            //{
            //    var id = int.Parse(item);
            //    var battle = this.context.battle;
            //    battle.AddSkillEffect(id, context);
            //}

            foreach (var item in tableConfig.StartEffectList)
            {
                var id = item;
                var battle = this.context.battle;
                battle.AddSkillEffect(id, context);
            }
        }


        public override void OnUpdate(float timeDelta)
        {
            var battle = this.context.battle;
            //强制移动
            //默认是技能目标点
            //var moveTargetPos = this.context.fromSkill.targetPos;
            //默认是技能释放者(可拓展)
            var entity = this.context.fromSkill.releser;

            var vector = moveTargetPos - entity.position;
            var dir = vector.normalized;
            var speed = this.tableConfig.MoveSpeed / 1000.0f;

            var currFramePos = entity.position;
            var nextFramePos = entity.position + dir * speed * timeDelta;

            //检测下一帧是否出界
            var isOut = battle.IsOutOfMap((int)nextFramePos.x, (int)nextFramePos.z);
            var isObstacle = battle.IsObstacle((int)nextFramePos.x, (int)nextFramePos.z);
            if (isObstacle || isOut)
            {
              
                battle.OnEntityStopMove(entity.guid, entity.position);
                this.SetWillEndState();
                return;
            }
            //
          
            var dotValue = Vector3.Dot(nextFramePos - moveTargetPos, moveTargetPos - currFramePos);


            if (dotValue >= 0)
            {
                //到达目的地
                entity.SetPosition(moveTargetPos);
                battle.OnEntityStopMove(entity.guid, moveTargetPos);
                this.SetWillEndState();
            }
            else
            {
                var nextPos = entity.position + dir * speed * timeDelta;
                entity.SetPosition(nextPos);
            }

        }

        public override void OnEnd()
        {
           
            foreach (var item in tableConfig.EndRemoveEffectList)
            {
                var configId = item;
                //this.context.fromSkill.releser.RemoveBuffByConfigId(id);
                //默认为技能释放者 之后拓展

                //检查 buff(这个可以直接用 entity 进行删除 buff)
                var releaser = this.context.fromSkill.releser;
                battle.DeleteBuffFromEntity(releaser.guid, configId);

                //检查 被动
                releaser.DeletePassiveSkill(configId);
            }

            //持续施法结束引起技能释放结束 转到后摇状态
            //var isForEndSkill = this.tableConfig.IsThisEndForSkillEnd == 1;
            //if (isForEndSkill)
            //{
            //    this.context.fromSkill.OnChageToSkillAfter();
            //}

            var isForEndSkill = this.tableConfig.IsThisEndForSkillEnd;
            if (isForEndSkill)
            {
                this.context.fromSkill.OnChageToSkillAfter();
            }


        }


    }
}


