

using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUIMgr
{
    BattleUI battleUI;

    public Transform transform;
    public GameObject gameObject;
    //hp
    public Transform hpRoot;
    public GameObject hpTemp;
    Dictionary<int, HpUIShowObj> hpShowObjDic = new Dictionary<int, HpUIShowObj>();
    Dictionary<int, GameObject> poolObjs = new Dictionary<int, GameObject>();

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;

        hpRoot = this.transform.Find("root");
        hpTemp = hpRoot.Find("hpItem").gameObject;
        hpTemp.SetActive(false);


        poolObjs.Clear();
        for (int i = 0; i < hpRoot.childCount; i++)
        {
            var currObj = hpRoot.GetChild(i).gameObject;
            poolObjs.Add(currObj.GetInstanceID(), currObj);

        }
    }

    public void Update(float timeDelta)
    {
        foreach (var item in hpShowObjDic)
        {
            item.Value.Update(timeDelta);
        }

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

    public void ShowFloatWord(string str, GameObject gameObject, int style, Color color)
    {
        this.battleUI.ShowFloatWord(str, gameObject, style, color);
    }
}
