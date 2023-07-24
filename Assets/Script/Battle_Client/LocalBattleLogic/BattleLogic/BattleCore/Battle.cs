using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Battle.BattleTrigger;
using System.IO;
using Battle.BattleTrigger.Runtime;

namespace Battle
{
    public enum BattleType
    {
        HeroExperiment = 1,
        MainTask = 10,
    }
    public enum BattleState
    {
        Null = 0,
        CanUse = 1,
        Loading = 2,
        LoadingFinish = 3,
        Battling = 4,
        End = 5,
    }

    public class BattleCreateArg
    {
        //guid
        //public int guid;
        //对应 center server 的房间 id 
        public int roomId;
        //配置 id , 表格中的 id
        public int configId;
        //public BattleType battleType;
        public BattlePlayerInitArg battlePlayerInitArg;
        public MapInitArg mapInitArg;
        public BattleEntityInitArg entityInitArg;
        public int funcId;
        public TriggerSourceResData triggerSrcData;
        //public int stageId;//业务层的关卡 id
        ////地图尺寸
        //public int mapSizeX;
        //public int mapSizeY;

    }

    public class EffectMoveArg
    {
        public int effectGuid;
        public Vector3 startPos;
        public Vector3 targetPos;
        public int targetGuid;
        public bool isFollow;
        public float moveSpeed;
    }

    public class Battle
    {
        private int guid;
        public int Guid { get => guid; }

        int roomId;
        public int RoomId { get => roomId; }

        public int tableId;

        BattlePlayerMgr battlePlayerMgr = new BattlePlayerMgr();

        BattleMapMgr battleMapMgr = new BattleMapMgr();

        BattleEntityMgr battleEntityMgr = new BattleEntityMgr();

        PlayerActionMgr playerActionMgr = new PlayerActionMgr();

        SkillEffectMgr skillEffectMgr = new SkillEffectMgr();

        AIMgr aiMgr = new AIMgr();

        public CollisionMgr collisionMgr = new CollisionMgr();

        public TriggerMgr triggerMgr = new TriggerMgr();

        private BattleState battleState = BattleState.Null;

        private float timeDelta = 0.033f;

        int randSeek;
        Random rand;

        public int currFrame = 1;

        ////center server 中所属 room 的 id
        //internal int battleRoomId;

        public int stageId;

        public int funcId;

        public BattleState BattleState { get => battleState; set => battleState = value; }

        public Action<TriggerArg> OnTimePassAction;
        public Action<TriggerArg> OnEntityEventAction;
        public Action<TriggerArg> OnBattleStartAction;
        public Action<TriggerArg> OnPlotStartAction;
        public Action<TriggerArg> OnPlotEndAction;


        //private FileReader fileReader;
        private ITriggerReader triggerReader;

        private IPlayerMsgSender playerMsgSender;
        private IPlayerMsgReceiver playerMsgReceiver;
        //public FileReader FileReader { get => fileReader; set => fileReader = value; }
        public ITriggerReader TriggerReader { get => triggerReader; set => triggerReader = value; }
        public IPlayerMsgSender PlayerMsgSender { get => playerMsgSender; set => playerMsgSender = value; }


        private IConfigManager configManager;
        public IConfigManager ConfigManager { get => configManager; set => configManager = value; }
        public IPlayerMsgReceiver PlayerMsgReceiver { get => playerMsgReceiver; set => playerMsgReceiver = value; }
        public float TimeDelta { get => timeDelta; set => timeDelta = value; }

        public Action<Battle, int> OnBattleEnd;

        internal void Init(int id)
        {
            this.guid = id;

            randSeek = unchecked((int)DateTime.Now.Ticks);
            rand = new Random(randSeek);

            //_G.Log("the battle init : guid : " + this.guid);
            BattleState = BattleState.CanUse;

            this.configManager.Init();
        }

