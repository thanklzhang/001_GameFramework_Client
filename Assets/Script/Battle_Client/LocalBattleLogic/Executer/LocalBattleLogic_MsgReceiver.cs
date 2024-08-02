using Battle;
using Battle_Client;
using System.Collections.Generic;
namespace Battle_Client
{
    //战斗逻辑中接受玩家消息的接受器
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

        public void On_UseItem(Battle_ItemUseArg itemUseArg)
        {
            Logx.Log(LogxType.BattleItem,"local battle logic : On_UseItem : itemIndex : " + itemUseArg.itemIndex);
            UseItemAction useItemAction = new UseItemAction()
            {
                arg = itemUseArg
            };
            battle.AddPlayerAction(useItemAction);
        }

        public void On_UseSkillItem(Battle_ItemUseArg itemUseArg)
        {
            Logx.Log(LogxType.BattleItem,"local battle logic : On_UseSkillItem : itemIndex : " + itemUseArg.itemIndex);
            UseSkillItemAction useItemAction = new UseSkillItemAction()
            {
                arg = itemUseArg
            };
            battle.AddPlayerAction(useItemAction);
        }

        public void On_OpenBox(Battle_OpenBoxArg arg)
        {
            Logx.Log(LogxType.BattleItem,"local battle logic : On_OpenBox ");
            var action = new OpenBoxAction()
            {
                arg = arg
            };
            battle.AddPlayerAction(action);
        }

        public void On_SelectBoxReward(Battle_SelectBoxRewardArg arg)
        {
            Logx.Log(LogxType.BattleItem,"local battle logic : On_SelectBoxReward ");
            var action = new SelectBoxRewardAction()
            {
                arg = arg
            };
            battle.AddPlayerAction(action);
        }
    }
}