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

    string scenePath;
    public override void OnInit()
    {
        //this.isParallel = false;

        //生命周期好像有点不对
        EventDispatcher.AddListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
        EventDispatcher.AddListener<BattleEntity>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.AddListener<BattleEntity>(EventIDs.OnChangeEntityBattleData, OnChangeEntityBattleData);
        EventDispatcher.AddListener<BattleEntity>(EventIDs.OnEntityDestroy, OnEntityDestroy);
        EventDispatcher.AddListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, OnOnBattleEnd);
        EventDispatcher.AddListener<BattleEntity, bool>(EventIDs.OnEntityChangeShowState, this.OnEntityChangeShowState);

        EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);


        hpModule = new HpModule();

    }

    public override void OnStartLoad()
    {

        var battleTableId = BattleManager.Instance.battleTableId;
        var battleTb = Table.TableManager.Instance.GetById<Table.Battle>(battleTableId);
        var battleTriggerTb = Table.TableManager.Instance.GetById<Table.BattleTrigger>(battleTb.TriggerId);

        //scene
        var sceneResId = 15010001;
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
            mapCellView.SetMap(map);
            //mapCellView.SetRenderPath(new List<Pos>());
        }
    }

    public void OnEntityLoadFinish(BattleEntity viewEntity, GameObject obj)
    {
        viewEntity.OnLoadModelFinish(obj);
    }

    public override void OnLoadFinish()
    {
        Logx.Log("battle ctrl : OnLoadFinish");
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

        this.resultUI.onClickConfirmBtn += OnClickResultConfirmBtn;

        //EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        //EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);

        ui.Show();
        ui.SetReadyBattleBtnShowState(false);
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
        Logx.Log("OnPlayerClickGround : clickPos : " + clickPos);
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
                    Logx.Log("hit ground : " + currHit.collider.gameObject.name);
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
                    Logx.Log("hit entity : " + currHit.collider.gameObject.name);
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

        var battleState = BattleManager.Instance.BattleState;
        if (battleState == BattleState.End)
        {
            return;
        }

        //这里逻辑应该再封装一层 input 

        //判断用户点击右键
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 resultPos;
            if (TryToGetRayOnGroundPos(out resultPos))
            {
                this.OnPlayerClickGround(resultPos);
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

    public void OnUseSkill(int index)
    {
        int targetGuid = 0;
        Vector3 targetPos = Vector3.zero;
        var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);

        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();

        var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);

        var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
        var localInstanceID = localCtrlHeroGameObject.GetInstanceID();

        var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

        var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
        if (releaseTargetType == SkillReleaseTargeType.Point)
        {
            Vector3 resultPos;
            if (TryToGetRayOnGroundPos(out resultPos))
            {
                targetPos = resultPos;
                //battleNet.SendUseSkill(skillId, targetGuid, targetPos);
                BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
            }
        }
        else if (releaseTargetType == SkillReleaseTargeType.Entity)
        {
            GameObject gameObject = null;
            if (TryToGetRayOnEntity(out gameObject))
            {
                //遍历寻找 效率低下 之后更改
                var battleEntity = BattleEntityManager.Instance.FindEntityByColliderInstanceId(gameObject.GetInstanceID());
                if (battleEntity != null)
                {
                    Logx.Log("battle entity not null");
                    targetGuid = battleEntity.guid;


                    //先排除自己
                    if (localEntity.collider.gameObject.GetInstanceID() != battleEntity.collider.gameObject.GetInstanceID())
                    {
                        //battleNet.SendUseSkill(skillId, targetGuid, targetPos);
                        BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
                    }

                }
            }
        }
        else if (releaseTargetType == SkillReleaseTargeType.NoTarget)
        {
            //battleNet.SendUseSkill(skillId, targetGuid, targetPos);
            BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
        }


        //var myUid = GameDataManager.Instance.UserGameDataStore.Uid;

        Logx.Log("use skill : skillId : " + skillId + " targetGuid : " + targetGuid + " targetPos : " + targetPos);


    }

    //当创建了 entity 的时候
    public void OnCreateEntity(BattleEntity entity)
    {
        hpModule.RefreshEntityData(entity);
    }


    //当实体战斗数据改变了
    public void OnChangeEntityBattleData(BattleEntity entity)
    {
        hpModule.RefreshEntityData(entity);
    }

    public void OnEntityDestroy(BattleEntity entity)
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
    public void OnEntityChangeShowState(BattleEntity entity, bool isShow)
    {
        //找到血条也要显隐
        ui.SetHpShowState(entity.guid, isShow);
    }

    public override void OnInactive()
    {
        //EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        //EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);


        ui.Hide();

        ui.onReadyStartBtnClick -= OnClickReadyStartBtn;
        ui.onCloseBtnClick -= OnClickCloseBtn;

        resultUI.onClickConfirmBtn -= OnClickResultConfirmBtn;
    }

    public override void OnExit()
    {

        UIManager.Instance.ReleaseUI<BattleUI>();
        UIManager.Instance.ReleaseUI<BattleResultUI>();
        ResourceManager.Instance.ReturnObject(scenePath, this.sceneObj);
        BattleEntityManager.Instance.ReleaseAllEntities();
        BattleSkillEffectManager.Instance.ReleaseAll();

        EventDispatcher.RemoveListener<string>(EventIDs.OnPlotEnd, OnPlayPlotEnd);
        EventDispatcher.RemoveListener<BattleEntity>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.RemoveListener<BattleEntity>(EventIDs.OnChangeEntityBattleData, OnChangeEntityBattleData);
        EventDispatcher.RemoveListener<BattleEntity>(EventIDs.OnEntityDestroy, this.OnEntityDestroy);
        EventDispatcher.RemoveListener<BattleEntity, bool>(EventIDs.OnEntityChangeShowState, this.OnEntityChangeShowState);
        EventDispatcher.RemoveListener<BattleResultDataArgs>(EventIDs.OnBattleEnd, this.OnOnBattleEnd);
        EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);


    }

    public enum SkillReleaseTargeType
    {
        NoTarget = 0,
        Entity = 1,
        Point = 2

    }
}
