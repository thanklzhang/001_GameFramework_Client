// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class UIManager : Singleton<UIManager>
// {
//     Transform uiRoot;
//     public Dictionary<UIShowLayer, Transform> layerRootDic;
//
//
//     public static Dictionary<UIShowLayer, string> uiShowLayerDic = new Dictionary<UIShowLayer, string>()
//     {
//         { UIShowLayer.Floor_0,"FloorUIRoot/layer" },
//         { UIShowLayer.Middle_0,"MiddleUIRoot/layer" },
//         { UIShowLayer.Top_0,"TopUIRoot/layer" },
//     };
//
//     internal void Init(Transform uiRoot)
//     {
//         this.uiRoot = uiRoot;
//
//         layerRootDic = new Dictionary<UIShowLayer, Transform>();
//         foreach (var kv in uiShowLayerDic)
//         {
//             var type = kv.Key;
//             var tranPath = kv.Value;
//
//             var layerRoot = this.uiRoot.Find(tranPath);
//             layerRootDic.Add(type, layerRoot);
//         }
//
//     }
//
//     public Dictionary<Type, BaseUI> uiCacheDic = new Dictionary<Type, BaseUI>();
//
//
//     internal void GetUICache<T>(Action<T> finishCallback = null) where T : BaseUI, new()
//     {
//         var type = typeof(T);
//         if (uiCacheDic.ContainsKey(type))
//         {
//             var ui = (T)uiCacheDic[type];
//             finishCallback?.Invoke(ui);
//         }
//         else
//         {
//             ////Logx.LogError("loadUI : the type is not found : " + type);
//             //return null;
//
//             var uiConfigInfo = UIConfigInfoDic.GetInfo<T>();
//             if (null == uiConfigInfo)
//             {
//                 Logx.LogError("GetUICache : the type is not found : " + typeof(T));
//                 return;
//             }
//
//             //var fullPath = Table.ResDefine.ResIdDic[uiConfigInfo.resId];
//             var resId = (int)uiConfigInfo.resId;
//             ResourceManager.Instance.GetObject<GameObject>(resId, (gameObject) =>
//             {
//                 var layerRoot = layerRootDic[uiConfigInfo.showLayer];
//                 gameObject.transform.SetParent(layerRoot, false);
//                 gameObject.transform.SetAsLastSibling();
//                 T t = new T();
//                 
//                 Logx.Log(LogxType.UI,"ui init : " + type.ToString());
//                 
//                 // t.Init(gameObject, uiConfigInfo.resId);
//                 uiCacheDic.Add(t.GetType(), t);
//                 finishCallback?.Invoke(t);
//             });
//         }
//     }
//
//     public void UnloadUI<T>()
//     {
//         var type = typeof(T);
//         if (uiCacheDic.ContainsKey(type))
//         {
//             var ui = uiCacheDic[type];
//             uiCacheDic.Remove(type);
//             
//             Logx.Log(LogxType.UI,"ui release : " + type.ToString());
//             
//             ui.Unload();
//             
//
//             var uiConfigInfo = UIConfigInfoDic.GetInfo<T>();
//             ResourceManager.Instance.ReturnObject<GameObject>((int)uiConfigInfo.resId, ui.gameObject);
//             //AssetManager.Instance.Release(uiConfigInfo.path);
//         }
//         else
//         {
//             //Logx.LogWarning("the ui is not exist in cache dic : " + type);
//         }
//     }
//
// }
//
// public enum UIShowLayer
// {
//     Floor_0 = 0,
//     Floor_1 = 1,
//     Floor_2 = 2,
//
//     Middle_0 = 10,
//     Middle_1 = 11,
//     Middle_2 = 12,
//
//     Top_0 = 20,
//     Top_1 = 21,
//     Top_2 = 22,
//
// }