        public void Load(BattleCreateArg battleArg)
        //public void Load(MapInitArg mapInitArg, BattleEntityInitArg entityInitArg)
        {
            //_G.Log("battle server : Battle : Load ");
            this.roomId = battleArg.roomId;

            tableId = battleArg.configId;
            //stageId = battleArg.stageId;
            this.funcId = battleArg.funcId;

            var battlePlayerArg = battleArg.battlePlayerInitArg;
            var mapInitArg = battleArg.mapInitArg;
            var entityInitArg = battleArg.entityInitArg;
            //var stageLogicArg = battleArg.stageLogicArg;

            //玩家初始化
            battlePlayerMgr.Init(battlePlayerArg);

            //地图数据初始化
            battleMapMgr.Init(mapInitArg);

            this.InitBattleConfig();

            //初始化触发器
            this.InitTrigger(battleArg.triggerSrcData);

            //AI初始化
            aiMgr.Init(this);

            //单位初始化
            battleEntityMgr.Init(entityInitArg, this);

            //玩家行为初始化
            playerActionMgr.Init(this);

            //技能效果初始化
            skillEffectMgr.Init(this);

            //碰撞器管理初始化
            collisionMgr.Init(this);

            BattleState = BattleState.Loading;

        }

        public Map GetMap()
        {
            return battleMapMgr.GetMap();
        }

        public bool IsOutOfMap(int x, int y)
        {
            return this.battleMapMgr.IsOutOfMap(x, y);
        }

        public bool IsObstacle(int x, int y)
        {
            return this.battleMapMgr.IsObstacle(x, y);
        }

        internal void DeleteBuffFromEntity(int guid, int id)
        {
            this.skillEffectMgr.DeleteBuffFromEntity(guid, id);
        }


        //初始化战斗配置
        public void InitBattleConfig()
        {

        }

        public int GetMapSizeX()
        {
            return this.battleMapMgr.MapSizeX;
        }

        public int GetMapSizeZ()
        {
            return this.battleMapMgr.MapSizeX;
        }

        //初始化触发器
        public void InitTrigger(TriggerSourceResData srcData)
        {
            triggerMgr.Init(this);

            //triggerMgr.Load(triggerJsonStrs);
            triggerMgr.Load(srcData);

            triggerMgr.Register();
        }

        internal void SetPlayerCtrlEntity(int playerIndex, int entityGuid)
        {
            var player = battlePlayerMgr.FindPlayerByPlayerIndex(playerIndex);
            player.SetCtrlHeroGuid(entityGuid);
        }

        internal int GenSkillGuid()
        {
            return this.skillEffectMgr.GenGuid();
        }

        public void Start()
        {
            //_G.Log("battle start !");

            BattleState = BattleState.Battling;

            //所有实体同步数据
            var entities = battleEntityMgr.GetAllEntity();
            foreach (var item in entities)
            {
                var entity = item.Value;
                entity.NotifyBattleData();
            }

            this.PlayerMsgSender.NotifyAll_BattleStart();

            this.OnBattleStartAction?.Invoke(null);

        }


        public float totalTime;

        public void Update()
        {
            if (BattleState == BattleState.Battling)
            {
                //_G.Log("battle", "currframe start update : " + currFrame);
                playerActionMgr.Update();
                this.collisionMgr.Update(TimeDelta);
                skillEffectMgr.Update(TimeDelta);
                aiMgr.Update(TimeDelta);
                battleEntityMgr.Update(TimeDelta);
                battleMapMgr.Update();
                this.triggerMgr.Update(TimeDelta);


                currFrame = currFrame + 1;

                totalTime = currFrame * TimeDelta;

                OnTimePassAction?.Invoke(new EventTimePassArg { currTime = totalTime });

            }
        }

        //op func
        public BattleEntity FindEntity(int guid,bool isIncludeDeath = false)
        {
            return battleEntityMgr.FindEntity(guid,isIncludeDeath);
        }

        public BattlePlayer FindPlayerByUid(long uid)
        {
            return battlePlayerMgr.FindPlayerByUid(uid);
        }

        public BattlePlayer FindPlayerByPlayerIndex(int index)
        {
            return battlePlayerMgr.FindPlayerByPlayerIndex(index);
        }

        public BaseAI FindAI(int guid)
        {
            return aiMgr.FindAI(guid);
        }


        public bool IsAllPlayerFinishLoading()
        {
            return battlePlayerMgr.IsAllPlayerFinishLoading();
        }

        public void SetAllPlayerLodingState()
        {
            this.battleState = BattleState.LoadingFinish;
        }

