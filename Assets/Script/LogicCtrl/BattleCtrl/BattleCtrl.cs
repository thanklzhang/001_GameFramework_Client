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
    GameObject sceneObj;

    HpModule hpModule;
    public override void OnInit()
    {
        //this.isParallel = false;

        //生命周期好像有点不对
        EventDispatcher.AddListener<BattleEntity>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.AddListener<BattleEntity>(EventIDs.OnChangeEntityBattleData, OnChangeEntityBattleData);

        hpModule = new HpModule();

        

    }

    public override void OnStartLoad()
    {
        //scene
        var sceneResId = 15010001;
        var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(sceneResId);
        var scenePath = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;

        //fill load data
        var objsRequestList = new List<LoadObjectRequest>();
        //ui
        objsRequestList.Add(new LoadUIRequest<BattleUI>() { selfFinishCallback = OnUILoadFinish });
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

    public void OnSceneLoadFinish(HashSet<GameObject> gameObjects)
    {
        //先这样去 之后增加 scene 读取的接口
        sceneObj = gameObjects.ToList()[0];
        sceneObj.transform.position = new Vector3(0, 0, 0);

        var tempCameraTran = sceneObj.transform.Find("Camera");

        var camera3D = CameraManager.Instance.GetCamera3D();
        camera3D.SetPosition(tempCameraTran.position);
        camera3D.SetRotation(tempCameraTran.rotation);
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
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        battleNet.SendPlayerLoadProgress(1000);


        ui.SetStateText("ready all player load finish");
    }

    public override void OnActive()
    {
        ui.onCloseBtnClick += OnClickCloseBtn;
        ui.onReadyStartBtnClick += OnClickReadyStartBtn;

        EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);

        ui.Show();
        ui.SetReadyBattleBtnShowState(false);
    }

    void OnClickCloseBtn()
    {
        CtrlManager.Instance.Exit<BattleCtrl>();
    }

    void OnClickReadyStartBtn()
    {
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        battleNet.SendBattleReadyFinish(null);
    }

    void OnAllPlayerLoadFinish()
    {
        ui.SetReadyBattleBtnShowState(true);

        ui.SetStateText("ready battle start");

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

        var myUid = GameDataManager.Instance.UserGameDataStore.Uid;

        var guid = 1;//目前这个不用发 因为 1 个玩家只控制一个英雄实体 服务器已经记录 这里先保留 entity guid
        battleNet.SendMoveEntity(guid, clickPos);
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

    public override void OnUpdate(float timeDelta)
    {
        this.ui.Update(timeDelta);

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

    public void OnUseSkill(int index)
    {
        int targetGuid = 0;
        Vector3 targetPos = Vector3.zero;
        var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);

        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();

        var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);
        var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
        if (releaseTargetType == SkillReleaseTargeType.Point)
        {
            Vector3 resultPos;
            if (TryToGetRayOnGroundPos(out resultPos))
            {
                targetPos = resultPos;
                battleNet.SendUseSkill(skillId, targetGuid, targetPos);
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
                    var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
                    var localInstanceID = localCtrlHeroGameObject.GetInstanceID();

                    var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

                    //先排除自己
                    if (localEntity.collider.gameObject.GetInstanceID() != battleEntity.collider.gameObject.GetInstanceID())
                    {
                        battleNet.SendUseSkill(skillId, targetGuid, targetPos);
                    }

                }
            }
        }
        else if (releaseTargetType == SkillReleaseTargeType.NoTarget)
        {
            battleNet.SendUseSkill(skillId, targetGuid, targetPos);
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

    public override void OnInactive()
    {
        EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);

        ui.Hide();

        ui.onCloseBtnClick -= OnClickCloseBtn;
    }

    public override void OnExit()
    {
        EventDispatcher.RemoveListener<BattleEntity>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.RemoveListener<BattleEntity>(EventIDs.OnChangeEntityBattleData, OnChangeEntityBattleData);
    }

    public enum SkillReleaseTargeType
    {
        NoTarget = 0,
        Entity = 1,
        Point = 2

    }
}
