using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using GameData;
using NetProto;
using UnityEngine;

namespace Battle_Client
{
    //战斗中的加载创建相关
    public partial class BattleManager
    {
        public IEnumerator StartLoad()
        {
            if (this.battleType == BattleType.Remote)
            {
                yield return StartLoad_Remote();
            }
            else if (this.battleType == BattleType.PureLocal)
            {
                yield return StartLoad_PureLocal();
            }

            OnLoadFinish();
        }

        /// <summary>
        /// 远端战斗加载
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartLoad_Remote()
        {
            //填充客户端所需组件
            MsgSender = new BattleClient_MsgSender_Remote();
            // msgReceiver = new BattleClient_MsgReceiver_Impl();

            //创建战斗数据
            CreateBattleData(battleClientArgs);

            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
        }

        /// <summary>
        /// 纯本地战斗加载
        /// </summary>
        /// <returns></returns>
        public IEnumerator StartLoad_PureLocal()
        {
            Logx.Log(LogxType.Game, "StartLoad_PureLocal : start laod");

            GameDataManager.Instance.UserData.Uid = 1;
            var uid = GameDataManager.Instance.UserData.Uid;

            //地图数据由本地加载
            EventSender.SendLoadingProgress(0.0f, "开始加载地图数据");
            MapSaveData mapSaveData = null;
            yield return LoadMapData(battleConfigId, (map) => { mapSaveData = map; });

            EventSender.SendLoadingProgress(0.2f, "开始加载本地战斗所需数据");
            
            Logx.Log(LogxType.Game, "StartLoad_PureLocal : load map config finish");

            //获得申请战斗参数
            var applyArg = ApplyBattleUtil.MakePureLocalApplyBattleArg(battleConfigId, (int)uid);

            MapInitArg mapInitData = new MapInitArg();
            mapInitData.mapList = mapSaveData.mapList;
            mapInitData.posList = VectorConvert.ToVector3(mapSaveData.posList);
            mapInitData.playerInitPosList = VectorConvert.ToVector3(mapSaveData.playerInitPosList);

            //设置本地战斗逻辑
            SetLocalBattleLogic(applyArg, mapInitData, true);

            //创建本地战斗数据
            CreateBattleData(battleClientArgs);

            //根据数据开始加载通用战斗资源
            yield return StartLoad_Common();
        }

        
        //加载地图
        public IEnumerator LoadMapData(int battleConfigId, Action<MapSaveData> finishCallback)
        {
            var battleConfigTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
            var mapConfig = Config.ConfigManager.Instance.GetById<Config.BattleMap>(battleConfigTb.MapId);

            var isFinish = false;
            // var mapList = new List<List<int>>();
            var mapSaveData = new MapSaveData();
            var path = GlobalConfig.buildPath + "/" + mapConfig.MapDataPath;
            ResourceManager.Instance.GetObject<TextAsset>(path, (textAsset) =>
            {
                //Logx.Log("local execute : load text finish: " + textAsset.text);
                var json = textAsset.text;
                // mapList = LitJson.JsonMapper.ToObject<List<List<int>>>(json);
                mapSaveData = LitJson.JsonMapper.ToObject<MapSaveData>(json);
                isFinish = true;
            });

            while (!isFinish)
            {
                yield return null;
            }

            // finishCallback?.Invoke(mapList);
            finishCallback?.Invoke(mapSaveData);
        }
        
        public IEnumerator StartLoad_Common()
        {
            Logx.Log(LogxType.Game, "StartLoad_Common : load start");


            //读取战斗相关配置数据
            var battleTableId = BattleManager.Instance.battleConfigId;
            var battleTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleTableId);
            var mapConfig = Config.ConfigManager.Instance.GetById<Config.BattleMap>(battleTb.MapId);
            var battleTriggerTb = Config.ConfigManager.Instance.GetById<Config.BattleTrigger>(battleTb.TriggerId);

            var sceneResTb = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(mapConfig.ResId);

