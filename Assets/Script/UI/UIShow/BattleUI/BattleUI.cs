

using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EntityRelationType
{
    Self = 0,
    Friend = 1,
    Enemy = 2
}

public class BattleUI : BaseUI
{
    public Action onCloseBtnClick;
    public Action onReadyStartBtnClick;
    public Action onAttrBtnClick;

    Button closeBtn;
    Button readyStartBtn;
    Button attrBtn;

    public Text stateText;

    //血条
    HpUIMgr hpUIMgr;
    //飘字
    FloatWordMgr floatWordMgr;
    //属性面板
    BattleAttrUI attrUI;
    //技能显示面板
    BattleSkillUI skillUI;
    //buff 显示面板
    BattleBuffUI buffUI;
    //通用描述面板
    DescribeUI describeUI;


    protected override void OnInit()
    {
        closeBtn = this.transform.Find("closeBtn").GetComponent<Button>();
        readyStartBtn = this.transform.Find("readyStartBtn").GetComponent<Button>();
        stateText = this.transform.Find("stateText").GetComponent<Text>();
        attrBtn = this.transform.Find("attrBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(() =>
        {
            onCloseBtnClick?.Invoke();
        });
        readyStartBtn.onClick.AddListener(() =>
        {
            onReadyStartBtnClick?.Invoke();
        });
        attrBtn.onClick.AddListener(() =>
        {
            onAttrBtnClick?.Invoke();
        });

        //血条管理
        this.hpUIMgr = new HpUIMgr();
        var hpUIRoot = this.transform.Find("HpShow");
        this.hpUIMgr.Init(hpUIRoot.gameObject, this);

        //飘字管理
        floatWordMgr = new FloatWordMgr();
        var floatRoot = this.transform.Find("float_word_root");
        floatWordMgr.Init(floatRoot);

        //属性面板
        attrUI = new BattleAttrUI();
        var attrUIRoot = this.transform.Find("attrBar");
        attrUI.Init(attrUIRoot.gameObject, this);
        attrUI.Hide();

        //技能面板
        var skillUIRoot = this.transform.Find("skillBar");
        skillUI = new BattleSkillUI();
        skillUI.Init(skillUIRoot.gameObject, this);

        //buff 面板
        var buffUIRoot = this.transform.Find("buffBar");
        buffUI = new BattleBuffUI();
        buffUI.Init(buffUIRoot.gameObject, this);

        //通用描述面板
        var describeUIRoot = this.transform.Find("DescribeBar");
        describeUI = new DescribeUI();
        describeUI.Init(describeUIRoot.gameObject, this);
        describeUI.Hide();

    }
    protected override void OnUpdate(float timeDelta)
    {
        this.hpUIMgr.Update(timeDelta);

        this.floatWordMgr.Update(timeDelta);

        this.skillUI.Update(timeDelta);

        this.buffUI.Update(timeDelta);

        this.describeUI.Update(timeDelta);
    }


    public void SetReadyBattleBtnShowState(bool isShow)
    {
        readyStartBtn.gameObject.SetActive(isShow);
    }

    public void SetStateText(string stateStr)
    {
        stateText.text = stateStr;
    }


    #region 血条相关
    public void RefreshHpShow(UIArgs args)
    {
        this.hpUIMgr.RefreshHpShow(args);
    }

    public void DestoryHpUI(int guid)
    {
        this.hpUIMgr.DestoryHpUI(guid);
    }

    public void SetHpShowState(int entityGuid, bool isShow)
    {
        this.hpUIMgr.SetHpShowState(entityGuid, isShow);
    }
    #endregion

    #region 飘字相关
    internal void ShowFloatWord(string word, GameObject go, int floatStyle, Color color)
    {
        floatWordMgr.ShowFloatWord(word, go, floatStyle, color);
    }
    #endregion

    #region 属性面板相关
    public void OpenAttrUI()
    {
        this.attrUI.Show();
    }

    public void CloseAttrUI()
    {
        this.attrUI.Hide();
    }

    public void RefreshBattleAttrUI(UIArgs args)
    {
        this.attrUI.Refresh(args);
    }
    #endregion

    #region 技能面板相关
    internal void RefreshBattleSkillUI(UIArgs args)
    {
        this.skillUI.Refresh(args);
    }

    public void RefreshSkillInfo(int skillConfigId, float currCDTime)
    {
        this.skillUI.UpdateSkillInfo(skillConfigId, currCDTime);
    }

    public void ShowSkillTipText(string str)
    {
        this.skillUI.SetSkillTipText(str);
    }
    #endregion

    #region buff 面板相关
    internal void RefreshBattleBuffUI(UIArgs args)
    {
        this.buffUI.Refresh(args);
    }

    public void RefreshBuffInfo(BattleBuffUIData buffInfo)
    {
        this.buffUI.UpdateBuffInfo(buffInfo);
    }


    #endregion

    #region 描述面板相关
    public void ShowDescribeUI(UIArgs arg)
    {
        this.describeUI.Refresh(arg);
        this.describeUI.Show();
    }

    public void HideDescribeUI()
    {
        this.describeUI.Hide();
    }

    #endregion

    protected override void OnRelease()
    {
        onCloseBtnClick = null;
        onReadyStartBtnClick = null;

        this.attrUI.Release();
        this.skillUI.Release();
        this.buffUI.Release();
        this.describeUI.Release();
    }


}
