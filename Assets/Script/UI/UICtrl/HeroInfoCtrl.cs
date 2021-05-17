using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class HeroInfoCtrl : BaseCtrl
{
    HeroInfoUI ui;
    public override void OnInit()
    {
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<HeroInfoUI>(){selfFinishCallback = OnUILoadFinish},
        });
    }

    public void OnUILoadFinish(HeroInfoUI heroInfoUI)
    {
        this.ui = heroInfoUI;
    }


    public override void OnLoadFinish()
    {
        ui.onCloseClickEvent += () =>
        {
            CtrlManager.Instance.Exit<HeroInfoCtrl>();
        };
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }


    public override void OnExit()
    {
        UIManager.Instance.ReleaseUI<HeroInfoUI>();
    }

}
