using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoUI : NormalUI
{
    Text heroIdText;
    Text heroNameText;
    Text heroLevelText;

    protected override void OnInit()
    {
        this.name = UIName.HeroInfoUI;
        this.resPath = "Assets/BuildRes/Prefabs/UI/HeroInfoUI.prefab";
        this.type = UIType.Normal;
    }


    protected override void OnLoadFinish()
    {
        heroIdText = this.transform.Find("HeroInfo/id").GetComponent<Text>();
        heroNameText = this.transform.Find("HeroInfo/name").GetComponent<Text>();
        heroLevelText = this.transform.Find("HeroInfo/level").GetComponent<Text>();

    }

    protected override void OnOpen()
    {

    }

    protected override void OnActive()
    {

    }
    //提供给外部的接口--------------------------------
    public void RefreshHeroInfoData(int currHeroId)
    {
        var heroId = "" + currHeroId;
        var heroName = "战士1";
        var heroLevel = "12";

        heroIdText.text = heroId;
        heroNameText.text = heroName;
        heroLevelText.text = heroLevel;

    }
    
    //----------------------------------------------------
    protected override void OnClose()
    {

    }

}

