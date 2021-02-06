//using GameModelData;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class MessagerHandler
//{
//    public void HandleMsg(MsgPack pack)
//    {
//        var msgId = pack.msgId;
//        var data = pack.data;

//        Loom.QueueOnMainThread(() =>
//        {
//            switch (msgId)
//            {
//                case (int)GS2GC.MsgId.Gs2GcVerificationAskLoginResult:
//                    VerificationLogin(data);
//                    break;
//                case (int)GS2GC.MsgId.Gs2GcVerifyTokenResult:
//                    Gs2GcVerifyTokenResult(data);
//                    break;
//                case (int)GS2GC.MsgId.Gs2GcFromCsAskEnterGameServiceResult:
//                    EnterGame(data);
//                    break;
//                    //case (int)GS2GC.MsgId.Gs2GcFromCsSyncPlayerBaseInfo:
//                    //    HandleSyncPlayerBaseInfo(data);
//                    //    break;
//                    //case (int)GS2GC.MsgId.Gs2GcFromCsSyncHeroListInfo:
//                    //    HandleSyncHeroesInfo(data);
//                    //    break;
//                    //case (int)GS2GC.MsgId.Gs2GcFromCsSyncItemListInfo:
//                    //    HandleSyncItemsInfo(data);
//                    //    break;
//                    //case (int)GS2GC.MsgId.Gs2GcFromCsSyncDrawingListInfo:
//                    //    HandleSyncDrawingsInfo(data);
//                    //    break;
//                    //case (int)GS2GC.MsgId.Gs2GcFromCsSyncManagementInfo:
//                    //    HandleSyncManagementInfo(data);
//                    //    break;
//                    //case (int)GS2GC.MsgId.Gs2GcFromCsPostManagementResult:
//                    //    EnterManagemanetResult(data);
//                    //    break;
//                    //case (int)GS2GC.MsgId.Gs2GcFromCsStartCombat:
//                    //    StartCombat(data);
//                    //    break;
//            }
//        });


//    }

//    /// <summary>
//    /// 验证登录
//    /// </summary>
//    public void VerificationLogin(byte[] data)
//    {
//        GS2GC.VerificationAskLoginResult result = GS2GC.VerificationAskLoginResult.Parser.ParseFrom(data);
//        Debug.Log("token :" + result.Token);
//        //LoginController.Instance.LoginResult(result.IsSuccess, result.UserAccount, result.UserId, result.Token, result.GateServerIp,
//        //    result.GateServerPort);

//    }

//    public void Gs2GcVerifyTokenResult(byte[] data)
//    {
//        GS2GC.VerifyTokenResult result = GS2GC.VerifyTokenResult.Parser.ParseFrom(data);
//        //LoginController.Instance.VerifyTokenResult(result.IsSuccess);

//    }

//    public void EnterGame(byte[] data)
//    {
//        GS2GC.AskEnterGameServiceResult enterResult = GS2GC.AskEnterGameServiceResult.Parser.ParseFrom(data);
//        //Messenger.Broadcast(GameEvent.ExitLogin);
//        //var user = enterResult.UserInfo;
//        //var serverInfo = enterResult.ManagementInfo;
//        //Debug.Log("total bench num : " + serverInfo.TotalUseBenchNum);
//        //var info = ManagementInfo.Create((ManagementState)serverInfo.State, serverInfo.LastStartTime, serverInfo.LastFinishTime,serverInfo.TotalUseBenchNum);

//        //LobbyController.Instance.SetUserData(user.Account, user.NickName, user.Level, user.Coin, user.PortraitURL, info);
//        //LobbyController.Instance.Enter();

//        //卸载 login 资源
//        //GameResource.Instance.UnloadLoginResource();

//        //加载 lobby 资源
//        //GameResource.Instance.LoadLobbyResource();

//        //UIManager.Instance.GoToCombat(UIType.LoginUI);
//        GameStateManager.Instance.ChangeState(GameState.Lobby);

//        Debug.Log("enter game result");

//    }


//    //void HandleSyncPlayerBaseInfo(byte[] data)
//    //{
//    //    GS2GC.PlayerBaseInfo info = GS2GC.PlayerBaseInfo.Parser.ParseFrom(data);
//    //    PlayerData.Instance.SetUserBaseData(info);
//    //}

//    //void HandleSyncHeroesInfo(byte[] data)
//    //{
//    //    GS2GC.HeroListInfo info = GS2GC.HeroListInfo.Parser.ParseFrom(data);
//    //    PlayerData.Instance.SetHeroes(info);
//    //}

//    //void HandleSyncItemsInfo(byte[] data)
//    //{
//    //    GS2GC.ItemListInfo info = GS2GC.ItemListInfo.Parser.ParseFrom(data);
//    //    PlayerData.Instance.SetItems(info);
//    //}

//    //void HandleSyncDrawingsInfo(byte[] data)
//    //{
//    //    GS2GC.DrawingListInfo info = GS2GC.DrawingListInfo.Parser.ParseFrom(data);
//    //    PlayerData.Instance.SetDrawings(info);
//    //}

//    //void HandleSyncManagementInfo(byte[] data)
//    //{
//    //    GS2GC.ManagementInfo info = GS2GC.ManagementInfo.Parser.ParseFrom(data);
//    //    ManagementData.Instance.SetManagementInfo(info);
//    //}


//    //void EnterManagemanetResult(byte[] data)
//    //{
//    //    GS2GC.PostManagementResult result = GS2GC.PostManagementResult.Parser.ParseFrom(data);

//    //    var serverItems = result.ItemList;
//    //    Debug.Log(serverItems.Count);
//    //    List<GameModel.Item> items = serverItems.Select(sItem =>
//    //    {
//    //        GameModel.Item item = GameModel.Item.Create(sItem);
//    //        return item;
//    //    }).ToList();

//    //    //ManagementResultController.Instance.SetInfo(items);

//    //    //ManagementResultController.Instance.Enter();

//    //}

//    void StartCombat(byte[] data)
//    {
//        GS2GC.StartCombat startCombat = GS2GC.StartCombat.Parser.ParseFrom(data);
//        //convert startCombat
//        GlobaData.Instance.combatInitInfo = new GameModelData.CombatInitInfo();

//        GlobaData.Instance.combatInitInfo.currCombatId = startCombat.CombatId;
//        GlobaData.Instance.combatInitInfo.localSeat = startCombat.Seat;
//        Debug.Log("local seat : " + GlobaData.Instance.combatInitInfo.localSeat);

//        GlobaData.Instance.combatInitInfo.combatHeroInfos = new List<CombatHeroInitInfo>();
//        for (int i = 0; i < startCombat.CombatHeroes.Count; ++i)
//        {
//            var currHero = startCombat.CombatHeroes[i];

//            GlobaData.Instance.combatInitInfo.combatHeroInfos.Add(new CombatHeroInitInfo()
//            {
//                seat = currHero.Seat,
//                SN = currHero.SN,
//                team = currHero.Team
//            });
//        }

//        GameLogic.Instance.StartCombat();

//    }
//}

