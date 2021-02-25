using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum EnumColor
{
    White,
    Green,
    Blue,
    Purple,
    Orange

}

/// <summary>
/// 需在  1000 - 9999之间 （10000+ 是 net 协议 ， 1 - 999 是基本协议 如 心跳等）
/// </summary>
public enum GameEvent
{
    Null,
    ConnectLoginServerResult = 1000,
    VerifyTokenResult = 1001,


    //AB 加载任务完成
    LoadABTaskFinish,
    //加载资源任务完成
    LoadAssetTaskFinish,

    CombatStart,        //战斗开始 玩家此时可以开始行动了
    CombatRoundActionStart,//回合开始
    CombatRoundShowStart,//回合展示开始
    CombatEnd,       

    ClickEntity,//点击世界中的实体
    //SelectTargetEntity,//选择目标的实体

    //UserClickEntity,

    SelectEntity,
    CancelSelectEntity,

    ////UI
    //EnterLogin,
    //ExitLogin,
    //LoginResult,//登录结果
    ConnectGateServerResult,
    PlayerClickEntity,
    //EnterLobby,//进入大厅
    //ExitLobby,
    //EnterHeroList,
    //ExitHeroList,
    //EnterItemList,
    //ExitItemList,
    //EnterManagementMain,
    //ExitManagementMain,
    //EnterManagementResult,
    //ExitManagementResult,
    //EnterManagementBench,
    //ExitManagementBench,
    //EnterManagementSelectDrawing,
    //ExitManagementSelectDrawing,
    //EnterArena,
    //ExitArena,

    //EnterCombat,
    //ExitCombat,

    //SyncPlayerBaseInfo,
    //UpdatePlayerCoin,


    //UpdateManagementProject,
    //UpdateManagementBench
}
