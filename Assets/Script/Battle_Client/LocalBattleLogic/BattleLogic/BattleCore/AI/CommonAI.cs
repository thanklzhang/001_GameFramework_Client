namespace Battle
{
    //通用 AI
    //1 ：在攻击范围内自动普通攻击(期间自身不会移动)
    public class CommonAI : BaseAI
    {
        public override void OnUpdate(float timeDelta)
        {
            //判断是否周围有在攻击范围内的的敌方单位
            BattleEntity nearestEntity = battle.FindNearestEnemyEntity(this.entity);

            if (nearestEntity != null)
            {
                var normalAttackSkill = this.entity.GetNormalAttackSkill();
                var sqrtDis = Vector3.SqrtDistance(nearestEntity.position, entity.position);
                var attackRange = normalAttackSkill.GetReleaseRange();
                if (sqrtDis <= attackRange * attackRange)
                {
                    var targetEntity = nearestEntity;
                    var normalSkillConfigId = this.entity.GetNormalAttackSkill().configId;
                    this.entity.CheckAndReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
                    //AskReleaseSkill(normalSkillConfigId, targetEntity.guid, Vector3.one);
                }
            }
        }

    }
}


