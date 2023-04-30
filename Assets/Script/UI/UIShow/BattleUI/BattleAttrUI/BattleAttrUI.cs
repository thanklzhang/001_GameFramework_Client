using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;


public class BattleAttrUIArgs : UIArgs
{
    public List<BattleAttrUIData> battleAttrList;
}

public enum AttrValueShowType
{
    Int = 0,
    //浮点型 保留两位
    Float_2 = 1
}
public class BattleAttrUIData
{
    public string name;
    public int iconResId;
    public float value;
    public AttrValueShowType valueShowType;
}

public class BattleAttrUIShowObj : BaseUIShowObj<BattleAttrUI>
{
    Text nameText;
    Text valueText;
    Texture icon;
    public BattleAttrUIData uiData;

    public override void OnInit()
    {
        nameText = this.transform.Find("name_text").GetComponent<Text>();
        valueText = this.transform.Find("value_text").GetComponent<Text>();
        //icon
    }

    public override void OnRefresh(object data, int index)
    {
        this.uiData = (BattleAttrUIData)data;

        nameText.text = "" + this.uiData.name;

        valueText.text = "" + this.uiData.value;
        if (this.uiData.valueShowType == AttrValueShowType.Int)
        {
            valueText.text = "" + (int)this.uiData.value;
        }
        else if (this.uiData.valueShowType == AttrValueShowType.Float_2)
        {
            valueText.text = string.Format("{0:F}", this.uiData.value);
        }
    }

    public override void OnRelease()
    {

    }

}

public class BattleAttrUI
{
    public GameObject gameObject;
    public Transform transform;

    public Action onCloseBtnClick;

    Button closeBtn;

    Transform attrListRoot;

    List<BattleAttrUIData> attrDataList = new List<BattleAttrUIData>();
    List<BattleAttrUIShowObj> attrShowObjList = new List<BattleAttrUIShowObj>();

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        attrListRoot = this.transform.Find("group");

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
            this.Close();
        });
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(UIArgs args)
    {
        BattleAttrUIArgs uiDataArgs = (BattleAttrUIArgs)args;

        this.attrDataList = uiDataArgs.battleAttrList;

        this.RefreshAttrList();
    }

    void RefreshAttrList()
    {
        UIListArgs<BattleAttrUIShowObj, BattleAttrUI> args = new UIListArgs<BattleAttrUIShowObj, BattleAttrUI>();
        args.dataList = attrDataList;
        args.showObjList = attrShowObjList;
        args.root = attrListRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);
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
        closeBtn.onClick.RemoveAllListeners();
        onCloseBtnClick = null;

        foreach (var item in attrShowObjList)
        {
            item.Release();
        }

        attrShowObjList = null;
        attrDataList = null;
    }

}