        public bool IsAllPlayerReadyFinish()
        {
            return battlePlayerMgr.IsAllPlayerReadyFinish();
        }

        public bool IsAllPlayerPlotEnd()
        {
            return battlePlayerMgr.IsAllPlayerPlotEnd();
        }

        public List<BattlePlayer> GetAllPlayers()
        {
            return this.battlePlayerMgr.battlePlayerList;
        }

        public Dictionary<int, BattleEntity> GetAllEntities(bool isIncludeDeath = false)
        {
            return this.battleEntityMgr.GetAllEntity(isIncludeDeath);
        }

        public BattleEntity FindNearestEnemyEntity(BattleEntity entity)
        {
            BattleEntity minDisEntity = null;
            float minDis = 9999999;
            var allEntities = GetAllEntities();
            foreach (var item in allEntities)
            {
                var currEntity = item.Value;
                var sqrtDis = Vector3.SqrtDistance(currEntity.position, entity.position);

                if (sqrtDis <= minDis && currEntity.Team != entity.Team)
                {
                    minDis = sqrtDis;
                    minDisEntity = currEntity;
                }

            }
            return minDisEntity;
        }

        // progress

        //有玩家上传加载进度
        public void SetPlayerProgress(long uid, int progress)
        {
            var player = FindPlayerByUid(uid);
            if (null == player)
            {
                Logx.LogError("Battle : SetPlayerProgress : the player is not found : uid : " + uid);
            }
            player.Progress = progress;

            //_G.Log("battle : SetPlayerProgress : uid : " + uid + " progress : " + progress);

            //判断是否都加载好了
            var isAllFinishLoading = IsAllPlayerFinishLoading();
            if (isAllFinishLoading)
            {
                ////notify msg (at presend only for remote battle )
                //csNotifyAllPlayerLoadFinish allFinish = new csNotifyAllPlayerLoadFinish();
                //NotifyAllPlayerMsg(ProtoIDs.NotifyAllPlayerLoadFinish, allFinish);
                this.PlayerMsgSender.NotifyAll_AllPlayerLoadFinish();
            }
        }

        //有玩家战斗准备好了
        internal void PlayerReadyFinish(long uid)
        {
            var player = FindPlayerByUid(uid);
            player.IsReadyFinish = true;

            //_G.Log("battle : PlayerReadyFinish : uid : " + uid);

            //判断是否都准备好了
            var isAllReadyFinish = IsAllPlayerReadyFinish();
            if (isAllReadyFinish)
            {
                //战斗开始
                Start();
            }
        }

        //添加一个玩家操作
        public void AddPlayerAction(PlayerAction action)
        {
            playerActionMgr.AddPlayerAction(action);
        }

        //添加一个技能效果
        internal void AddSkillEffect(int effectConfigId, SkillEffectContext context)
        {
            this.skillEffectMgr.Add(effectConfigId, context);
        }

        //public void CreateEntity(EntityInit entityInit)
        //{
        //    this.battleEntityMgr.CreateEntity(entityInit);
        //}

        //有玩家剧情播放完事了
        internal void PlayerPlotEnd(long uid)
        {
            var player = FindPlayerByUid(uid);
            player.IsPlotEnd = true;

            //_G.Log("battle : PlayerPlotEnd : uid : " + uid);

            //判断是否都播放完成了
            var isAllPlotEnd = IsAllPlayerReadyFinish();
            if (isAllPlotEnd)
            {
                this.OnAllPlayerPlotEnd("");
            }
        }

        internal void OnEntityBeHurt(BattleEntity beHurtEntity, float resultDamage, Skill damageSrcSkill)
        {
            var attacker = damageSrcSkill.releser;
            beHurtEntity.OnSyncCurrHealth(attacker.guid);

            if (resultDamage > 0)
            {
                attacker.OnHurtToOtherSuccess();
                if (damageSrcSkill.isNormalAttack)
                {
                    attacker.OnNormalAttackToOtehrSuccess(beHurtEntity, resultDamage, damageSrcSkill);
                }
                else
                {
                    attacker.OnSkillToOtehrSuccess();
                }

                this.aiMgr.OnEntityBeHurt(beHurtEntity, resultDamage, damageSrcSkill);
            }
        }

