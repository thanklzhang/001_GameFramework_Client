//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.U2D;
//using UnityEngine.UI;

//public class ResourceManager : Singleton<ResourceManager>
//{
//    bool isUseAB = true;

//    /// <summary>
//    /// 加载预设 注意这里是 asset 预设
//    /// </summary>
//    /// <param name="path"></param>
//    /// <param name="callback"></param>
//    /// <param name="isAsync"></param>
//    public void LoadPrefab(string path, Action<GameObject> callback, bool isAsync)
//    {
//        // AssetManager.Instance.Load<GameObject>(path, ( prefab) =>
//        // {
//        //     callback?.Invoke(prefab);

//        // }, isAsync);



//    }

//    // /// <summary>
//    // /// 加载预设
//    // /// </summary>
//    // /// <param name="path"></param>
//    // /// <param name="callback"></param>
//    // /// <param name="isAsync"></param>
//    // public void LoadPrefabByPath(string path, Action<bool, GameObject> callback, bool isAsync)
//    // {
//    //     ResLoadManager.Instance.Load<GameObject>(path, (isSuccess, prefab) =>
//    //     {
//    //         if (isSuccess)
//    //         {
//    //             callback?.Invoke(true, prefab);
//    //         }
//    //         else
//    //         {
//    //             callback?.Invoke(false, null);
//    //         }
//    //     }, isAsync);

//    // }

//    // //加载并且创建
//    // public void CreateGameObjectByPath(string path, Action<bool, GameObject> callback, bool isAsync)
//    // {
//    //     ResLoadManager.Instance.Load<GameObject>(path, (isSuccess, prefab) =>
//    //     {
//    //         Debug.Log("zxy : CreateGameObjectByPath isSuccess : " + isSuccess);
//    //         if (isSuccess)
//    //         {
//    //             GameObject obj = GameObject.Instantiate(prefab);
//    //             callback?.Invoke(true, obj);
//    //         }
//    //         else
//    //         {
//    //             callback?.Invoke(false, null);
//    //         }
//    //     }, isAsync);

//    // }


//    // //类似这种的多参数方法 可以改成多个函数 或者多个类
//    // public void LoadPrefabById(int resId, Action<bool, GameObject> callback, bool isAsync)
//    // {
//    //     var resConfig = Config.ConfigManager.Instance.GetById<Config.ResourcePath>(resId);
//    //     if (resConfig.type == "prefab")
//    //     {
//    //         this.LoadPrefabByPath(resConfig.path, callback, isAsync);
//    //     }
//    //     else
//    //     {
//    //         Debug.Log("the type of the resource is not prefab");
//    //     }

//    // }

//    // public void CreateGameObjectById(int resId, Action<bool, GameObject> callback, bool isAsync)
//    // {
//    //     var resConfig = Config.ConfigManager.Instance.GetById<Config.ResourcePath>(resId);
//    //     if (resConfig.type == "prefab")
//    //     {
//    //         this.CreateGameObjectByPath(resConfig.path + "/" + resConfig.name, callback, isAsync);
//    //     }
//    //     else
//    //     {
//    //         Debug.Log("the type of the resource is not prefab");
//    //     }

//    // }

//    // public void LoadTexture()
//    // {

//    // }

//    // public void LoadAtlas()
//    // {

//    // }

//    // public void LoadScene(string path, Action<bool, AsyncOperation> callback)
//    // {
//    //     SceneLoadManager.Instance.Load(path, (isSuccess, op) =>
//    //     {
//    //         callback?.Invoke(isSuccess, op);
//    //     }, true);
//    // }

//}
