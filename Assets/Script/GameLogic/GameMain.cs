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
using UnityEditor;


public class GameMain : MonoBehaviour
{
    public static GameMain Instance;

    public GameObject tempModelAsset;

    public Transform gameObjectRoot;
    
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

    public IEnumerator GameInit()
    {
        Logx.Log(LogxType.Game, "game init start ...");

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

        // UIManager.Instance.Init(uiRoot);

        var cameraRoot = transform.Find("CameraRoot");
        CameraManager.Instance.Init(cameraRoot);

        UICtrlManager.Instance.Init(uiRoot);
        
      

        if (Const.isUseAB)
        {
            AssetBundleManager.Instance.Init();
        }

        AssetManager.Instance.Init();
        LoadTaskManager.Instance.Init();


        BattleEntityManager.Instance.Init();
        BattleSkillEffect_Client_Manager.Instance.Init();
        PlotManager.Instance.Init();

        var audioRoot = transform.Find("AudioRoot");
        AudioManager.Instance.Init(audioRoot);

        // yield break;

        //读取表数据 这里可能换成异步操作
        TableManager.Instance.Init();
        yield return TableManager.Instance.LoadAllTableData();

        //全局 UI
        GlobalUICtrlMgr.Instance.Init();
        yield return UICtrlManager.Instance.LoadGlobalCtrlReq();
        
        GameDataManager.Instance.Init();
        // ServiceManager.Instance.Init();

        SceneLoadManager.Instance.Init();
        SceneCtrlManager.Instance.Init();

        //对象池
        var objPoolRoot = transform.Find("ObjectPoolRoot");
        ObjectPoolManager.Instance.Init(objPoolRoot);
        
        gameObjectRoot = transform.Find("GameObjectRoot");
        
        InitHelper();

        //全局 ctrl
        // yield return CtrlManager.Instance.EnterGlobalCtrl();

        OperateViewManager.Instance.Init();
        OperateViewManager.Instance.StartLoad();
        
        BattleManager.Instance.Init();

        Logx.Log(LogxType.Game, "game init finish");

        isLoadFinish = true;
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
        Logx.Log(LogxType.Game, "enter login logic");

         // UICtrlManager.Instance.Enter<LoginUICtrl>();
         
         
         SceneCtrlManager.Instance.Enter<LoginSceneCtrl>();
    }

    public void StartLocalBattle()
    {
        //TODO: 纯本地战斗 里面的英雄是配置的 结算也是本地的
        //int battleConfigId = 5900001;
        int battleConfigId = 5900010;

        Logx.Log(LogxType.Game, "create local battle test");

        BattleManager.Instance.CreatePureLocalBattle(battleConfigId);
    }

    private GameObject assss = null;

    IEnumerator sss()
    {
        yield return null;
        // while (true)
        // {
        //     yield return new WaitForSeconds(0.50f);
        //     CtrlManager.Instance.globalCtrl.ShowTips("tips 1");
        // }

        ResourceManager.Instance.GetObject<GameObject>(15000001, (aaa) => { assss = aaa; });
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
        // Logx.Log("StartGame !!!");
    }

    public int currBattleFrameNum = 0;
    List<GameObject> gggg = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        LoadTaskManager.Instance.Update(Time.deltaTime);
        ResourceManager.Instance.Update(Time.deltaTime);
        UICtrlManager.Instance.Update(Time.deltaTime);

        BattleManager.Instance.Update(Time.deltaTime);
        BattleEntityManager.Instance.Update(Time.deltaTime);
        BattleSkillEffect_Client_Manager.Instance.Update(Time.deltaTime);

        PlotManager.Instance.Update(Time.deltaTime);

        NetworkManager.Instance.Update();
        
        SceneLoadManager.Instance.Update();

        currBattleFrameNum = currBattleFrameNum + 1;


        // //test ===>
        //
        //
        // var path = "assets/buildres/prefabs/ui/loginui.prefab";
        //
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     // ResourceManager.Instance.ReturnObject(15000001,assss);
        //     // assss = null;
        //
        //     //start test
        //
        //     var uiRoot = transform.Find("Canvas");
        //
        //     AssetManager.Instance.Load<GameObject>(path, (asset) =>
        //     {
        //         var go = GameObject.Instantiate(asset, uiRoot, false);
        //     });
        //
        //     // AssetManager.Instance.Load<GameObject>(path, (asset) =>
        //     // {
        //     //     //var go = GameObject.Instantiate(asset, uiRoot, false);
        //     // });
        //     //
        //     // AssetManager.Instance.Load<GameObject>(path, (asset) =>
        //     // {
        //     //     //var go = GameObject.Instantiate(asset, uiRoot, false);
        //     // });
        //     //
        //     //end test
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     var uiRoot = transform.Find("Canvas");
        //     ResourceManager.Instance.GetObject<GameObject>(path, (go) =>
        //     {
        //         gggg.Insert(0,go);
        //         
        //         go.transform.SetParent(uiRoot,false);
        //     });
        // }
        //
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //
        //     if (gggg.Count > 0)
        //     {
        //         ResourceManager.Instance.ReturnObject<GameObject>(path,gggg[0]);
        //         gggg.RemoveAt(0);
        //     }
        // }
        //
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //     ResourceManager.Instance.Release();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     // Resources.UnloadUnusedAssets();
        //     // ResourceManager.Instance.Release();
        //
        //     AssetManager.Instance.UnloadAsset(path);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     // Resources.UnloadUnusedAssets();
        //     // ResourceManager.Instance.Release();
        //
        //     AssetManager.Instance.ReleaseUnusedAssets();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     // Resources.UnloadUnusedAssets();
        //     // ResourceManager.Instance.Release();
        //
        //     AssetBundleManager.Instance.ReleaseAssetBundles();
        // }
        //

        if (Input.GetKeyDown(KeyCode.T))
        {
            // GameSceneManager.Instance.Load("",()=);
        }


        // // <===
    }

    private void FixedUpdate()
    {
        BattleManager.Instance.FixedUpdate(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        UICtrlManager.Instance.LateUpdate(Time.deltaTime);
        
        BattleManager.Instance.LateUpdate(Time.deltaTime);
    }

    private void Release()
    {
        BattleManager.Instance.Release();
    }

    private void OnDestroy()
    {
    }
}