        public void CreateEntities(List<EntityInit> entityInitList)
        {
            this.battleEntityMgr.CreateEntities(entityInitList);
        }

        public void SetEntitiesShowState(List<int> entityGuids, bool isShow)
        {
            this.battleEntityMgr.SetEntitiesShowState(entityGuids, isShow);
        }


        //public void ForceEntityMoveToPos(int guid, Vector3 moveTargetPos, Vector3 dir, float speed)
        //{
        //    var entity = this.FindEntity(guid);
        //    if (entity != null)
        //    {
        //        entity.ForceToMove(moveTargetPos,speed);
        //    }
        //}

        //播放剧情
        public void PlayPlot(string name)
        {
            this.battlePlayerMgr.ResetAllPlayerPlotEndState();

            OnPlotStartAction?.Invoke(null);

            //scNotifyPlayPlot playPlot = new scNotifyPlayPlot();
            //playPlot.PlotName = name;
            //NotifyAllPlayerMsg(ProtoIDs.NotifyPlayPlot, playPlot);
            this.PlayerMsgSender.NotifyAll_PlayPlot(name);
        }

        public void BattleEnd(int winTeam)
        {
            BattleState = BattleState.End;

            //var str = bIsWin ? "Win" : "Fail";

            ////test 先只有两队
            //var winTeam = 0;
            //if (bIsWin)
            //{
            //    winTeam = 0;
            //}
            //else
            //{
            //    winTeam = 1;
            //}

            OnBattleEnd?.Invoke(this, winTeam);

            //this.PlayerMsgSender.NotifyAll_BattleEnd(winTeam);
            //_G.Log("BattleEnd : " + str);

            ////战斗结束监听触发 todo

            //scNotifyBattleEnd2S batleEnd = new scNotifyBattleEnd2S();
            //var allPlayers = GetAllPlayers();
            //foreach (var player in allPlayers)
            //{
            //    var uid = player.uid;
            //    batleEnd.PlayerEndInfoList.Add(new PlayerBattleEndInfo()
            //    {
            //        Uid = (int)uid,
            //        IsWin = bIsWin ? 1 : 0
            //    });
            //}
            //batleEnd.RoomId = this.roomId;
            //batleEnd.StageId = stageId;
            //batleEnd.BattleTableId = this.tableId;



            //ProxyMgr.CenterProxy.RequestAync((int)ProtoIDs.NotifyBattleEnd2S, batleEnd.ToByteArray());

            ////清理本次战斗 (最好是 battleMgr 监听 然后调用 clear)
            //ServerMgr.DestroyBattle(this.guid);
        }


        //事件----------

        public void OnEntityStartMoveByOnePos(int guid, Vector3 pos, float finalMoveSpeed)
        {
            var posList = new List<Vector3>();
            posList.Add(pos);
            this.PlayerMsgSender.NotifyAll_EntityStartMoveByPath(guid, posList, finalMoveSpeed);
        }

        public void OnEntityStartMoveByPath(int guid, List<Vector3> posList, float finalMoveSpeed)
        {
            this.PlayerMsgSender.NotifyAll_EntityStartMoveByPath(guid, posList, finalMoveSpeed);
        }


        ////有实体移动了
        //internal void OnEntityStartMove(int guid, Vector3 targetPos, Vector3 dir, float finalMoveSpeed)
        //{
        //    ////send msg to clients
        //    //scNotifyEntityMove notify = new scNotifyEntityMove()
        //    //{
        //    //    Guid = guid,
        //    //    EndPos = BattleConvert.ConvertToVector3Proto(targetPos),
        //    //    MoveSpeed = BattleConvert.ToValue(finalMoveSpeed)
        //    //};
        //    //NotifyAllPlayerMsg(ProtoIDs.NotifyEntityMove, notify);

        //    this.PlayerMsgSender.NotifyAll_EntityStartMove(guid, targetPos, dir, finalMoveSpeed);
        //}

