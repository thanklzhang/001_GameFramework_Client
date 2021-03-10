using GameModelData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopAUI : BaseUI
{
    Transform btnRoot;
    Button closeBtn;
    protected override void OnInit()
    {
        this.name = UIName.PopAUI;
        this.resPath = UIName.PopAUI;
        this.type = UIType.Pop;
    }

    protected override void OnLoadFinish()
    {
        btnRoot = this.transform.Find("bg/btnRoot");
        var nameList = UIManager.GetAllNameList();
        for (int i = 0; i < nameList.Count; i++)
        {
            var name = nameList[i];
            var child = btnRoot.GetChild(i);
            var btnText =  child.Find("Text").GetComponent<Text>();
            var btn = child.GetComponent<Button>();
            btnText.text = "" + name;
            btn.onClick.AddListener(()=>
            {
                UIManager.Instance.OpenUI(name);
            });
        }


        closeBtn = this.transform.Find("bg/closeBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(() =>
        {
            this.ActiveClose();
        });
    }

    protected override void OnOpen()
    {

    }
}
