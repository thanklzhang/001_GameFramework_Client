using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroListUI : NormalUI
{
    Transform heroRoot;
    GameObject heroObjPrefab;
    List<HeroShowItem> heroShowItems;
    protected override void OnInit()
    {
        this.name = UIName.HeroListUI;
        this.resPath = "Assets/BuildRes/Prefabs/UI/HeroListUI.prefab";
        this.type = UIType.Normal;
    }


    protected override void OnLoadFinish()
    {
        heroRoot = this.transform.Find("root");
        heroObjPrefab = this.transform.Find("root/HeroCard").gameObject;
        heroObjPrefab.SetActive(false);
    }

    protected override void OnOpen()
    {
        //var dataList = new List<Hero>();
        //heroShowItems = new List<HeroShowItem>();
        //for (int i = 0; i < dataList.Count; i++)
        //{
        //    var obj = GameObject.Instantiate(heroObjPrefab, heroRoot);
        //    HeroShowItem showItem = new HeroShowItem();
        //    showItem.Init(obj);
        //    heroShowItems.Add(showItem);
        //}

    }

    protected override void OnActive()
    {
        //var dataList = new List<Hero>();
        //for (int i = 0; i < dataList.Count; i++)
        //{
        //    //这里要判断是否多了或者少了进行显隐或者创建
        //    HeroShowItem showItem = heroShowItems[i];
        //    Hero heroData = dataList[i];
        //    showItem.Refresh(heroData);
        //}
    }

    internal void CreateHeroList(List<Hero> heroDataList)
    {

        heroShowItems = new List<HeroShowItem>();
        for (int i = 0; i < heroDataList.Count; i++)
        {
            var obj = GameObject.Instantiate(heroObjPrefab, heroRoot);
            HeroShowItem showItem = new HeroShowItem();
            showItem.Init(obj);
            showItem.Refresh(heroDataList[i]);
            showItem.Show();
            heroShowItems.Add(showItem);
        }
    }

    internal void RefreshLevelText(int level)
    {

    }

    protected override void OnClose()
    {

    }



}


public class HeroShowItem
{
    public Hero heroData;

    public GameObject gameObject;
    public Transform transform;

    public Image iconImg;
    public Text nameText;
    public Button updateBtn;

    public Text idText;
    public Text levelText;

    internal void Init(GameObject obj)
    {
        this.gameObject = obj;
        this.transform = this.gameObject.transform;

        this.idText = this.transform.Find("id").GetComponent<Text>();
        this.levelText = this.transform.Find("level").GetComponent<Text>();
    }

    internal void Refresh(Hero heroData)
    {
        this.heroData = heroData;

        this.idText.text = "id:" + this.heroData.id;
        this.levelText.text = "Lv." + this.heroData.level;
    }

    public void OnClickUpdateBtn()
    {
        //升级
    }

    internal void Show()
    {
        this.gameObject.SetActive(true);
    }

    internal void Hide()
    {
        this.gameObject.SetActive(true);
    }
}