        //有实体停止了移动
        internal void OnEntityStopMove(int guid, Vector3 position)
        {
            //scNotifyEntityStopMove stopMove = new scNotifyEntityStopMove()
            //{
            //    Guid = guid,
            //    EndPos = BattleConvert.ConvertToVector3Proto(position),
            //};
            //NotifyAllPlayerMsg(ProtoIDs.NotifyEntityStopMove, stopMove);

            aiMgr.OnEntityStopMove(guid, position);

            this.PlayerMsgSender.NotifyAll_EntityStopMove(guid, position);
        }

        //同步某个 entity 的朝向
        public void SyncEntityDir(int guid, Vector3 dir)
        {
            this.PlayerMsgSender.NotifyAll_SyncEntityDir(guid, dir);
        }

        //当实体符合放技能条件的时候(但是前摇还没开始)
        public void OnEntityReleaseSkill(int entityGuid, Skill skill, int targetGuid, Vector3 targetPos)
        {
            var skillConfigId = skill.configId;
            //aiMgr.OnEntityStartSkillEffect(entityGuid, skill, targetGuid, targetPos);

            this.PlayerMsgSender.NotifyAll_OnEntityReleaseSkill(entityGuid, skillConfigId);
        }

        public void OnEntityFinishSkillEffect(int entityGuid, Skill skill)
        {
            aiMgr.OnEntityFinishSkillEffect(entityGuid, skill);
        }

        ////有技能效果被创建了
        //public void OnCreateSkillEffect(int guid, int resId, Vector3 position, int followEntityGuid, bool isAutoDestroy)
        //{
        //    this.PlayerMsgSender.NotifyAll_CreateSkillEffect(guid, resId, position, followEntityGuid, isAutoDestroy);
        //}

        //有技能效果被创建了
        public void OnCreateSkillEffect(CreateEffectInfo createEffectInfo)
        {
            this.PlayerMsgSender.NotifyAll_CreateSkillEffect(createEffectInfo);
        }

        //更新 buff 信息
        public void OnUpdateBuffInfo(BuffEffectInfo buffInfo)
        {
            this.PlayerMsgSender.NotifyAll_NotifyUpdateBuffInfo(buffInfo);
        }

        //有技能效果开始移动
        public void OnSkillEffectStartMove(EffectMoveArg effectrMoveArg)
        {
            //scNotifySkillEffectStartMove startMove = new scNotifySkillEffectStartMove()
            //{
            //    EffectGuid = effectrMoveArg.effectGuid,
            //    StartPos = BattleConvert.ConvertToVector3Proto(effectrMoveArg.startPos),
            //    TargetPos = BattleConvert.ConvertToVector3Proto(effectrMoveArg.targetPos),
            //    TargetGuid = effectrMoveArg.targetGuid,
            //    IsFollow = effectrMoveArg.isFollow,
            //    MoveSpeed = BattleConvert.ToValue(effectrMoveArg.moveSpeed)

            //};
            //NotifyAllPlayerMsg(ProtoIDs.NotifySkillEffectStartMove, startMove);

            this.PlayerMsgSender.NotifyAll_SkillEffectStartMove(effectrMoveArg);
        }

        public bool CheckCollision(BattleEntity battleEntity, out BattleEntity collisionEntity, out bool isNeedCollisionWait)
        {
            return this.collisionMgr.IsMoveCollision(battleEntity, out collisionEntity, out isNeedCollisionWait);
        }

        //当一个实体正常移动时发生碰撞的时候
        public void OnEntityMoveCollision(int checkEntityGuid, int collisionEntityGuid, bool isNeedCollisionWait)
        {
            var entity = this.FindEntity(checkEntityGuid);
            if (entity != null)
            {
                if (!isNeedCollisionWait)
                {
                    //如果不需要碰撞等待的话 就直接寻路
                    var ai = this.aiMgr.FindAI(entity.guid);
                    ai.FindPathByCurrPath();
                }
            }
        }

        public void EntityContinueFindPath(BattleEntity battleEntity)
        {
            var ai = this.aiMgr.FindAI(battleEntity.guid);
            ai.FindPathByCurrPath();
        }


        //有技能效果销毁了
        public void OnSkillEffectDestroy(int guid)
        {
            //scNotifySkillEffectDestroy skillEffectDestroy = new scNotifySkillEffectDestroy()
            //{
            //    EffectGuid = guid
            //};
            //NotifyAllPlayerMsg(ProtoIDs.NotifySkillEffectDestroy, skillEffectDestroy);

            this.PlayerMsgSender.NotifyAll_SkillEffectDestroy(guid);
        }

