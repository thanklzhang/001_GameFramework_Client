using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//战斗 ctrl
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
        ui.onCloseBtnClick += OnClickCloseBtn;
    }

    void OnClickCloseBtn()
    {
        CtrlManager.Instance.Exit<BattleCtrl>();
    }

    public override void OnEnter(CtrlArgs args)
    {
        ui.Show();

        //假设加载好了
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        battleNet.SendPlayerLoadProgress(1000);


    }

    void OnAllPlayerLoadFinish()
    {
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        battleNet.SendBattleReadyFinish(null);
    }

    void OnBattleStart()
    {

    }

    public override void OnExit()
    {
        ui.onCloseBtnClick -= OnClickCloseBtn;
        UIManager.Instance.ReleaseUI<BattleUI>();
    }

}
