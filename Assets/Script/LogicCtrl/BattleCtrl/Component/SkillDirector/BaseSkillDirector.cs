using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BaseSkillDirector
{
    public GameObject gameObject;
    public Transform transform;

    protected GameObject followEntity;
    protected Vector3 mousePositionOnGround;

    protected int resourceId;

    bool isEnable;
    bool isFinishLoad;

    public void Init(int skillDirectorType, string param)
    {
        this.OnInit(skillDirectorType, param);
    }

    public virtual void OnInit(int skillDirectorType, string param)
    {

    }

    public void Show(GameObject followGameObject)
    {
        if (isFinishLoad)
        {
            this.OnShow();

            isEnable = true;

            gameObject.SetActive(true);
        }
        else
        {
            this.followEntity = followGameObject;
            StartLoad();
        }
    }

    public virtual void OnShow()
    {

    }

    public void StartLoad()
    {
        isFinishLoad = false;
        if (this.resourceId > 0)
        {
            ResourceManager.Instance.GetObject<GameObject>(this.resourceId, OnLoadFinish);
        }
    }

    public bool IsEnable()
    {
        return this.isEnable;
    }

    public void OnLoadFinish(GameObject go)
    {
        isFinishLoad = true;
        this.gameObject = go;

        this.transform = this.gameObject.transform;

        this.transform.position = followEntity.transform.position;

        this.Show(this.followEntity);
    }



    internal void UpdateMousePosition(Vector3 resultPos)
    {
        if (!this.isEnable)
        {
            return;
        }
        mousePositionOnGround = resultPos;
    }

    public void Update(float deltaTime)
    {
        if (!this.isEnable)
        {
            return;
        }
        this.OnUpdate(deltaTime);

    }

    public virtual void OnUpdate(float deltaTime)
    {
        this.transform.position = followEntity.transform.position;

        ////跟随鼠标
        //var mousePosition = mousePositionOnGround;
        //Vector3 toMouseDir = mousePosition - this.transform.position;
        //toMouseDir = new Vector3(toMouseDir.x, 0, toMouseDir.z);
        //this.transform.forward = toMouseDir;
    }

    public void Hide()
    {
        isEnable = false;
        if (this.resourceId > 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Release()
    {
        if (this.resourceId > 0)
        {
            ResourceManager.Instance.ReturnObject(this.resourceId, this.gameObject);
        }
    }

}
