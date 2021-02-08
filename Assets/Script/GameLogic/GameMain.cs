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
        //// role 
        //var rolePath = "001_guiwuzhe";//Models/Role/
        //AssetResManager.Instance.Load(rolePath,(assetInfo)=>
        //{
        //    var obj = assetInfo.assetObj as GameObject;
        //    GameObject.Instantiate(obj);

        //},false);

        //var path = "Assets/BuildRes/Prefabs/UI/EquipmentListUI.prefab";
        //AssetManager.Instance.Load(path, (resInfo) =>
        // {
        //     Debug.Log("zxy : success to load asset : " + resInfo.path);
        //     var obj = resInfo.assetObj as GameObject;

        //     var newObj = GameObject.Instantiate(obj, UIManager.Instance.normalRoot, false);

        //     AssetManager.Instance.Relase(path);
        // }, true);

        //AssetBundleManager.Instance.Load("Assets/BuildRes/Prefabs/UI/EquipmentListUI" + ".ab", (info) =>
        //{

        //    Debug.Log("zxy : async : load ab test finish1");


        //}, false);
        //Debug.Log("zxy : ------------------");

        //AssetBundleManager.Instance.Load("Assets/BuildRes/Prefabs/UI/EquipmentListUI" + ".ab", (info) =>
        //{

        //    Debug.Log("zxy : sync : load ab test finish1");


        //}, true);
        //Debug.Log("zxy : ------------------");

      
        AssetBundleManager.Instance.Load("Assets/BuildRes/Prefabs/UI/EquipmentListUI" + ".ab", (info) =>
        {

            Debug.Log("zxy : async : load ab test finish2");
            AssetBundleManager.Instance.Release("Assets/BuildRes/Prefabs/UI/EquipmentListUI" + ".ab");


        }, false);
        Debug.Log("zxy : ------------------");
      




    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log("zxy : currFrame : " + Time.frameCount);
        AssetManager.Instance.Update(Time.deltaTime);
        AssetBundleManager.Instance.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {

    }

}
