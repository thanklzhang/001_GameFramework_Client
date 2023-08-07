using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
using Table;
using GameData;
using PlotDesigner.Runtime;
using Battle_Client;
using XLua;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance;

    public GameObject tempModelAsset;

    bool isLoadFinish;
    // public Texture2D selectCursor;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        Instance = this;
    }

    private void Start()
    {


        //this.StartToLogin();
    }


    //[RuntimeInitializeOnLoadMethod]
    //public static void Startup()
    //{
    //    var scene = SceneManager.GetActiveScene();
    //    var gameInitPrefab = Resources.Load("GameInit") as GameObject;
    //    var go = Instantiate(gameInitPrefab);
    //}

    public IEnumerator GameInit(Action finishCallback)
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

        if (Const.isUseAB)
        {
            AssetBundleManager.Instance.Init();
        }

        AssetManager.Instance.Init();
        LoadTaskManager.Instance.Init();


        BattleEntityManager.Instance.Init();
        BattleSkillEffectManager.Instance.Init();
        PlotManager.Instance.Init();

        var audioRoot = transform.Find("AudioRoot");
        AudioManager.Instance.Init(audioRoot);

        //读取表数据 这里可能换成异步操作
        TableManager.Instance.Init();
        yield return TableManager.Instance.LoadAllTableData();

        GameDataManager.Instance.Init();
        ServiceManager.Instance.Init();

        InitHelper();

        //全局 ctrl
        yield return CtrlManager.Instance.EnterGlobalCtrl();


        //lua start
        //LuaEnv luaEnv = new LuaEnv();
        //luaEnv.AddLoader(MyLoader);
        //luaEnv.DoString("require 'main.lua'");
        //

        OperateViewManager.Instance.Init();
        OperateViewManager.Instance.StartLoad();

        Logx.Log("!!!finish init game");


        ////test
        //PlotManager.Instance.Test();
        isLoadFinish = true;
        finishCallback?.Invoke();

    }

    public void InitHelper()
    {
        AttrInfoHelper.Instance.Init();
    }

    //test loader
    static byte[] MyLoader(ref string filePath)
    {
        //PC
        Debug.Log("Application.dataPath : " + Application.dataPath);
        var path = Application.dataPath.Replace("/Assets", "") + "/LuaProject/" + filePath + "";
        return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(path));
    }

    public void StartToLogin()
    {
        CtrlManager.Instance.Enter<LoginCtrl>();
    }

    public void StartLocalBattle()
    {
        //TODO: 纯本地战斗 里面的英雄是配置的 结算也是本地的
        //int battleConfigId = 5900001;
        int battleConfigId = 5900010;

        BattleManager.Instance.CreatePureLocalBattle(battleConfigId);
    }

    IEnumerator sss()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.50f);
            CtrlManager.Instance.globalCtrl.ShowTips("tips 1");
        }
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

    public int currBattleFrameNum = 0;

    // Update is called once per frame
    void Update()
    {

        LoadTaskManager.Instance.Update(Time.deltaTime);
        ResourceManager.Instance.Update(Time.deltaTime);
        CtrlManager.Instance.Update(Time.deltaTime);

        BattleManager.Instance.Update(Time.deltaTime);
        BattleEntityManager.Instance.Update(Time.deltaTime);
        BattleSkillEffectManager.Instance.Update(Time.deltaTime);

        PlotManager.Instance.Update(Time.deltaTime);

        NetworkManager.Instance.Update();

        currBattleFrameNum = currBattleFrameNum + 1;

    }

    private void FixedUpdate()
    {
        BattleManager.Instance.FixedUpdate(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        CtrlManager.Instance.LateUpdate(Time.deltaTime);
    }

    private void OnDestroy()
    {

    }

}
