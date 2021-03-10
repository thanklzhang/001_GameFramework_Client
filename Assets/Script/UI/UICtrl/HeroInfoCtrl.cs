//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

////英雄列表 ctrl
//public class HeroInfoCtrl : BaseCtrl
//{
//    HeroInfoUI ui;
//    int currHeroId;
//    public override void Enter(CtrlArgs args)
//    {
//        HeroInfoCtrlArgs heroInfoArgs = (HeroInfoCtrlArgs)args;
//        this.currHeroId = heroInfoArgs.heroId;
//        ui = (HeroInfoUI)UIManager.Instance.OpenUI(UIName.HeroInfoUI);
//        ui.openCallback += OnUIOpen;
//    }

//    private void OnUIOpen()
//    {
//        ui.RefreshHeroInfoData(currHeroId);
//    }

//    protected override void Exit()
//    {
//        ui.openCallback += OnUIOpen;
//    }
    
//}
