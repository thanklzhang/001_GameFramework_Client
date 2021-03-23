using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script.Combat;

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

        //游戏中不依赖下载资源的初始化
        var uiRoot = transform.Find("Canvas");
        if (null == uiRoot)
        {
            Debug.LogError("the uiRoot with 'Canvas' name is null");
            return;
        }

        DataManager.Instance.Init();
        NetHandlerManager.Instance.Init();

        UIManager.Instance.Init(uiRoot);

        AssetBundleManager.Instance.Init();
        AssetManager.Instance.Init();
        LoadTaskManager.Instance.Init();

        CtrlManager.Instance.Init();

        CtrlManager.Instance.Enter<LobbyCtrl>();

        //GameFunction.Instance.EnterHeroList();

        //UIManager.Instance.OpenUI(UIName.EquipmentListUI);

        //var abPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.ab";
        //var assetPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
        //AssetManager.Instance.Load(assetPath, (assetCache) =>
        //{
        //    var newObj = GameObject.Instantiate(assetCache, uiRoot);
        //    //AssetManager.Instance.Release(assetPath);

        //}, false);

        //AssetManager.Instance.Load(assetPath, (assetCache) =>
        //{
        //    var newObj = GameObject.Instantiate(assetCache, uiRoot);
        //    //AssetManager.Instance.Release(assetPath);

        //}, false);

        //StartCoroutine(dd());
    }

    IEnumerator dd()
    {
        yield return new WaitForSeconds(1);
        var assetPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
        var abPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.ab";
        AssetManager.Instance.Release(assetPath);
        AssetManager.Instance.Release(assetPath);

        //这里可以认为是 切场景的时候 所有引用为 0 的都会被释放掉 或者没隔多长时间的策略也可以
        AssetBundleManager.Instance.TrueReleaseAB(abPath);

    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("zxy : currFrame : " + Time.frameCount);
        //AssetManager.Instance.Update(Time.deltaTime);
        //AssetBundleManager.Instance.Update(Time.deltaTime);
        LoadTaskManager.Instance.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {

    }

}
