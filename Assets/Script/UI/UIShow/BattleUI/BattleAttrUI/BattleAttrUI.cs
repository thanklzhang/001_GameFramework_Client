using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public EntityAttrType type;
    public string name;
    public int iconResId;
    public float value;
    public string describe;
    public AttrValueShowType valueShowType;
}

public class BattleAttrUIShowObj : BaseUIShowObj<BattleAttrUI>
{
    Text nameText;
    Text valueText;
    Texture icon;
    public BattleAttrUIData uiData;
    UIEventTrigger evetnTrigger;

    public override void OnInit()
    {
        nameText = this.transform.Find("name_text").GetComponent<Text>();
        valueText = this.transform.Find("value_text").GetComponent<Text>();
        evetnTrigger = nameText.GetComponent<UIEventTrigger>();

        evetnTrigger.OnPointEnterEvent += OnPointEnter;
        evetnTrigger.OnPointerExitEvent += OnPointExit;

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


    public void OnPointEnter(PointerEventData e)
    {
        //转换成点在 BattleUI 中的 localPosition

        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();

        var screenPos = e.position;

        Vector2 uiPos;
        var battleUIRect = parentObj.battleUI.gameObject.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);

        EventDispatcher.Broadcast<EntityAttrType,Vector2>(EventIDs.On_UIAttrOption_PointEnter, this.uiData.type, uiPos);

    }

    public void OnPointExit(PointerEventData e)
    {
        EventDispatcher.Broadcast<EntityAttrType>(EventIDs.On_UIAttrOption_PointExit, this.uiData.type);
    }


    public override void OnRelease()
    {
        evetnTrigger.OnPointEnterEvent -= OnPointEnter;
        evetnTrigger.OnPointerExitEvent -= OnPointExit;
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
    public BattleUI battleUI;
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
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
