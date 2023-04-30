using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillUIShowObj : BaseUIShowObj<BattleSkillUI>
{
    Texture icon;
    GameObject canUseMaskGo;
    GameObject cdRootGo;
    Image cdImg;
    Text cdTimeText;

    public BattleSkillUIData uiData;

    float currCDTimer = 0;

    public override void OnInit()
    {

        canUseMaskGo = this.transform.Find("cantUse").gameObject;
        cdRootGo = this.transform.Find("CDRoot").gameObject;
        cdTimeText = this.transform.Find("CDRoot/CDShow/cd_text").GetComponent<Text>();
        cdImg = this.transform.Find("CDRoot/CDShow").GetComponent<Image>();
        //icon
    }

    public override void OnRefresh(object data, int index)
    {
        this.uiData = (BattleSkillUIData)data;
    }

    internal void UpdateInfo(float cdTime)
    {
        currCDTimer = cdTime;

        if (cdTime <= 0)
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
    }


    public void Update(float deltaTime)
    {
        if (currCDTimer < 0)
        {
            currCDTimer = 0;
        }

        if (currCDTimer > 0)
        {
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

            currCDTimer -= deltaTime;
            cdTimeText.text = showStr;
            cdImg.fillAmount = currCDTimer / uiData.maxCDTime;
        }

    }

    internal int GetSkillConfigId()
    {
        return this.uiData.skillId;
    }

    public override void OnRelease()
    {

    }

}


public class BattleSkillUIArgs : UIArgs
{
    public List<BattleSkillUIData> battleSkillList;
}

public class BattleSkillUIData
{
    public int skillId;
    public int iconResId;
    public float maxCDTime;
}
