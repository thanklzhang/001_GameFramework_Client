using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle_Client;
using Config;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceSkillCell
{
    public GameObject gameObject;
    public Transform transform;

    private Image icon;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;
    private GameObject selectFlagGo;
    private Button clickBtn;

    public int skillId;

    private BattleReplaceSkillUI parentUI;

    public void Init(GameObject gameObject, BattleReplaceSkillUI parentUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        this.parentUI = parentUI;

        nameText = transform.Find("name").GetComponent<TextMeshProUGUI>();
        levelText = transform.Find("level").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("icon").GetComponent<Image>();
        selectFlagGo = transform.Find("selectFlag").gameObject;
        clickBtn = icon.GetComponent<Button>();

        clickBtn.onClick.RemoveAllListeners();
        clickBtn.onClick.AddListener(OnClick);
    }

    protected void OnClick()
    {
        this.parentUI.SelectSkill(this.skillId);
    }

    public void Refresh(int skillConfigId)
    {
        this.skillId = skillConfigId;

        var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skillConfigId);
        this.nameText.text = skillConfig.Name;
        this.levelText.text = "等级 " + skillConfig.Level;
        ResourceManager.Instance.GetObject<Sprite>(skillConfig.IconResId, (sprite) => { this.icon.sprite = sprite; });
    }

    public void SetSelectState(bool isSelect)
    {
        selectFlagGo.SetActive(isSelect);
    }
}

public class BattleReplaceSkillUI : BaseUI
{
    private Transform replaceSkillRoot;
    private Transform opSkillRoot;
    private GameObject opSkillGo;
    private Button replaceBtn;
    private Button giveUpBtn;

    private List<ReplaceSkillCell> replaceSkillShowObjList;
    private ReplaceSkillCell opSkillShowObj;

    private ReplaceSkillCell currSelectSkillShowObj;

    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.BattleReplaceHeroUI;
        this.uiShowLayer = UIShowLayer.Top_0;
        this.showMode = CtrlShowMode.Float;
    }

    protected override void OnLoadFinish()
    {
        var panel = transform.Find("Panel");
        var skillArea = panel.Find("skillArea");
        replaceSkillRoot = skillArea.transform.Find("leaderSkillRoot/layout");
        opSkillRoot = skillArea.transform.Find("opSkillRoot/skillItem");
        replaceBtn = panel.transform.Find("ConfirmBtn").GetComponent<Button>();
        giveUpBtn = panel.transform.Find("GiveUpBtn").GetComponent<Button>();

        replaceBtn.onClick.RemoveAllListeners();
        replaceBtn.onClick.AddListener(this.OnClickReplaceBtn);

        giveUpBtn.onClick.RemoveAllListeners();
        giveUpBtn.onClick.AddListener(this.OnClickGiveUpBtn);
    }

    private BattleReplaceSkillUIArgs currUIArgs;

    protected override void OnOpen(UICtrlArgs args)
    {
        currUIArgs = (BattleReplaceSkillUIArgs)args;

        RefreshUI();
    }

    void RefreshUI()
    {
        replaceSkillShowObjList = new List<ReplaceSkillCell>();
        var mySkills = BattleManager.Instance.GetLocalCtrlHeroSkills();
        var leaderSkills = mySkills.FindAll(skill =>
        {
            var skillConfig = ConfigManager.Instance.GetById<Config.Skill>(skill.configId);
            return (SkillCategory)skillConfig.SkillCategory == SkillCategory.LeaderSkill;
        });

        //目前拥有的队长技能
        for (int i = 0; i < replaceSkillRoot.childCount; i++)
        {
            var child = replaceSkillRoot.GetChild(i);
            var skillShowObj = new ReplaceSkillCell();
            var skillData = leaderSkills[i];
            skillShowObj.Init(child.gameObject,this);
            skillShowObj.Refresh(skillData.configId);
            replaceSkillShowObjList.Add(skillShowObj);
        }

        SelectSkill(replaceSkillShowObjList[0].skillId);

        //将要替换的技能
        opSkillShowObj = new ReplaceSkillCell();
        opSkillShowObj.Init(opSkillGo, this);
        opSkillShowObj.Refresh(currUIArgs.opSkillId);
    }

    public void SelectSkill(int skillId)
    {
        if (currSelectSkillShowObj != null)
        {
            currSelectSkillShowObj.SetSelectState(false);
        }

        var skillShowObj = replaceSkillShowObjList.Find(s => s.skillId == skillId);
        currSelectSkillShowObj = skillShowObj;
        currSelectSkillShowObj.SetSelectState(true);
    }

    public void OnClickReplaceBtn()
    {
        var skillId = currSelectSkillShowObj.skillId;
        BattleManager.Instance.MsgSender.Send_SelectReplaceSkill(skillId);

        UIManager.Instance.Close<BattleReplaceSkillUI>();
    }

    public void OnClickGiveUpBtn()
    {
        var skillId = -1;
        BattleManager.Instance.MsgSender.Send_SelectReplaceSkill(skillId);

        UIManager.Instance.Close<BattleReplaceSkillUI>();
    }

    protected override void OnClose()
    {
    }
}

public class BattleReplaceSkillUIArgs : UICtrlArgs
{
    // public List<int> replaceSkillIdList;
    public int opSkillId;
}