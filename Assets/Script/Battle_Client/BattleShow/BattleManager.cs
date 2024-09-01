using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;

using Battle_Client;
using GameData;
using NetProto;

using UnityEditor;
using UnityEngine;

namespace Battle_Client
{
    public enum BattleState
    {
        Null = 0,
        Loading = 1,
        Running = 2,
        End = 3
    }

    public class ClientPlayer
    {
        public int playerIndex;
        public int team;
        public int uid;
        public int ctrlHeroGuid;
    }

    //战斗创建的管理
    public partial class BattleManager : Singleton<BattleManager>
    {
        //战斗信息
        public int battleGuid;
        public int battleConfigId;
        public int battleRoomId;

        //玩家信息
        public Dictionary<int, ClientPlayer> playerDic;
        public List<ClientPlayer> playerList;

        ClientPlayer localPlayer;

        //本地玩家控制的英雄
        BattleEntity_Client localCtrlEntity;

        public ClientPlayer GetLocalPlayer()
        {
            return this.localPlayer;
        }

        private BattleState battleState;

        public BattleState BattleState
        {
            get => battleState;
            set => battleState = value;
        }

        LocalBattleLogic_Executer localBattleExecuter;

        IBattleClientMsgSender msgSender;

        public IBattleClientMsgSender MsgSender
        {
            get => msgSender;
            set => msgSender = value;
        }

        private IBattleClientMsgReceiver msgReceiver;

        public IBattleClientMsgReceiver MsgReceiver
        {
            get => msgReceiver;
            set => msgReceiver = value;
        }

        public void Init()
        {
            InitBattleRecvMsg();
            this.RegisterListener();
            BattleState = BattleState.Null;
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
            EventDispatcher.AddListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
            EventDispatcher.AddListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
        }

        internal Battle.Battle GetBattle()
        {
            return this.localBattleExecuter.GetBattle();
        }

        public int GetTeamByPlayerIndex(int playerIndex)
        {
            foreach (var item in playerDic)
            {
                var player = item.Value;
                if (player.playerIndex == playerIndex)
                {
                    return player.team;
                }
            }

            return -1;
        }

        public bool IsSameTeam(int index0,int index1)
        {
            var team = GetTeamByPlayerIndex(index0);
            var team2 = GetTeamByPlayerIndex(index1);

            return team == team2;
        }

        public List<ClientPlayer> GetAllPlayerList()
        {
            return this.playerList;
        }

        /// <summary>
        /// 创建战斗数据
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

            battleState = BattleState.Loading;

            this.OnEnterBattle();

            // //进入战斗状态
            // CtrlManager.Instance.Enter<BattleCtrlPre>();
        }

        public void OnEnterBattle()
        {
            Logx.Log(LogxType.Game, "battle start");

            this.localBattleExecuter?.OnEnterBattle();
        }

        public void OnExitBattle()
        {
            Logx.Log(LogxType.Game, "battle end");

            this.localBattleExecuter?.OnExitBattle();

            this.Clear();
        }

        public bool IsLocalBattle()
        {
            return localBattleExecuter != null;
        }

        public Map GetLocalBattleMap()
        {
            return localBattleExecuter.GetMap();
        }

        public BattleType battleType;

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

        private BattleClient_CreateBattleArgs battleClientArgs;

        //设置本地战斗
        public void SetLocalBattle(NetProto.ApplyBattleArg applyArg,
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
            msgSender = new BattleClient_MsgSender_Local(battleLogic);
            // msgReceiver = new BattleClient_MsgReceiver_Impl();

            ////客户端开启战斗
            //BattleManager.Instance.CreateBattle(battleClientArgs);

            ////启动本地战斗后台逻辑
            //localBattleExecuter.StartBattleLogic();

            // GameMain.Instance.StartCoroutine(StartLocalBattle(battleClientArgs));
        }

        IEnumerator StartLocalBattle(BattleClient_CreateBattleArgs battleClientArgs)
        {
            yield return new WaitForSeconds(0.1f);

            //客户端开启战斗
            BattleManager.Instance.CreateBattleData(battleClientArgs);

            yield return null;

            //启动本地战斗后台逻辑
            localBattleExecuter.StartBattleLogic();
        }


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

