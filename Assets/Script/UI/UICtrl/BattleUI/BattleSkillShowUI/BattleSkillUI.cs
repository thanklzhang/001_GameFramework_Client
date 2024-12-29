using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using UnityEngine;
using UnityEngine.UI;
using Skill = Config.Skill;

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

    public BattleUI BattleUI;

    public void Init(GameObject gameObject, BattleUI battleUIPre)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.BattleUI = battleUIPre;

        skillListRoot = this.transform.Find("group");
        this.skillTipText = this.transform.Find("skillTipText").GetComponent<Text>();

        // bigSkillGo = this.transform.Find("bigSkill").gameObject;
        // bigSkillShowObj = new BattleBigSkillUIShowObj();
        // bigSkillShowObj.Init(bigSkillGo,this);

        EventDispatcher.AddListener<int, BattleSkillInfo>(EventIDs.OnSkillInfoUpdate, OnSkillInfoUpdate);
        EventDispatcher.AddListener<string>(EventIDs.OnSkillTips, OnSkillTips);
        EventDispatcher.AddListener(EventIDs.OnSkillRefreshAll, OnSkillRefreshAll);
    }

    public void OnSkillInfoUpdate(int entityGuid, BattleSkillInfo skillInfo)
    {
        //var entity = BattleEntityManager.Instance.FindEntity(entityGuid);

        var myEntityGuid = BattleManager.Instance.GetLocalCtrlHeroGuid();

        if (myEntityGuid == entityGuid)
        {
            this.UpdateSkillInfo(skillInfo);

            //RefreshAllUI();
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
        List<BattleSkillUIData> dataList = new List<BattleSkillUIData>();
        
        var localEntity = BattleManager.Instance.GetLocalCtrlHero();
        var entityConfig = ConfigManager.Instance.GetById<Config.EntityInfo>(localEntity.configId);
        var skills = BattleManager.Instance.GetLocalCtrlHeroSkills();

        //填充专属小招
        {
            //填充小招
            var minorSkillId = entityConfig.SkillIds.Find(skillId =>
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillId);
                return (SkillCategory)skillConfig.SkillCategory == SkillCategory.MinorSkill;
            });

            var minorSkill = skills.Find(skill =>
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
                return (SkillCategory)skillConfig.SkillCategory == SkillCategory.MinorSkill;
            });

            BattleSkillUIData minorSkillUIData = new BattleSkillUIData();
            // minorSkillUIData.showIndex = 0;
            minorSkillUIData.skillId = minorSkillId;
            if (minorSkillId > 0)
            {
                //有专属小招
                if (minorSkill != null)
                {
                    //专属小招解锁了
                    minorSkillUIData.maxCDTime = minorSkill.maxCDTime;
                    minorSkillUIData.exp = minorSkill.exp;
                    minorSkillUIData.isUnlock = true;
                }
            }
            dataList.Add(minorSkillUIData);
        }

        //填充队长技能
        {
            var leaderSkills = skills.FindAll(skill =>
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
                return (SkillCategory)skillConfig.SkillCategory == SkillCategory.LeaderSkill;
            });

            for (int i = 0; i < 2; i++)
            {
                BattleSkillUIData leaderSkillUIData = new BattleSkillUIData();
                dataList.Add(leaderSkillUIData);
            }

            if (leaderSkills != null)
            {
                {
                    //队长技能 1
                    var player = BattleManager.Instance.GetLocalPlayer();
                    var leaderSkill = GetLeaderSkill(leaderSkills,PlayerCommandType.Skill_Leader_1);

                    if (leaderSkill != null)
                    {
                        var leaderSkillUIData = dataList[1];
                        leaderSkillUIData.skillId = leaderSkill.configId;
                        leaderSkillUIData.maxCDTime = leaderSkill.maxCDTime;
                        leaderSkillUIData.exp = leaderSkill.exp;
                        leaderSkillUIData.isUnlock = true;
                    }
                }
                
                {
                    //队长技能 1
                    var player = BattleManager.Instance.GetLocalPlayer();
                    var leaderSkill = GetLeaderSkill(leaderSkills,PlayerCommandType.Skill_Leader_2);

                    if (leaderSkill != null)
                    {
                        var leaderSkillUIData = dataList[2];
                        leaderSkillUIData.skillId = leaderSkill.configId;
                        leaderSkillUIData.maxCDTime = leaderSkill.maxCDTime;
                        leaderSkillUIData.exp = leaderSkill.exp;
                        leaderSkillUIData.isUnlock = true;
                    }
                }
            }
        }

        //填充大招技能
        {
            var ultimateSkillId = entityConfig.UltimateSkillId;
            var ultimateSkill = skills.Find(skill =>
            {
                var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
                return (SkillCategory)skillConfig.SkillCategory == SkillCategory.UltimateSkill;
            });

            BattleSkillUIData ultimateSkillUIData = new BattleSkillUIData();
            // ultimateSkillUIData.showIndex = 0;
            ultimateSkillUIData.skillId = ultimateSkillId;
            if (ultimateSkillId > 0)
            {
                //有专属大招
                if (ultimateSkill != null)
                {
                    //专属大招解锁了
                    ultimateSkillUIData.maxCDTime = ultimateSkill.maxCDTime;
                    ultimateSkillUIData.exp = ultimateSkill.exp;
                    ultimateSkillUIData.isUnlock = true;
                }
            }
            
            dataList.Add(ultimateSkillUIData);
        }

        this.skillDataList = dataList;
    }

    BattleSkillInfo GetLeaderSkill(List<BattleSkillInfo> leaderSkills,PlayerCommandType commanType)
    {
        //队长技能 1
        var player = BattleManager.Instance.GetLocalPlayer();
        var leaderSkill1 = leaderSkills.Find(skill =>
        {

            var commandMode = player.GetInputCommand(commanType);
            if (commandMode != null)
            {
                var skillCM = commandMode as SkillCommandModel;
                if (skillCM.skillConfigId == skill.configId)
                {
                    return true;
                }
            }

            return false;
        });

        return leaderSkill1;
    }


    void RefreshSkillShowList()
    {
        for (int i = 0; i < this.skillDataList.Count; i++)
        {
            GameObject go = null;
            var data = this.skillDataList[i];
            BattleSkillUIShowObj showObj = null;
            if (i < skillListRoot.childCount)
            {
                go = skillListRoot.GetChild(i).gameObject;
                if (i < this.skillShowObjList.Count)
                {
                    showObj = skillShowObjList[i];
                }
                else
                {
                    showObj = new BattleSkillUIShowObj();
                    showObj.Init(go, this);
                    this.skillShowObjList.Add(showObj);
                }
            }
            else
            {
                go = GameObject.Instantiate(skillListRoot.GetChild(0).gameObject, skillListRoot, false);
                showObj = new BattleSkillUIShowObj();
                showObj.Init(go, this);
                this.skillShowObjList.Add(showObj);
            }

            go.SetActive(true);
            showObj.Refresh(data, i);
        }

        for (int i = skillDataList.Count; i < skillListRoot.childCount; i++)
        {
            this.skillShowObjList[i].Release();
            skillListRoot.GetChild(i).gameObject.SetActive(false);
        }

        //this.bigSkillShowObj.Refresh(this.bigSkillData);
    }

    public void UpdateSkillInfo(BattleSkillInfo skillInfo)
    {
        var skillShowObj = FindSkill(skillInfo.configId);
        if (skillShowObj != null)
        {
            skillShowObj.UpdateInfo(skillInfo);

            if (skillInfo.isDelete)
            {
            }
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

    public void OnSkillRefreshAll()
    {
        this.RefreshAllUI();
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
        EventDispatcher.RemoveListener(EventIDs.OnSkillRefreshAll, OnSkillRefreshAll);
        
        EventDispatcher.RemoveListener<string>(EventIDs.OnSkillTips, OnSkillTips);

        foreach (var item in skillShowObjList)
        {
            item.Release();
        }

        skillShowObjList = null;
        skillDataList = null;
    }
}