            //加载场景
            EventSender.SendLoadingProgress(0.3f, "加载 场景 中");
            yield return SceneLoadManager.Instance.LoadRequest(sceneResTb.Name);
            SetCameraInfo();
            Logx.Log(LogxType.Game, "StartLoad_Common : scene load finish");

            //加载 UI 并打开
            EventSender.SendLoadingProgress(0.4f, "加载 战斗界面 中");
            yield return UIManager.Instance.EnterRequest<BattleUI>();
            Logx.Log(LogxType.Game, "StartLoad_Common : BattleUICtrl load finish");
            //battle ui
            // objsRequestList.Add(new LoadUIRequest<BattleUIPre>() { selfFinishCallback = OnUILoadFinish });

            //战斗实体资源
            EventSender.SendLoadingProgress(0.5f, "加载 战斗实体 中");
            yield return BattleEntityManager.Instance.LoadInitEntities();
            Logx.Log(LogxType.Game, "StartLoad_Common : entities load finish");
        }

        public void OnLoadFinish()
        {
            this.playerInput.InitPlayerInput();
        }

        #region 创建战斗

        //创建远端战斗
        public void CreateRemoteBattle(BattleClient_CreateBattleArgs battleClientArgs)
        {
            battleType = BattleType.Remote;

            this.battleClientArgs = battleClientArgs;

            Logx.Log(LogxType.Game, "start create a remote battle");

            //进入战斗场景
            SceneCtrlManager.Instance.Enter<BattleSceneCtrl>();
        }

        //创建纯本地战斗
        public void CreatePureLocalBattle(int battleConfigId)
        {
            this.battleConfigId = battleConfigId;

            battleType = BattleType.PureLocal;

            //进入战斗场景
            SceneCtrlManager.Instance.Enter<BattleSceneCtrl>();
        }

        //创建远端结算的本地战斗
        public void CreateLocalButRemoteResultBattle(ApplyBattleArg applyArg)
        {
            //TODO 进入战斗状态机 从而进行加载
            battleType = BattleType.LocalButRemoteResult;
            Logx.Log(LogxType.Game, "start create a local battle (result at remote)");

            var battleConfigId = applyArg.BattleTableId;
            // CoroutineManager.Instance.StartCoroutine(LoadTriggerResource(battleConfigId, (sourceData) =>
            // {
            //     //CreateLocalBattle(applyArg, sourceData, false);
            // }));
        }

        #endregion

        //设置本地战斗逻辑 供运行本地战斗使用
        public void SetLocalBattleLogic(NetProto.ApplyBattleArg applyArg,
            MapInitArg mapInitData, bool isPureLocal)
        {
            Logx.Log(LogxType.Game, "start create a local battle");
            //填充数据

            //初始化本地战斗后台逻辑
            localBattleExecuter = new LocalBattleLogic_Executer();
            localBattleExecuter.Init();

            //根据申请战斗参数 获得后台战斗逻辑
            var battleLogic = localBattleExecuter.CreateLocalBattleLogic(applyArg, mapInitData, isPureLocal);

            //通过后台战斗逻辑 获得客户端战斗初始化参数（供客户端初始化，如地图加载 模型加载等）
            battleClientArgs = GetBattleClientArgs(battleLogic);

            //填充客户端所需组件
            MsgSender = new BattleClient_MsgSender_Local(battleLogic);
        }

