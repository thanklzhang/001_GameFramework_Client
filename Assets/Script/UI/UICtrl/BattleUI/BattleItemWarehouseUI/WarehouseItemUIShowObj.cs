using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarehouseItemUIShowObj
{
    public GameObject gameObject;
    public Transform transform;

    private Transform itemTran;
    private Image itemIconImg;
    private TextMeshProUGUI countText;
    private UIEventTrigger evetnTrigger;
    
    //runtime
    public int index;
    public BattleItemData_Client data;
    private BattleItemWarehouseUI parentUI;

    public void Init(GameObject gameObject,BattleItemWarehouseUI parentUI)
    {
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;
        this.parentUI = parentUI;

        itemTran = this.transform.Find("item");
        this.itemIconImg = itemTran.Find("icon").GetComponent<Image>();

        countText = this.transform.Find("item/countText").GetComponent<TextMeshProUGUI>();

        evetnTrigger = itemIconImg.GetComponent<UIEventTrigger>();
        evetnTrigger.OnPointEnterEvent += OnPointEnter;
        evetnTrigger.OnPointerExitEvent += OnPointExit;
    }

    public void RefreshUI(BattleItemData_Client data, int index)
    {
        this.index = index;
        this.data = data;

        RefreshItemShow();
    }

    void RefreshItemShow()
    {
        if (data != null)
        {
            this.itemTran.gameObject.SetActive(true);
            
            var itemConfig = ConfigManager.Instance.GetById<Config.BattleItem>(this.data.configId);
            var iconResId = itemConfig.IconResId;
            ResourceManager.Instance.GetObject<Sprite>(iconResId, (sprite) => { this.itemIconImg.sprite = sprite; });

            var count = this.data.count;
            countText.text = "" + this.data.count;
            if (count > 1)
            {
                countText.gameObject.SetActive(true);
            }
            else
            {
                countText.gameObject.SetActive(false);
            }
        }
        else
        {
            this.itemTran.gameObject.SetActive(false);
        }
    }

    public void Update(float deltaTime)
    {
    }

    public void OnPointEnter(PointerEventData e)
    {
        //转换成点在 BattleUI 中的 localPosition
    
        var camera3D = CameraManager.Instance.GetCamera3D();
        var cameraUI = CameraManager.Instance.GetCameraUI();
    
        var screenPos = e.position;
    
        Vector2 uiPos;
        var battleUIRect = parentUI.battleUI.gameObject.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(battleUIRect, screenPos, cameraUI.camera, out uiPos);
    
        EventDispatcher.Broadcast<int, Vector2>(EventIDs.On_UIItemOption_PointEnter, this.data.configId, uiPos);
    }

    public void OnPointExit(PointerEventData e)
    {
        EventDispatcher.Broadcast<int>(EventIDs.On_UIItemOption_PointExit, this.data.configId);
    }


    public void Release()
    {
        evetnTrigger.OnPointEnterEvent -= OnPointEnter;
        evetnTrigger.OnPointerExitEvent -= OnPointExit;
    }
}


// public class BattleSkillUIArgs : UICtrlArgs
// {
//     public List<BattleSkillUIData> battleSkillList;
// }

// public class BattleItemUIData
// {
//     public int index;
//     public int configId;
//     public int count;
//
//     public float currCDTime;
//     public float maxCDTime;
//     
//     // public int skillId;
//     // public int iconResId;
//     
// }