        public void RegisterEntityAI(BattleEntity entity)
        {
            //var isPlayerCtrl = entity.playerIndex >= 0;
            var isPlayerCtrl = entity.isPlayerCtrl;
            BaseAI ai = null;
            if (isPlayerCtrl)
            {
                ai = new PlayerAI();
            }
            else
            {
                ai = new EnemyAI();
            }


            this.aiMgr.UseAIToEntity(ai, entity);
        }

        //实体到达了当前目标点
        public void OnMoveToCurrTargetPosFinish(int entityGuid)
        {
            this.aiMgr.OnMoveToCurrTargetPosFinish(entityGuid);
        }

        //有新实体创建了
        internal void OnCreateEntities(List<BattleEntity> entities)
        {
            //scNotifyCreateEntities create = new scNotifyCreateEntities();
            //foreach (var entity in entities)
            //{
            //    var serverEntity = new BattleEntityProto()
            //    {
            //        Guid = entity.guid,
            //        ConfigId = entity.configId,
            //        PlayerIndex = entity.playerIndex,
            //        Position = BattleConvert.ConvertToVector3Proto(entity.position),
            //        Level = entity.level,
            //    };
            //    create.BattleEntities.Add(serverEntity);
            //}
            //NotifyAllPlayerMsg(ProtoIDs.NotifyCreateEntities, create);


            this.PlayerMsgSender.NotifyAll_CreateEntities(entities);

        }


        //有实体改变的属性需要同步
        internal void OnSyncEntityAttr(int guid, Dictionary<EntityAttrType, float> dic)// EntityAttrGroup finalAttrGroup
        {
            //scNotifySyncEntityAttr syncAttr = new scNotifySyncEntityAttr();
            //syncAttr.EntityGuid = guid;
            //finalAttrGroup.MakeAttrProto(syncAttr.Attrs);

            //NotifyAllPlayerMsg(ProtoIDs.NotifySyncEntityAttr, syncAttr);

            this.PlayerMsgSender.NotifyAll_SyncEntityAttr(guid, dic);
        }

        ////有实体改变的数据值需要同步
        //internal void OnSyncEntityCurrValue(int guid, int hp)
        //{

        //}

        //同步当前血量
        public void OnSyncEntityCurrHealth(int guid, int hp, int fromEntityGuid)
        {
            //scNotifySyncEntityValue syncValue = new scNotifySyncEntityValue();
            //syncValue.EntityGuid = guid;
            //syncValue.Values.Add(new BattleEntityValueProto()
            //{
            //    Type = (int)(EntityCurrValueType.CurrHealth),
            //    Value = hp
            //});

            //NotifyAllPlayerMsg(ProtoIDs.NotifySyncEntityValue, syncValue);

            this.PlayerMsgSender.NotifyAll_SyncEntityCurrHealth(guid, hp, fromEntityGuid);
        }

        internal void OnEntityAddBuff(int guid, BuffEffect buff)
        {
            this.PlayerMsgSender.NotifyAll_EntityAddBuff(guid, buff);
        }

        //有实体死亡了
        internal void OnEntityDead(BattleEntity battleEntity)
        {
            var guid = battleEntity.guid;

            //scNotifyEntityDead notifyDead = new scNotifyEntityDead();
            //notifyDead.EntityGuid = guid;

            //NotifyAllPlayerMsg(ProtoIDs.NotifyEntityDead, notifyDead);

            //this.aiMgr.RemoveAI(guid);

            this.skillEffectMgr.DeleteAllBuffsFromEntity(guid);

            this.PlayerMsgSender.NotifyAll_EntityDead(battleEntity);

            //_G.Log("--------------OnEntityDead");
            var eventArgs = new EventEntityEventArg
            {
                context = new ActionContext()
                {
                    DeadEntity = battleEntity,
                    battle = this
                },
                entityEventType = EntityEventType.Dead
            };
            //Logx.Log("Battle : entity dead : configId : " + battleEntity.configId);
            this.OnEntityEventAction?.Invoke(eventArgs);

        }

