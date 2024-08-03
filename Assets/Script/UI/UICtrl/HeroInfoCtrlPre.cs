// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// //英雄列表 ctrl
// public class HeroInfoCtrlPre : BaseCtrl_pre
// {
//     HeroInfoUIPre _uiPre;
//     public override void OnInit()
//     {
//         //this.isParallel = false;
//     }
//     public override void OnStartLoad()
//     {
//         this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
//         {
//             new LoadUIRequest<HeroInfoUIPre>(){selfFinishCallback = OnUILoadFinish},
//         });
//     }
//
//     public void OnUILoadFinish(HeroInfoUIPre heroInfoUIPre)
//     {
//         this._uiPre = heroInfoUIPre;
//     }
//
//
//     public override void OnLoadFinish()
//     {
//         _uiPre.onCloseClickEvent += () =>
//         {
//             CtrlManager.Instance.Exit<HeroInfoCtrlPre>();
//         };
//     }
//
//     public override void OnEnter(CtrlArgs args)
//     {
//        
//     }
//
//     public override void OnActive()
//     {
//         _uiPre.Show();
//     }
//
//     public override void OnInactive()
//     {
//         _uiPre.Hide();
//     }
//
//     public override void OnExit()
//     {
//         //UIManager.Instance.ReleaseUI<HeroInfoUI>();
//     }
//
// }
