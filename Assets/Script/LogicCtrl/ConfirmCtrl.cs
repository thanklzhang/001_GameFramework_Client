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
        //this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
        {
            new LoadUIRequest<ConfirmUI>(){selfFinishCallback = OnUILoadFinish},
        });

     
    }
    public void OnUILoadFinish(ConfirmUI confirmUI)
    {
        this.ui = confirmUI;
    }

    public override void OnLoadFinish()
    {
        ui.onCloseClickEvent += OnClickCloseBtn;
        ui.onClickYesBtn += OnClickYesBtn;
        ui.onClickNoBtn += OnClickNoBtn;
    }

    public void OnClickCloseBtn()
    {
        CtrlManager.Instance.Exit<ConfirmCtrl>();
        confirmArgs?.closeAction?.Invoke();
    }
    public void OnClickYesBtn()
    {
        CtrlManager.Instance.Exit<ConfirmCtrl>();
        confirmArgs?.yesAction?.Invoke();
    }
    public void OnClickNoBtn()
    {
        CtrlManager.Instance.Exit<ConfirmCtrl>();
        confirmArgs?.noAction?.Invoke();
    }

    public override void OnEnter(CtrlArgs args)
    {
        confirmArgs = (ConfirmCtrlArgs)args;

        ui.Show();
    }


    public override void OnExit()
    {
        ui.onCloseClickEvent -= OnClickCloseBtn;
        ui.onClickYesBtn -= OnClickYesBtn;
        ui.onClickNoBtn -= OnClickNoBtn;

        UIManager.Instance.ReleaseUI<ConfirmUI>();
    }

}
