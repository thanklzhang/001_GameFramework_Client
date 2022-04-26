using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class TitleBarUIArgs : UIArgs
{
    public List<TitleOptionUIData> optionList;
}

public class TitleOptionUIData
{
    public int configId;
    public int count;
}

public class TitleOptionShowObj : BaseUIShowObj<TitleBarUI>
{
    Text nameText;
    Text countText;

    public TitleOptionUIData uiData;

    public override void OnInit()
    {
        nameText = this.transform.Find("name").GetComponent<Text>();
        countText = this.transform.Find("count").GetComponent<Text>();
    }

    public override void OnRefresh(object data, int index)
    {

        this.uiData = (TitleOptionUIData)data;

        var configId = this.uiData.configId;
        var itemTb = TableManager.Instance.GetById<Table.Item>(configId);
        nameText.text = itemTb.Name;
        countText.text = "" + this.uiData.count;

    }

    public override void OnRelease()
    {

    }

}


public class TitleBarUI : BaseUI
{
    public Transform optionRoot;
    List<TitleOptionUIData> optionDataList = new List<TitleOptionUIData>();
    List<TitleOptionShowObj> optionShowList = new List<TitleOptionShowObj>();

    protected override void OnInit()
    {
        this.optionRoot = this.transform.Find("root");
    }

    public override void Refresh(UIArgs args)
    {
        TitleBarUIArgs titleBarListArgs = (TitleBarUIArgs)args;

        this.optionDataList = titleBarListArgs.optionList;

        this.RefreshOptionList();
    }

    void RefreshOptionList()
    {
        UIListArgs<TitleOptionShowObj, TitleBarUI> args = new
            UIListArgs<TitleOptionShowObj, TitleBarUI>();
        args.dataList = optionDataList;
        args.showObjList = optionShowList;
        args.root = optionRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);

    }

    protected override void OnRelease()
    {

    }
}

