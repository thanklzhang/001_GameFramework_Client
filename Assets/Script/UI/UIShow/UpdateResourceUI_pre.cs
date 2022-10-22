//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class UpdateResourceUI : Singleton<UpdateResourceUI>// : MonoSingleton<UpdateResourceUI>
//{
//    public GameObject uiObj;
    
//    public void Init(GameObject uiObj)
//    {
//        this.uiObj = uiObj;
//        //var uiRoot = GameObject.Find("Canvas").transform;

//    }

//    public void Show()
//    {
//        this.uiObj.SetActive(true);
//    }
    

//    public void Hide()
//    {
//        this.uiObj.SetActive(false);
//    }

//    public void DestroySelfObj()
//    {
//        GameObject.Destroy(this.uiObj);
//    }

//    internal void UpdateState(UpdateResourceInfo info)
//    {
//        //显示 进度 和 状态
//        //拷贝游戏资源到持久化目录中 : 正在检查游戏资源 -> 正在解压资源(xxx.ab):20% -> 解压资源完成
//        //更新游戏资源到持久化目录中 : 正在检查需要更新的游戏资源 -> 正在下载资源(xxxx.ab):15% -> 更新完成
//    }

//    internal void Finish()
//    {
//        DestroySelfObj();
//    }
//}
