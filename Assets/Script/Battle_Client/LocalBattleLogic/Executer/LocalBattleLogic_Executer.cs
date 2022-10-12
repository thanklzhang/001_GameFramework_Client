using Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{


    public class LocalBattleLogic_Executer
    {
        Battle.Battle battle;
        public void Init()
        {

        }

        //创建本地战斗
        public Battle.Battle CreateLocalBattleLogic(NetProto.ApplyBattleArg applyArg)
        {
            //创建战斗逻辑参数
            //TODO:要根据 center server 传来的玩家参数进行创建
            var logicArgs = GetBattleLogicArgs(applyArg);

            battle = new Battle.Battle();
            int battleGuid = 0;
            battle.TimeDelta = Time.fixedDeltaTime;

            _Battle_Log.RegisterLog(new BattleLog_Impl());

            battle.PlayerMsgSender = new LocalBattleLogic_MsgSender();
            battle.PlayerMsgReceiver = new LocalBattleLogic_MsgReceiver(battle);
            battle.TriggerReader = new TriggerReader_Impl(battle);
            battle.ConfigManager = new ConfigManager_Proxy();

            battle.Init(battleGuid);
            battle.Load(logicArgs);

            currTargetLogicTime = battle.TimeDelta;

            return battle;
        }


        public void OnEnterBattle()
        {
            battle.OnBattleEnd += OnBattleLogicEnd;
        }

        public void OnBattleLogicEnd(Battle.Battle battle, int winTeam)
        {
            //本地战斗结算是在 center server
            var arg = BattleEndUtil.MakeApplyBattleArgProto(battle, winTeam);
            var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            battleNet.SendApplyBattleEnd(arg);
        }

        public void OnExitBattle()
        {
            battle.OnBattleEnd -= OnBattleLogicEnd;
        }

        public Map GetMap()
        {
            return this.battle.GetMap();
        }

        public void StartBattleLogic()
        {
            //battle.Start();
        }

        float currTimer;
        int currBattleClientFrame;


        float currLogicTimer;
        float currTargetLogicTime;

        public void Update(float timeDelta)
        {
            //Logx.Log("battleClientTimeDelta : " + timeDelta);
            //Logx.Log("currTimer : " + currTimer);

            if (battle.BattleState == Battle.BattleState.Battling)
            {
                //var currBattleFrameNum = GameMain.Instance.currBattleFrameNum;
                //Logx.Log("moveTest : currFrame : " + currBattleFrameNum);
                //Logx.Log("moveTest : BattleClient");


                //currTimer = currTimer + timeDelta;

                //if (currTimer >= currTargetLogicTime)
                //{
                //    //Logx.Log("moveTest : BattleLogic");
                //    battle.Update();
                //    var fixTime = currTimer - currTargetLogicTime;
                //    currTargetLogicTime = battle.timeDelta - fixTime;
                //    currTimer = 0;
                //}
            }

        }

        public void FixedUpdate(float fixedTime)
        {
            battle.Update();
        }

        internal Battle.Battle GetBattle()
        {
            return this.battle;
        }

        //根据战斗 id 来开始运行战斗逻辑  在本地跑战斗逻辑
        //TODO:center server 中的 player info 转换为战斗初始参数
        public Battle.BattleArg GetBattleLogicArgs(NetProto.ApplyBattleArg applyArg)
        {
            var battleArg = ApplyBattleUtil.ToBattleArg(applyArg);
            return battleArg;

        }


    }


}