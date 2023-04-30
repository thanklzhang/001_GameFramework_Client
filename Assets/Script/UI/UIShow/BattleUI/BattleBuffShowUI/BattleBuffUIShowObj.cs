using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleBuffUIShowObj : BaseUIShowObj<BattleBuffUI>
{
    RawImage icon;
    GameObject canUseMaskGo;
    GameObject cdRootGo;
    Image cdImg;
    Text stackCountText;


    public BattleBuffUIData uiData;

    float currCDTimer = 0;

    public override void OnInit()
    {

        canUseMaskGo = this.transform.Find("cantUse").gameObject;
        cdRootGo = this.transform.Find("CDRoot").gameObject;
        //cdTimeText = this.transform.Find("CDRoot/CDShow/cd_text").GetComponent<Text>();
        cdImg = this.transform.Find("CDRoot/CDShow").GetComponent<Image>();
        icon = this.transform.Find("icon").GetComponent<RawImage>();
        stackCountText = this.transform.Find("count_text").GetComponent<Text>();
    }

    public override void OnRefresh(object data, int index)
    {
        this.uiData = (BattleBuffUIData)data;

        this.UpdateInfo(this.uiData);

    }

    internal void UpdateInfo(BattleBuffUIData buffUIData)
    {
        this.uiData = buffUIData;

        var cdTime = buffUIData.currCDTime;
        currCDTimer = cdTime / 1000.0f;

        if (cdTime <= 0)
        {
            //-1 时 为没有时间概念的的 buff 例：目前时永久性 buff
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

        ResourceManager.Instance.GetObject<Texture>(uiData.iconResId, (tex) =>
        {
            icon.texture = tex;
        });

        stackCountText.text = "" + this.uiData.stackCount;
    }


    public void Update(float deltaTime)
    {
        if (currCDTimer < 0)
        {
            currCDTimer = 0;
        }

        if (currCDTimer > 0)
        {
            //float showTime = 0.0f;
            //var showStr = "";
            //if (currCDTimer >= 0.50f)
            //{
            //    showTime = (float)Math.Ceiling(currCDTimer);

            //    showStr = string.Format("{0}", (int)showTime);
            //}
            //else
            //{
            //    showTime = currCDTimer;
            //    showStr = string.Format("{0:F}", showTime);
            //}

            currCDTimer -= deltaTime;

            this.uiData.currCDTime = currCDTimer * 1000.0f;
            //cdTimeText.text = showStr;
            cdImg.fillAmount = currCDTimer / (uiData.maxCDTime / 1000.0f);
        }

    }

    internal int Guid => this.uiData.guid;

    public override void OnRelease()
    {

    }

}


public class BattleBuffUIArgs : UIArgs
{
    public List<BattleBuffUIData> battleBuffList;
}

public class BattleBuffUIData
{
    public int guid;
    public int iconResId;
    public float currCDTime;
    public float maxCDTime;
    public int stackCount;

    public bool isRemove;
}
