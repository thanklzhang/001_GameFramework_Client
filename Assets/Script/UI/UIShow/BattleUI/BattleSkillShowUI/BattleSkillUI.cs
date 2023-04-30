using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillUI
{
    public GameObject gameObject;
    public Transform transform;

    Transform skillListRoot;

    Text skillTipText;
    float skillTipShowTimer;
    float skillTipMaxShowTime = 1.6f;

    List<BattleSkillUIData> skillDataList = new List<BattleSkillUIData>();
    List<BattleSkillUIShowObj> skillShowObjList = new List<BattleSkillUIShowObj>();

    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        skillListRoot = this.transform.Find("group");
        this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Refresh(UIArgs args)
    {
        BattleSkillUIArgs uiDataArgs = (BattleSkillUIArgs)args;

        this.skillDataList = uiDataArgs.battleSkillList;

        this.RefreshSkillList();
    }

    void RefreshSkillList()
    {
        UIListArgs<BattleSkillUIShowObj, BattleSkillUI> args = new UIListArgs<BattleSkillUIShowObj, BattleSkillUI>();
        args.dataList = skillDataList;
        args.showObjList = skillShowObjList;
        args.root = skillListRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);
    }

    public void UpdateSkillInfo(int skillId, float cdTime)
    {
        var skillShowObj = FindSkill(skillId);
        if (skillShowObj != null)
        {
            skillShowObj.UpdateInfo(cdTime);
        }
        else
        {
            Logx.LogWarning("BattleSkillUI : UpdateSkillInfo : the skillId is not found : " + skillId);
        }
    }

    public void SetSkillTipText(string str)
    {
        this.skillTipText.text = str;
        this.skillTipText.gameObject.SetActive(true);
        this.skillTipShowTimer = this.skillTipMaxShowTime;
    }

    public void Update(float deltaTime)
    {
        foreach (var item in skillShowObjList)
        {
            item.Update(deltaTime);
        }

        if (this.skillTipShowTimer > 0)
        {
            this.skillTipShowTimer -= deltaTime;
            if (this.skillTipShowTimer <= 0)
            {
                this.skillTipText.gameObject.SetActive(false);
            }
        }



    }

    public BattleSkillUIShowObj FindSkill(int skillId)
    {
        foreach (var skill in skillShowObjList)
        {
            if (skill.GetSkillConfigId() == skillId)
            {
                return skill;
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
        foreach (var item in skillShowObjList)
        {
            item.Release();
        }

        skillShowObjList = null;
        skillDataList = null;
    }

}
