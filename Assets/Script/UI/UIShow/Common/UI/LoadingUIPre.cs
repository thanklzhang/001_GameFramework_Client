// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Table;
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class LoadingUIPre : BaseUI
// {
//     public Transform bgTran;
//     public Transform progressTran;
//
//     public Text progressText;
//
//     private RectTransform bgRectTran;
//     private RectTransform progressRectTran;
//
//
//     protected override void OnInit()
//     {
//         bgTran = this.transform.Find("progressBar/back");
//         progressTran = this.transform.Find("progressBar/front");
//
//         progressText = this.transform.Find("progressBar/progressText").GetComponent<Text>();
//
//         bgRectTran = bgTran.GetComponent<RectTransform>();
//         progressRectTran = progressTran.GetComponent<RectTransform>();
//     }
//
//     public override void Refresh(UIArgs args)
//     {
//     }
//
//     void RefreshInfo()
//     {
//     }
//
//     public void SetProgress(float progress)
//     {
//         progressText.text = string.Format("{0:F2}%", progress * 100);
//
//         var pre = progressRectTran.sizeDelta;
//
//         progressRectTran.sizeDelta = new Vector2(bgRectTran.rect.width * progress, pre.y);
//     }
//
//     protected override void OnUnload()
//     {
//     }
// }