using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleGameDataStore : GameDataStore
{
    public void SetBattleInitData(NetProto.BattleInitArg battleInitArg)
    {
        Logx.Log("SetBattleInitData");
    }
}
