using Battle;
using Battle_Client;
using System.Collections.Generic;
namespace Battle_Client
{
    //战斗逻辑中发送给玩家的发送器
    public class LocalBattleLogic_MsgReceiver : IPlayerMsgReceiver
    {
        Battle.Battle battle;

        public LocalBattleLogic_MsgReceiver(Battle.Battle battle)
        {
            this.battle = battle;
        }

        public void On_PlayerLoadProgress(long uid, int progress)
        {
            battle.SetPlayerProgress(uid, progress);
        }

        public void On_BattleReadyFinish(long uid)
        {
            battle.PlayerReadyFinish(uid);
        }

        public void On_ClientPlotEnd(long uid)
        {
            battle.PlayerPlotEnd(uid);
        }

        public void On_MoveEntity(int entityGuid, Vector3 targetPos)
        {
            MoveAction action = new MoveAction()
            {
                moveEntityGuid = entityGuid,
                targetPos = targetPos
            };
            battle.AddPlayerAction(action);
        }

        public void On_UseSkill(int releaserGuid, int skillId, int targetGuid, Vector3 targetPos)
        {
            UseSkillAction action = new UseSkillAction()
            {
                releaserGuid = releaserGuid,
                skillId = skillId,
                targetGuid = targetGuid,
                targetPos = new Vector3(targetPos.x, targetPos.y, targetPos.z)
            };
            battle.AddPlayerAction(action);
        }
    }
}