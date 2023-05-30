using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuffUI
{
    public GameObject gameObject;
    public Transform transform;

    Transform buffListRoot;

    //Text skillTipText;
    //float skillTipShowTimer;
    //float skillTipMaxShowTime = 1.6f;
    public BattleUI battleUI;
    List<BattleBuffUIData> buffDataList = new List<BattleBuffUIData>();
    List<BattleBuffUIShowObj> buffShowObjList = new List<BattleBuffUIShowObj>();

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        buffListRoot = this.transform.Find("group");

        this.battleUI = battleUI;

        buffDataList = new List<BattleBuffUIData>();
        //this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(UIArgs args)
    {
        BattleBuffUIArgs uiDataArgs = (BattleBuffUIArgs)args;

        this.buffDataList = uiDataArgs.battleBuffList;
        if (null == this.buffDataList)
        {
            this.buffDataList = new List<BattleBuffUIData>();
        }

        this.RefreshBuffList();

    }

    void RefreshBuffList()
    {
        UIListArgs<BattleBuffUIShowObj, BattleBuffUI> args = new UIListArgs<BattleBuffUIShowObj, BattleBuffUI>();
        args.dataList = buffDataList;
        args.showObjList = buffShowObjList;
        args.root = buffListRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);
    }

    public void RemoveBuffFromDataList(int guid)
    {
        for (int i = this.buffDataList.Count - 1; i >= 0; --i)
        {
            var buffData = this.buffDataList[i];
            if (buffData.guid == guid)
            {
                this.buffDataList.RemoveAt(i);
                return;
            }
        }

    }

    public void UpdateBuffInfo(BattleBuffUIData buffInfo)
    {
        var buffShowObj = FindBuff(buffInfo.guid);
        if (buffShowObj != null)
        {
            if (buffInfo.isRemove)
            {
                RemoveBuffFromDataList(buffInfo.guid);
                this.RefreshBuffList();
            }
            else
            {
                buffShowObj.UpdateInfo(buffInfo);
            }

        }
        else
        {
            buffDataList.Add(buffInfo);
            this.RefreshBuffList();
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var item in buffShowObjList)
        {
            item.Update(deltaTime);
        }

    }

    public BattleBuffUIShowObj FindBuff(int buffId)
    {
        foreach (var buff in buffShowObjList)
        {
            if (buff.Guid == buffId)
            {
                return buff;
            }
        }
        return null;
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
        foreach (var item in buffShowObjList)
        {
            item.Release();
        }

        buffShowObjList = null;
        buffDataList = null;
    }

}
