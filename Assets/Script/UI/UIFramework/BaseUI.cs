using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;

public class UIArgs
{

}

public abstract class BaseUI
{
    public GameObject gameObject;
    public Transform transform;
    public ResIds resId;
    //string path;
    public void Init(GameObject obj, ResIds resId)
    {
        //this.path = path;
        this.resId = resId;
        gameObject = obj;
        transform = gameObject.transform;
        this.OnInit();
    }


    protected virtual void OnInit()
    {

    }

    public void Show()
    {
        Logx.Log(LogxType.UI,"show ui  : " + this.GetType());
        gameObject.SetActive(true);
    }

    public bool IsShow()
    {
        return this.gameObject.activeSelf;
    }

    public virtual void Refresh(UIArgs args)
    {

    }

    public void Update(float timeDelta)
    {
        this.OnUpdate(timeDelta);
    }

    public void Hide()
    {
        Logx.Log(LogxType.UI,"hide ui  : " + this.GetType());
        
        gameObject.SetActive(false);
    }

    public void Release()
    {
        //GameObject.Destroy(this.gameObject);
        //ResourceManager.Instance.ReturnGameObject(this.gameObject);
        
        Logx.Log(LogxType.UI,"release ui  : " + this.GetType());
        
        this.OnRelease();
    }

    protected virtual void OnUpdate(float timeDelta)
    {

    }

    protected virtual void OnRelease()
    {

    }

}
