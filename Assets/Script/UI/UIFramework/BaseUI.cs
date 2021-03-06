﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArgs
{

}

public abstract class BaseUI
{
    public GameObject gameObject;
    public Transform transform;
    string path;
    public void Init(GameObject obj, string path)
    {
        this.path = path;
        gameObject = obj;
        transform = gameObject.transform;
        this.OnInit();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Refresh(UIArgs args)
    {
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Release()
    {
        //GameObject.Destroy(this.gameObject);
        //ResourceManager.Instance.ReturnGameObject(this.gameObject);
        this.OnRelease();
    }

    protected virtual void OnRelease()
    {
        
    }

    protected virtual void OnInit()
    {

    }
}
