using System;
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

        //由于同帧请求同帧返回回有逻辑问题 所以先延迟调用
        //之后会变成统一收敛战斗队列消息进行处理
        IEnumerator DelayLoadProgress(int progress)
        {
            yield return new WaitForSeconds(0.5f);
            var myUid = GameDataManager.Instance.UserStore.Uid;
            battle.PlayerMsgReceiver.On_PlayerLoadProgress((long)myUid, progress);
        }

        public void Send_PlayerLoadProgress(int progress)
        {
            GameMain.Instance.StartCoroutine(DelayLoadProgress(progress));
            //battle.PlayerMsgReceiver.On_PlayerLoadProgress(0, progress);
        }

        IEnumerator DelayBattleReadyFinish()
        {
            yield return new WaitForSeconds(0.1f);
            var myUid = GameDataManager.Instance.UserStore.Uid;
            battle.PlayerMsgReceiver.On_BattleReadyFinish((long)myUid);
        }
        public void Send_BattleReadyFinish()
        {
            GameMain.Instance.StartCoroutine(DelayBattleReadyFinish());
            //battle.PlayerMsgReceiver.On_BattleReadyFinish(0);
        }

        public void Send_ClientPlotEnd()
        {
            var myUid = GameDataManager.Instance.UserStore.Uid;
            battle.PlayerMsgReceiver.On_ClientPlotEnd((long)myUid);
        }

        public void Send_MoveEntity(int guid, UnityEngine.Vector3 targetPos)
        {
            var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);
            battle.PlayerMsgReceiver.On_MoveEntity(guid, pos);
        }


        public void Send_UseSkill(int releaserGuid, int skillId, int targetGuid, UnityEngine.Vector3 targetPos)
        {
            var pos = new Battle.Vector3(targetPos.x, targetPos.y, targetPos.z);
            battle.PlayerMsgReceiver.On_UseSkill(releaserGuid, skillId, targetGuid, pos);
        }
    }
}