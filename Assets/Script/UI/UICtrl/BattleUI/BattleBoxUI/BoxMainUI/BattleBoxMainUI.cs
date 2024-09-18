﻿using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;


//战斗宝箱界面
public partial class BattleBoxMainUI
{
    public GameObject gameObject;
    public Transform transform;

    private BattleUI battleUI;


    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Update(float deltaTime)
    {
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Release()
    {
    }
}