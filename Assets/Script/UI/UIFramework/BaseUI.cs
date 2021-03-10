using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UILayout
{
    Base,
    Middle,
    Top
}

///// <summary>
///// UI 上下文 带有一些初始化信息和函数
///// </summary>
//public class UIContext
//{
//    //public string resPath;//资源路径
//    public GameObject rootObj;
//    public Action closeAction;
//    public void Close()
//    {
//        closeAction?.Invoke();
//    }

//}

public enum UIState
{
    //无
    Null,
    //最先开始的初始化时候
    Init,
    //加载资源中
    Loading,
    //加载完成
    LoadFinish,
    //第一次打开 可以在这里取数据等
    Open,
    //未激活
    Inactive,
    //激活
    Active,
    //关闭
    Close,
    //销毁
    Dispose
}



public class NormalUI : BaseUI
{

}

public class PopUI : BaseUI
{

}

public abstract class BaseUI
{
   
    public UIState state;
    public GameObject gameObject;
    public Transform transform;

    public string name;
    public string resPath;
    public UIType type;

    //event
    public Action loadFinishCallback;
    public Action openCallback;
    public Action activeCallback;

    internal void Init()
    {
        state = UIState.Init;
        this.OnInit();
    }
    
    internal void StartLoad()
    {
        state = UIState.Loading;
        this.OnStartLoad();
    }
    
    internal void LoadFinish(GameObject uiGameObject)
    {
        state = UIState.LoadFinish;
        this.gameObject = uiGameObject;
        this.transform = this.gameObject.transform;

        this.OnLoadFinish();
        loadFinishCallback?.Invoke();
    }

    public void Open()
    {
        state = UIState.Open;
        this.OnOpen();
        openCallback?.Invoke();
    }

    public void Active()
    {
        this.gameObject.SetActive(true);
        state = UIState.Active;
        Debug.Log("zxy : active : " + this.name);
        
        this.OnActive();

        activeCallback?.Invoke();
    }

    public void Refresh()
    {
        this.OnRefresh();
    }

    internal void Inactive()
    {
        this.gameObject.SetActive(false);

        state = UIState.Inactive;
        Debug.Log("zxy : inactive : " + this.name);
        this.OnInactive();
    }
    
    //供外部使用 自己被动关闭
    public void Close()
    {
        state = UIState.Close;
        this.OnClose();
    }

    public void Dispose()
    {
        state = UIState.Dispose;
        GameObject.Destroy(this.gameObject);//是否立即删除
        this.OnDispose();
    }

    
    protected virtual void OnInit() { }

    protected void OnStartLoad() { }

    protected virtual void OnLoadFinish() { }

    protected virtual void OnOpen() { }

    protected virtual void OnActive() { }

    protected virtual void OnRefresh(){}

    protected virtual void OnInactive() { }

    protected virtual void OnClose() { }

    protected virtual void OnDispose() { }

    
    public void ActiveClose()
    {
        UIManager.Instance.CloseUI(this.name);
    }

    //public UIContext context;

    //protected Transform root;

    //public UIResIds resId;


    //public abstract void Init();


    //public virtual void LoadFinish(UIContext context)
    //{
    //    this.context = context;
    //    root = context.rootObj.transform;
    //}


    ////public virtual void OnEnable() { }

    ////public virtual void OnDisable() { }

    //public virtual void Update(float deltaTime) { }

    //public virtual void Refresh() { }

    //public virtual void Show()
    //{
    //    this.root.gameObject.SetActive(true);
    //}

    //public virtual void Hide()
    //{
    //    this.root.gameObject.SetActive(false);
    //}

    //public virtual void Destroy()
    //{
    //    if (root)
    //    {
    //        GameObject.Destroy(root.gameObject);
    //    }
    //}

    //protected void Close()
    //{
    //    context.Close();
    //}


}
