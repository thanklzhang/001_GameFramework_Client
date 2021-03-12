using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Freeze()
    {

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Release()
    {
        GameObject.Destroy(this.gameObject);
    }

    protected virtual void OnInit()
    {

    }
}
