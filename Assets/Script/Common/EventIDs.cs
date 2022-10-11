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

    //自定义事件--------------
    OnUpgradeHeroLevel = 100001,
    OnRefreshAllMainTaskData = 100051,
    OnPlotEnd = 100071,
    OnRefreshHeroListData = 100101,
    OnRefreshBagData = 100102,
    //team
    OnPlayerEnterTeamRoom = 100131,
    OnPlayerChangeInfoInTeamRoom = 100132,
    OnPlayerLeaveTeamRoom = 100133,

    //battle-------------
    OnCreateBattle = 101001,
    OnAllPlayerLoadFinish = 1010002,
    OnBattleStart = 1010003,

    OnCreateEntity = 101101,
    OnChangeEntityBattleData = 101102,
    OnEntityDestroy = 101103,//实体销毁(死亡之后)
    OnBattleEnd = 101104,
    OnEntityChangeShowState = 101105,//实体改变了显隐时

    OnSetEntityPosition = 101201,
    //


}
