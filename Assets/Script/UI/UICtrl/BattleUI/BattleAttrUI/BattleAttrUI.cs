using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Table;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BattleAttrUI
{
    public GameObject gameObject;
    public Transform transform;

    public Action onCloseBtnClick;

    Button closeBtn;

    Transform attrListRoot;

    List<BattleAttrUIData> attrDataList = new List<BattleAttrUIData>();
    List<BattleAttrUIShowObj> attrShowObjList = new List<BattleAttrUIShowObj>();
    public BattleUI BattleUIPre;

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.BattleUIPre = battleUIPre;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        attrListRoot = this.transform.Find("group");

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
            this.Close();
        });
        
        EventDispatcher.AddListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
    }

    public void OnChangeEntityBattleData(BattleEntity_Client entity,int fromEntityGuid)
    {
        RefreshAllUI();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        RefreshDataList();
        RefreshShowList();
    }

    public void RefreshDataList()
    {
        // BattleAttrUIArgs attrUIArgs = new BattleAttrUIArgs();
        attrDataList = new List<BattleAttrUIData>();

        var attr = BattleManager.Instance.GetLocalCtrlHeroAttrs();
        List<EntityAttrType> types = new List<EntityAttrType>()
        {
            EntityAttrType.Attack,
            EntityAttrType.Defence,
            EntityAttrType.MaxHealth,
            EntityAttrType.AttackSpeed,
            EntityAttrType.AttackRange,
            EntityAttrType.MoveSpeed,
        };
        ////之后配置
        //List<string> typeNameList = new List<string>()
        //{
        //     "攻击",
        //     "防御",
        //     "生命值",
        //     "攻击速度",
        //     "攻击距离",
        //     "移动速度",
        //};
        for (int i = 0; i < types.Count; i++)
        {
            var attrType = types[i];

            var attrOption = AttrInfoHelper.Instance.GetAttrInfo(attrType);

            string name = "" + attrOption.name;
            float value = attr.GetValue(attrType);
            AttrValueShowType showType = AttrValueShowType.Int;
            if (attrType == EntityAttrType.AttackSpeed)
            {
                showType = AttrValueShowType.Float_2;
            }
            else if (attrType == EntityAttrType.AttackRange)
            {
                showType = AttrValueShowType.Float_2;
            }
            else if (attrType == EntityAttrType.MoveSpeed)
            {
                showType = AttrValueShowType.Float_2;
            }

            BattleAttrUIData uiData = new BattleAttrUIData()
            {
                type = attrType,
                describe = attrOption.describe,
                name = name,
                value = value,
                valueShowType = showType,
                iconResId = attrOption.iconResId
            };

            attrDataList.Add(uiData);
        }
    }

    //用这个开始显示
    public void RefreshShowList()
    {
        var args = new UIListArgs<BattleAttrUIShowObj, BattleAttrUI>();
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
        
        EventDispatcher.RemoveListener<BattleEntity_Client, int>(EventIDs.OnChangeEntityBattleData,
            OnChangeEntityBattleData);
        
        
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