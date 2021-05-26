using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BaseNode
{
    public int frame;

    public float startTime;
    public float lastTime;

  
    bool isStart = false;
    bool isEnd = false;

    public void Start()
    {
        Debug.Log("zxy : node start : frame : " + frame);
        isStart = true;
        OnStart();
    }

    public void Update(float deltaTime)
    {
        OnUpdate(deltaTime);
    }


    public void End()
    {
        this.isEnd = true;
        this.OnEnd();
    }


    public virtual void OnStart()
    {

    }

    public virtual void OnUpdate(float deltaTime)
    {

    }

    public virtual void OnEnd()
    {

    }

    public bool IsStart()
    {
        return this.isStart;
    }


    public bool IsEnd()
    {
        return this.isEnd;
    }


}