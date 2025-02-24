using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// public class BattlePopulationUIUIArgs : UICtrlArgs
// {
// }


//当前波的目标显示
public class WaveTargetInfoUI
{
    public GameObject gameObject;
    public Transform transform;

    private int battleConfigId;

    TextMeshProUGUI populationText;
    // private GameObject bossLimitGo;
    // private RectTransform bossLimitRootRectTran;
    // Text bossLimitTimeText;

    private BattleUI battleUI;

    // //runtime
    // private float currTimer;
    // private bool isHasBossCountdown;
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.battleUI = battleUI;

        populationText = this.transform.Find("countText").GetComponent<TextMeshProUGUI>();


        EventDispatcher.AddListener<int>(EventIDs.OnProcessReadyStateEnter, OnReadyStateEnter);
        EventDispatcher.AddListener<int>(EventIDs.OnProcessBattleStateEnter, OnBattleStateEnter);
        EventDispatcher.AddListener<int>(EventIDs.OnProcessBossStateEnter, OnBossStateEnter);
    }

    //TODO 请开始的时候
    public void OnBattleStateEnter_TODO(int time,int currKillCount,int maxKillCount)
    {
        
    }

    //TODO 当游戏流程中的目标进度更新的时候
    public void OnProcessTargetProgressUpdate(int currKillCount,int maxKillCount)
    {
        
    }

    public void OnReadyStateEnter(int x)
    {
    }

    public void OnBattleStateEnter(int x)
    {
        //根据传的类型 变更进度显示字样 ， 这里需要传进来一个（Wave）配置
    }

    public void OnBossStateEnter(int x)
    {
        //进度变成 ‘击杀 bossxxx’ 字样 
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        RefreshCountShow();
    }

    void RefreshCountShow()
    {
        var player = BattleManager.Instance.GetLocalPlayer();
        var currCount = player.teamMemberGuids.Count;
        var maxCount = player.currency.GetCurrencyCount(BattleCurrency.PopulationId);
        populationText.text = $"{currCount}/{maxCount}";
    }

    public void OnUpdatePlayerTeamMembersInfo(List<int> guids)
    {
        this.RefreshAllUI();
    }

    public void OnUpdateBattleCurrencyInfo()
    {
        this.RefreshAllUI();
    }


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
    }

    public void Release()
    {
        EventDispatcher.RemoveListener<int>(EventIDs.OnProcessReadyStateEnter, OnReadyStateEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.OnProcessBattleStateEnter, OnBattleStateEnter);
        EventDispatcher.RemoveListener<int>(EventIDs.OnProcessBossStateEnter, OnBossStateEnter);
    }
}