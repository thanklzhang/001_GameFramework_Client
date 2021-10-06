//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

////英雄列表 ctrl
//public class TitleBarCtrl : BaseCtrl
//{
//    TitleBarUI ui;

//    public override void Init()
//    {

//    }
    
//    public override void Enter(CtrlArgs args)
//    {
//        //1 先获得数据然后再进界面 或者 2 先进界面在取数据 或者 3 两个同时
//        //这里目前采用 2 
//        ui = (TitleBarUI)UIManager.Instance.OpenUI(UIName.TitleBarUI);
//        ui.openCallback += OnUIOpen;
//        ui.onClickBackBtn += OnClickBackBtn;
//    }

   
//    public void OnUIOpen()
//    {
        
//    }

//    private void OnClickBackBtn()
//    {
           
//    }
//    //----------------------------------------------------------------

//    protected override void Exit()
//    {
//        ui.openCallback -= OnUIOpen;
//        ui.onClickBackBtn -= OnClickBackBtn;
//    }

//}
