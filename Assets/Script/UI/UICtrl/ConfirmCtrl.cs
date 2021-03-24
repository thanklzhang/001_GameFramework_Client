using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmCtrlArgs : CtrlArgs
{
    public Action yesAction;
    public Action noAction;
    public Action closeAction;
}

//英雄列表 ctrl
public class ConfirmCtrl : BaseCtrl
{
    ConfirmUI ui;
    ConfirmCtrlArgs confirmArgs;
    public override void OnInit()
    {
        this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        UIManager.Instance.LoadUI<ConfirmUI>((finishUI) =>
        {
            ui = finishUI;
            this.LoadFinish();
        });
    }

    public override void OnLoadFinish()
    {
        ui.onCloseClickEvent += () =>
        {
            CtrlManager.Instance.Exit<ConfirmCtrl>();
            confirmArgs?.closeAction?.Invoke();
        };
        ui.onClickYesBtn += () =>
        {
            CtrlManager.Instance.Exit<ConfirmCtrl>();
            confirmArgs?.yesAction?.Invoke();
        };
        ui.onClickNoBtn += () =>
        {
            CtrlManager.Instance.Exit<ConfirmCtrl>();
            confirmArgs?.noAction?.Invoke();
            
        };
    }

    public override void OnEnter(CtrlArgs args)
    {
        confirmArgs = (ConfirmCtrlArgs)args;

        ui.Show();
    }


    public override void OnExit()
    {
        UIManager.Instance.ReleaseUI<ConfirmUI>();
    }

}
