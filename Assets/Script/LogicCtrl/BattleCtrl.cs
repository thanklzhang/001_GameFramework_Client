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
    public override void OnInit()
    {
        //this.isParallel = false;
    }

    public override void OnStartLoad()
    {
        //scene
        var sceneResId = 15010001;
        var resTb = Table.TableManager.Instance.GetById<Table.ResourceConfig>(sceneResId);
        var scenePath = "Assets/BuildRes/" + resTb.Path + "/" + resTb.Name + "." + resTb.Ext;

        //fill load data
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<BattleUI>(){selfFinishCallback = OnUILoadFinish},
            new LoadGameObjectRequest(scenePath,1){selfFinishCallback = OnSceneLoadFinish}
        });
    }

    public void OnUILoadFinish(BattleUI battleUI)
    {
        this.ui = battleUI;
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

    public override void OnLoadFinish()
    {


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


    public override void OnUpdate(float timeDelta)
    {
        this.ui.Update(timeDelta);

        //这里逻辑应该再封装一层 input 

        //判断用户点击右键
        if (Input.GetMouseButtonDown(1))
        {
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
                        this.OnPlayerClickGround(currHit.point);
                    }
                }
            }
        }
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
        //UIManager.Instance.ReleaseUI<BattleUI>();
    }

}
