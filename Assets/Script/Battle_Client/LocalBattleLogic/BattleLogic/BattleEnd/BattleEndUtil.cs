using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using Battle;
using Google.Protobuf.Collections;

//申请战斗所用的工具类
public class BattleEndUtil
{
    //申请战斗通用参数 转化为 战斗逻辑所需战斗参数(前后端通用)
    public static NetProto.ApplyBattleEndArg MakeApplyBattleArgProto(Battle.Battle battle, int winTeam)
    {
        //Logx.Log("MakeApplyBattleArgProto : battle room id : " + battle.RoomId + " , winTeam : " + winTeam);
       
        var applyBattleEndArg = new ApplyBattleEndArg();
        var allPlayers = battle.GetAllPlayers();
        foreach (var player in allPlayers)
        {
            var uid = player.uid;
            var playerEndInfo = new PlayerBattleEndInfo()
            {
                Uid = (int)uid
            };
            var team = player.team;
            var isWin = team == winTeam;
            playerEndInfo.IsWin = isWin ? 1 : 0;
            applyBattleEndArg.PlayerEndInfoList.Add(playerEndInfo);
        }
        applyBattleEndArg.RoomId = battle.RoomId;
        //batleEnd.StageId = stageId;
        applyBattleEndArg.BattleTableId = battle.tableId;

        return applyBattleEndArg;

    }

}

