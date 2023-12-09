using System;
using System.Collections;
using System.Collections.Generic;
using Battle.BattleTrigger.Runtime;
using Table;
using UnityEngine;


public class BaseUI
{
    public GameObject gameObject;
    public Transform transform;

    protected BaseUICtrl contextCtrl;
    
    public void Init(BaseUICtrl baseCtrl)
    {
        contextCtrl = baseCtrl;
        this.OnInit();
    }

    public void LoadFinish(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.OnLoadFinish();   
    }

    public void Enter()
    {
        this.OnEnter();
    }
    
    
    public void Active()
    {
        this.OnActive();
    }

    public void Inactive()
    {
        this.OnInactive();
    }

    public void Exit()
    {
        this.OnExit();
        this.gameObject = null;
    }
    

    //拓展------------------------------
    public virtual void OnInit()
    {
    }

    public virtual void OnLoadFinish()
    {

    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnActive()
    {
        
    }

    public virtual void OnInactive()
    {
        
    }

    public virtual void OnExit()
    {
        
    }


}
