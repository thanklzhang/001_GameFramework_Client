using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 1 - 100000 网络协议  100000+ 是自定义事件
/// </summary>
public enum EventIDs
{
    Null = 0,
    
    //自定义事件
    OnUpgradeHeroLevel = 100001,
    
    //battle
    OnCreateBattle = 101001,
    OnAllPlayerLoadFinish = 1010002,
    OnBattleStart = 1010003,

    OnCreateEntity = 101101,

    OnSetEntityPosition = 101201,

}
