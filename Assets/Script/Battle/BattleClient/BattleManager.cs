﻿using System.Collections.Generic;
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
        LocalBattleLogic_Executer localBattleExecuter;
        public IBattleClientMsgSender MsgSender;
        public IBattleClientMsgReceiver MsgReceiver;
        public BattleType battleType;        
        private BattleClient_CreateBattleArgs battleClientArgs;



        #endregion

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

        public void OnEnterBattle()
        {
            Logx.Log(LogxType.Game, "battle start");

            this.localBattleExecuter?.OnEnterBattle();
        }
        
        
        public void Update(float timeDelta)
        {
            if (this.BattleState == BattleState.Null)
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
            this.BattleState = BattleState.Null;
        }

       
        public void Release()
        {
            RemoveListener();
        }
    }
}