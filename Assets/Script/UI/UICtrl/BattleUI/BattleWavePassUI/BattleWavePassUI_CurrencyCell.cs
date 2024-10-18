using Battle_Client;
using UnityEngine;
using UnityEngine.UI;

//战斗当前波结算UI 中的宝箱
public class BattleWavePassUI_CurrencyCell
{
    private GameObject gameObject;
    private Transform transform;

    private WavePassCurrency_RecvMsg data;
   
    public void Init()
    {
      
    }

    public void Show()
    {
      
    }

    public void RefreshUI(WavePassCurrency_RecvMsg data)
    {
        this.data = data;
    }

    public void Hide()
    {
      
    }
}