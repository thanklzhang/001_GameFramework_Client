using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HpData
{
    public BattleEntity entity;

    internal void Init()
    {

    }

    internal void Refresh(BattleEntity entity)
    {
        this.entity = entity;
    }

}

public class HpModule
{
    BattleUI battleUI;
    Dictionary<int, HpData> hpDic;

    public void Init(BattleUI battleUI)
    {
        this.battleUI = battleUI;
        hpDic = new Dictionary<int, HpData>();
    }

    public void Refresh()
    {

    }

    public void RefreshEntityData(BattleEntity entity)
    {
        this.RefreshEntityHp(entity);
    }

    public void RefreshEntityHp(BattleEntity entity)
    {
        HpData hpData = null;
        if (hpDic.ContainsKey(entity.guid))
        {
            hpData = hpDic[entity.guid];
        }
        else
        {
            hpData = new HpData();
            hpData.Init();
        }

        hpData.Refresh(entity);

        HpUIData args = new HpUIData()
        {
            entityGuid = entity.guid,
            preCurrHp = entity.CurrHealth,
            nowCurrHp = entity.CurrHealth,
            maxHp = entity.MaxHealth,
            entityObj = entity.gameObject
        };

        battleUI.RefreshHpShow(args);
    }

    public void DestroyEntityHp(BattleEntity entity)
    {

    }

    public void Release()
    {

    }
}
