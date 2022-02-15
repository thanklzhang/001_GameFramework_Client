using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListArgs<T, K>
{
    public IList dataList;
    public IList<T> showObjList;
    public Transform root;
    public K parentObj;
}

//关于 UI 的基础显示对象 供列表显示使用
public class BaseUIShowObj<K>
{
    public GameObject gameObject;
    public Transform transform;

    public K parentObj;

    public void Init(GameObject gameObject, K parentObj)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.parentObj = parentObj;
        this.OnInit();
    }

    public void Refresh(object obj, int index)
    {
        this.OnRefresh(obj, index);
    }

    public void Release()
    {
        this.OnRelease();
    }

    //////////////////////////////////////////

    public virtual void OnInit()
    {

    }

    public virtual void OnRefresh(object obj, int index)
    {

    }

    public virtual void OnRelease()
    {

    }
}
