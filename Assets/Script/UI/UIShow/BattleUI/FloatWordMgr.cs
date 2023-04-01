using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatWordMgr
{
    public Transform root;
    public GameObject floatWordPrefab;
    int initInstanceCount = 10;
    public Dictionary<int, FloatWord> floatWordDic;
    public void Init(Transform root)
    {
        this.root = root;
        this.floatWordPrefab = root.Find("word").gameObject;

        floatWordPrefab.SetActive(false);

        floatWordDic = new Dictionary<int, FloatWord>();

        for (int i = 0; i < initInstanceCount; i++)
        {
            AddNew();
        }
    }

    FloatWord AddNew()
    {
        GameObject newGo = GameObject.Instantiate(floatWordPrefab, root, false);

        FloatWord fw = new FloatWord();
        fw.Init(newGo, this.root);

        floatWordDic.Add(newGo.GetInstanceID(), fw);

        return fw;
    }

    public void ShowFloatWord(string word, GameObject followGo, int floatStyle, Color color)
    {
        var fw = FindCanUse();
        if (null == fw)
        {
            fw = AddNew();
        }

        fw.Start(word, followGo, floatStyle, color);
    }

    public FloatWord FindCanUse()
    {
        foreach (var item in floatWordDic)
        {
            if (!item.Value.IsUsing)
            {
                return item.Value;
            }
        }

        return null;
    }

    public void Update(float deltaTime)
    {
        foreach (var item in floatWordDic)
        {
            if (item.Value.IsUsing)
            {
                item.Value.Update(deltaTime);
            }
        }
    }
}