using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CombatData : Singleton<CombatData>
{
    SyncCombatInitInfo combatInitInfo;

    public void SetCombatInitData(SyncCombatInitInfo initInfo)
    {
        //这里根据战斗的初始信息 进行预加载资源
        this.combatInitInfo = initInfo;

    }

}



