using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleStageInfoUIArgs : UIArgs
{
    public int battleConfigId;
}

public class BattleStageInfoUI
{
    public GameObject gameObject;
    public Transform transform;

    private int battleConfigId;

    Text stageNameText;
    // private GameObject bossLimitGo;
    // private RectTransform bossLimitRootRectTran;
    // Text bossLimitTimeText;


    // //runtime
    // private float currTimer;
    // private bool isHasBossCountdown;
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        stageNameText = this.transform.Find("stageName").GetComponent<Text>();
        // bossLimitGo = this.transform.Find("stageTimeRoot").gameObject;
        // bossLimitRootRectTran = bossLimitGo.transform.GetComponent<RectTransform>();
        // bossLimitTimeText = bossLimitGo.transform.Find("stageTime").GetComponent<Text>();
       
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(UIArgs args)
    {
        var uiDataArgs = (BattleStageInfoUIArgs)args;
        battleConfigId = uiDataArgs.battleConfigId;

        this.RefreshInfo();
    }

    // public void StartBossLimitCountdown()
    // {
    //     var battleConfig = TableManager.Instance.GetById<Table.Battle>(battleConfigId);
    //     currTimer = battleConfig.BossLimitTime / 1000.0f;
    //     isHasBossCountdown = true;
    //
    //     this.RefreshInfo();
    //
    //
    // }

    void RefreshInfo()
    {
        var battleConfig = TableManager.Instance.GetById<Table.Battle>(battleConfigId);

        stageNameText.text = battleConfig.Name;

        // if (isHasBossCountdown)
        // {
        //     RefreshBossLimitTimeShow();
        // }
        //
        // this.bossLimitGo.SetActive(isHasBossCountdown);
        
    }

    // void RefreshBossLimitTimeShow()
    // {
    //     if (this.currTimer < 0)
    //     {
    //         this.currTimer = 0;
    //     }
    //
    //     var minutes = (int)(this.currTimer / 60.0f);
    //     var seconds = (int)(this.currTimer % 60.0f);
    //     var ms = (int)((this.currTimer - (int)currTimer) * 1000);
    //    
    //     bossLimitTimeText.text = string.Format("{0}分{1}秒{2:D3}",minutes,seconds, ms);
    //     
    //     LayoutRebuilder.ForceRebuildLayoutImmediate(bossLimitRootRectTran);
    // }

    public void Update(float deltaTime)
    {
        // if (isHasBossCountdown)
        // {
        //     this.currTimer -= deltaTime;
        //     RefreshBossLimitTimeShow();
        // }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Close()
    {
        this.Hide();
        // isHasBossCountdown = false;
    }

    public void Release()
    {
    }
}