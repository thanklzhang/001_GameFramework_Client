using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSkillUIShowObj : BaseUIShowObj<BattleSkillUI>
{
    protected Image icon;
    protected GameObject canUseMaskGo;
    protected GameObject cdRootGo;
    protected Image cdImg;
    protected Text cdTimeText;
    private Slider expSlider;
    private TextMeshProUGUI expText;
    private TextMeshProUGUI levelText;
    protected UIEventTrigger evetnTrigger;
    public BattleSkillUIData uiData;

    float currCDTimer = 0;

    public override void OnInit()
    {
        canUseMaskGo = this.transform.Find("cantUse").gameObject;
        cdRootGo = this.transform.Find("CDRoot").gameObject;
        cdTimeText = this.transform.Find("CDRoot/CDShow/cd_text").GetComponent<Text>();
        cdImg = this.transform.Find("CDRoot/CDShow").GetComponent<Image>();
        icon = this.transform.Find("icon").GetComponent<Image>();
        expSlider = this.transform.Find("expSlider").GetComponent<Slider>();
        expText = expSlider.transform.Find("exp_text").GetComponent<TextMeshProUGUI>();
        levelText = this.transform.Find("lv_text").GetComponent<TextMeshProUGUI>();

        evetnTrigger = icon.GetComponent<UIEventTrigger>();

        canUseMaskGo.SetActive(false);
        cdRootGo.SetActive(false);

        evetnTrigger.OnPointEnterEvent += OnPointEnter;
        evetnTrigger.OnPointerExitEvent += OnPointExit;
    }

    public override void OnRefresh(object data, int index)
    {
        if (null == data)
        {
            return;
        }

        this.uiData = (BattleSkillUIData)data;

        //技能图标
        var skillId = this.uiData.skillId;
        var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
        ResourceManager.Instance.GetObject<Sprite>(skillConfig.IconResId, (sprite) => { this.icon.sprite = sprite; });

        RefreshLevelExpShow();
    }

    public void RefreshLevelExpShow()
    {
        var skillId = this.uiData.skillId;
        var skillConfig = Config.ConfigManager.Instance.GetById<Config.Skill>(skillId);
        var currExp = this.uiData.exp;
        var skillUpdateConfig = Config.ConfigManager.Instance.GetById<SkillUpdateParam>(1);
        var maxExp = skillUpdateConfig.UpgradeExpPerLevel.Sum();
        this.expSlider.value = currExp / (float)maxExp;
        this.expText.text = $"{currExp}/{maxExp}";
        
        this.levelText.text = "" + skillConfig.Level;
    }

    internal void UpdateInfo(BattleSkillInfo skillInfo)
    {
        currCDTimer = skillInfo.currCDTime;

        if (currCDTimer <= 0)
        {
            //可以使用技能了
            cdRootGo.SetActive(false);
            canUseMaskGo.SetActive(false);
        }
        else
        {
            //在 cd 中 ，刷新 cd 时间
            cdRootGo.SetActive(true);
            //mask 是代表不能用 并不是只有 cd 这个因素 尽管现在只有 cd
            canUseMaskGo.SetActive(true);
        }

        RefreshLevelExpShow();
    }


    public void Update(float deltaTime)
    {
        if (currCDTimer < 0)
        {
            currCDTimer = 0;
        }

        if (currCDTimer > 0)
        {
            currCDTimer -= deltaTime;

            float showTime = 0.0f;
            var showStr = "";
            if (currCDTimer >= 0.50f)
            {
                showTime = (float)Math.Ceiling(currCDTimer);

                showStr = string.Format("{0}", (int)showTime);
            }
            else
            {
                showTime = currCDTimer;
                showStr = string.Format("{0:F}", showTime);
            }


            //Debug.LogError("currCDTimer " + currCDTimer + " " + uiData.maxCDTime);
            cdTimeText.text = showStr;
            //Debug.Log("currCDTimer" + currCDTimer);
            //Debug.Log("uiData.maxCDTime" + uiData.maxCDTime);
            cdImg.fillAmount = currCDTimer / uiData.maxCDTime;
        }
    }

    internal int GetSkillConfigId()
    {
        return this.uiData.skillId;
    }

    public void OnPointEnter(PointerEventData e)
    {
        //转换成点在 BattleUI 中的 localPosition

        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();

        var screenPos = e.position;

        Vector2 uiPos;
        var battleUIRect = parentObj.BattleUIPre.gameObject.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);

        EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UISkillOption_PointEnter, this.uiData.skillId, uiPos);
    }

    public void OnPointExit(PointerEventData e)
    {
        EventDispatcher.Broadcast<int>(EventIDs.On_UISkillOption_PointExit, this.uiData.skillId);
    }


    public override void OnRelease()
    {
        evetnTrigger.OnPointEnterEvent -= OnPointEnter;
        evetnTrigger.OnPointerExitEvent -= OnPointExit;
    }
}


// public class BattleSkillUIArgs : UICtrlArgs
// {
//     public List<BattleSkillUIData> battleSkillList;
// }

public class BattleSkillUIData
{
    public int skillId;
    public int iconResId;
    public float maxCDTime;
    public int exp;
    public int showIndex;
}