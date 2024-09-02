using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;

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
    public BattleUI BattleUIPre;
    List<BuffEffectInfo_Client> buffDataList = new List<BuffEffectInfo_Client>();
    List<BattleBuffUIShowObj> buffShowObjList = new List<BattleBuffUIShowObj>();

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        buffListRoot = this.transform.Find("group");

        this.BattleUIPre = battleUIPre;

        buffDataList = new List<BuffEffectInfo_Client>();
        //this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();
        
        EventDispatcher.AddListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh()
    {
        // BattleBuffUIArgs uiDataArgs = (BattleBuffUIArgs)args;

        // this.buffDataList = uiDataArgs.battleBuffList;
        if (null == this.buffDataList)
        {
            this.buffDataList = new List<BuffEffectInfo_Client>();
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

    public void UpdateBuffInfo(BuffEffectInfo_Client buffInfo)
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
            var eft = BattleSkillEffectManager_Client.Instance.FindSkillEffect(buffInfo.guid);
            if (eft != null)
            {
                //如果在 技能效果中找到了 那么应该是非 buff 的显示特效 需要删除 (这块逻辑待修改)
                BattleSkillEffectManager_Client.Instance.DestorySkillEffect(buffInfo.guid);
            }
            else
            {
                buffDataList.Add(buffInfo);
                this.RefreshBuffList();                
            }

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

    public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
    {
        
        if (BattleManager.Instance.GetLocalPlayer().ctrlHeroGuid == buffInfo.targetEntityGuid)
        {
            UpdateBuffInfo(buffInfo);
        }
    }

    public void Release()
    {
        foreach (var item in buffShowObjList)
        {
            item.Release();
        }

        buffShowObjList = null;
        buffDataList = null;
        
        EventDispatcher.RemoveListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);

    }

}
