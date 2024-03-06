using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//战斗血条 UI 管理
public class HpUIMgr
{
    public Transform transform;
    public GameObject gameObject;

    BattleUICtrl _battleUICtrl;
    
    public Transform hpRoot;
    public GameObject hpTemp;

    Dictionary<int, HpUIShowObj> hpShowObjDic = new Dictionary<int, HpUIShowObj>();

    public void Init(GameObject gameObject, BattleUICtrl ctrl)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this._battleUICtrl = ctrl;

        hpRoot = this.transform.Find("root");
        hpTemp = hpRoot.Find("hpItem").gameObject;
        hpTemp.SetActive(false);

        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, OnEntityDestroy);
        EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
        EventDispatcher.AddListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
            this.OnEntityChangeShowState);
    }


    public void OnCreateEntity(BattleEntity_Client entity)
    {
        RefreshHpShow(entity, 0);
    }

    public void OnEntityDestroy(BattleEntity_Client entity)
    {
        DestoryHpUI(entity);
    }

    public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
    {
        this.RefreshHpShow(entity, fromEntityGuid);
    }

    public void OnEntityChangeShowState(BattleEntity_Client entity, bool isShow)
    {
        this.SetHpShowState(entity, isShow);
    }


    public void RefreshHpShow(BattleEntity_Client entity, int fromEntityGuid)
    {
        HpUIShowObj showObj = null;
        if (hpShowObjDic.ContainsKey(entity.guid))
        {
            showObj = hpShowObjDic[entity.guid];
        }
        else
        {
            showObj = new HpUIShowObj();

            GameObject newObj = GameObject.Instantiate(hpTemp, this.hpRoot, false);
            newObj.SetActive(true);
            showObj.Init(newObj, entity,this);

            hpShowObjDic.Add(entity.guid, showObj);
        }

        showObj.Refresh(entity, fromEntityGuid);
    }

    public void DestoryHpUI(BattleEntity_Client entity)
    {
        var entityGuid = entity.guid;
        if (hpShowObjDic.ContainsKey(entityGuid))
        {
            var hpShowObj = hpShowObjDic[entityGuid];
            hpShowObj.Destroy();
            hpShowObjDic.Remove(entityGuid);
        }
        else
        {
            Logx.LogWarning(LogxType.UI, "BattleUI DestoryHpUI : the entityGuid is not found : " + entityGuid);
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
            Logx.LogWarning(LogxType.UI, "BattleUI DestoryHpUI : the entityGuid is not found : " + entityGuid);
        }

        return showObj;
    }

    public void SetHpShowState(BattleEntity_Client entity, bool isShow)
    {
        var entityGuid = entity.guid;
        var hpUI = FindHpUI(entityGuid);
        if (hpUI != null)
        {
            hpUI.SetShowState(isShow);
        }
    }

    public void ShowFloatWord(string str, GameObject gameObject, int style, Color color)
    {
        this._battleUICtrl.ShowFloatWord(str, gameObject, style, color);
    }


    public void Update(float timeDelta)
    {
        foreach (var item in hpShowObjDic)
        {
            item.Value.Update(timeDelta);
        }
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnCreateEntity, OnCreateEntity);
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnEntityDestroy, OnEntityDestroy);
        EventDispatcher.RemoveListener<BattleEntity_Client,int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);

        EventDispatcher.RemoveListener<BattleEntity_Client, bool>(EventIDs.OnEntityChangeShowState,
            this.OnEntityChangeShowState);
    }
}