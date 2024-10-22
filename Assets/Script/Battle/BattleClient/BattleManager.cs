using System.Collections.Generic;
using Battle;
using GameData;

namespace Battle_Client
{
    //战斗创建的管理
    public partial class BattleManager : Singleton<BattleManager>
    {
        #region Var

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
        public BattleState BattleState;
        public BattleProcessState processState;
        LocalBattleLogic_Executer localBattleExecuter;
        public IBattleClientMsgSender MsgSender;
        public IBattleClientMsgReceiver MsgReceiver;
        public BattleType battleType;
        private BattleClient_CreateBattleArgs battleClientArgs;

        private PlayerInput playerInput;
        //当前通过波的结算信息
        public BattleWavePass_RecvMsg_Arg wavePassArg;
        //是否智能施法
        private bool isIntelligentRelease;

        public bool IsIntelligentRelease
        {
            get
            {
                return LocalData.GetInt("intelligentRelease_switch") > 0;
            }
            set
            {
                isIntelligentRelease = value;
                LocalData.SetInt("intelligentRelease_switch", isIntelligentRelease ? 1 : 0);
            }
        }

        #endregion

        public void Init()
        {
            InitBattleRecvMsg();
            this.RegisterListener();
            BattleState = BattleState.Null;

            playerInput = new PlayerInput();
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
            EventDispatcher.AddListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
            EventDispatcher.AddListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
        }

        public void OnEnterBattle()
        {
            Logx.Log(LogxType.Game, "battle start");

            this.localBattleExecuter?.OnEnterBattle();
        }

        public void OnEnterProcessState(BattleProcessState state,int timeMS)
        {
            this.processState = state;

            if (state == BattleProcessState.Ready)
            {
                EventDispatcher.Broadcast<int>(EventIDs.OnProcessReadyStateEnter,timeMS);
            }
            else if(state == BattleProcessState.Battle)
            {
                EventDispatcher.Broadcast<int>(EventIDs.OnProcessBattleStateEnter,timeMS);
            }
            else if(state == BattleProcessState.Boss)
            {
                EventDispatcher.Broadcast<int>(EventIDs.OnProcessBossStateEnter,timeMS);
            }
        }

        public void Update(float deltaTime)
        {
            if (this.BattleState == BattleState.Null)
            {
                return;
            }

            UpdateRecvMsgList();
            localBattleExecuter?.Update(deltaTime);

            playerInput.Update(deltaTime);
        }

        public void LateUpdate(float timeDelta)
        {
            UpdateCamera();
        }

        public void FixedUpdate(float fixedTime)
        {
            localBattleExecuter?.FixedUpdate(fixedTime);
        }

       
        public void OnBattleWavePass(BattleWavePass_RecvMsg_Arg wavePassArg)
        {
            this.wavePassArg = wavePassArg;
            EventDispatcher.Broadcast(EventIDs.OnProcessWavePass);
        }

        public void OnExitBattle()
        {
            Logx.Log(LogxType.Game, "battle end");

            this.localBattleExecuter?.OnExitBattle();

            this.Clear();
        }

        public void BattleEnd(BattleResultDataArgs battleResultDataArgs)
        {
            BattleManager.Instance.BattleState = BattleState.End;

            EventDispatcher.Broadcast(EventIDs.OnBattleEnd, battleResultDataArgs);

            this.OnExitBattle();

            //战斗结算界面
            var args = new BattleResultUIArgs()
            {
                isWin = battleResultDataArgs.isWin,
                itemDataList = battleResultDataArgs.rewardDataList
            };
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
            
            // this._resultUIPre.Refresh(args);
            // this._resultUIPre.Show();
            //
            UIManager.Instance.Open<BattleResultUI>(args);

            BattleEntityManager.Instance.OnBattleEnd();
            playerInput.OnBattleEnd();
            BattleSkillEffectManager_Client.Instance.OnBattleEnd();
        }

        public void RemoveListener()
        {
            //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, OnCreateEntity);
            EventDispatcher.RemoveListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
            EventDispatcher.RemoveListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
        }

        void OnSkillTrackStart(TrackBean trackBean)
        {
            this.playerInput.OnSkillTrackStart(trackBean);
        }

        void OnSkillTrackEnd(int entityGuid, int trackId)
        {
            this.playerInput.OnSkillTrackEnd(entityGuid, trackId);
        }

        public void Clear()
        {
            localBattleExecuter = null;
            ClearRecvMsg();
            this.BattleState = BattleState.Null;
        }


        public void Release()
        {
            RemoveListener();
        }
    }
}