using GameModelData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : BaseUI
{

    Button closeBtn;
    Text titleNameText;
    protected override void OnInit()
    {
        this.name = UIName.TitleUI;
        this.resPath = "Assets/BuildRes/Prefabs/UI/TitleUI.prefab";
        this.type = UIType.Title;
    }

    protected override void OnLoadFinish()
    {
        //titleNameText = this.transform.Find("title").GetComponent<Text>();

        //closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        //closeBtn.onClick.AddListener(() =>
        //{
        //    UIManager.Instance.CloseCurrNormalUI();
        //});
    }

    protected override void OnOpen()
    {

    }

    protected override void OnActive()
    {
        //var currNormalUIName = UIManager.Instance.GetCurrActiveNormalUIName();

        ////这里之后会拓展成 根据 name 或者 id 去找相应的界面配置 进而找到标题栏样式 资源图标 等
        //titleNameText.text = currNormalUIName;
    }
}
