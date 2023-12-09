// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class ConfirmCtrlArgs : CtrlArgs
// {
//     public Action yesAction;
//     public Action noAction;
//     public Action closeAction;
// }
//
// //英雄列表 ctrl
// public class ConfirmCtrlPre : BaseCtrl_pre
// {
//     ConfirmUIPre _uiPre;
//     ConfirmCtrlArgs confirmArgs;
//     public override void OnInit()
//     {
//         //this.isParallel = false;
//     }
//     public override void OnStartLoad()
//     {
//         this.loadRequest = ResourceManager.Instance.LoadObjects(new List<LoadObjectRequest>()
//         {
//             new LoadUIRequest<ConfirmUIPre>(){selfFinishCallback = OnUILoadFinish},
//         });
//
//      
//     }
//     public void OnUILoadFinish(ConfirmUIPre confirmUIPre)
//     {
//         this._uiPre = confirmUIPre;
//     }
//
//     public override void OnLoadFinish()
//     {
//         _uiPre.onCloseClickEvent += OnClickCloseBtn;
//         _uiPre.onClickYesBtn += OnClickYesBtn;
//         _uiPre.onClickNoBtn += OnClickNoBtn;
//     }
//
//     public void OnClickCloseBtn()
//     {
//         CtrlManager.Instance.Exit<ConfirmCtrlPre>();
//         confirmArgs?.closeAction?.Invoke();
//     }
//     public void OnClickYesBtn()
//     {
//         CtrlManager.Instance.Exit<ConfirmCtrlPre>();
//         confirmArgs?.yesAction?.Invoke();
//     }
//     public void OnClickNoBtn()
//     {
//         CtrlManager.Instance.Exit<ConfirmCtrlPre>();
//         confirmArgs?.noAction?.Invoke();
//     }
//
//     public override void OnEnter(CtrlArgs args)
//     {
//         confirmArgs = (ConfirmCtrlArgs)args;
//
//         _uiPre.Show();
//     }
//
//
//     public override void OnExit()
//     {
//         _uiPre.onCloseClickEvent -= OnClickCloseBtn;
//         _uiPre.onClickYesBtn -= OnClickYesBtn;
//         _uiPre.onClickNoBtn -= OnClickNoBtn;
//
//         //UIManager.Instance.ReleaseUI<ConfirmUI>();
//     }
//
// }
