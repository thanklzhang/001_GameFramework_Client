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

public class GameMain : MonoBehaviour
{
    public static GameMain Instance;
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
        Debug.Log("game startup ... ");

        NetworkManager.Instance.Init();



        //TODO 此时热更完成

        //游戏中不依赖下载资源的初始化
        var uiRoot = transform.Find("Canvas");
        if (null == uiRoot)
        {
            Debug.LogError("the uiRoot with 'Canvas' name is null");
        }
        UIManager.Instance.Init(uiRoot);
        CtrlManager.Instance.Init();

        //读取表数据 这里可能换成异步操作
        TableManager.Instance.Init();
        TableManager.Instance.LoadAllTableData();
        //var heroTb = TableManager.Instance.GetById<Table.HeroInfo>(1000002);
        //Logx.Log("hero : " + heroTb.Name);
        //

       
        ////table test
        //var ss = TableManager.Instance.GetList<Table.CalculateEffect>();
        //for (int i = 0; i < ss.Count; i++)
        //{
        //    Logx.Log(ss[i].Describe);
        //}

        //AssetBundleManager.Instance.Init();
        //AssetManager.Instance.Init();
        //LoadTaskManager.Instance.Init();

        //GameDataManager.Instance.Init();

        //ServiceManager.Instance.Init();

        ////TODO 进入登录状态
        ////CtrlManager.Instance.Enter<LoginCtrl>();


        //NetworkManager.Instance.ConnectToLoginServer();

        //net test :




        ////登录后 模拟服务端推游戏数据
        //List<HeroData> heroList = new List<HeroData>()
        //{
        //    new HeroData(){  id = 1000002, level = 12},
        //    new HeroData(){  id = 1000003, level = 27},
        //};
        //var heroDataStore = GameDataManager.Instance.HeroGameDataStore;
        //heroDataStore.SetHeroDataList(heroList);

        ////数据发送成功后 进入大厅
        //CtrlManager.Instance.Enter<LobbyCtrl>();



    }

    IEnumerator dd()
    {

        //var assetPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
        //ResourceManager.Instance.GetObject<GameObject>(assetPath, (gameObject) =>
        //{
        //    gameObject.transform.SetParent(uiRoot, false);
        //    ResourceManager.Instance.ReturnObject(assetPath, gameObject);
        //});
        //ResourceManager.Instance.GetObject<GameObject>(assetPath, (gameObject) =>
        //{
        //    gameObject.transform.SetParent(uiRoot, false);
        //    ResourceManager.Instance.ReturnObject(assetPath, gameObject);
        //});
        //ResourceManager.Instance.GetObject<GameObject>(assetPath, (gameObject) =>
        //{
        //    gameObject.transform.SetParent(uiRoot, false);
        //    ResourceManager.Instance.ReturnObject(assetPath, gameObject);
        //});


        //GameFunction.Instance.EnterHeroList();

        //UIManager.Instance.OpenUI(UIName.EquipmentListUI);

        //var abPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.ab";
        //var assetPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
        ////AssetManager.Instance.Load(assetPath, (assetCache) =>
        ////{
        ////    var newObj = GameObject.Instantiate(assetCache, uiRoot);
        ////    //AssetManager.Instance.Release(assetPath);

        ////}, false);
        //AssetBundleManager.Instance.Load(abPath,(ab)=>
        //{

        //},false);

        //AssetManager.Instance.Load(assetPath, (assetCache) =>
        //{
        //    var newObj = GameObject.Instantiate(assetCache, uiRoot);
        //    //AssetManager.Instance.Release(assetPath);

        //}, false);

        //StartCoroutine(dd());


        yield return new WaitForSeconds(1);
        var assetPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
        var abPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.ab";
        //AssetManager.Instance.Release(assetPath);
        //AssetManager.Instance.Release(assetPath);

        //这里可以认为是 切场景的时候 所有引用为 0 的都会被释放掉 或者没隔多长时间的策略也可以
        //AssetBundleManager.Instance.TrueReleaseAB(abPath);

        AssetBundleManager.Instance.ReduceAssetBundleReference(abPath);

    }


    // Update is called once per frame
    void Update()
    {
        //LoadTaskManager.Instance.Update(Time.deltaTime);
        //ResourceManager.Instance.Update(Time.deltaTime);
        //CtrlManager.Instance.Update(Time.deltaTime);
        //NetworkManager.Instance.Update();
    }

    private void OnDestroy()
    {

    }

}
