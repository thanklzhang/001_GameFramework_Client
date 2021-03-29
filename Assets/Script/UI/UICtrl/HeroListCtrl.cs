using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class HeroListCtrl : BaseCtrl
{
    HeroListUI ui;


    public override void OnInit()
    {
        this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        loadRequest = ResourceManager.Instance.LoadObjects(new string[]
        {
            "Assets/BuildRes/Prefabs/UI/HeroListUI.prefab"
        });
        //UIManager.Instance.LoadUI<HeroListUI>();

    }

    public override void OnLoadFinish()
    {
        ui = UIManager.Instance.GetUICache<HeroListUI>();

        ui.onCloseBtnClick += () =>
        {
            CtrlManager.Instance.Exit<HeroListCtrl>();
        };
        ui.onGoInfoUIBtnClick += () =>
        {
            ConfirmCtrlArgs args = new ConfirmCtrlArgs();
            args.yesAction = () =>
            {
                CtrlManager.Instance.Enter<HeroInfoCtrl>();
            };

            CtrlManager.Instance.Enter<ConfirmCtrl>(args);
        };
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();
    }


    public override void OnExit()
    {
        UIManager.Instance.ReleaseUI<HeroListUI>();
    }

}
