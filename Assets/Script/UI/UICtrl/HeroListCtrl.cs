using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//英雄列表 ctrl
public class HeroListCtrl : BaseCtrl
{
    GameObject prefab;
    public override void OnInit()
    {
        this.isParallel = false;
    }
    public override void OnStartLoad()
    {
        var assetPath = "Assets/BuildRes/Prefabs/UI/HeroListUI.prefab";
        AssetManager.Instance.Load(assetPath, (prefab) =>
         {
             this.prefab = prefab as GameObject;
             this.LoadFinish();
         }, false);
    }

    public override void OnEnter()
    {
        var obj = GameObject.Instantiate(prefab, UIManager.Instance.normalRoot);
    }

}
