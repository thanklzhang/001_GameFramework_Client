using System;
using System.Collections;
using System.Collections.Generic;
using Battle_Client;
using Table;
using UnityEngine;
using UnityEngine.UI;


//战斗宝箱界面（技能书列表 学习技能 合成技能）
public class BattleBoxUI
{
    public GameObject gameObject;
    public Transform transform;

    private Button closeBtn;
    private BattleUI battleUI;

    private Transform selectionRoot;

    private List<BattleBoxCell> boxCellShowList;

    private Text boxCountText;


    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        selectionRoot = this.transform.Find("root/select/rewardScroll/mask/content");

        boxCountText = this.transform.Find("root/select/count").GetComponent<Text>();

        closeBtn = transform.Find("root/select/closeBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);

        EventDispatcher.AddListener(EventIDs.OnBoxOpen, OnOpenBox);

        boxCellShowList = new List<BattleBoxCell>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void CheckBoxState()
    {
        var entity = BattleManager.Instance.GetLocalCtrlHero();
        if (null == entity)
        {
            return;
        }

        entity.TryOpenBox();
    }

    void OnOpenBox()
    {
        this.Show();

        this.RefreshSelectionListShow();
    }

    public void RefreshAllUI()
    {
        RefreshSelectionListShow();
    }


    public void RefreshSelectionListShow()
    {
        var entity = BattleManager.Instance.GetLocalCtrlHero();
        if (null == entity)
        {
            return;
        }

        if (entity.currOpenBox != null)
        {
            var selections = entity.currOpenBox.selections;

            boxCellShowList.Clear();

            for (int i = 0; i < selections.Count; i++)
            {
                var data = selections[i];
                GameObject go = null;
                if (i < selectionRoot.childCount)
                {
                    go = selectionRoot.GetChild(i).gameObject;
                }
                else
                {
                    go = GameObject.Instantiate(selectionRoot.GetChild(0).gameObject,
                        selectionRoot, false);
                }

                BattleBoxCell cell = new BattleBoxCell();
                cell.Init(go, this);
                cell.Show();
                cell.RefreshUI(data,i);

                boxCellShowList.Add(cell);
            }

            for (int i = selections.Count; i < selectionRoot.childCount; i++)
            {
                selectionRoot.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    void OnClickCloseBtn()
    {
        this.Hide();
    }

    public void OnSelectSelection(BattleBoxCell boxCell)
    {
        this.Hide();

        var selectIndex = boxCell.index;
        BattleManager.Instance.MsgSender.Send_SelectBoxReward(selectIndex);
    }

    public void OnRefreshBoxInfo()
    {
        this.boxCountText.text = "";
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
        EventDispatcher.RemoveListener(EventIDs.OnBoxOpen, OnOpenBox);

        // for (int i = 0; i < boxCellShowList.Count; i++)
        // {
        //     var cell = boxCellShowList[i];
        //     cell.Release();
        // }
    }
}