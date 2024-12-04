using Battle_Client;
using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;
using UnityEngine.UI;


//选中实体的信息界面
public class BattleLookEntityInfoUI
{
    public GameObject gameObject;
    public Transform transform;

    // Button closeBtn;

    Transform attrListRoot;

    List<AttrUIData> attrDataList = new List<AttrUIData>();
    private List<AttrShowObj> attrShowObjList = new List<AttrShowObj>();

    public BattleUI battleUI;

    private LookBuffUI lookBuffUI;
    
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        //closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        attrListRoot = this.transform.Find("lookAttrBar/group");

        //closeBtn.onClick.AddListener(() => { this.Close(); });

        EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);

        EventDispatcher.AddListener<BattleEntity_Client>(EventIDs.OnSelectEntity,
            OnSelectEntity);

        EventDispatcher.AddListener(EventIDs.OnCancelSelectEntity,
            OnCancelSelectEntity);
        
        EventDispatcher.AddListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);


        lookBuffUI = new LookBuffUI();
        lookBuffUI.Init(this.transform.Find("lookBuffBar").gameObject,this.battleUI);
    }

    public void OnChangeEntityBattleData(BattleEntity_Client entity, int fromEntityGuid)
    {
        if (this.gameObject.activeSelf)
        {
            if (currShowEntity != null && currShowEntity.guid == entity.guid)
            {
                RefreshAllUI();
            }
        }
    }

    public void OnSelectEntity(BattleEntity_Client entity)
    {
        currShowEntity = entity;
        this.Show();
        this.RefreshAllUI();
    }

    public void OnCancelSelectEntity()
    {
        this.Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    private BattleEntity_Client currShowEntity;

    public void RefreshAllUI()
    {
        //attr
        RefreshDataList(currShowEntity);
        RefreshShowList();
        
        //buff
        lookBuffUI.RefreshAllUI(currShowEntity);
    }

    public void RefreshDataList(BattleEntity_Client entity)
    {
        // BattleAttrUIArgs attrUIArgs = new BattleAttrUIArgs();
        attrDataList = new List<AttrUIData>();

        //var attr = BattleManager.Instance.GetLocalCtrlHeroAttrs();

        var attr = entity.attr;
        List<EntityAttrType> types = new List<EntityAttrType>()
        {
            EntityAttrType.Attack,
            EntityAttrType.Defence,
            EntityAttrType.MaxHealth,
            EntityAttrType.AttackSpeed,
            EntityAttrType.AttackRange,
            EntityAttrType.MoveSpeed,
        };

        for (int i = 0; i < types.Count; i++)
        {
            var attrType = types[i];
            float value = attr.GetValue(attrType);
            AttrUIData uiData = new AttrUIData()
            {
                type = attrType,
                value = value
            };
            attrDataList.Add(uiData);
        }
    }

    //用这个开始显示
    public void RefreshShowList()
    {
        attrShowObjList.Clear();

        for (int i = 0; i < attrDataList.Count; i++)
        {
            var data = attrDataList[i];
            GameObject go = null;
            if (i < this.attrListRoot.childCount)
            {
                go = this.attrListRoot.GetChild(i).gameObject;
            }
            else
            {
                go = GameObject.Instantiate(this.attrListRoot.GetChild(0).gameObject,
                    this.attrListRoot, false);
            }

            AttrShowObj cell = new AttrShowObj();
            cell.Init(go);
            cell.Show();
            cell.Refresh(data, i);

            attrShowObjList.Add(cell);
        }
        
        for (int i = attrDataList.Count; i < this.attrListRoot.childCount; i++)
        {
            var go = this.attrListRoot.GetChild(i).gameObject;
            go.SetActive(false);
        }
    }

    public void Update(float deltaTime)
    {
        this.lookBuffUI.Update(deltaTime);
    }
    
    public void OnBuffInfoUpdate(BuffEffectInfo_Client buffInfo)
    {
        if (null == this.currShowEntity)
        {
            return;
        }

        if (this.currShowEntity.guid == buffInfo.targetEntityGuid)
        {
            var buffShowObj = lookBuffUI.FindBuff(buffInfo.guid);
            if (buffShowObj != null)
            {
                if (buffInfo.isRemove)
                {
                    var eft = BattleSkillEffectManager_Client.Instance.FindSkillEffect(buffInfo.guid);
                    if (eft != null)
                    {
                        // (这块逻辑待修改)
                        BattleSkillEffectManager_Client.Instance.DestorySkillEffect(buffInfo.guid);
                    }

                    // this.lookBuffUI.RemoveBuffFromDataList(buffInfo.guid);
                }
            }

            
            this.lookBuffUI.RefreshAllUI(this.currShowEntity);
        }
    }
  

    public void Hide()
    {
        currShowEntity = null;
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
        EventDispatcher.RemoveListener<BattleEntity_Client>(EventIDs.OnSelectEntity,
            OnSelectEntity);
        EventDispatcher.RemoveListener(EventIDs.OnCancelSelectEntity,
            OnCancelSelectEntity);
        
        EventDispatcher.RemoveListener<BuffEffectInfo_Client>(EventIDs.OnBuffInfoUpdate, OnBuffInfoUpdate);


        // closeBtn.onClick.RemoveAllListeners();

        foreach (var item in attrShowObjList)
        {
            item.Release();
        }

        attrShowObjList = null;
        attrDataList = null;

        lookBuffUI.Release();
    }
}