        //所有玩家的剧情都准备好了
        public void OnAllPlayerPlotEnd(string plotName)
        {
            //scNotifyPlotEnd notifyPlotEnd = new scNotifyPlotEnd();
            //NotifyAllPlayerMsg(ProtoIDs.NotifyPlotEnd, notifyPlotEnd);
            this.PlayerMsgSender.NotifyAll_AllPlayerPlotEnd(plotName);

            OnPlotEndAction?.Invoke(null);
        }

        //战斗实体控制显隐
        internal void OnSetEntitiesShowState(List<int> entityGuids, bool isShow)
        {
            //scNotifySetEntityShowState showState = new scNotifySetEntityShowState();
            //foreach (var guid in entityGuids)
            //{
            //    showState.Guids.Add(guid);
            //}
            //showState.IsShow = isShow;
            //NotifyAllPlayerMsg(ProtoIDs.NotifySetEntityShowState, showState);

            this.PlayerMsgSender.NotifyAll_SetEntitiesShowState(entityGuids, isShow);
        }

        public void OnSkillInfoUpdate(Skill skill)
        {
            this.PlayerMsgSender.NotifyAll_NotifySkillInfoUpdate(skill);
        }


        internal void OnNotifySkillTrackStart(Skill skill, int skillTrackId)
        {
            this.PlayerMsgSender.NotifyAll_NotifySkillTrackStart(skill, skillTrackId);
        }

        internal void OnNotifySkillTrackEnd(Skill skill, int skillTrackId)
        {
            this.PlayerMsgSender.NotifyAll_NotifySkillTrackEnd(skill, skillTrackId);
        }

        //-------------------------

        ////广播战斗消息
        //public void NotifyAllPlayerMsg(ProtoIDs cmd, IMessage msgData)
        //{
        //    var allPlayers = GetAllPlayers();
        //    foreach (var item in allPlayers)
        //    {
        //        var battlePlayer = item;
        //        SendMsgToClient(battlePlayer.uid, cmd, msgData);

        //    }
        //}

        ////向某个玩家发送消息
        //public void SendMsgToClient(long uid, ProtoIDs cmd, IMessage msgData)
        //{
        //    if (uid < 0)
        //    {
        //        return;
        //    }

        //    _G.Log("batte : send msg to client(" + uid + ") : cmd : " + (int)cmd + " " + cmd);
        //    scTransitionBattle2Player transBattle = new scTransitionBattle2Player();
        //    transBattle.Cmd = (int)cmd;

        //    //转发
        //    ClientProtoHead clientProtoHead = new ClientProtoHead()
        //    {
        //        cmd = (ushort)cmd,
        //        uid = (ulong)uid,
        //    };
        //    var clientBytes = ProtoMsgUtil.MakeClientMsgBytes(msgData.ToByteArray(), clientProtoHead);
        //    transBattle.Data = ByteString.CopyFrom(clientBytes);

        //    //发送
        //    ServerProtoHead head = new ServerProtoHead()
        //    {
        //        cmd = (ushort)ProtoIDs.TransitionBattle2Player2S,
        //        uid = (ulong)uid
        //    };
        //    ProxyMgr.CenterProxy.RequestAync(head, transBattle.ToByteArray());
        //}

        public int GetRandInt(int a, int b)
        {
            return MyRandom.Next(a, b, this.rand);
        }

        public float GetRandFloat(float a, float b)
        {
            return MyRandom.Next(a, b, this.rand);
        }

        public Vector3 GetRectRandVector3(Vector3 min, Vector3 max)
        {
            var x = this.GetRandFloat(min.x, max.x);
            var y = this.GetRandFloat(min.y, max.y);
            var z = this.GetRandFloat(min.z, max.z);

            return new Vector3(x, y, z);

        }

        public void Destroy()
        {
            OnTimePassAction = null;
            OnEntityEventAction = null;

            OnBattleEnd = null;
            //TODO Clear
            //battlePlayerMgr.Clear();
            //battleMapMgr.Clear();
            //battleEntityMgr.Clear();
            //playerActionMgr.Clear();
            //skillEffectMgr.Clear();


            //注意 目前是删除 没有复用战斗

            triggerMgr.Clear();

        }

    }
}

