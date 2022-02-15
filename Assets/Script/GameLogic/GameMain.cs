using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script.Combat;
using LitJson;
using Table;
using GameData;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance;

    public GameObject tempModelAsset;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Instance = this;

        this.GameInit();
    }


    void GameInit()
    {
        Logx.Log("!!!game startup ... ");

        Logx.Log("!!!start to init game");

        NetworkManager.Instance.Init();
        NetMsgManager.Instance.Init();
        NetHandlerManager.Instance.Init();

        //TODO 此时热更完成

        //游戏中不依赖下载资源的初始化
        var uiRoot = transform.Find("Canvas");
        if (null == uiRoot)
        {
            Debug.LogError("the uiRoot with 'Canvas' name is null");
        }

        UIManager.Instance.Init(uiRoot);

        var cameraRoot = transform.Find("CameraRoot");
        CameraManager.Instance.Init(cameraRoot);

        CtrlManager.Instance.Init();

        //读取表数据 这里可能换成异步操作
        TableManager.Instance.Init();
        TableManager.Instance.LoadAllTableData();

        AssetBundleManager.Instance.Init();
        AssetManager.Instance.Init();
        LoadTaskManager.Instance.Init();

        GameDataManager.Instance.Init();
        ServiceManager.Instance.Init();

        BattleEntityManager.Instance.Init();
        BattleSkillEffectManager.Instance.Init();


        Logx.Log("!!!finish init game");

        this.StartToLogin();

    }

    void StartToLogin()
    {
        CtrlManager.Instance.Enter<LoginCtrl>();



    }

    //void StartToEnterGame(NetProto.scCheckLogin result)
    //{
    //    var ip = result.Ip;
    //    var port = result.Port;
    //    var uid = result.Uid;
    //    Logx.Log("get uid : " + uid);
    //    NetworkManager.Instance.ConnectToGateServer(ip, port, (isSuccess) =>
    //     {
    //         if (isSuccess)
    //         {

    //             var loginHandler = NetHandlerManager.Instance.GetHandler<LoginNetHandler>();
    //             loginHandler.SendEnterGame(uid, 1, (enterResult) =>
    //             {
    //                 Logx.Log("StartToEnterGame : success");
    //                 this.StartGame();
    //             });
    //         }
    //         else
    //         {
    //             Logx.Log("StartToEnterGame : fail");
    //         }
    //     });
    //}

    //真正开始游戏
    void StartGame()
    {
        Logx.Log("StartGame !!!");
    }

    // Update is called once per frame
    void Update()
    {
        LoadTaskManager.Instance.Update(Time.deltaTime);
        ResourceManager.Instance.Update(Time.deltaTime);
        CtrlManager.Instance.Update(Time.deltaTime);
        BattleEntityManager.Instance.Update(Time.deltaTime);
        BattleSkillEffectManager.Instance.Update(Time.deltaTime);

        NetworkManager.Instance.Update();
        
    }

    private void OnDestroy()
    {

    }

}
