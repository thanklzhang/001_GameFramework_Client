//using GameModelData;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TitleBarUI : BaseUI
//{

//    Button closeBtn;
//    Text titleNameText;

//    public Action onClickBackBtn;

//    protected override void OnInit()
//    {
//        this.name = UIName.TitleBarUI;
//        this.resPath = "Assets/BuildRes/Prefabs/UI/TitleBarUI.prefab";
//        this.type = UIType.Title;
//    }

//    protected override void OnLoadFinish()
//    {
//        //titleNameText = this.transform.Find("title").GetComponent<Text>();

//        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
//        closeBtn.onClick.AddListener(() =>
//        {
//            //UIManager.Instance.CloseCurrNormalUI();
//            onClickBackBtn?.Invoke();
//        });


//    }

//    protected override void OnOpen()
//    {

//    }

//    protected override void OnActive()
//    {
//        //var currNormalUIName = UIManager.Instance.GetCurrActiveNormalUIName();

//        ////这里之后会拓展成 根据 name 或者 id 去找相应的界面配置 进而找到标题栏样式 资源图标 等
//        //titleNameText.text = currNormalUIName;
//    }
//}