        /// <summary>
        /// 根据战斗后台逻辑 得到 创建客户端战斗的战斗初始化参数
        /// </summary>
        /// <param name="battle"></param>
        /// <returns></returns>
        public Battle_Client.BattleClient_CreateBattleArgs GetBattleClientArgs(Battle.Battle battle)
        {
            var _battle = battle;
            Battle_Client.BattleClient_CreateBattleArgs battleClientArgs = new BattleClient_CreateBattleArgs();
            battleClientArgs.guid = _battle.guid;
            battleClientArgs.configId = _battle.battleConfigId;
            battleClientArgs.roomId = _battle.roomId;

            battleClientArgs.clientPlayers = new List<BattleClient_ClientPlayer>();

            var _battlePlayerList = _battle.GetAllPlayers();
            foreach (var _player in _battlePlayerList)
            {
                BattleClient_ClientPlayer player = new BattleClient_ClientPlayer();
                player.playerIndex = _player.playerIndex;
                player.team = _player.team;
                player.uid = _player.uid;
                player.ctrlHeroGuid = _player.ctrlHeroGuid;

                battleClientArgs.clientPlayers.Add(player);
            }

            //地图尺寸
            battleClientArgs.mapSizeX = _battle.GetMapSizeX();
            battleClientArgs.mapSizeZ = _battle.GetMapSizeZ();


            battleClientArgs.entityList = new List<BattleClientMsg_Entity>();
            var entities = _battle.GetAllEntities();
            foreach (var keyV in entities)
            {
                var _entity = keyV.Value;

                var entity = new BattleClientMsg_Entity();
                entity.guid = _entity.guid;
                entity.configId = _entity.configId;
                entity.playerIndex = _entity.playerIndex;

                entity.position = new UnityEngine.Vector3(_entity.position.x, _entity.position.y, _entity.position.z);
                ;

                //netEntity.MaxHp = (int)entity.MaxHealth;
                //netEntity.CurrHp = netEntity.MaxHp;

                //技能
                var skills = _entity.GetAllSkills();
                entity.skills = new List<BattleClientMsg_Skill>();
                foreach (var skillKV in skills)
                {
                    var skill = skillKV.Value;

                    BattleClientMsg_Skill skillInfo = new BattleClientMsg_Skill();
                    skillInfo.configId = skill.configId;
                    skillInfo.level = skill.level;
                    skillInfo.maxCDTime = skill.GetCDTotalTime();
                    entity.skills.Add(skillInfo);
                }

                battleClientArgs.entityList.Add(entity);
            }

            return battleClientArgs;
        }

        /// <summary>
        /// 根据初始化数据 创建战斗所需数据
        /// </summary>
        /// <param name="battleInit"></param>
        public void CreateBattleData(BattleClient_CreateBattleArgs battleInit)
        {
            // Logx.Log("battle manager : CreateBattle");
            Logx.Log(LogxType.Game, "create battle");

            //战斗信息
            this.battleGuid = battleInit.guid;
            this.battleConfigId = battleInit.configId;
            this.battleRoomId = battleInit.roomId;

            //玩家信息
            playerDic = new Dictionary<int, ClientPlayer>();
            playerList = new List<ClientPlayer>();
            foreach (var serverPlayer in battleInit.clientPlayers)
            {
                ClientPlayer player = new ClientPlayer()
                {
                    playerIndex = serverPlayer.playerIndex,
                    team = serverPlayer.team,
                    uid = (int)serverPlayer.uid,
                    ctrlHeroGuid = serverPlayer.ctrlHeroGuid
                };

                this.playerDic.Add(player.uid, player);
                this.playerList.Add(player);
            }

            //设置本地玩家
            var userDataStore = GameDataManager.Instance.UserData;
            var uid = (int)userDataStore.Uid;
            if (playerDic.ContainsKey(uid))
            {
                this.localPlayer = playerDic[uid];
            }
            else
            {
                Logx.LogError("the uid of localPlayer is not found : " + uid);
            }

            //实体信息
            // Logx.Log(LogxType.Battle,"battle manager : CreateInitEntity");
            foreach (var serverEntity in battleInit.entityList)
            {
                BattleEntityManager.Instance.CreateViewEntityInfo(serverEntity);
            }

            //设置本地玩家控制的英雄
            this.localCtrlEntity = BattleEntityManager.Instance.FindEntity(this.localPlayer.ctrlHeroGuid);
            if (null == this.localCtrlEntity)
            {
                Logx.LogError("the localCtrlEntity is not found : ctrlHeroGuid : " + this.localPlayer.ctrlHeroGuid);
            }

            BattleState = BattleState.Loading;

            this.OnEnterBattle();

            // //进入战斗状态
            // CtrlManager.Instance.Enter<BattleCtrlPre>();
        }
    }
}