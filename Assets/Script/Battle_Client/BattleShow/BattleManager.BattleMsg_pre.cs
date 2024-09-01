// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Battle;
// 
// using Battle_Client;
// using GameData;
// using NetProto;
// 
// using UnityEditor;
// using Vector2 = UnityEngine.Vector2;
//
// namespace Battle_Client
// {
//     //战斗事件消息的发送和处理
//     public partial class BattleManager
//     {
//         public void InitBattleMsgHandle()
//         {
//             EventDispatcher.AddListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
//              // EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
//             // EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
//             //     OnChangeEntityBattleData);
//             // EventDispatcher.AddListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
//             // EventDispatcher.AddListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
//             // EventDispatcher.AddListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
//             // EventDispatcher.AddListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
//             // EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnEntityDead, OnEntityDead);
//             // EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, OnEntityDestroy);
//             EventDispatcher.AddListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, OnOnBattleEnd);
//             // EventDispatcher.AddListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
//             //     this.OnEntityChangeShowState);
//
//             // EventDispatcher.AddListener<int, bool>(EventIDs.OnPlayerReadyState, this.OnPlayerReadyState);
//             // EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
//             // EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);
//
//             // EventDispatcher.AddListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter,
//             //     OnUIAttrOptionPointEnter);
//             // EventDispatcher.AddListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
//             // EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, OnUISkillOptionPointEnter);
//             // EventDispatcher.AddListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
//             // EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
//             // EventDispatcher.AddListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);
//         }
//
//         void OnAllPlayerLoadFinish()
//         {
//             _uiPre.SetReadyShowState(true);
//
//             _uiPre.SetStateText("wait to battle start");
//
//             CtrlManager.Instance.GlobalCtrlPre.LoadingUIPre.Hide();
//         }
//
//         void OnPlayerReadyState(int uid, bool isReady)
//         {
//             var myUid = (int)GameDataManager.Instance.UserStore.Uid;
//
//             if (myUid == uid)
//             {
//                 _uiPre.SetReadyBtnShowState(isReady);
//             }
//         }
//
//         void OnBattleStart()
//         {
//             _uiPre.SetReadyShowState(false);
//             _uiPre.SetStateText("OnBattleStart");
//         }
//
//         
//         public void OnPlayPlotEnd(string plotName)
//         {
//             var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
//             battleNet.SendClientPlotEnd();
//         }
//
//         //当创建了 entity 的时候
//         public void OnCreateEntity(BattleEntity_Client entity)
//         {
//             hpModule.RefreshEntityData(entity);
//
//             var entityConfig = Config.ConfigManager.Instance.GetById<EntityInfo>(entity.configId);
//             var isBoss = 1 == entityConfig.IsBoss;
//             if (isBoss)
//             {
//                 this._uiPre.StartBossComingAni();
//                 this._uiPre.StartBossLimitCountdown();
//             }
//
//             this.RefreshBattleSingleHeroInfo(entity, 0);
//         }
//
//
//         //当实体战斗数据改变了
//         public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
//         {
//             hpModule.RefreshEntityData(entity, fromEntityGuid);
//             this.RefreshBattleAttrUI();
//             this.RefreshBattleSingleHeroInfo(entity, fromEntityGuid);
//         }
//
//         //当技能信息改变了
//         public void OnSkillInfoUpdate(int entityGuid, BattleSkillInfo skillInfo)
//         {
//             //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//
//             var myEntityGuid = BattleManager.Instance.GetLocalCtrlHerGuid();
//
//             if (myEntityGuid == entityGuid)
//             {
//                 this._uiPre.RefreshSkillInfo(skillInfo.configId, skillInfo.currCDTime);
//             }
//         }
//
//         //当 buff 信息改变了
//         public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
//         {
//             if (BattleManager.Instance.GetLocalPlayer().ctrlHeroGuid == buffInfo.targetEntityGuid)
//             {
//                 //Debug.Log("zxy : OnBuffInfoUpdate : ");
//                 BattleBuffUIData buff = new BattleBuffUIData();
//                 buff.guid = buffInfo.guid;
//                 buff.maxCDTime = buffInfo.maxCDTime;
//                 buff.currCDTime = buffInfo.currCDTime;
//                 buff.isRemove = buffInfo.isRemove;
//                 //buff.iconResId = buffInfo.iconResId;
//                 buff.configId = buffInfo.configId;
//                 buff.stackCount = buffInfo.stackCount;
//
//                 this._uiPre.RefreshBuffInfo(buff);
//             }
//         }
//
//         public void OnUIAttrOptionPointEnter(EntityAttrType type, Vector2 pos)
//         {
//             var attrOption = AttrInfoHelper.Instance.GetAttrInfo(type);
//             UIArgs args = new DescribeUIArgs()
//             {
//                 name = attrOption.name,
//                 content = attrOption.describe,
//                 pos = pos + Vector2.right * 50,
//                 iconResId = attrOption.iconResId
//             };
//             this._uiPre.ShowDescribeUI(args);
//         }
//
//         public void OnUIAttrOptionPointExit(EntityAttrType type)
//         {
//             this._uiPre.HideDescribeUI();
//         }
//
//         public void OnUISkillOptionPointEnter(int skillId, Vector2 pos)
//         {
//             var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
//
//             var des = skillConfig.Describe;
//             UIArgs args = new DescribeUIArgs()
//             {
//                 name = skillConfig.Name,
//                 content = des,
//                 pos = pos + Vector2.right * 50,
//                 iconResId = skillConfig.IconResId
//             };
//             this._uiPre.ShowDescribeUI(args);
//         }
//
//         public void OnUISkillOptionPointExit(int skillId)
//         {
//             this._uiPre.HideDescribeUI();
//         }
//
//         public void OnUIBuffOptionPointEnter(int buffConfigId, Vector2 pos)
//         {
//             var buffConfig = Config.ConfigManager.Instance.GetById<Config.BuffEffect>(buffConfigId);
//
//             var des = buffConfig.Describe;
//             UIArgs args = new DescribeUIArgs()
//             {
//                 name = buffConfig.Name,
//                 content = des,
//                 pos = pos + Vector2.right * 50,
//                 iconResId = buffConfig.IconResId
//             };
//             this._uiPre.ShowDescribeUI(args);
//         }
//
//         public void OnUIBuffOptionPointExit(int OnUIBuffOptionPointEnter)
//         {
//             this._uiPre.HideDescribeUI();
//         }
//
//
//         public void OnEntityDestroy(BattleEntity_Client entity)
//         {
//             hpModule.DestroyEntityHp(entity);
//         }
//
//         public void OnEntityDead(BattleEntity_Client entity)
//         {
//             _uiPre.SetHpShowState(entity.guid, false);
//         }
//
//         public void OnOnBattleEnd(BattleResultDataArgs battleResultArgs)
//         {
//             //Battle end UI
//             
//             var args = new BattleResultUIArgs()
//             {
//                 isWin = battleResultArgs.isWin,
//                 //reward
//             };
//             args.uiItem = new List<CommonItemUIArgs>();
//
//             foreach (var item in battleResultArgs.rewardDataList)
//             {
//                 var _item = new CommonItemUIArgs()
//                 {
//                     configId = item.configId,
//                     count = item.count
//                 };
//                 args.uiItem.Add(_item);
//             }
//
//             this._resultUIPre.Refresh(args);
//             this._resultUIPre.Show();
//
//             BattleEntityManager.Instance.OnBattleEnd();
//             skillTrackModule.OnBattleEnd();
//             BattleSkillEffect_Client_Manager.Instance.OnBattleEnd();
//         }
//
//         //entity 改变显隐的时候
//         public void OnEntityChangeShowState(BattleEntity_Client entity, bool isShow)
//         {
//             //找到血条也要显隐
//             _uiPre.SetHpShowState(entity.guid, isShow);
//         }
//
//         public void OnSkillTrackStart(TrackBean trackBean)
//         {
//             this.skillTrackModule.AddTrack(trackBean);
//         }
//
//         public void OnSkillTrackEnd(int entityGuid, int trackId)
//         {
//             this.skillTrackModule.DeleteTrack(entityGuid, trackId);
//         }
//
//
//         public void ReleaseBattleMsgHandle()
//         {
//             EventDispatcher.RemoveListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
//             EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
//             EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
//                 OnChangeEntityBattleData);
//             EventDispatcher.RemoveListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
//             EventDispatcher.RemoveListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
//             EventDispatcher.RemoveListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
//             EventDispatcher.RemoveListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
//             EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnEntityDead, OnEntityDead);
//             EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, this.OnEntityDestroy);
//             EventDispatcher.RemoveListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
//                 this.OnEntityChangeShowState);
//             EventDispatcher.RemoveListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, this.OnOnBattleEnd);
//             EventDispatcher.RemoveListener<int, bool>(EventIDs.OnPlayerReadyState, this.OnPlayerReadyState);
//             EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
//             EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);
//             EventDispatcher.RemoveListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter,
//                 OnUIAttrOptionPointEnter);
//             EventDispatcher.RemoveListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
//             EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter,
//                 OnUISkillOptionPointEnter);
//             EventDispatcher.RemoveListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
//             EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
//             EventDispatcher.RemoveListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);
//         }
//     }
// }