﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    //战斗客户端消息发送器
    public class BattleClient_MsgSender_Local : IBattleClientMsgSender
    {
        Battle.Battle battle;

        public BattleClient_MsgSender_Local(Battle.Battle battle)
        {
            this.battle = battle;
        }

        public void Send_PlayerLoadProgress(int progress)
        {
            GameMain.Instance.StartCoroutine(DelayLoadProgress(progress));
            //battle.PlayerMsgReceiver.On_PlayerLoadProgress(0, progress);
        }

        //由于同帧请求同帧返回回有逻辑问题 所以先延迟调用
        //之后会变成统一收敛战斗队列消息进行处理
        IEnumerator DelayLoadProgress(int progress)
        {
            var hero = BattleManager.Instance.GetLocalCtrlHero();

            yield return new WaitForSeconds(0.5f);
            // var myUid = GameDataManager.Instance.UserData.Uid;
            //  
            // // PlayerLoadProgress_BattleMsgArg msgArg = new PlayerLoadProgress_BattleMsgArg();
            // // msg.progress = progress;
            // //
            var arg = new PlayerLoadProgress_MsgArg()
            {
                progress = progress
            };

            battle.OnRecvBattleMsg<PlayerLoadProgress_BattleMsg>(
                hero.playerIndex, arg);
            //battle.PlayerMsgReceiver.On_PlayerLoadProgress((long)myUid, progress);
        }

        public void Send_BattleReadyFinish()
        {
            GameMain.Instance.StartCoroutine(DelayBattleReadyFinish());
            //battle.PlayerMsgReceiver.On_BattleReadyFinish(0);
        }

        IEnumerator DelayBattleReadyFinish()
        {
            yield return new WaitForSeconds(0.1f);
            var myUid = GameDataManager.Instance.UserData.Uid;

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            var arg = new BattleReadyFinish_MsgArg()
            {
            };
            battle.OnRecvBattleMsg<ReadyFinish_BattleMsg>(
                hero.playerIndex, arg);


            //battle.PlayerMsgReceiver.On_BattleReadyFinish((long)myUid);
        }


        public void Send_ClientPlotEnd()
        {
            var hero = BattleManager.Instance.GetLocalCtrlHero();

            // var msg = new ClientPlotEnd_BattleMsg()
            // {
            // };
            // var myUid = GameDataManager.Instance.UserData.Uid;
            // battle.PlayerMsgReceiver.On_ClientPlotEnd((long)myUid);
            
            battle.OnRecvBattleMsg<ClientPlotEnd_BattleMsg>(
                hero.playerIndex, null);
        }

        public void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos)
        {
            var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            var arg = new MoveEntity_MsgArg()
            {
                moveEntityGuid = guid,
                targetPos = pos,
            };
            battle.OnRecvBattleMsg<MoveEntity_BattleMsg>(
                hero.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_MoveEntity(guid, pos);
        }


        public void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos)
        {
            var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);
            //battle.PlayerMsgReceiver.On_UseSkill(releaserGuid, skillId, targetGuid, pos);

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            var msg = new UseSkill_BattleMsgArg()
            {
                releaserGuid = releaserGuid,
                skillId = skillId,
                targetGuid = targetGuid,
                targetPos = pos,
            };
            battle.OnRecvBattleMsg<UseSkill_BattleMsg>(
                hero.playerIndex, msg);
        }

        public void Send_UseItem(ItemUseArg_Client itemUseArg)
        {
            var pos = new Battle.Vector3(itemUseArg.targetPos.x, itemUseArg.targetPos.y, itemUseArg.targetPos.z);
            var hero = BattleManager.Instance.GetLocalCtrlHero();

            var arg = new Battle_ItemUseArg()
            {
                itemIndex = itemUseArg.itemIndex,
                releaserGuid = itemUseArg.releaserGuid,
                targetGuid = itemUseArg.targetGuid,
                targetPos = pos,
            };

            // var useItemMsg = new Battle_ItemUseArg()
            // {
            //     itemUseArg = arg
            // };
            battle.OnRecvBattleMsg<UseItem_BattleMsg>(
                hero.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_UseItem(arg);
        }

        public void Send_UseSkillItem(ItemUseArg_Client itemUseArg)
        {
            var pos = new Battle.Vector3(itemUseArg.targetPos.x, itemUseArg.targetPos.y, itemUseArg.targetPos.z);
            var arg = new Battle_ItemUseArg()
            {
                itemIndex = itemUseArg.itemIndex,
                releaserGuid = itemUseArg.releaserGuid,
                targetGuid = itemUseArg.targetGuid,
                targetPos = pos,
            };
            // battle.PlayerMsgReceiver.On_UseSkillItem(arg);

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            // var useSkillItemMsg = new UseSkillItem_BattleMsg()
            // {
            //     itemUseArg = arg
            // };
            battle.OnRecvBattleMsg<UseSkillItem_BattleMsg>(
                hero.playerIndex, arg);
        }

        public void Send_OpenBox()
        {
            Battle_OpenBoxArg arg = new Battle_OpenBoxArg();
            arg.releaserGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();

            var hero = BattleManager.Instance.GetLocalCtrlHero();

            // var arg = new Battle_OpenBoxArg()
            // {
            //     msgArg = arg
            // };

            battle.OnRecvBattleMsg<OpenBox_BattleMsg>(
                hero.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_OpenBox(arg);
        }

        public void Send_SelectBoxReward(int index)
        {
            var hero = BattleManager.Instance.GetLocalCtrlHero();


            Battle_SelectBoxRewardArg arg = new Battle_SelectBoxRewardArg();
            arg.releaserGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();
            arg.index = index;

            // SelectBoxReward_BattleMsg msg = new SelectBoxReward_BattleMsg()
            // {
            //     arg = arg
            // };

            battle.OnRecvBattleMsg<SelectBoxReward_BattleMsg>(hero.playerIndex, arg);

            // battle.PlayerMsgReceiver.On_SelectBoxReward(arg);
        }
    }
}