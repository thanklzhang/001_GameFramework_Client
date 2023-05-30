using Battle_Client;
using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//战斗 ctrl
public class BattleCtrl : BaseCtrl
{
    BattleUI ui;
    BattleResultUI resultUI;
    GameObject sceneObj;

    HpModule hpModule;

    SkillDirectorModule skillDirectModule;
    SkillTrackModule skillTrackModule;

    string scenePath;
    public override void OnInit()
    {
        //this.isParallel = false;

        //生命周期好像有点不对
        EventDispatcher.AddListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData, OnChangeEntityBattleData);
        EventDispatcher.AddListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
        EventDispatcher.AddListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
        EventDispatcher.AddListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
        EventDispatcher.AddListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, OnEntityDestroy);
        EventDispatcher.AddListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, OnOnBattleEnd);
        EventDispatcher.AddListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState, this.OnEntityChangeShowState);

        EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);

        EventDispatcher.AddListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter, OnUIAttrOptionPointEnter);
        EventDispatcher.AddListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
        EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, OnUISkillOptionPointEnter);
        EventDispatcher.AddListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
        EventDispatcher.AddListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
        EventDispatcher.AddListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);




        hpModule = new HpModule();

        skillDirectModule = new SkillDirectorModule();
        skillTrackModule = new SkillTrackModule();


    }

    public override void OnStartLoad()
    {

        var battleTableId = BattleManager.Instance.battleTableId;
        var battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleTableId);
        var battleTriggerTb = Table.TableManager.Instance.GetById<Table.BattleTrigger>(battleTb.TriggerId);

        //scene
        var mapConfig = Table.TableManager.Instance.GetById<Table.BattleMap>(battleTb.MapId);
        var sceneResId = mapConfig.ResId;
        var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(sceneResId);
        scenePath = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;

        //fill load data
        var objsRequestList = new List<LoadObjectRequest>();
        //battle ui
        objsRequestList.Add(new LoadUIRequest<BattleUI>() { selfFinishCallback = OnUILoadFinish });
        //battle end ui
        objsRequestList.Add(new LoadUIRequest<BattleResultUI>() { selfFinishCallback = OnBattleResultUILoadFinish });
        //scene
        objsRequestList.Add(new LoadGameObjectRequest(scenePath, 1) { selfFinishCallback = OnSceneLoadFinish });
        //entity
        var entityLoadReqs = BattleEntityManager.Instance.MakeCurrBattleAllEntityLoadRequests(OnEntityLoadFinish);
        objsRequestList.AddRange(entityLoadReqs);
        //

        this.loadRequest = ResourceManager.Instance.LoadObjects(objsRequestList);

    }

    //初始话战斗相关资源
    public void LoadBattleInitRes()
    {

    }

    public void OnUILoadFinish(BattleUI battleUI)
    {
        this.ui = battleUI;

        hpModule.Init(ui);


    }

    public void OnBattleResultUILoadFinish(BattleResultUI battleResultUI)
    {
        this.resultUI = battleResultUI;
        this.resultUI.Hide();

    }


    public Quaternion cameraRotationOffset;
    MapCellView mapCellView;
    public void OnSceneLoadFinish(HashSet<GameObject> gameObjects)
    {
        //先这样去 之后增加 scene 读取的接口
        sceneObj = gameObjects.ToList()[0];
        sceneObj.transform.position = new Vector3(0, 0, 0);

        var tempCameraTran = sceneObj.transform.Find("Camera");

        var camera3D = CameraManager.Instance.GetCamera3D();
        camera3D.SetPosition(tempCameraTran.position);
        camera3D.SetRotation(tempCameraTran.rotation);
        cameraRotationOffset = tempCameraTran.rotation;

        //地图 cell 视图工具查看器(目前只限本地战斗)
        if (BattleManager.Instance.IsLocalBattle())
        {
            mapCellView = sceneObj.GetComponent<MapCellView>();
            var map = BattleManager.Instance.GetLocalBattleMap();
            mapCellView?.SetMap(map);
            //mapCellView.SetRenderPath(new List<Pos>());
        }
    }

    public void OnEntityLoadFinish(BattleEntity_Client viewEntity, GameObject obj)
    {
        viewEntity.OnLoadModelFinish(obj);
    }

    public override void OnLoadFinish()
    {
        Logx.Log("battle ctrl : battle res OnLoadFinish");

        skillDirectModule.Init(this);
        skillTrackModule.Init(this);
    }

    public override void OnEnter(CtrlArgs args)
    {
        //假设加载好了
        //var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        //battleNet.SendPlayerLoadProgress(1000);

        ui.SetStateText("self load finish , wait all player load finish");
        BattleManager.Instance.MsgSender.Send_PlayerLoadProgress(1000);



    }

    public override void OnActive()
    {
        CtrlManager.Instance.HideTitleBar();

        ui.onCloseBtnClick += OnClickCloseBtn;
        ui.onReadyStartBtnClick += OnClickReadyStartBtn;
        ui.onAttrBtnClick += OnClickAttrBtn;

        this.resultUI.onClickConfirmBtn += OnClickResultConfirmBtn;

        //EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        //EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);

        ui.Show();
        ui.SetReadyBattleBtnShowState(false);

        RefreshBattleAttrUI();

        RefreshBattleSkillUI();

        RefreshBattleBuffUI();

    }

    public void RefreshBattleAttrUI()
    {
        BattleAttrUIArgs attrUIArgs = new BattleAttrUIArgs();
        attrUIArgs.battleAttrList = new List<BattleAttrUIData>();

        var attr = BattleManager.Instance.GetLocalCtrlHeroAttrs();
        List<EntityAttrType> types = new List<EntityAttrType>()
        {
             EntityAttrType.Attack,
             EntityAttrType.Defence,
             EntityAttrType.MaxHealth,
             EntityAttrType.AttackSpeed,
             EntityAttrType.AttackRange,
             EntityAttrType.MoveSpeed,

        };
        ////之后配置
        //List<string> typeNameList = new List<string>()
        //{
        //     "攻击",
        //     "防御",
        //     "生命值",
        //     "攻击速度",
        //     "攻击距离",
        //     "移动速度",
        //};
        for (int i = 0; i < types.Count; i++)
        {
            var attrType = types[i];

            var attrOption = AttrInfoHelper.Instance.GetAttrInfo(attrType);

            string name = "" + attrOption.name;
            float value = attr.GetValue(attrType);
            AttrValueShowType showType = AttrValueShowType.Int;
            if (attrType == EntityAttrType.AttackSpeed)
            {
                showType = AttrValueShowType.Float_2;
            }
            else if (attrType == EntityAttrType.AttackRange)
            {
                showType = AttrValueShowType.Float_2;
            }
            else if (attrType == EntityAttrType.MoveSpeed)
            {
                showType = AttrValueShowType.Float_2;
            }

            BattleAttrUIData uiData = new BattleAttrUIData()
            {
                type = attrType,
                describe = attrOption.describe,
                name = name,
                value = value,
                valueShowType = showType
            };

            attrUIArgs.battleAttrList.Add(uiData);

        }
        ui.RefreshBattleAttrUI(attrUIArgs);
    }

    void RefreshBattleSkillUI()
    {
        BattleSkillUIArgs skillUIArgs = new BattleSkillUIArgs();
        skillUIArgs.battleSkillList = new List<BattleSkillUIData>();

        var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();

        for (int i = 0; i < skills.Count; i++)
        {
            if (0 == i)
            {
                //第一个是普通攻击 不显示
                continue;
            }
            var skillInfo = skills[i];
            BattleSkillUIData skill = new BattleSkillUIData()
            {
                //skill.iconResId
                skillId = skillInfo.configId,
                maxCDTime = skillInfo.maxCDTime,
                //iconResId = 0
            };

            skillUIArgs.battleSkillList.Add(skill);
        }


        ui.RefreshBattleSkillUI(skillUIArgs);

    }

    void RefreshBattleBuffUI()
    {
        //add 通过 update info 来走逻辑 ， 所以这个先空调用一下
        ui.RefreshBattleBuffUI(new BattleBuffUIArgs());
    }

    void OnClickCloseBtn()
    {
        CtrlManager.Instance.Exit<BattleCtrl>();
    }

    void OnClickReadyStartBtn()
    {
        //var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        //battleNet.SendBattleReadyFinish(null);

        BattleManager.Instance.MsgSender.Send_BattleReadyFinish();
    }

    void OnClickResultConfirmBtn()
    {
        CtrlManager.Instance.Exit<BattleCtrl>();
    }

    void OnClickAttrBtn()
    {
        this.ui.OpenAttrUI();
    }

    void OnAllPlayerLoadFinish()
    {
        ui.SetReadyBattleBtnShowState(true);

        ui.SetStateText("wait to battle start");

    }

    void OnBattleStart()
    {
        ui.SetReadyBattleBtnShowState(false);
        ui.SetStateText("OnBattleStart");
    }


    //用户点击地面的时候(右键)
    void OnPlayerClickGround(Vector3 clickPos)
    {
        //Logx.Log("OnPlayerClickGround : clickPos : " + clickPos);
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();

        var myUid = GameDataManager.Instance.UserStore.Uid;

        var guid = 1;//目前这个不用发 因为 1 个玩家只控制一个英雄实体 服务器已经记录 这里先保留 entity guid
        //battleNet.SendMoveEntity(guid, clickPos);
        BattleManager.Instance.MsgSender.Send_MoveEntity(guid, clickPos);

    }

    public bool TryToGetRayOnGroundPos(out Vector3 pos)
    {
        pos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                var currHit = hits[i];
                var tag = currHit.collider.tag;
                if (tag == "Ground")
                {
                    //Logx.Log("hit ground : " + currHit.collider.gameObject.name);
                    pos = currHit.point;
                    return true;

                    //this.OnPlayerClickGround(currHit.point);
                }
            }
        }
        return false;
    }


    public bool TryToGetRayOnEntity(out GameObject gameObject)
    {
        gameObject = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                var currHit = hits[i];
                var tag = currHit.collider.tag;
                if (tag == "EntityCollider")
                {
                    //Logx.Log("hit entity : " + currHit.collider.gameObject.name);
                    gameObject = currHit.collider.gameObject;
                    return true;
                }
            }
        }
        return false;
    }


    public void OnPlayPlotEnd(string plotName)
    {
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        battleNet.SendClientPlotEnd();

    }



    public override void OnUpdate(float timeDelta)
    {
        this.ui.Update(timeDelta);
        this.skillDirectModule.Update(timeDelta);
        skillTrackModule.Update(timeDelta);

        var battleState = BattleManager.Instance.BattleState;
        if (battleState == BattleState.End)
        {
            return;
        }

        //这里逻辑应该再封装一层 input 

        ////判断用户点击右键
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 resultPos;
        //    if (TryToGetRayOnGroundPos(out resultPos))
        //    {
        //        this.OnPlayerClickGround(resultPos);
        //    }
        //}


        //判断用户输入
        var isSelectSkillState = skillDirectModule.GetSelectState();
        var isMouseLeftButtonDown = Input.GetMouseButtonDown(0);
        var isMouseRightButtonDown = Input.GetMouseButtonDown(1);

        if (isSelectSkillState)
        {
            var index = willReleaserSkillIndex;
            var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
            var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);
            var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
            var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
            var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
            var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);
            if (isMouseLeftButtonDown)
            {
                if (releaseTargetType == SkillReleaseTargeType.Point)
                {
                    Vector3 resultPos;
                    var isColliderGround = TryToGetRayOnGroundPos(out resultPos);
                    //确定选择技能目标
                    if (isColliderGround)
                    {
                        skillDirectModule.FinishSelect();

                        int targetGuid = 0;
                        Vector3 targetPos = resultPos;

                        BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);

                    }
                    skillDirectModule.FinishSelect();

                }
                else if (releaseTargetType == SkillReleaseTargeType.Entity)
                {
                    //-----------------------

                    GameObject gameObject = null;
                    BattleEntity_Client battleEntity = null;
                    if (TryToGetRayOnEntity(out gameObject))
                    {

                        //遍历寻找 效率低下 之后更改
                        battleEntity = BattleEntityManager.Instance.FindEntityByColliderInstanceId(gameObject.GetInstanceID());
                    }
                    else
                    {
                        //没有目标 那么就选择最近一段距离的某个单位
                        float dis = 10.0f;
                        battleEntity = BattleEntityManager.Instance.FindNearestEntity(localEntity, dis);
                    }

                    if (battleEntity != null)
                    {
                        //Logx.Log("battle entity not null");
                        var targetGuid = battleEntity.guid;

                        var targetPos = Vector3.right;
                        //先排除自己
                        if (localEntity.collider.gameObject.GetInstanceID() != battleEntity.collider.gameObject.GetInstanceID())
                        {
                            //battleNet.SendUseSkill(skillId, targetGuid, targetPos);
                            BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
                        }
                    }


                    skillDirectModule.FinishSelect();
                }

            }
            else if (isMouseRightButtonDown)
            {
                //取消技能选择操作 
                skillDirectModule.FinishSelect();
            }
            else
            {
                //技能目标选择中
                Vector3 resultPos;
                var isColliderGround = TryToGetRayOnGroundPos(out resultPos);
                skillDirectModule.UpdateMousePosition(resultPos);

                GameObject gameObject = null;
                BattleEntity_Client battleEntity = null;
                if (TryToGetRayOnEntity(out gameObject))
                {
                    var entityModelRootGo = gameObject.transform.parent.gameObject;
                    //判断当前鼠标是否检测到是敌人
                    var isEnemy = true;
                    if (isEnemy)
                    {
                        OperateViewManager.Instance.cursorModule.SetCursor(CursorType.SelectAttack);
                    }
                    else
                    {
                        OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Select);
                    }

                    OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo, true);
                }
                else
                {
                    OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Select);

                    OperateViewManager.Instance.modelOutlineModule.CloseAllModelOutline();
                }

            }
        }
        else
        {
            if (isMouseLeftButtonDown)
            {
                //仅仅是左键点击了某处
            }
            else if (isMouseRightButtonDown)
            {
                //移动到某处
                Vector3 resultPos;
                if (TryToGetRayOnGroundPos(out resultPos))
                {
                    this.OnPlayerClickGround(resultPos);
                }
            }
            else
            {
                GameObject gameObject = null;
                BattleEntity_Client battleEntity = null;
                if (TryToGetRayOnEntity(out gameObject))
                {
                    //遍历寻找 效率低下 之后更改
                    battleEntity = BattleEntityManager.Instance.FindEntityByColliderInstanceId(gameObject.GetInstanceID());

                    var entityModelRootGo = gameObject.transform.parent.gameObject;
                    //判断当前鼠标是否检测到是敌人
                    var isEnemy = true;
                    if (isEnemy)
                    {
                        OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Attack);
                    }
                    else
                    {
                        OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Normal);
                    }

                    OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo, true);
                }
                else
                {
                    OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Normal);

                    OperateViewManager.Instance.modelOutlineModule.CloseAllModelOutline();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            this.OnUseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.OnUseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            this.OnUseSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            this.OnUseSkill(3);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.OnUseSkill(4);
        }

    }

    public override void OnLateUpdate(float deltaTime)
    {
        //摄像机
        UpdateCamera();
    }


    public Vector3 cameraPosOffset = new Vector3(0, 10, -3.2f);
    //public Vector3 cameraForwardOffset = new Vector3(0, 10, -3.2f);
    //public Quaternion cameraQuaternionOffset = new Quaternion(69.94f, -0.032f, -0.001f,1.0f);

    public void UpdateCamera()
    {
        if (PlotManager.Instance.IsRunning())
        {
            return;
        }
        var heroObj = BattleManager.Instance.GetLocalCtrlHeroGameObject();
        if (null == heroObj)
        {
            return;
        }
        var camera3D = CameraManager.Instance.GetCamera3D();
        var heroPos = heroObj.transform.position + cameraPosOffset;

        camera3D.SetPosition(heroPos);
        //camera3D.SetForward(heroPos);
        camera3D.SetRotation(this.cameraRotationOffset);
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

    public bool CheckLocalHeroSkillRelease(int skillId)
    {
        //检测 cd
        var skill = FindLocalHeroSkill(skillId);
        if (skill != null)
        {

            if (skill.currCDTime <= 0)
            {
                return true;
            }
        }

        //CtrlManager.Instance.globalCtrl.ShowTips("这个技能还不能释放");

        var tips = "这个技能还不能释放";
        ui.ShowSkillTipText(tips);
        return false;

    }

    public int willReleaserSkillIndex;
    public void OnUseSkill(int index)
    {
        willReleaserSkillIndex = index;
        int targetGuid = 0;
        Vector3 targetPos = Vector3.zero;
        var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);

        var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
        var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
        var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

        //判断释放条件 现在本地检测一下
        //普通攻击不提示
        var isNormalAttack = 0 == index;
        if (!isNormalAttack)
        {
            var isCanRelease = CheckLocalHeroSkillRelease(skillId);
            if (!isCanRelease)
            {
                return;
            }
        }

        var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
        if (releaseTargetType == SkillReleaseTargeType.Point)
        {

            this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);

        }
        else if (releaseTargetType == SkillReleaseTargeType.Entity)
        {

            this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);

        }
        else if (releaseTargetType == SkillReleaseTargeType.NoTarget)
        {
            BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
        }

        //Logx.Log("use skill : skillId : " + skillId + " targetGuid : " + targetGuid + " targetPos : " + targetPos);

    }

    //当创建了 entity 的时候
    public void OnCreateEntity(BattleEntity_Client entity)
    {
        hpModule.RefreshEntityData(entity);
    }


    //当实体战斗数据改变了
    public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
    {
        hpModule.RefreshEntityData(entity, fromEntityGuid);
        this.RefreshBattleAttrUI();
    }

    //当技能信息改变了
    public void OnSkillInfoUpdate(int entityGuid, BattleSkillInfo skillInfo)
    {
        //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);

        var myEntityGuid = BattleManager.Instance.GetLocalCtrlHerGuid();

        if (myEntityGuid == entityGuid)
        {
            this.ui.RefreshSkillInfo(skillInfo.configId, skillInfo.currCDTime);
        }

    }

    //当 buff 信息改变了
    public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
    {
        if (BattleManager.Instance.GetLocalPlayer().ctrlHeroGuid == buffInfo.targetEntityGuid)
        {
            //Debug.Log("zxy : OnBuffInfoUpdate : ");
            BattleBuffUIData buff = new BattleBuffUIData();
            buff.guid = buffInfo.guid;
            buff.maxCDTime = buffInfo.maxCDTime;
            buff.currCDTime = buffInfo.currCDTime;
            buff.isRemove = buffInfo.isRemove;
            //buff.iconResId = buffInfo.iconResId;
            buff.configId = buffInfo.configId;
            buff.stackCount = buffInfo.stackCount;

            this.ui.RefreshBuffInfo(buff);
        }

    }

    public void OnUIAttrOptionPointEnter(EntityAttrType type, Vector2 pos)
    {
        //这里应该通过属性 key 来进行寻找文本
        var attrOption = AttrInfoHelper.Instance.GetAttrInfo(type);
        UIArgs args = new DescribeUIArgs()
        {
            name = attrOption.name,
            content = attrOption.describe,
            pos = pos + Vector2.right * 50

        };
        this.ui.ShowDescribeUI(args);
    }

    public void OnUIAttrOptionPointExit(EntityAttrType type)
    {
        this.ui.HideDescribeUI();
    }

    public void OnUISkillOptionPointEnter(int skillId, Vector2 pos)
    {
        var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);

        var des = skillConfig.Describe;
        UIArgs args = new DescribeUIArgs()
        {
            name = skillConfig.Name,
            content = des,
            pos = pos + Vector2.right * 50

        };
        this.ui.ShowDescribeUI(args);
    }

    public void OnUISkillOptionPointExit(int skillId)
    {
        this.ui.HideDescribeUI();
    }

    public void OnUIBuffOptionPointEnter(int buffConfigId, Vector2 pos)
    {
        var skillConfig = Table.TableManager.Instance.GetById<Table.BuffEffect>(buffConfigId);

        var des = skillConfig.Describe;
        UIArgs args = new DescribeUIArgs()
        {
            name = skillConfig.Name,
            content = des,
            pos = pos + Vector2.right * 50

        };
        this.ui.ShowDescribeUI(args);
    }

    public void OnUIBuffOptionPointExit(int OnUIBuffOptionPointEnter)
    {
        this.ui.HideDescribeUI();
    }



    public void OnEntityDestroy(BattleEntity_Client entity)
    {
        hpModule.DestroyEntityHp(entity);
    }

    public void OnOnBattleEnd(BattleResultDataArgs battleResultArgs)
    {
        var args = new BattleResultUIArgs()
        {
            isWin = battleResultArgs.isWin,
            //reward
        };
        args.uiItem = new List<CommonItemUIArgs>();

        foreach (var item in battleResultArgs.rewardDataList)
        {
            var _item = new CommonItemUIArgs()
            {
                configId = item.configId,
                count = item.count
            };
            args.uiItem.Add(_item);
        }

        this.resultUI.Refresh(args);
        this.resultUI.Show();
    }

    //entity 改变显隐的时候
    public void OnEntityChangeShowState(BattleEntity_Client entity, bool isShow)
    {
        //找到血条也要显隐
        ui.SetHpShowState(entity.guid, isShow);
    }

    public void OnSkillTrackStart(TrackBean trackBean)
    {
        this.skillTrackModule.AddTrack(trackBean);
    }

    public void OnSkillTrackEnd(int entityGuid, int trackId)
    {
        this.skillTrackModule.DeleteTrack(entityGuid, trackId);
    }

    public override void OnInactive()
    {
        //EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        //EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);


        ui.Hide();

        ui.onReadyStartBtnClick -= OnClickReadyStartBtn;
        ui.onCloseBtnClick -= OnClickCloseBtn;
        ui.onAttrBtnClick -= OnClickAttrBtn;

        resultUI.onClickConfirmBtn -= OnClickResultConfirmBtn;
    }

    public override void OnExit()
    {

        UIManager.Instance.ReleaseUI<BattleUI>();
        UIManager.Instance.ReleaseUI<BattleResultUI>();
        ResourceManager.Instance.ReturnObject(scenePath, this.sceneObj);
        BattleEntityManager.Instance.ReleaseAllEntities();
        BattleSkillEffectManager.Instance.ReleaseAll();
        skillDirectModule.Release();
        skillTrackModule.Release();

        EventDispatcher.RemoveListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData, OnChangeEntityBattleData);
        EventDispatcher.RemoveListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
        EventDispatcher.RemoveListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
        EventDispatcher.RemoveListener<TrackBean>(EventIDs.OnSkillTrackStart, OnSkillTrackStart);
        EventDispatcher.RemoveListener<int, int>(EventIDs.OnSkillTrackEnd, OnSkillTrackEnd);
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, this.OnEntityDestroy);
        EventDispatcher.RemoveListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState, this.OnEntityChangeShowState);
        EventDispatcher.RemoveListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, this.OnOnBattleEnd);
        EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);
        EventDispatcher.RemoveListener<EntityAttrType, Vector2>(EventIDs.On_UIAttrOption_PointEnter, OnUIAttrOptionPointEnter);
        EventDispatcher.RemoveListener<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, OnUIAttrOptionPointExit);
        EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, OnUISkillOptionPointEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.On_UISkillOption_PointExit, OnUISkillOptionPointExit);
        EventDispatcher.RemoveListener<int, Vector2>(EventIDs.On_UIBuffOption_PointEnter, OnUIBuffOptionPointEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.On_UIBuffOption_PointExit, OnUIBuffOptionPointExit);


    }

    public enum SkillReleaseTargeType
    {
        NoTarget = 0,
        Entity = 1,
        Point = 2

    }
}
