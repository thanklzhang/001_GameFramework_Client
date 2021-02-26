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

        UIManager.Instance.Init(uiRoot);


        AssetBundleManager.Instance.Init();
        AssetManager.Instance.Init();

        LoadTaskManager.Instance.Init();
        //// role 
        //var rolePath = "001_guiwuzhe";//Models/Role/
        //AssetResManager.Instance.Load(rolePath,(assetInfo)=>
        //{
        //    var obj = assetInfo.assetObj as GameObject;
        //    GameObject.Instantiate(obj);

        //},false);


        //AssetManager.Instance.Load("Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab", (assetReq) =>
        // {
        //     Logx.LogZxy("GameMain","load finish");
        //     var obj = assetReq.assetObj as GameObject;
        //     GameObject.Instantiate(obj, uiRoot);

        //     AssetManager.Instance.Release("Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab");

        // }, false);

        var abPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.ab";
        var assetPath = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
        AssetBundleManager.Instance.Load(abPath, (assetCache) =>
        {
            Logx.Logz("gameMain : load finish");
            var prefab = assetCache.assetBundle.LoadAsset<GameObject>(assetPath);
            var newObj = GameObject.Instantiate(prefab, uiRoot);
        }, false);


        //StartCoroutine(dd());
    }

    IEnumerator dd()
    {
        yield return new WaitForSeconds(1);
        //AssetManager.Instance.Release("Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab");

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
