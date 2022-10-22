using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EntityRelationType
{
    Self = 0,
    Friend = 1,
    Enemy = 2
}

public class HpUIData : UIArgs
{
    public int entityGuid;
    public float preCurrHp;
    public float nowCurrHp;
    public float maxHp;
    public GameObject entityObj;

    public EntityRelationType relationType;

}

public class HpUIShowObj
{
    HpUIData data;
    GameObject gameObject;
    Transform transform;

    public Transform bgRoot;
    public Transform hp;
    public Text valueText;


    BaseUI parentUI;
    RectTransform parentTranRect;
    EntityHpColorSelector colorSelector;
    public Image hpBg;
    public void Init(GameObject gameObject, BaseUI parentUI)
    {
        this.gameObject = gameObject;
        this.parentUI = parentUI;
        parentTranRect = parentUI.transform.GetComponent<RectTransform>();
        this.transform = this.gameObject.transform;

        bgRoot = this.transform.Find("bg");
        hp = this.transform.Find("bg/hp");
        hpBg = hp.GetComponent<Image>();
        valueText = this.transform.Find("bg/valueText").GetComponent<Text>();
        colorSelector = hp.GetComponent<EntityHpColorSelector>();

    }

    public void Refresh(HpUIData hpData)
    {
        this.data = hpData;

        var currHp = this.data.nowCurrHp;
        var maxHp = this.data.maxHp;
        var ratio = currHp / maxHp;

        var width = bgRoot.GetComponent<RectTransform>().rect.width;
        var currLen = width * ratio;

        var rect = hp.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(currLen, rect.sizeDelta.y);

        valueText.text = "" + currHp + "/" + maxHp;

        //背景颜色和字体颜色
        var relationType = this.data.relationType;

        if (relationType == EntityRelationType.Self)
        {
            hpBg.color = this.colorSelector.selfBgColor;
            valueText.color = this.colorSelector.selfTextColor;

        }
        else if (relationType == EntityRelationType.Enemy)
        {
            hpBg.color = this.colorSelector.enemyBgColor;
            valueText.color = this.colorSelector.enemyTextColor;
        }
        else if (relationType == EntityRelationType.Friend)
        {
            hpBg.color = this.colorSelector.friendBgColor;
            valueText.color = this.colorSelector.friendTextColor;
        }

    }

    public void Update(float timeDelta)
    {
        var entityObj = this.data.entityObj;
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
        var screenPos = RectTransformUtility.WorldToScreenPoint(camera3D.camera, entityObj.transform.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTranRect, screenPos, cameraUI.camera, out uiPos);

        //这里可以换成实体上的血条挂点
        this.transform.localPosition = uiPos + Vector2.up * 100;
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    internal void SetShowState(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}



public class BattleUI : BaseUI
{
    public Action onCloseBtnClick;
    public Action onReadyStartBtnClick;

    Button closeBtn;
    Button readyStartBtn;

    public Text stateText;

    public Transform hpRoot;
    public GameObject hpTemp;
    //data
    Dictionary<int, HpUIShowObj> hpShowObjDic = new Dictionary<int, HpUIShowObj>();
    Dictionary<int, GameObject> poolObjs = new Dictionary<int, GameObject>();

    protected override void OnInit()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        readyStartBtn = this.transform.Find("readyStartBtn").GetComponent<Button>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();

        hpRoot = this.transform.Find("HpShow/root");
        hpTemp = hpRoot.Find("hpItem").gameObject;
        hpTemp.SetActive(false);

        closeBtn.onClick.AddListener(() =>
         {
             onCloseBtnClick?.Invoke();
         });
        readyStartBtn.onClick.AddListener(() =>
        {
            onReadyStartBtnClick?.Invoke();
        });

        poolObjs.Clear();
        for (int i = 0; i < hpRoot.childCount; i++)
        {
            var currObj = hpRoot.GetChild(i).gameObject;
            poolObjs.Add(currObj.GetInstanceID(), currObj);

        }


    }

    public void SetReadyBattleBtnShowState(bool isShow)
    {
        readyStartBtn.gameObject.SetActive(isShow);
    }

    public void SetStateText(string stateStr)
    {
        stateText.text = stateStr;
    }

    public void RefreshHpShow(UIArgs args)
    {
        HpUIData hpData = (HpUIData)args;

        HpUIShowObj showObj = null;
        if (hpShowObjDic.ContainsKey(hpData.entityGuid))
        {
            showObj = hpShowObjDic[hpData.entityGuid];
        }
        else
        {
            showObj = new HpUIShowObj();
            GameObject newObj = GameObject.Instantiate(hpTemp, this.hpRoot, false);
            newObj.SetActive(true);
            poolObjs.Add(newObj.GetInstanceID(), newObj);
            showObj.Init(newObj, this);
            hpShowObjDic.Add(hpData.entityGuid, showObj);
        }

        showObj.Refresh(hpData);
    }

    public void DestoryHpUI(int entityGuid)
    {
        if (hpShowObjDic.ContainsKey(entityGuid))
        {
            var hpShowObj = hpShowObjDic[entityGuid];
            hpShowObj.Destroy();
            hpShowObjDic.Remove(entityGuid);
        }
        else
        {
            Logx.LogWarning("BattleUI DestoryHpUI : the entityGuid is not found : " + entityGuid);
        }
    }

    public HpUIShowObj FindHpUI(int entityGuid)
    {
        HpUIShowObj showObj = null;
        if (hpShowObjDic.ContainsKey(entityGuid))
        {
            showObj = hpShowObjDic[entityGuid];
        }
        else
        {
            Logx.LogWarning("BattleUI DestoryHpUI : the entityGuid is not found : " + entityGuid);
        }
        return showObj;
    }

    public void SetHpShowState(int entityGuid, bool isShow)
    {
        var hpUI = FindHpUI(entityGuid);
        if (hpUI != null)
        {
            hpUI.SetShowState(isShow);
        }
    }

    protected override void OnUpdate(float timeDelta)
    {
        foreach (var item in hpShowObjDic)
        {
            item.Value.Update(timeDelta);
        }
    }

    protected override void OnRelease()
    {
        onCloseBtnClick = null;
        onReadyStartBtnClick = null;
    }

}
