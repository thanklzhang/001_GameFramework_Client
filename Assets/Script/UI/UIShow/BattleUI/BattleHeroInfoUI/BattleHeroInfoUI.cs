using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleHeroInfoUIArgs : UIArgs
{
    public int battleConfigId;
    public List<BattleHeroInfoUIData> battleHeroInfoUIDataList;
    public BattleHeroInfoUIData bossData;
}
public class BattleHeroInfoUI : BaseUI
{
    //cmp
    public GameObject gameObject;
    public Transform transform;

    Transform heroRoot;
   
    private GameObject bossLimitGo;
    private RectTransform bossLimitRootRectTran;
    Text bossLimitTimeText;
    
    
    //runtime
    
    public BattleUI battleUI;
    
    private List<BattleHeroInfoUIData> dataList;
    private List<BattleHeroInfoShowObj> showObjList;

    private BattleHeroInfoUIData bossData;
    private BattleHeroInfoShowObj bossShowObj;
    private int battleConfigId;
    private float currTimer;
    private bool isHasBossCountdown;
    
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;

        heroRoot = this.transform.Find("root");
        
        
        dataList = new List<BattleHeroInfoUIData>();
        showObjList = new List<BattleHeroInfoShowObj>();

        bossData = null;
        bossShowObj = new BattleHeroInfoShowObj();
        
        var bossRootGo = this.transform.Find("bossInfo").gameObject;
        bossShowObj.Init(bossRootGo,this);
        
        //限时 boss 击杀倒计时
        bossLimitGo = this.transform.Find("stageTimeRoot").gameObject;
        bossLimitRootRectTran = bossLimitGo.transform.GetComponent<RectTransform>();
        bossLimitTimeText = bossLimitGo.transform.Find("stageTime").GetComponent<Text>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(UIArgs args)
    {
        BattleHeroInfoUIArgs uiDataArgs = (BattleHeroInfoUIArgs)args;

        this.dataList = uiDataArgs.battleHeroInfoUIDataList;
        this.bossData = uiDataArgs.bossData;
        battleConfigId = uiDataArgs.battleConfigId;

        this.RefreshHeroInfoist();
        this.RefreshBossInfo();
    }

    
    
    void RefreshHeroInfoist()
    {
        UIListArgs<BattleHeroInfoShowObj, BattleHeroInfoUI> args = new UIListArgs<BattleHeroInfoShowObj, BattleHeroInfoUI>();
        args.dataList = dataList;
        args.showObjList = showObjList;
        args.root = heroRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);
    }

    void RefreshBossInfo()
    {
        // boss 血条
        bossShowObj.Refresh(this.bossData);
        
        //boss 倒计时
        if (isHasBossCountdown)
        {
            RefreshBossLimitTimeShow();
        }
        
        this.bossLimitGo.SetActive(isHasBossCountdown);
      

    }
    
    void RefreshBossLimitTimeShow()
    {
        if (this.currTimer < 0)
        {
            this.currTimer = 0;
        }

        var minutes = (int)(this.currTimer / 60.0f);
        var seconds = (int)(this.currTimer % 60.0f);
        var ms = (int)((this.currTimer - (int)currTimer) * 1000);
       
        bossLimitTimeText.text = string.Format("{0:D2}分{1:D2}秒{2:D3}",minutes,seconds, ms);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(bossLimitRootRectTran);
    }
    
    public void StartBossLimitCountdown()
    {
        var battleConfig = TableManager.Instance.GetById<Table.Battle>(battleConfigId);
        currTimer = battleConfig.BossLimitTime / 1000.0f;
        isHasBossCountdown = true;

        this.RefreshBossInfo();


    }
    
    public void RefreshSingleHeroInfo(BattleHeroInfoUIData info,int fromEntityGuid)
    {
        
        var findEntity = BattleEntityManager.Instance.FindEntity(info.heroGuid);
        if (null == findEntity)
        {
            Logx.LogWarning("the entity is not found : " + info.heroGuid);
            return;
        }
        
        var entityConfig = Table.TableManager.Instance.GetById<EntityInfo>(findEntity.configId);
        var isBoss = 1 == entityConfig.IsBoss;

        if (isBoss)
        {
            if (bossData != null)
            {
                if (info.heroGuid == bossData.heroGuid)
                {
                    bossShowObj.UpdateInfo(info);
                }
            }
            else
            {
                bossShowObj.Refresh(info);
            }
        }
        else
        {
            var heroInfo = FindHeroInfo(info.heroGuid);
            if (heroInfo != null)
            {
                heroInfo.UpdateInfo(info);
            }
            
        }      
    }

    public void Update(float deltaTime)
    {
        foreach (var item in showObjList)
        {
            item.Update(deltaTime);
        }
        
        if (isHasBossCountdown)
        {
            this.currTimer -= deltaTime;
            RefreshBossLimitTimeShow();
        }

    }

    public BattleHeroInfoShowObj FindHeroInfo(int heroGuid)
    {
        foreach (var heroInfo in showObjList)
        {
            if (heroInfo.GetHeroGuid() == heroGuid)
            {
                return heroInfo;
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
        
        isHasBossCountdown = false;
    }

    public void Release()
    {
        foreach (var item in showObjList)
        {
            item.Release();
        }

        dataList = null;
        showObjList = null;
    }

}
