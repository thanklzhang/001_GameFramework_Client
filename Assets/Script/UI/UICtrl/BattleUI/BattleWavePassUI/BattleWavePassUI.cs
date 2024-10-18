using System.Collections.Generic;
using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

//战斗当前波结算UI
public partial class BattleWavePassUI
{
    public GameObject gameObject;
    public Transform transform;

    private BattleUI battleUI;
    
    private Button closeBtn;
    private Button confirmBtn;
    private Text passTotalTimeText;
    private Transform currencyRoot;
    private Transform boxRoot;

    private BattleWavePass_RecvMsg_Arg data;

    private List<BattleWavePassUI_CurrencyCell> currencyShowList;
    private List<BattleWavePassUI_BoxCell> cboxShowList;
    
    public void Init(GameObject gameObject, BattleUI battleUI)
    {
        this.battleUI = battleUI;
        this.gameObject = gameObject;
        this.transform = this.gameObject.transform;

        closeBtn = transform.Find("root/select/closeBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnClickCloseBtn);
    }

    public void ShowByData(BattleWavePass_RecvMsg_Arg battleWavePassRecvMsgArg)
    {
        this.data = battleWavePassRecvMsgArg;
        this.Show();
        this.RefreshAllUI();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void RefreshAllUI()
    {
        if (null == this.data)
        {
            return;
        }
    }

    void OnClickCloseBtn()
    {
        this.Hide();
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