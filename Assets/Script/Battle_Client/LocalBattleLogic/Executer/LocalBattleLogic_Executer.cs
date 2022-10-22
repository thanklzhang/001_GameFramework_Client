using Battle;
using Battle.BattleTrigger.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Battle_Client
{


    public class LocalBattleLogic_Executer
    {
        Battle.Battle battle;
        public void Init()
        {

        }

        bool isPureLocal;
        //创建本地战斗
        public Battle.Battle CreateLocalBattleLogic(NetProto.ApplyBattleArg applyArg, TriggerSourceResData sourceData, bool isPureLocal)
        {
            this.isPureLocal = isPureLocal;

            Logx.Log("local execute : CreateLocalBattleLogic");

            //创建战斗逻辑参数
            var logicArgs = GetBattleLogicArgs(applyArg, sourceData);

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

        //public IEnumerator LoadTriggerResource(int battleConfigId, Action<TriggerSourceData> finishCallback)
        //{
        //    Logx.Log("local execute : LoadTriggerResource ");

        //    TriggerSourceData source = new TriggerSourceData();
        //    source.dataStrList = new List<string>();

        //    var battleConfigTb = Table.TableManager.Instance.GetById<Table.Battle>(battleConfigId);
        //    var triggerTb = Table.TableManager.Instance.GetById<Table.BattleTrigger>(battleConfigTb.TriggerId);

        //    var loadPath = Const.AssetBundlePath + "/" + Const.buildPath + "/" + triggerTb.ScriptPath;
        //    var files = System.IO.Directory.GetFiles(loadPath, "*.ab", System.IO.SearchOption.AllDirectories);

        //    foreach (var filePath in files)
        //    {
        //        bool isLoadFinish = false;
        //        string loadText = "";
        //        //Logx.Log("local execute : start load : filePath :  " + filePath);
        //        var partPath = filePath.Replace(Const.AssetBundlePath + "/","").Replace(".ab",".json").Replace("\\","/");
        //        ResourceManager.Instance.GetObject<TextAsset>(partPath, (textAsset) =>
        //        {
        //            Logx.Log("local execute : load text finish: " + textAsset.text);
        //            loadText = textAsset.text;
        //            isLoadFinish = true;
        //        });

        //        while (true)
        //        {
        //            yield return null;

        //            if (isLoadFinish)
        //            {
        //                source.dataStrList.Add(loadText);
        //                break;
        //            }
        //        }
        //    }

        //    Logx.Log("local execute : finish all ");
        //    finishCallback?.Invoke(source);
        //}

        public void OnEnterBattle()
        {
            battle.OnBattleEnd += OnBattleLogicEnd;
        }

        public void OnBattleLogicEnd(Battle.Battle battle, int winTeam)
        {
            //本地战斗结算是在 center server
            var arg = BattleEndUtil.MakeApplyBattleArgProto(battle, winTeam);


            //判断是否是服务端结算
            if (!isPureLocal)
            {
                //本地战斗 在服务端结算
                var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
                battleNet.SendApplyBattleEnd(arg);

            }
            else
            {
                //纯本地战斗
                Logx.Log("pure battle : battle result");
            }


        }

        public void OnExitBattle()
        {
            battle.OnBattleEnd -= OnBattleLogicEnd;
            isPureLocal = false;
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
            ////Logx.Log("battleClientTimeDelta : " + timeDelta);
            ////Logx.Log("currTimer : " + currTimer);

            //if (battle.BattleState == Battle.BattleState.Battling)
            //{
            //    //var currBattleFrameNum = GameMain.Instance.currBattleFrameNum;
            //    //Logx.Log("moveTest : currFrame : " + currBattleFrameNum);
            //    //Logx.Log("moveTest : BattleClient");


            //    //currTimer = currTimer + timeDelta;

            //    //if (currTimer >= currTargetLogicTime)
            //    //{
            //    //    //Logx.Log("moveTest : BattleLogic");
            //    //    battle.Update();
            //    //    var fixTime = currTimer - currTargetLogicTime;
            //    //    currTargetLogicTime = battle.timeDelta - fixTime;
            //    //    currTimer = 0;
            //    //}
            //}

        }

        public void FixedUpdate(float fixedTime)
        {
            battle?.Update();
        }

        internal Battle.Battle GetBattle()
        {
            return this.battle;
        }

        //根据战斗服务端发来的战斗参数来开始运行战斗逻辑  在本地跑战斗逻辑 服务端结算
        public Battle.BattleCreateArg GetBattleLogicArgs(NetProto.ApplyBattleArg applyArg, TriggerSourceResData sourceData)
        {
            var battleArg = ApplyBattleUtil.ToBattleArg(applyArg, sourceData);
            return battleArg;

        }

    }


}