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

    //title
    OnTitleBarClickCloseBtn = 100900,

    //battle-------------
    OnCreateBattle = 101001,
    OnAllPlayerLoadFinish = 1010002,
    OnBattleStart = 1010003,

    OnCreateEntity = 101101,
    OnChangeEntityBattleData = 101102,
    OnEntityDestroy = 101103,           //实体销毁(死亡之后)
    OnBattleEnd = 101104,
    OnEntityChangeShowState = 101105,   //实体改变了显隐时

    OnSetEntityPosition = 101201,
    OnSkillInfoUpdate = 101202,         //当有技能信息改变的时候
    OnBuffInfoUpdate = 101203,         //当有buff信息改变的时候
    OnSkillTrackStart = 101204,         //当有技能轨迹开始的时候
    OnSkillTrackEnd = 101205,         //当有技能轨迹结束的时候
    //

    //UI-------------
    On_UIAttrOption_PointEnter = 101501,
    On_UIAttrOption_PointExit = 101502,
    On_UISkillOption_PointEnter = 101503,
    On_UISkillOption_PointExit = 101504,
    On_UIBuffOption_PointEnter = 101505,
    On_UIBuffOption_PointExit = 101506,
    //

}
