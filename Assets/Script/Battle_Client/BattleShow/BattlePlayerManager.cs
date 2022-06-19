using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Battle_Client
{
    public class BattlePlayerManager : Singleton<BattlePlayerManager>
    {

        ////本地玩家
        //private ClientPlayer localPlayer;
        //public ClientPlayer LocalPlayer
        //{
        //    get
        //    {
        //        var userDataStore = GameDataManager.Instance.UserGameDataStore;
        //        var uid = (int)userDataStore.Uid;
        //        if (battleInfo.players.ContainsKey(uid))
        //        {
        //            return battleInfo.players[uid];
        //        }
        //        else
        //        {
        //            Logx.LogError("the uid is not found : " + uid);
        //            return null;
        //        }

        //    }
        //}

        //public void Init()
        //{

        //}

        //public void SetPlayerData()
        //{
        //    //玩家信息
        //    battleInfo.players = new Dictionary<int, ClientPlayer>();
        //    foreach (var serverPlayer in arg.BattlePlayerInitArg.PlayerList)
        //    {
        //        ClientPlayer player = new ClientPlayer()
        //        {
        //            //ctrlHeroGuid = serverPlayer.CtrlHeroGuid,
        //            playerIndex = serverPlayer.PlayerIndex,
        //            team = serverPlayer.Team,
        //            uid = serverPlayer.Uid,
        //            ctrlHeroGuid = serverPlayer.CtrlHeroGuid
        //        };

        //        battleInfo.players.Add(player.uid, player);
        //    }
        //}
    }
}