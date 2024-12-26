using System;
using System.Collections;
using System.Collections.Generic;
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

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
    }

    public void Refresh(int skillConfigId)
    {
        
    }
}

public class BattleReplaceHeroUI : BaseUI
{
    private Transform replaceSkillRoot;
    private Transform opSkillRoot;
    private Button replaceBtn;
    private Button giveUpBtn;
    
    private List<ReplaceSkillCell> replaceSkillShowObjList;
    private ReplaceSkillCell opSkillShowObj;
    
    protected override void OnInit()
    {
        this.uiResId = (int)ResIds.BattleReplaceHeroUI;
        this.uiShowLayer = UIShowLayer.Top_0;
        this.showMode = CtrlShowMode.Float;
    }

    protected override void OnLoadFinish()
    {
        replaceSkillRoot = this.transform.Find("");
        opSkillRoot = this.transform.Find("");
        replaceBtn = this.transform.Find("").GetComponent<Button>();
        giveUpBtn = this.transform.Find("").GetComponent<Button>();
        
        replaceBtn.onClick.RemoveAllListeners();
        replaceBtn.onClick.AddListener(this.OnClickReplaceBtn);
        
        giveUpBtn.onClick.RemoveAllListeners();
        giveUpBtn.onClick.AddListener(this.OnClickGiveUpBtn);
    }

    protected override void OnOpen(UICtrlArgs args)
    {
        var resultArgs = (BattleResultUIArgs)args;
        RefreshUI();
    }

    void RefreshUI()
    {
    }

    public void OnClickReplaceBtn()
    {
        //sendMsg
        
        UIManager.Instance.Close<BattleReplaceHeroUI>();
    }

    public void OnClickGiveUpBtn()
    {
        //sendMsg
        
        UIManager.Instance.Close<BattleReplaceHeroUI>();
    }

    protected override void OnClose()
    {
    }
}

public class BattleReplaceSkillUIArgs : UICtrlArgs
{
    public List<int> replaceSkillIdList;
    protected int opSkillId;

}