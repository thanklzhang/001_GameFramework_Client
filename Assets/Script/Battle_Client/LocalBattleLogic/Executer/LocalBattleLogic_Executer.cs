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

        public Battle.Battle CreateLocalBattleLogic(int battleConfigId)
        {
            //创建战斗逻辑参数
            var logicArgs = GetBattleLogicArgs(battleConfigId);

            battle = new Battle.Battle();
            int battleGuid = 0;

            _Battle_Log.RegisterLog(new BattleLog_Impl());

            battle.PlayerMsgSender = new LocalBattleLogic_MsgSender();
            battle.PlayerMsgReceiver = new LocalBattleLogic_MsgReceiver(battle);
            battle.TriggerReader = new TriggerReader_Impl(battle);
            battle.ConfigManager = new ConfigManager_Proxy();

            battle.Init(battleGuid);
            battle.Load(logicArgs);

            currTargetLogicTime = battle.timeDelta;

            return battle;
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


                currTimer = currTimer + timeDelta;

                if (currTimer >= currTargetLogicTime)
                {
                    //Logx.Log("moveTest : BattleLogic");
                    battle.Update();
                    var fixTime = currTimer - currTargetLogicTime;
                    currTargetLogicTime = battle.timeDelta - fixTime;
                    currTimer = 0;
                }
            }
        
        }

        internal Battle.Battle GetBattle()
        {
            return this.battle;
        }

        //根据战斗 id 来开始运行战斗逻辑  在本地跑战斗逻辑
        public Battle.BattleArg GetBattleLogicArgs(int battleConfigId)
        {
            Table.Battle battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleConfigId);

            Battle.BattleArg battleArg = new BattleArg();
            battleArg.guid = 0;
            battleArg.roomId = 0;
            battleArg.configId = battleConfigId;
            battleArg.battleType = BattleType.MainTask;

            //填充玩家信息 和 填充实体信息
            battleArg.battlePlayerInitArg = new Battle.BattlePlayerInitArg();
            var playerArg = battleArg.battlePlayerInitArg;
            playerArg.battlePlayerInitList = new List<BattlePlayerInit>();
            battleArg.entityInitArg = new Battle.BattleEntityInitArg();
            var entityArg = battleArg.entityInitArg;
            entityArg.entityInitList = new List<EntityInit>();

            var battleSettingId = battleTb.BattleConfigId;
            var settingTb = Table.TableManager.Instance.GetById<Table.BattleConfig>(battleSettingId);
            var settingPath = Const.buildPath + "/BattleConfig/" + settingTb.ConfigFilePath;
            var settingJson = FileOperate.GetTextFromFile(settingPath);
            var configNode = LitJson.JsonMapper.ToObject(settingJson);
            var configPlayerInfoList = configNode["playerInitInfo"]["infoList"];

            //地图尺寸
            var mapSizeX = int.Parse(configNode["mapSizeX"].ToString());
            var mapSizeZ = int.Parse(configNode["mapSizeZ"].ToString());
            battleArg.mapInitArg = new MapInitArg();
            battleArg.mapInitArg.mapSizeX = mapSizeX;
            battleArg.mapInitArg.mapSizeZ = mapSizeZ;

            for (int i = 0; i < configPlayerInfoList.Count; i++)
            {
                var playerInfoJd = configPlayerInfoList[i];

                BattlePlayerInit playerInfo = new BattlePlayerInit();
                playerInfo.playerIndex = int.Parse(playerInfoJd["playerIndex"].ToString());
                playerInfo.team = int.Parse(playerInfoJd["team"].ToString());
                playerInfo.uid = 0;

                playerArg.battlePlayerInitList.Add(playerInfo);


                //填充控制的实体 目前一个玩家只控制一个实体
                Battle.EntityInit entityInit = new EntityInit();
                entityInit.configId = int.Parse(playerInfoJd["forceUseEntityConfigId"].ToString());

                var entityTb = Table.TableManager.Instance.GetById<Table.EntityInfo>(entityInit.configId);
              
                entityInit.isHeroCtrl = true;
                entityInit.level = entityTb.Level;
                entityInit.playerIndex = playerInfo.playerIndex;
                entityInit.position = new Battle.Vector3();
                entityInit.position.x = int.Parse(playerInfoJd["initPos"]["x"].ToString());
                entityInit.position.y = int.Parse(playerInfoJd["initPos"]["y"].ToString());
                entityInit.position.z = int.Parse(playerInfoJd["initPos"]["z"].ToString());
                entityInit.skillInitList = new List<SkillInit>();

                //技能填充
                entityInit.skillInitList = new List<SkillInit>();
             
                var skillsStr = entityTb.SkillIds.Split(',');
                foreach (var skillIdStr in skillsStr)
                {
                    var skillConfigId = int.Parse(skillIdStr);

                    SkillInit skillInit = new SkillInit();
                    skillInit.configId = skillConfigId;
                    skillInit.level = 1;

                    entityInit.skillInitList.Add(skillInit);

                }
                entityArg.entityInitList.Add(entityInit);
            }

            return battleArg;
        }


    }


}