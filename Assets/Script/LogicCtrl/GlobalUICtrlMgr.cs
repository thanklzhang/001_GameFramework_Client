using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//全局 ctrl 游戏进程中一直存在 不受正常的 ctrl 管理
public class GlobalUICtrlMgr : Singleton<GlobalUICtrlMgr>
{

    public Dictionary<Type,BaseUICtrl> ctrlDic = new  Dictionary<Type,BaseUICtrl>();
    
    public void Init()
    {
        
    }

    public IEnumerator LoadReq()
    {
        yield return LoadUIReq<TitleBarUICtrl>();
    }

    public IEnumerator LoadUIReq<T>() where T : BaseUICtrl , new ()
    {
        yield return null;

        //标题栏
        T ctrl = new T();
        ctrlDic.Add(ctrl.GetType(),ctrl);
        ctrl.Init();

        GameObject loadGo = null;
        var isFinishLoad = false;

        ctrl.StartLoad((go) =>
        {
            loadGo = go;
            
            UICtrlManager.Instance.SetParent(ctrl,go); 
            isFinishLoad = true;
        });

        while (!isFinishLoad)
        {
            yield return null;
        }

        if (null == loadGo)
        {
            yield break;
        }

        loadGo.gameObject.SetActive(false);
        ctrl.LoadFinish(loadGo);
    }

    public BaseUICtrl Get<T>() where T : BaseUICtrl
    {
        BaseUICtrl ctrl = null;
        var type = typeof(T);
        if (ctrlDic.TryGetValue(type, out ctrl))
        {
            return ctrl;
        }
        return null;
    }

    public void Open<T>(UICtrlArgs args) where T : BaseUICtrl
    {
        var ctrl = Get<T>();
        ctrl?.Open(args);
        ctrl?.Active();
    }

    public void Close<T>()where T : BaseUICtrl
    {
        var ctrl = Get<T>();
        ctrl.Inactive();
        ctrl?.Close();
        
    }

    public void Release()
    {
        
    }
}