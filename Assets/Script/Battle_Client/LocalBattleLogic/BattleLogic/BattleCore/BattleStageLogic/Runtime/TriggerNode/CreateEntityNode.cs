using System.Collections.Generic;

namespace Battle.BattleTrigger.Runtime
{
    public class CreateEntityNode : ExecuteNode
    {
        public NumberVarField count;
        public int configId;
        public Vector3VarField createPosition;

        public override void Parse(ITriggerDataNode nodeJsonData)
        {
            count = NumberVarField.ParseNumberVarField(nodeJsonData["count"]);

            configId = (int.Parse(nodeJsonData["entityType"]["configId"].ToString()));

            createPosition = Vector3VarField.ParseVector3VarField(nodeJsonData["createPosition"]);
        }

        public override void OnExecute(ActionContext context)
        {
            string str = string.Format("create {0} entity with configId {1} , at {2}",
                count, configId, createPosition);
         
            var battle = this.trigger.GetCurrActionContext().battle;

            List<EntityInit> entityInitList = new List<EntityInit>();
            for (int i = 0; i < count.Get(context); i++)
            {
                var entityConfigId = configId;
                var pos = createPosition.Get(context);

                var entityConfig = battle.ConfigManager.GetById<IEntityInfoConfig>(entityConfigId);
                EntityInit entityInit = new EntityInit()
                {
                    configId = entityConfigId,
                    playerIndex = -1,//应该是传过来的 现在全是敌人， 中立敌对 -1 ，中立无敌意 -2
                    position = pos,
                    isPlayerCtrl = false,
                    level = entityConfig.Level,
                };

                entityInit.skillInitList = new List<SkillInit>();

                foreach (var skillId in entityConfig.SkillIds)
                {
                    SkillInit skill = new SkillInit()
                    {
                        configId = skillId,
                        level = 1
                    };
                    entityInit.skillInitList.Add(skill);
                }

                entityInitList.Add(entityInit);

            }

            battle.CreateEntities(entityInitList);

            this.Finish(context);

        }
    }


}
