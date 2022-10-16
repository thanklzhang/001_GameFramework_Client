using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultUI : BaseUI
{
    public Action onClickConfirmBtn;

    //当前选中的关卡相关组件--
    Text winContent;
    Button confirmBtn;

    Transform rewardRoot;

    List<CommonItemUIArgs> optionDataList = new List<CommonItemUIArgs>();
    List<ResultOptionShowObj> optionShowList = new List<ResultOptionShowObj>();

    protected override void OnInit()
    {
        this.winContent = this.transform.Find("Panel/Result").GetComponent<Text>();
        this.confirmBtn = this.transform.Find("Panel/ConfirmBtn").GetComponent<Button>();
        this.rewardRoot = this.transform.Find("Panel/reward/scroll/mask/content");

        confirmBtn.onClick.AddListener(() =>
        {
            onClickConfirmBtn?.Invoke();
        });
    }

    public override void Refresh(UIArgs args)
    {
        var resultArgs = (BattleResultUIArgs)args;
        var isWin = resultArgs.isWin;
        var showStr = isWin ? "you win" : "you fail";
        winContent.text = showStr;


        this.optionDataList = resultArgs.uiItem;


        this.RefreshRewardList();
    }

    void RefreshRewardList()
    {
        UIListArgs<ResultOptionShowObj, BattleResultUI> args = new
            UIListArgs<ResultOptionShowObj, BattleResultUI>();
        args.dataList = optionDataList;
        args.showObjList = optionShowList;
        args.root = rewardRoot;
        args.parentObj = this;
        UIFunc.DoUIList(args);

    }

    protected override void OnRelease()
    {
        onClickConfirmBtn = null;
        this.confirmBtn.onClick.RemoveAllListeners();
    }
}

public class BattleResultUIArgs : UIArgs
{
    public bool isWin;
    public List<CommonItemUIArgs> uiItem;
}


//public class ResultOptionUIData
//{
//    public int configId;
//    public int count;
//}

public class ResultOptionShowObj : BaseUIShowObj<BattleResultUI>
{
    Text nameText;
    Text countText;
    RawImage iconRawImg;
    public CommonItemUIArgs uiData;

    int currIconResId;
    Texture currIconTex;

    public override void OnInit()
    {
        nameText = this.transform.Find("root/name").GetComponent<Text>();
        countText = this.transform.Find("root/count").GetComponent<Text>();
        iconRawImg = this.transform.Find("root/icon").GetComponent<RawImage>();
    }

    public override void OnRefresh(object data, int index)
    {

        this.uiData = (CommonItemUIArgs)data;

        var configId = this.uiData.configId;
        var itemTb = TableManager.Instance.GetById<Table.Item>(configId);
        nameText.text = itemTb.Name;
        countText.text = "" + this.uiData.count;

        currIconResId = itemTb.IconResId;
        ResourceManager.Instance.GetObject<Texture>(currIconResId, (tex) =>
        {
            //TODO
            //注意 这里界面关闭了还会再次执行
            //这里应该判断是否界面界面关闭了等状态
            currIconTex = tex;
            iconRawImg.texture = tex;
        });

    }

    public override void OnRelease()
    {
        if (currIconTex != null)
        {
            ResourceManager.Instance.ReturnObject<Texture>(currIconResId, currIconTex);
        }

    }

}
