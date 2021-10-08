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


    }

    public override void OnEnter(CtrlArgs args)
    {
        //假设加载好了
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        battleNet.SendPlayerLoadProgress(1000);


        ui.SetStateText("ready all player load finish");
    }

    public override void OnActive()
    {
        ui.onCloseBtnClick += OnClickCloseBtn;
        ui.onReadyStartBtnClick += OnClickReadyStartBtn;

        EventDispatcher.AddListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.AddListener(EventIDs.OnBattleStart, this.OnBattleStart);

        ui.Show();
        ui.SetReadyBattleBtnShowState(false);
    }

    void OnClickCloseBtn()
    {
        CtrlManager.Instance.Exit<BattleCtrl>();
    }

    void OnClickReadyStartBtn()
    {
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        battleNet.SendBattleReadyFinish(null);
    }

    void OnAllPlayerLoadFinish()
    {
        ui.SetReadyBattleBtnShowState(true);

        ui.SetStateText("ready battle start");

    }

    void OnBattleStart()
    {
        ui.SetReadyBattleBtnShowState(false);
        ui.SetStateText("OnBattleStart");
    }

    public override void OnInactive()
    {
        EventDispatcher.RemoveListener(EventIDs.OnAllPlayerLoadFinish, this.OnAllPlayerLoadFinish);
        EventDispatcher.RemoveListener(EventIDs.OnBattleStart, this.OnBattleStart);

        ui.Hide();

        ui.onCloseBtnClick -= OnClickCloseBtn;
    }

    public override void OnExit()
    {



        UIManager.Instance.ReleaseUI<BattleUI>();
    }

}
