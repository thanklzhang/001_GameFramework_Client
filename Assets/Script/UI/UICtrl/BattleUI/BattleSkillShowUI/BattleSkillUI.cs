using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
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

    // private BattleSkillUIData bigSkillData;
    // private BattleBigSkillUIShowObj bigSkillShowObj;

    private GameObject bigSkillGo;
    
    public BattleUICtrl BattleUIPre;

    public void Init(GameObject gameObject, BattleUICtrl battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.BattleUIPre = battleUIPre;

        skillListRoot = this.transform.Find("group");
        this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();

        // bigSkillGo = this.transform.Find("bigSkill").gameObject;
        // bigSkillShowObj = new BattleBigSkillUIShowObj();
        // bigSkillShowObj.Init(bigSkillGo,this);
        
        EventDispatcher.AddListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
        EventDispatcher.AddListener<string>(EventIDs.OnSkillTips,OnSkillTips);
        
    }
    public void OnSkillInfoUpdate(int entityGuid, BattleSkillInfo skillInfo)
    {
        //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);

        var myEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();

        if (myEntityGuid == entityGuid)
        {
            this.UpdateSkillInfo(skillInfo.configId, skillInfo.currCDTime);

            RefreshAllUI();
        }
    }

    void OnSkillTips(string str)
    {
        this.SetSkillTipText(str);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        RefreshDataList();
        RefreshSkillShowList();
    }

    public void RefreshDataList()
    {

        var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();
        List<BattleSkillUIData> dataList = new List<BattleSkillUIData>();
        for (int i = 1; i < skills.Count; i++)
        {
            var skillData = skills[i];
            BattleSkillUIData skill = new BattleSkillUIData()
            {
                //skill.iconResId
                skillId = skillData.configId,
                maxCDTime = skillData.maxCDTime,
            };
            
            var skillConfig = Table.TableManager.Instance.GetById<Skill>(skillData.configId);
            // if (skillConfig.IsBigSkill != 1)
            // {
            //    
            //     
            //     dataList.Add(skill);
            // }
            // else
            // {
            //     this.bigSkillData = skill;
            // }
            
            dataList.Add(skill);
        }
        this.skillDataList = dataList;
       
    
    }

    void RefreshSkillShowList()
    {
        UIListArgs<BattleSkillUIShowObj, BattleSkillUI> args = new UIListArgs<BattleSkillUIShowObj, BattleSkillUI>();
        args.dataList = skillDataList;
        args.showObjList = skillShowObjList;
        args.root = skillListRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);
        
        //this.bigSkillShowObj.Refresh(this.bigSkillData);
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
            //Logx.LogWarning("BattleSkillUI : UpdateSkillInfo : the skillId is not found : " + skillId);
            
            //not found , perhaps add a new skill
            
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

        //bigSkillShowObj.Update(deltaTime);
        
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

        // if (this.bigSkillData != null && skillId == this.bigSkillData.skillId)
        // {
        //     return this.bigSkillShowObj;
        // }

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
        EventDispatcher.RemoveListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
        EventDispatcher.RemoveListener<string>(EventIDs.OnSkillTips,OnSkillTips);
        
        foreach (var item in skillShowObjList)
        {
            item.Release();
        }

        skillShowObjList = null;
        skillDataList = null;
    }

}