        // //加载战斗触发器资源文件
        // public IEnumerator LoadTriggerResource(int battleConfigId, Action<TriggerSourceResData> finishCallback)
        // {
        //     Logx.Log(LogxType.Battle, "BattleManager : LoadTriggerResource ");
        //
        //     // TriggerSourceResData source = new TriggerSourceResData();
        //     // source.dataStrList = new List<string>();
        //     //
        //     // var battleConfigTb = Config.ConfigManager.Instance.GetById<Config.Battle>(battleConfigId);
        //     // var triggerTb = Config.ConfigManager.Instance.GetById<Config.BattleTrigger>(battleConfigTb.TriggerId);
        //     //
        //     // //需要更改方式 可以全配 或者引用关系用配置表 不要用 cs 文件
        //     // var files = BattlrTriggerPathDefine.GetTriggerPathList(triggerTb.ScriptPath);
        //     //
        //     //
        //     // if (!GlobalConfig.isUseAB)
        //     // {
        //     //     var loadPath2 = GlobalConfig.buildPath + "/" + triggerTb.ScriptPath;
        //     //     var files2 = System.IO.Directory.GetFiles(loadPath2, "*.json", System.IO.SearchOption.AllDirectories)
        //     //         .ToList();
        //     //
        //     //     files = new List<string>();
        //     //     for (int i = 0; i < files2.Count; i++)
        //     //     {
        //     //         var f = files2[i];
        //     //         var f2 = f.Replace(GlobalConfig.buildPath + "/", "");
        //     //         files.Add(f2);
        //     //     }
        //     // }
        //     // else
        //     // {
        //     //     var loadPath2 = GlobalConfig.buildPath + "/" + triggerTb.ScriptPath;
        //     //     var abPath = loadPath2 + ".ab";
        //     //     if (AssetManager.Instance.abToAssetsDic.ContainsKey(abPath))
        //     //     {
        //     //         files = new List<string>();
        //     //         var assets = AssetManager.Instance.abToAssetsDic[abPath];
        //     //         foreach (var assetPath in assets)
        //     //         {
        //     //             var resultPath = assetPath.Replace(GlobalConfig.buildPath.ToLower() + "/", "");
        //     //             files.Add(resultPath);
        //     //         }
        //     //     }
        //     //     else
        //     //     {
        //     //         Logx.LogError("the ab is not found : abPath : " + abPath);
        //     //     }
        //     // }
        //     //
        //     //
        //     // foreach (var filePath in files)
        //     // {
        //     //     bool isLoadFinish = false;
        //     //     string loadText = "";
        //     //     //Logx.Log("local execute : start load : filePath :  " + filePath);
        //     //     //var partPath = filePath.Replace(Const.AssetBundlePath + "/", "").Replace(".ab", ".json").Replace("\\", "/");
        //     //     var loadPath = Path.Combine(GlobalConfig.buildPath, filePath);
        //     //
        //     //     //Debug.Log("zxy : loadPath " + loadPath);
        //     //
        //     //     //里面已经判断 是否AB 模式了 所以这里通用
        //     //     ResourceManager.Instance.GetObject<TextAsset>(loadPath, (textAsset) =>
        //     //     {
        //     //         //Logx.Log("local execute : load text finish: " + textAsset.text);
        //     //         loadText = textAsset.text;
        //     //         isLoadFinish = true;
        //     //     });
        //     //
        //     //     while (true)
        //     //     {
        //     //         yield return null;
        //     //
        //     //         if (isLoadFinish)
        //     //         {
        //     //             source.dataStrList.Add(loadText);
        //     //             break;
        //     //         }
        //     //     }
        //     // }
        //
        //     yield return null;
        //
        //
        //     Logx.Log(LogxType.Battle, "local execute : finish all ");
        //     // finishCallback?.Invoke(source);
        //     
        //     finishCallback?.Invoke(null);
        // }


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

        public void BattleEnd(BattleResultDataArgs battleResultDataArgs)
        {
            BattleManager.Instance.BattleState = BattleState.End;
            
            EventDispatcher.Broadcast(EventIDs.OnBattleEnd, battleResultDataArgs);
            
            this.OnExitBattle();
            
            // //战斗结算界面
            // var args = new BattleResultUIArgs()
            // {
            //     isWin = battleResultArgs.isWin,
            //     //reward
            // };
            // args.uiItem = new List<CommonItemUIArgs>();
            //
            // foreach (var item in battleResultArgs.rewardDataList)
            // {
            //     var _item = new CommonItemUIArgs()
            //     {
            //         configId = item.configId,
            //         count = item.count
            //     };
            //     args.uiItem.Add(_item);
            // }
            //
            // this._resultUIPre.Refresh(args);
            // this._resultUIPre.Show();
            // //
            
            
            BattleEntityManager.Instance.OnBattleEnd();
            skillTrackModule.OnBattleEnd();
            BattleSkillEffect_Client_Manager.Instance.OnBattleEnd();
           
            
        }
        
        ////get --------------------

        public void GetSkillByIndex()
        {
        }

        //

        public void Update(float timeDelta)
        {
            if (this.battleState == BattleState.Null)
            {
                return;
            }

            UpdateRecvMsgList();
            localBattleExecuter?.Update(timeDelta);
            
            CheckInput();
            
            this.skillDirectModule?.Update(timeDelta);
            skillTrackModule?.Update(timeDelta);
            
            
        }

        public void LateUpdate(float timeDelta)
        {
            UpdateCamera();
           
        }

        public void FixedUpdate(float fixedTime)
        {
            localBattleExecuter?.FixedUpdate(fixedTime);    
        }

        public void RemoveListener()
        {
            //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
            EventDispatcher.RemoveListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
            EventDispatcher.RemoveListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
        }

        public void Clear()
        {
           
            localBattleExecuter = null;
            ClearRecvMsg();
            this.battleState = BattleState.Null;
        }

        public int GetCtrlHeroSkillIdByIndex(int index)
        {
            return this.localCtrlEntity.GetSkillIdByIndex(index);
        }

        public GameObject GetLocalCtrlHeroGameObject()
        {
            return this.localCtrlEntity?.gameObject;
        }

        public BattleEntity_Client GetLocalCtrlHero()
        {
            var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
            var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
            var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

            return localEntity;
        }

        public int GetLocalCtrlHeroGuid()
        {
            return this.localCtrlEntity.guid;
        }

        public BattleEntityAttr GetLocalCtrlHeroAttrs()
        {
            return this.localCtrlEntity.attr;
        }

        public List<BattleSkillInfo> GetLocalCtrlHeroSkills()
        {
            return this.localCtrlEntity.GetSkills();
        }
        
        public List<BattleItemInfo> GetLocalCtrlHeroItems()
        {
            return this.localCtrlEntity.GetItems();
        }
        
        public List<BattleItemInfo> GetLocalCtrlHeroSkillItems()
        {
            return this.localCtrlEntity.GetSkillItems();
        }

        public BattleSkillInfo FindLocalHeroSkill(int skillId)
        {
            var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();
            foreach (var skill in skills)
            {
                if (skill.configId == skillId)
                {
                    return skill;
                }
            }

            return null;
        }

        public void Release()
        {
            RemoveListener();
        }
    }
}