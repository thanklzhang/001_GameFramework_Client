using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleGameDataStore : GameDataStore
{
    //是否所有人都加载好了
    private bool isAllPlayerLoadFinish = false;

    public bool IsAllPlayerLoadFinish { get => isAllPlayerLoadFinish; set => isAllPlayerLoadFinish = value; }

    public void SetBattleInitData(NetProto.BattleInitArg battleInitArg)
    {
        Logx.Log("SetBattleInitData");
    }
}
