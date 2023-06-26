using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class DescribeUIArgs : UIArgs
{
    public int iconResId;
    public string name;
    public string content;

    public Vector2 pos;
}

public class DescribeUI
{
    public GameObject gameObject;
    public Transform transform;

    Image icon;
    Text nameText;
    Text contentText;

    DescribeUIArgs uiDataArgs;

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        icon = this.transform.Find("Layout/Up/iconBg/icon").GetComponent<Image>();
        nameText = this.transform.Find("Layout/Up/name_text").GetComponent<Text>();
        contentText = this.transform.Find("Layout/Down/content_text").GetComponent<Text>();

        //buffDataList = new List<BattleBuffUIData>();
        //this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(UIArgs args)
    {
        uiDataArgs = (DescribeUIArgs)args;

        this.RefreshInfo();

    }

    void RefreshInfo()
    {
        int iconResId = uiDataArgs.iconResId;
        string name = uiDataArgs.name;
        string content = uiDataArgs.content;
        //相对于 BattleUI 的位置
        var pos = uiDataArgs.pos;

        nameText.text = name;
        contentText.text = content;
        //icon

        this.transform.localPosition = pos;
    }

    public void Update(float deltaTime)
    {


    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
    }

    public void Release()
    {

    }

}
