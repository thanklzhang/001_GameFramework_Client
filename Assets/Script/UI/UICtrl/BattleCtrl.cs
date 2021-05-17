using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class BattleCtrl : BaseCtrl
{
    BattleUI ui;
    public override void OnInit()
    {
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<BattleUI>(){selfFinishCallback = OnUILoadFinish},
        });
    }

    public void OnUILoadFinish(BattleUI battleUI)
    {
        this.ui = battleUI;
    }

    public override void OnLoadFinish()
    {
        ui.onCloseBtnClick += () =>
        {
            CtrlManager.Instance.Exit<BattleCtrl>();
        };
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }


    public override void OnExit()
    {
        UIManager.Instance.ReleaseUI<BattleUI>();
    }

}
