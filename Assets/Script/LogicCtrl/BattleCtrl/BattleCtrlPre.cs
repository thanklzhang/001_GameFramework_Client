// using Battle_Client;
// using GameData;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.InteropServices;
// using Table;
// using UnityEngine;
// using UnityEngine.UI;
//
// //战斗 ctrl
// public class BattleCtrlPre : BaseCtrl_pre
// {B
//     BattleUIPre _uiPre;
//     BattleResultUIPre _resultUIPre;
//     GameObject sceneObj;
//
//     HpModule hpModule;
//
//     SkillDirectorModule skillDirectModule;
//     SkillTrackModule skillTrackModule;
//
//     string scenePath;
//
//     public override void OnInit()
//     {
//         //this.isParallel = false;
//
//         //生命周期好像有点不对
//         EventDispatcher.AddListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
//         EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
//         EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
//             OnChangeEntityBattleData);
//         EventDispatcher.AddListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
//         EventDispatcher.AddListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
//         EventDispatcher.AddListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
//         EventDispatcher.AddListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
//         EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnEntityDead, OnEntityDead);
//         EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, OnEntityDestroy);
//         EventDispatcher.AddListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, OnOnBattleEnd);
//         EventDispatcher.AddListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
//             this.OnEntityChangeShowState);
//
//         EventDispatcher.AddListener<int, bool>(EventIDs.OnPlayerReadyState, this.OnPlayerReadyState);
//         EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
//         EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);
//
//         EventDispatcher.AddListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter,
//             OnUIAttrOptionPointEnter);
//         EventDispatcher.AddListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
//         EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, OnUISkillOptionPointEnter);
//         EventDispatcher.AddListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
//         EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
//         EventDispatcher.AddListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);
//
//
//         hpModule = new HpModule();
//
//         skillDirectModule = new SkillDirectorModule();
//         skillTrackModule = new SkillTrackModule();
//     }
//
//     private List<LoadObjectRequest> entityLoadReqs;
//
//     private float currProgress = 0;
//     private float maxProgress = 1;
//
//     public override void OnStartLoad()
//     {
//         var battleTableId = BattleManager.Instance.battleTableId;
//         var battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleTableId);
//         var battleTriggerTb = Table.TableManager.Instance.GetById<Table.BattleTrigger>(battleTb.TriggerId);
//
//         //scene
//         var mapConfig = Table.TableManager.Instance.GetById<Table.BattleMap>(battleTb.MapId);
//         var sceneResId = mapConfig.ResId;
//         var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(sceneResId);
//         scenePath = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;
//
//         //fill load data
//         var objsRequestList = new List<LoadObjectRequest>();
//         //scene
//         // objsRequestList.Add(new LoadGameObjectRequest(scenePath, 1) { selfFinishCallback = OnSceneLoadFinish });
//         objsRequestList.Add(new LoadSceneRequest("BattleScene_001") { selfFinishCallback = OnSceneLoadFinish });
//         //battle ui
//         objsRequestList.Add(new LoadUIRequest<BattleUIPre>() { selfFinishCallback = OnUILoadFinish });
//         //battle end ui
//         objsRequestList.Add(new LoadUIRequest<BattleResultUIPre>() { selfFinishCallback = OnBattleResultUILoadFinish });
//      
//         
//         //entity
//         entityLoadReqs = BattleEntityManager.Instance.MakeCurrBattleAllEntityLoadRequests(OnEntityLoadFinish);
//         objsRequestList.AddRange(entityLoadReqs);
//         //
//
//         //loading
//         currProgress = 0;
//         maxProgress = 1;
//         SetLoadingProgress();
//         CtrlManager.Instance.GlobalCtrlPre.LoadingUIPre.Show();
//         //
//
//         this.loadRequest = ResourceManager.Instance.LoadObjects(objsRequestList);
//     }
//
//     void SetLoadingProgress()
//     {
//         var curr = currProgress / (float)maxProgress;
//         // Logx.Log("curr : " + curr);
//         CtrlManager.Instance.GlobalCtrlPre.LoadingUIPre.SetProgress(curr);
//     }
//
//     //初始话战斗相关资源
//     public void LoadBattleInitRes()
//     {
//     }
//
//     public void OnUILoadFinish(BattleUIPre battleUIPre)
//     {
//         this._uiPre = battleUIPre;
//
//         hpModule.Init(_uiPre);
//
//         currProgress += 0.15f;
//         SetLoadingProgress();
//     }
//
//     public void OnBattleResultUILoadFinish(BattleResultUIPre battleResultUIPre)
//     {
//         this._resultUIPre = battleResultUIPre;
//         this._resultUIPre.Hide();
//
//         currProgress += 0.15f;
//         SetLoadingProgress();
//     }
//
//
//     public Quaternion cameraRotationOffset;
//     MapCellView mapCellView;
//
//     public void OnSceneLoadFinish()
//     {
//         //先这样去 之后增加 scene 读取的接口
//         var sceneRoot = GameObject.Find("_scene_root").transform;
//         sceneObj = sceneRoot.GetChild(0).gameObject;
//         sceneObj.transform.position = new Vector3(0, 0, 0);
//
//         var tempCameraTran = sceneObj.transform.Find("Camera");
//
//         var camera3D = CameraManager.Instance.GetCamera3D();
//         camera3D.SetPosition(tempCameraTran.position);
//         camera3D.SetRotation(tempCameraTran.rotation);
//         cameraRotationOffset = tempCameraTran.rotation;
//
//         //地图 cell 视图工具查看器(目前只限本地战斗)
//         if (BattleManager.Instance.IsLocalBattle())
//         {
//             mapCellView = sceneObj.GetComponent<MapCellView>();
//             var map = BattleManager.Instance.GetLocalBattleMap();
//             mapCellView?.SetMap(map);
//             //mapCellView.SetRenderPath(new List<Pos>());
//         }
//
//         currProgress += 0.15f;
//         SetLoadingProgress();
//     }
//
//     public void OnEntityLoadFinish(BattleEntity_Client viewEntity, GameObject obj)
//     {
//         viewEntity.OnLoadModelFinish(obj);
//
//         var total = 1;
//         if (entityLoadReqs.Count > 0)
//         {
//             total = entityLoadReqs.Count;
//         }
//
//         currProgress += 0.55f * (1.0f / total);
//         SetLoadingProgress();
//     }
//
//     public override void OnLoadFinish()
//     {
//         // Logx.Log("battle ctrl : battle res OnLoadFinish");
//
//         skillDirectModule.Init(this);
//         skillTrackModule.Init(this);
//     }
//
//     public override void OnEnter(CtrlArgs args)
//     {
//         //假设加载好了
//         //var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
//         //battleNet.SendPlayerLoadProgress(1000);
//
//         _uiPre.SetStateText("self load finish , wait all player load finish");
//         BattleManager.Instance.MsgSender.Send_PlayerLoadProgress(1000);
//
//
//         AudioManager.Instance.PlayBGM((int)ResIds.bgm_battle_001);
//     }
//
//     public override void OnActive()
//     {
//         CtrlManager.Instance.HideTitleBar();
//
//         _uiPre.onCloseBtnClick += OnClickCloseBtn;
//         _uiPre.onReadyStartBtnClick += OnClickReadyStartBtn;
//         _uiPre.onAttrBtnClick += OnClickAttrBtn;
//
//         this._resultUIPre.onClickConfirmBtn += OnClickResultConfirmBtn;
//
//         //EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
//         //EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);
//
//         _uiPre.Show();
//         _uiPre.SetReadyShowState(false);
//
//         RefreshBattleAttrUI();
//
//         RefreshBattleSkillUI();
//
//         RefreshBattleBuffUI();
//
//         RefreshBattleHeroInfoUI();
//
//         RefreshBattleStageInfoUI();
//
//         _uiPre.HideBossComing();
//     }
//
//     public void RefreshBattleAttrUI()
//     {
//         BattleAttrUIArgs attrUIArgs = new BattleAttrUIArgs();
//         attrUIArgs.battleAttrList = new List<BattleAttrUIData>();
//
//         var attr = BattleManager.Instance.GetLocalCtrlHeroAttrs();
//         List<EntityAttrType> types = new List<EntityAttrType>()
//         {
//             EntityAttrType.Attack,
//             EntityAttrType.Defence,
//             EntityAttrType.MaxHealth,
//             EntityAttrType.AttackSpeed,
//             EntityAttrType.AttackRange,
//             EntityAttrType.MoveSpeed,
//         };
//         ////之后配置
//         //List<string> typeNameList = new List<string>()
//         //{
//         //     "攻击",
//         //     "防御",
//         //     "生命值",
//         //     "攻击速度",
//         //     "攻击距离",
//         //     "移动速度",
//         //};
//         for (int i = 0; i < types.Count; i++)
//         {
//             var attrType = types[i];
//
//             var attrOption = AttrInfoHelper.Instance.GetAttrInfo(attrType);
//
//             string name = "" + attrOption.name;
//             float value = attr.GetValue(attrType);
//             AttrValueShowType showType = AttrValueShowType.Int;
//             if (attrType == EntityAttrType.AttackSpeed)
//             {
//                 showType = AttrValueShowType.Float_2;
//             }
//             else if (attrType == EntityAttrType.AttackRange)
//             {
//                 showType = AttrValueShowType.Float_2;
//             }
//             else if (attrType == EntityAttrType.MoveSpeed)
//             {
//                 showType = AttrValueShowType.Float_2;
//             }
//
//             BattleAttrUIData uiData = new BattleAttrUIData()
//             {
//                 type = attrType,
//                 describe = attrOption.describe,
//                 name = name,
//                 value = value,
//                 valueShowType = showType,
//                 iconResId = attrOption.iconResId
//             };
//
//             attrUIArgs.battleAttrList.Add(uiData);
//         }
//
//         _uiPre.RefreshBattleAttrUI(attrUIArgs);
//     }
//
//     void RefreshBattleSkillUI()
//     {
//         BattleSkillUIArgs skillUIArgs = new BattleSkillUIArgs();
//         skillUIArgs.battleSkillList = new List<BattleSkillUIData>();
//
//         var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();
//
//         for (int i = 0; i < skills.Count; i++)
//         {
//             if (0 == i)
//             {
//                 //第一个是普通攻击 不显示
//                 continue;
//             }
//
//             var skillInfo = skills[i];
//             BattleSkillUIData skill = new BattleSkillUIData()
//             {
//                 //skill.iconResId
//                 skillId = skillInfo.configId,
//                 maxCDTime = skillInfo.maxCDTime,
//             };
//
//             skillUIArgs.battleSkillList.Add(skill);
//         }
//
//
//         _uiPre.RefreshBattleSkillUI(skillUIArgs);
//     }
//
//     void RefreshBattleBuffUI()
//     {
//         //add 通过 update info 来走逻辑 ， 所以这个先空调用一下
//         _uiPre.RefreshBattleBuffUI(new BattleBuffUIArgs());
//     }
//
//     void RefreshBattleHeroInfoUI()
//     {
//         //player hero info
//         var playerList = BattleManager.Instance.GetAllPlayerList();
//         BattleHeroInfoUIArgs args = new BattleHeroInfoUIArgs();
//         args.battleHeroInfoUIDataList = new List<BattleHeroInfoUIData>();
//
//         foreach (var player in playerList)
//         {
//             if (player.ctrlHeroGuid <= 0)
//             {
//                 //电脑
//                 continue;
//             }
//
//             BattleHeroInfoUIData uiData = new BattleHeroInfoUIData();
//             uiData.heroGuid = player.ctrlHeroGuid;
//
//             var heroEntity = BattleEntityManager.Instance.FindEntity(player.ctrlHeroGuid);
//
//             if (null == heroEntity)
//             {
//                 Logx.LogWarning("the heroEntity is null : uiData.heroGuid : " + uiData.heroGuid);
//                 return;
//             }
//
//             uiData.heroConfigId = heroEntity.configId;
//
//             uiData.level = heroEntity.level;
//             uiData.currHealth = heroEntity.CurrHealth;
//             uiData.maxHealth = heroEntity.attr.maxHealth;
//             uiData.playerIndex = heroEntity.playerIndex;
//
//             args.battleHeroInfoUIDataList.Add(uiData);
//         }
//
//         args.battleHeroInfoUIDataList.Sort((a, b) =>
//         {
//             int myHeroGuid = BattleManager.Instance.GetLocalCtrlHerGuid();
//             if (b.heroGuid == myHeroGuid)
//             {
//                 return 1;
//             }
//
//             return -1;
//         });
//
//         args.battleConfigId = BattleManager.Instance.battleTableId;
//
//         _uiPre.RefreshHeroInfoUI(args);
//     }
//
//     void RefreshBattleStageInfoUI()
//     {
//         var battleTableId = BattleManager.Instance.battleTableId;
//
//         BattleStageInfoUIArgs args = new BattleStageInfoUIArgs();
//         args.battleConfigId = battleTableId;
//         this._uiPre.ShowStageInfoUI(args);
//     }
//
//     void RefreshBattleSingleHeroInfo(BattleEntity_Client entity, int fromEntityGuid)
//     {
//         BattleHeroInfoUIData info = new BattleHeroInfoUIData();
//
//
//         var entityConfig = Table.TableManager.Instance.GetById<EntityInfo>(entity.configId);
//         var isBoss = 1 == entityConfig.IsBoss;
//
//         if (0 == entity.playerIndex || isBoss)
//         {
//             // 只有自己和队友和 boss 才显示界面他的信息
//             info.heroGuid = entity.guid;
//             info.currHealth = entity.CurrHealth;
//             info.maxHealth = entity.MaxHealth;
//             info.heroConfigId = entity.configId;
//
//             _uiPre.RefreshSingleHeroInfo(info, fromEntityGuid);
//         }
//     }
//
//     void OnClickCloseBtn()
//     {
//         CtrlManager.Instance.Exit<BattleCtrlPre>();
//
//     }
//
//     void OnClickReadyStartBtn()
//     {
//         //var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
//         //battleNet.SendBattleReadyFinish(null);
//
//         BattleManager.Instance.MsgSender.Send_BattleReadyFinish();
//     }
//
//     void OnClickResultConfirmBtn()
//     {
//         CtrlManager.Instance.Exit<BattleCtrlPre>();
//         
//         if (Const.isLocalBattleTest)
//         {
//             GameMain.Instance.StartLocalBattle();
//             // CoroutineManager.Instance.StartCoroutine(ReStartBattleTest());
//         }
//     }
//
//     IEnumerator ReStartBattleTest()
//     {
//         yield return new WaitForSeconds(0.1f);
//         GameMain.Instance.StartLocalBattle();
//     }
//
//     void OnClickAttrBtn()
//     {
//         this._uiPre.OpenAttrUI();
//     }
//
//     void OnAllPlayerLoadFinish()
//     {
//         _uiPre.SetReadyShowState(true);
//
//         _uiPre.SetStateText("wait to battle start");
//
//         CtrlManager.Instance.GlobalCtrlPre.LoadingUIPre.Hide();
//     }
//
//     void OnPlayerReadyState(int uid, bool isReady)
//     {
//         var myUid = (int)GameDataManager.Instance.UserStore.Uid;
//
//         if (myUid == uid)
//         {
//             _uiPre.SetReadyBtnShowState(isReady);
//         }
//     }
//
//     void OnBattleStart()
//     {
//         _uiPre.SetReadyShowState(false);
//         _uiPre.SetStateText("OnBattleStart");
//     }
//
//
//     //用户点击地面的时候(右键)
//     void OnPlayerClickGround(Vector3 clickPos)
//     {
//         //Logx.Log("OnPlayerClickGround : clickPos : " + clickPos);
//         var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
//
//         var myUid = GameDataManager.Instance.UserStore.Uid;
//
//         var guid = 1; //目前这个不用发 因为 1 个玩家只控制一个英雄实体 服务器已经记录 这里先保留 entity guid
//         //battleNet.SendMoveEntity(guid, clickPos);
//         BattleManager.Instance.MsgSender.Send_MoveEntity(guid, clickPos);
//     }
//
//     public bool TryToGetRayOnGroundPos(out Vector3 pos)
//     {
//         pos = Vector3.zero;
//
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         //Debug.DrawRay(ray.origin, ray.direction, Color.red);
//         RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
//         if (hits.Length > 0)
//         {
//             for (int i = 0; i < hits.Length; i++)
//             {
//                 var currHit = hits[i];
//                 var tag = currHit.collider.tag;
//                 if (tag == "Ground")
//                 {
//                     //Logx.Log("hit ground : " + currHit.collider.gameObject.name);
//                     pos = currHit.point;
//                     return true;
//
//                     //this.OnPlayerClickGround(currHit.point);
//                 }
//             }
//         }
//
//         return false;
//     }
//
//
//     public bool TryToGetRayOnEntity(out List<int> entityGuidList)
//     {
//         entityGuidList = new List<int>();
//
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         //Debug.DrawRay(ray.origin, ray.direction, Color.red);
//         RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
//         if (hits.Length > 0)
//         {
//             for (int i = 0; i < hits.Length; i++)
//             {
//                 var currHit = hits[i];
//                 var currHitGo = currHit.collider.gameObject;
//
//                 // Logx.Log("currHitGo : " + currHitGo.name);
//                 var entity =
//                     BattleEntityManager.Instance.FindEntityByColliderInstanceId(currHitGo.GetInstanceID());
//                 if (entity != null)
//                 {
//                     entityGuidList.Add(entity.guid);
//                 }
//             }
//         }
//
//         return entityGuidList.Count > 0;
//     }
//
//
//     public void OnPlayPlotEnd(string plotName)
//     {
//         var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
//         battleNet.SendClientPlotEnd();
//     }
//
//     private Color enemyOutlineColor = new Color(1, 0.2f, 0.2f, 1);
//     private Color myOutlineColor = new Color(0.5f, 1.0f, 0.5f, 1);
//     private Color friendOutlineColor = new Color(0.2f, 0.6f, 1.0f, 1);
//
//     
//     
//     public override void OnUpdate(float timeDelta)
//     {
//         this._uiPre.Update(timeDelta);
//         this.skillDirectModule.Update(timeDelta);
//         skillTrackModule.Update(timeDelta);
//
//         var battleState = BattleManager.Instance.BattleState;
//         // if (battleState == BattleState.End)
//         // {
//         //     return;
//         // }
//
//         //这里逻辑应该再封装一层 input 
//
//         ////判断用户点击右键
//         //if (Input.GetMouseButtonDown(1))
//         //{
//         //    Vector3 resultPos;
//         //    if (TryToGetRayOnGroundPos(out resultPos))
//         //    {
//         //        this.OnPlayerClickGround(resultPos);
//         //    }
//         //}
//
//
//         //判断用户输入
//         var isSelectSkillState = skillDirectModule.GetSelectState();
//         var isMouseLeftButtonDown = Input.GetMouseButtonDown(0);
//         var isMouseRightButtonDown = Input.GetMouseButtonDown(1);
//
//         var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
//         var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
//         var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);
//         
//         if (isSelectSkillState)
//         {
//             //选择技能中
//             var index = willReleaserSkillIndex;
//             var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
//             var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);
//             var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
//            
//             if (isMouseLeftButtonDown)
//             {
//                 if (releaseTargetType == SkillReleaseTargeType.Point)
//                 {
//                     Vector3 resultPos;
//                     var isColliderGround = TryToGetRayOnGroundPos(out resultPos);
//                     //确定选择技能目标
//                     if (isColliderGround)
//                     {
//                         skillDirectModule.FinishSelect();
//
//                         int targetGuid = 0;
//                         Vector3 targetPos = resultPos;
//
//                         BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid,
//                             targetPos);
//                     }
//
//                     skillDirectModule.FinishSelect();
//                 }
//                 else if (releaseTargetType == SkillReleaseTargeType.Entity)
//                 {
//                     //-----------------------
//
//                     GameObject gameObject = null;
//                     List<int> entityGuidList;
//                     BattleEntity_Client battleEntity = null;
//                     if (TryToGetRayOnEntity(out entityGuidList))
//                     {
//                         //遍历寻找 效率低下 之后更改
//                         //只找一个
//                         battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
//                         // battleEntity =
//                         //     BattleEntityManager.Instance.FindEntityByColliderInstanceId(gameObject.GetInstanceID());
//                     }
//                     else
//                     {
//                         //没有目标 那么就选择最近一段距离的某个单位
//                         float dis = 10.0f;
//                         battleEntity = BattleEntityManager.Instance.FindNearestEntity(localEntity, dis);
//                     }
//
//                     if (battleEntity != null)
//                     {
//                         //Logx.Log("battle entity not null");
//                         var targetGuid = battleEntity.guid;
//
//                         var targetPos = Vector3.right;
//                         //先排除自己
//                         if (localEntity.collider.gameObject.GetInstanceID() !=
//                             battleEntity.collider.gameObject.GetInstanceID())
//                         {
//                             //battleNet.SendUseSkill(skillId, targetGuid, targetPos);
//                             BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid,
//                                 targetPos);
//                         }
//                     }
//
//
//                     skillDirectModule.FinishSelect();
//                 }
//             }
//             else if (isMouseRightButtonDown)
//             {
//                 //取消技能选择操作 
//                 skillDirectModule.FinishSelect();
//             }
//             else
//             {
//                 //技能目标选择中
//                 Vector3 resultPos;
//                 var isColliderGround = TryToGetRayOnGroundPos(out resultPos);
//                 skillDirectModule.UpdateMousePosition(resultPos);
//
//                 GameObject gameObject = null;
//                 List<int> entityGuidList;
//                 BattleEntity_Client battleEntity = null;
//                 if (TryToGetRayOnEntity(out entityGuidList))
//                 {
//                     battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
//
//                     var entityModelRootGo = battleEntity.gameObject; //.transform.parent.gameObject
//                     //判断当前鼠标是否检测到是敌人
//                     
//                     var relationType = BattleEntity_Client.GetRelation(localEntity,battleEntity);
//                     if (relationType == Battle_Client.EntityRelationType.Enemy)
//                     {
//                         OperateViewManager.Instance.cursorModule.SetCursor(CursorType.SelectAttack);
//                         OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo, enemyOutlineColor,
//                             true);
//                     }
//                     else
//                     {
//                         OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Select);
//                         if (relationType == Battle_Client.EntityRelationType.Me)
//                         {
//                            
//                             OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo, myOutlineColor,
//                                 true);
//                         }
//                         else
//                         {
//                             OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo,friendOutlineColor,
//                                 true);
//                         }
//
//                       
//                     }
//                 }
//                 else
//                 {
//                     OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Select);
//
//                     OperateViewManager.Instance.modelOutlineModule.CloseAllModelOutline();
//                 }
//             }
//         }
//         else
//         {
//             //无技能释放动作
//             if (isMouseLeftButtonDown)
//             {
//                 //仅仅是左键点击了某处
//             }
//             else if (isMouseRightButtonDown)
//             {
//                 //移动到某处
//                 Vector3 resultPos;
//                 if (TryToGetRayOnGroundPos(out resultPos))
//                 {
//                     this.OnPlayerClickGround(resultPos);
//                 }
//             }
//             else
//             {
//                 GameObject gameObject = null;
//
//
//                 List<int> entityGuidList;
//
//                 BattleEntity_Client battleEntity = null;
//                 if (TryToGetRayOnEntity(out entityGuidList))
//                 {
//                     //遍历寻找 效率低下 之后更改
//                     battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
//
//                     var entityModelRootGo = battleEntity.gameObject; //.transform.parent.gameObject
//                     //判断当前鼠标是否检测到是敌人
//                     var relationType = BattleEntity_Client.GetRelation(localEntity,battleEntity);
//                    
//                     if (relationType == Battle_Client.EntityRelationType.Enemy)
//                     {
//                         OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Attack);
//                         OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo, enemyOutlineColor,
//                             true);
//                     }
//                     else
//                     {
//                         
//                         OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Normal);
//
//                         if (relationType == Battle_Client.EntityRelationType.Me)
//                         {
//                             OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo, myOutlineColor,
//                                 true);
//                         }
//                         else
//                         {
//                             OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo, friendOutlineColor,
//                                 true);
//                         }
//                     }
//                 }
//                 else
//                 {
//                     OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Normal);
//
//                     OperateViewManager.Instance.modelOutlineModule.CloseAllModelOutline();
//                 }
//             }
//         }
//
//
//         if (Input.GetKeyDown(KeyCode.A))
//         {
//             this.OnUseSkill(0);
//         }
//
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//             this.OnUseSkill(1);
//         }
//
//         if (Input.GetKeyDown(KeyCode.W))
//         {
//             this.OnUseSkill(2);
//         }
//
//         if (Input.GetKeyDown(KeyCode.E))
//         {
//             this.OnUseSkill(3);
//         }
//
//         if (Input.GetKeyDown(KeyCode.R))
//         {
//             this.OnUseSkill(4);
//         }
//     }
//
//     public override void OnLateUpdate(float deltaTime)
//     {
//         //摄像机
//         UpdateCamera();
//     }
//
//
//     public Vector3 cameraPosOffset = new Vector3(0, 10, -3.2f);
//     //public Vector3 cameraForwardOffset = new Vector3(0, 10, -3.2f);
//     //public Quaternion cameraQuaternionOffset = new Quaternion(69.94f, -0.032f, -0.001f,1.0f);
//
//     public void UpdateCamera()
//     {
//         if (PlotManager.Instance.IsRunning())
//         {
//             return;
//         }
//
//         var heroObj = BattleManager.Instance.GetLocalCtrlHeroGameObject();
//         if (null == heroObj)
//         {
//             return;
//         }
//
//         var camera3D = CameraManager.Instance.GetCamera3D();
//         var heroPos = heroObj.transform.position + cameraPosOffset;
//
//         camera3D.SetPosition(heroPos);
//         //camera3D.SetForward(heroPos);
//         camera3D.SetRotation(this.cameraRotationOffset);
//     }
//
//     public BattleSkillInfo FindLocalHeroSkill(int skillId)
//     {
//         var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();
//         foreach (var skill in skills)
//         {
//             if (skill.configId == skillId)
//             {
//                 return skill;
//             }
//         }
//
//         return null;
//     }
//
//     public bool CheckLocalHeroSkillRelease(int skillId)
//     {
//         //检测 cd
//         var skill = FindLocalHeroSkill(skillId);
//         if (skill != null)
//         {
//             if (skill.currCDTime <= 0)
//             {
//                 return true;
//             }
//         }
//
//         //CtrlManager.Instance.globalCtrl.ShowTips("这个技能还不能释放");
//
//         var tips = "这个技能还不能释放";
//         _uiPre.ShowSkillTipText(tips);
//         return false;
//     }
//
//     public int willReleaserSkillIndex;
//
//     public void OnUseSkill(int index)
//     {
//         willReleaserSkillIndex = index;
//         int targetGuid = 0;
//         Vector3 targetPos = Vector3.zero;
//         var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
//         var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
//         var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);
//
//         var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
//         var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
//         var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);
//
//         //判断释放条件 现在本地检测一下
//         //普通攻击不提示
//         var isNormalAttack = 0 == index;
//         if (!isNormalAttack)
//         {
//             var isCanRelease = CheckLocalHeroSkillRelease(skillId);
//             if (!isCanRelease)
//             {
//                 return;
//             }
//         }
//
//         var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
//         if (releaseTargetType == SkillReleaseTargeType.Point)
//         {
//             this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
//         }
//         else if (releaseTargetType == SkillReleaseTargeType.Entity)
//         {
//             this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
//         }
//         else if (releaseTargetType == SkillReleaseTargeType.NoTarget)
//         {
//             BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
//         }
//
//         //Logx.Log("use skill : skillId : " + skillId + " targetGuid : " + targetGuid + " targetPos : " + targetPos);
//     }
//
//     //当创建了 entity 的时候
//     public void OnCreateEntity(BattleEntity_Client entity)
//     {
//         hpModule.RefreshEntityData(entity);
//
//         var entityConfig = Table.TableManager.Instance.GetById<EntityInfo>(entity.configId);
//         var isBoss = 1 == entityConfig.IsBoss;
//         if (isBoss)
//         {
//             this._uiPre.StartBossComingAni();
//             this._uiPre.StartBossLimitCountdown();
//         }
//
//         this.RefreshBattleSingleHeroInfo(entity, 0);
//     }
//
//
//     //当实体战斗数据改变了
//     public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
//     {
//         hpModule.RefreshEntityData(entity, fromEntityGuid);
//         this.RefreshBattleAttrUI();
//         this.RefreshBattleSingleHeroInfo(entity, fromEntityGuid);
//     }
//
//     //当技能信息改变了
//     public void OnSkillInfoUpdate(int entityGuid, BattleSkillInfo skillInfo)
//     {
//         //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);
//
//         var myEntityGuid = BattleManager.Instance.GetLocalCtrlHerGuid();
//
//         if (myEntityGuid == entityGuid)
//         {
//             this._uiPre.RefreshSkillInfo(skillInfo.configId, skillInfo.currCDTime);
//         }
//     }
//
//     //当 buff 信息改变了
//     public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
//     {
//         if (BattleManager.Instance.GetLocalPlayer().ctrlHeroGuid == buffInfo.targetEntityGuid)
//         {
//             //Debug.Log("zxy : OnBuffInfoUpdate : ");
//             BattleBuffUIData buff = new BattleBuffUIData();
//             buff.guid = buffInfo.guid;
//             buff.maxCDTime = buffInfo.maxCDTime;
//             buff.currCDTime = buffInfo.currCDTime;
//             buff.isRemove = buffInfo.isRemove;
//             //buff.iconResId = buffInfo.iconResId;
//             buff.configId = buffInfo.configId;
//             buff.stackCount = buffInfo.stackCount;
//
//             this._uiPre.RefreshBuffInfo(buff);
//         }
//     }
//
//     public void OnUIAttrOptionPointEnter(EntityAttrType type, Vector2 pos)
//     {
//         var attrOption = AttrInfoHelper.Instance.GetAttrInfo(type);
//         UIArgs args = new DescribeUIArgs()
//         {
//             name = attrOption.name,
//             content = attrOption.describe,
//             pos = pos + Vector2.right * 50,
//             iconResId = attrOption.iconResId
//         };
//         this._uiPre.ShowDescribeUI(args);
//     }
//
//     public void OnUIAttrOptionPointExit(EntityAttrType type)
//     {
//         this._uiPre.HideDescribeUI();
//     }
//
//     public void OnUISkillOptionPointEnter(int skillId, Vector2 pos)
//     {
//         var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);
//
//         var des = skillConfig.Describe;
//         UIArgs args = new DescribeUIArgs()
//         {
//             name = skillConfig.Name,
//             content = des,
//             pos = pos + Vector2.right * 50,
//             iconResId = skillConfig.IconResId
//         };
//         this._uiPre.ShowDescribeUI(args);
//     }
//
//     public void OnUISkillOptionPointExit(int skillId)
//     {
//         this._uiPre.HideDescribeUI();
//     }
//
//     public void OnUIBuffOptionPointEnter(int buffConfigId, Vector2 pos)
//     {
//         var buffConfig = Table.TableManager.Instance.GetById<Table.BuffEffect>(buffConfigId);
//
//         var des = buffConfig.Describe;
//         UIArgs args = new DescribeUIArgs()
//         {
//             name = buffConfig.Name,
//             content = des,
//             pos = pos + Vector2.right * 50,
//             iconResId = buffConfig.IconResId
//         };
//         this._uiPre.ShowDescribeUI(args);
//     }
//
//     public void OnUIBuffOptionPointExit(int OnUIBuffOptionPointEnter)
//     {
//         this._uiPre.HideDescribeUI();
//     }
//
//
//     public void OnEntityDestroy(BattleEntity_Client entity)
//     {
//         hpModule.DestroyEntityHp(entity);
//     }
//
//     public void OnEntityDead(BattleEntity_Client entity)
//     {
//         _uiPre.SetHpShowState(entity.guid, false);
//     }
//
//     public void OnOnBattleEnd(BattleResultDataArgs battleResultArgs)
//     {
//         var args = new BattleResultUIArgs()
//         {
//             isWin = battleResultArgs.isWin,
//             //reward
//         };
//         args.uiItem = new List<CommonItemUIArgs>();
//
//         foreach (var item in battleResultArgs.rewardDataList)
//         {
//             var _item = new CommonItemUIArgs()
//             {
//                 configId = item.configId,
//                 count = item.count
//             };
//             args.uiItem.Add(_item);
//         }
//
//         this._resultUIPre.Refresh(args);
//         this._resultUIPre.Show();
//
//         BattleEntityManager.Instance.OnBattleEnd();
//         skillTrackModule.OnBattleEnd();
//         BattleSkillEffect_Client_Manager.Instance.OnBattleEnd();
//     }
//
//     //entity 改变显隐的时候
//     public void OnEntityChangeShowState(BattleEntity_Client entity, bool isShow)
//     {
//         //找到血条也要显隐
//         _uiPre.SetHpShowState(entity.guid, isShow);
//     }
//
//     public void OnSkillTrackStart(TrackBean trackBean)
//     {
//         this.skillTrackModule.AddTrack(trackBean);
//     }
//
//     public void OnSkillTrackEnd(int entityGuid, int trackId)
//     {
//         this.skillTrackModule.DeleteTrack(entityGuid, trackId);
//     }
//
//     public override void OnInactive()
//     {
//         //EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
//         //EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);
//
//
//         _uiPre.Hide();
//
//         _uiPre.onReadyStartBtnClick -= OnClickReadyStartBtn;
//         _uiPre.onCloseBtnClick -= OnClickCloseBtn;
//         _uiPre.onAttrBtnClick -= OnClickAttrBtn;
//
//         _resultUIPre.onClickConfirmBtn -= OnClickResultConfirmBtn;
//     }
//
//     public override void OnExit()
//     {
//         AudioManager.Instance.StopBGM();
//
//         UIManager.Instance.UnloadUI<BattleUIPre>();
//         UIManager.Instance.UnloadUI<BattleResultUIPre>();
//         ResourceManager.Instance.ReturnObject(scenePath, this.sceneObj);
//         BattleEntityManager.Instance.ReleaseAllEntities();
//         BattleSkillEffect_Client_Manager.Instance.ReleaseAll();
//         skillDirectModule.Release();
//         skillTrackModule.Release();
//
//         EventDispatcher.RemoveListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
//         EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
//         EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
//             OnChangeEntityBattleData);
//         EventDispatcher.RemoveListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
//         EventDispatcher.RemoveListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
//         EventDispatcher.RemoveListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
//         EventDispatcher.RemoveListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
//         EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnEntityDead, OnEntityDead);
//         EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, this.OnEntityDestroy);
//         EventDispatcher.RemoveListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
//             this.OnEntityChangeShowState);
//         EventDispatcher.RemoveListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, this.OnOnBattleEnd);
//         EventDispatcher.RemoveListener<int, bool>(EventIDs.OnPlayerReadyState, this.OnPlayerReadyState);
//         EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
//         EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);
//         EventDispatcher.RemoveListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter,
//             OnUIAttrOptionPointEnter);
//         EventDispatcher.RemoveListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
//         EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, OnUISkillOptionPointEnter);
//         EventDispatcher.RemoveListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
//         EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
//         EventDispatcher.RemoveListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);
//     }
//
//     public enum SkillReleaseTargeType
//     {
//         NoTarget = 0,
//         Entity = 1,
//         Point = 2
//     }
// }