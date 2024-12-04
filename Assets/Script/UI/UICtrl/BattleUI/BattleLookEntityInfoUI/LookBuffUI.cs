﻿using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//查看选中实体的 buff 界面
public class LookBuffUI
{
    public GameObject gameObject;
    public Transform transform;

    // Button closeBtn;

    Transform buffListRoot;

    // List<BattleAttrUIData> attrDataList = new List<BattleAttrUIData>();
    List<BuffEffectInfo_Client> buffDataList = new List<BuffEffectInfo_Client>();

    
    // List<BattleAttrUIShowObj> attrShowObjList = new List<BattleAttrUIShowObj>();

    private List<BuffShowObj> buffShowObjList = new List<BuffShowObj>();
    
    public BattleUI BattleUI;

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.BattleUI = battleUIPre;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        //closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        buffListRoot = this.transform.Find("group");

        // closeBtn.onClick.AddListener(() =>
        // {
        //     this.Close();
        // });
        
        // EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
        //     OnChangeEntityBattleData);
        

    }

    // public void OnChangeEntityBattleData(BattleEntity_Client entity,int fromEntityGuid)
    // {
    //     RefreshAllUI(entity);
    // }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI(BattleEntity_Client entity)
    {
        RefreshDataList(entity);
        RefreshShowList();
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

    public BuffShowObj FindBuff(int buffId)
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
    
    public void RefreshDataList(BattleEntity_Client entity)
    {
        // BattleAttrUIArgs attrUIArgs = new BattleAttrUIArgs();
        // buffDataList = new List<AttrUIData>();

        // var attr = BattleManager.Instance.GetLocalCtrlHeroAttrs();
        buffDataList = BattleSkillEffectManager_Client.Instance.GetBuffListFromEntity(entity);
        // var attr = entity.attr;
        // List<EntityAttrType> types = new List<EntityAttrType>()
        // {
        //     EntityAttrType.Attack,
        //     EntityAttrType.Defence,
        //     EntityAttrType.MaxHealth,
        //     EntityAttrType.AttackSpeed,
        //     EntityAttrType.AttackRange,
        //     EntityAttrType.MoveSpeed,
        // };
        //
        // for (int i = 0; i < types.Count; i++)
        // {
        //     var attrType = types[i];
        //     float value = attr.GetValue(attrType);  
        //     AttrUIData uiData = new AttrUIData()
        //     {
        //          type = attrType,
        //          value = value
        //     };
        //  
        //     buffDataList.Add(uiData);
        // }
    }

    //用这个开始显示
    public void RefreshShowList()
    {
        buffShowObjList.Clear();
        
        buffDataList.Sort((a, b) =>
        {
            return a.guid.CompareTo(b.guid);
        });
        
        for (int i = 0; i < buffDataList.Count; i++)
        {
            var data = buffDataList[i];
            GameObject go = null;
            if (i < this.buffListRoot.childCount)
            {
                go = this.buffListRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(this.buffListRoot.GetChild(0).gameObject,
                    this.buffListRoot, false);
            }

            BuffShowObj cell = new BuffShowObj();
            cell.Init(go);
            cell.Show();
            cell.Refresh(data,i);

            buffShowObjList.Add(cell);
        }
        
        for (int i = buffDataList.Count; i < this.buffListRoot.childCount; i++)
        {
            var go = this.buffListRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var item in buffShowObjList)
        {
            item.Update(deltaTime);
        }
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
        
        // EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
        //     OnChangeEntityBattleData);
        
        
        // closeBtn.onClick.RemoveAllListeners();

        foreach (var item in buffShowObjList)
        {
            item.Release();
        }

        buffShowObjList = null;
        buffDataList = null;

    }
}