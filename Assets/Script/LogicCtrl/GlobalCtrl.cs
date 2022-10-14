using GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//全局 ctrl 游戏进程中一直存在 不受正常的 ctrl 管理
public class GlobalCtrl : BaseCtrl
{
    TipsUI tipsUI;
    //TitleBarUI titleBarUI;
    public override void OnInit()
    {
        //this.isParallel = false;
    }

    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<TipsUI>(){selfFinishCallback = OnTipsUILoadFinish},
        });
    }

    public void OnTipsUILoadFinish(TipsUI tipsUI)
    {
        this.tipsUI = tipsUI;

    }


    public override void OnLoadFinish()
    {

    }

    public override void OnEnter(CtrlArgs args)
    {

    }

    public override void OnActive()
    {


    }

    public override void OnInactive()
    {
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        tipsUI.Update(deltaTime);
    }

    //------------------------------
    public void ShowTips(string tipStr)
    {
        tipsUI.Show();

        tipsUI.Refresh(new TipsUIArgs()
        {
            tipStr = tipStr
        });
    }


}
