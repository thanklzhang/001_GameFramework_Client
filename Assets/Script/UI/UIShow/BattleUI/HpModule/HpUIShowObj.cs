

using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUIShowObj
{
    HpUIData data;
    GameObject gameObject;
    Transform transform;

    public Transform bgRoot;
    public Transform hp;
    public Text valueText;


    HpUIMgr hpUIMgr;
    RectTransform parentTranRect;
    EntityHpColorSelector colorSelector;
    public Image hpBg;
    public void Init(GameObject gameObject, HpUIMgr hpUIMgr)
    {
        this.gameObject = gameObject;
        this.hpUIMgr = hpUIMgr;
        parentTranRect = hpUIMgr.transform.GetComponent<RectTransform>();
        this.transform = this.gameObject.transform;

        bgRoot = this.transform.Find("hpMain/bg");
        hp = this.transform.Find("hpMain/bg/hpFill");
        // hpBg = hp.GetComponent<Image>();
        valueText = this.transform.Find("hpMain/valueText").GetComponent<Text>();
        colorSelector = hp.GetComponent<EntityHpColorSelector>();

    }

    public void Refresh(HpUIData hpData)
    {
        var preCurrHp = -1f;
        if (this.data != null)
        {
            preCurrHp = this.data.preCurrHp;
        }

        this.data = hpData;

        var currHp = this.data.nowCurrHp;
        var maxHp = this.data.maxHp;

        if (0 == maxHp)
        {
            maxHp = 1;
        }

        var ratio = currHp / maxHp;

        if (preCurrHp >= 0)
        {
            //飘字
            var changeHp = currHp - preCurrHp;

            var word = "" + changeHp;
            if (changeHp > 0)
            {
                word = "+" + changeHp;
            }
            var go = this.data.entityObj;
            int floatStyle = 0;
            var fromEntityGuid = this.data.valueFromEntityGuid;
            if (fromEntityGuid > 0)
            {
                var currEntityPos = go.transform.position;
                var fromEntity = BattleEntityManager.Instance.FindEntity(this.data.valueFromEntityGuid);
                if (fromEntity != null)
                {
                    var fromEntityPos = fromEntity.gameObject.transform.position;
                    var dir = (fromEntityPos - currEntityPos).normalized;
                    if (dir.x > 0)
                    {
                        //受到来自左边敌人的攻击 那么飘字向右
                        floatStyle = 1;
                    }
                    else
                    {
                        floatStyle = 0;
                    }
                }

                Color color = normalDamageColor;
                if (changeHp > 0)
                {
                    color = addHpColor;
                }

                hpUIMgr.ShowFloatWord(word, go, floatStyle, color);
            }


        }

        var width = bgRoot.GetComponent<RectTransform>().rect.width;
        var currLen = width * ratio;

        var rect = hp.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(currLen, rect.sizeDelta.y);
        // Logx.Log("zxyzxy : " + rect.sizeDelta + " " + currLen);

        valueText.text = "" + currHp + "/" + maxHp;

        //背景颜色和字体颜色
        var relationType = this.data.relationType;

        // if (relationType == EntityRelationType.Self)
        // {
        //     hpBg.color = this.colorSelector.selfBgColor;
        //     valueText.color = this.colorSelector.selfTextColor;
        //
        // }
        // else if (relationType == EntityRelationType.Enemy)
        // {
        //     hpBg.color = this.colorSelector.enemyBgColor;
        //     valueText.color = this.colorSelector.enemyTextColor;
        // }
        // else if (relationType == EntityRelationType.Friend)
        // {
        //     hpBg.color = this.colorSelector.friendBgColor;
        //     valueText.color = this.colorSelector.friendTextColor;
        // }


    }

    Color normalDamageColor = new Color(1, 0.744f, 0.108f);
    Color addHpColor = new Color(0, 1, 0, 1);

    public void Update(float timeDelta)
    {
        var entityObj = this.data.entityObj;
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
        var screenPos = RectTransformUtility.WorldToScreenPoint(camera3D.camera, entityObj.transform.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTranRect, screenPos, cameraUI.camera, out uiPos);

        //这里可以换成实体上的血条挂点
        this.transform.localPosition = uiPos + Vector2.up * 100;
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    internal void SetShowState(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}

