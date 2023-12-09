// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Table;
//
// public class UIConfigInfo
// {
//     public ResIds resId;
//     //public string path;
//     public UIShowLayer showLayer;
// }
//
// public class UIConfigInfoDic
// {
//     ////TODO:抽取出来 用表格数据来驱动
//     //public static Dictionary<Type, UIConfigInfo> configDic = new Dictionary<Type, UIConfigInfo>()
//     //{
//     //    {typeof(LoginUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/LoginUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(LobbyUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/LobbyUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(HeroListUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/HeroListUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(HeroInfoUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/HeroInfoUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(BattleUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/BattleUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(ConfirmUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/ConfirmUI.prefab" , showLayer = UIShowLayer.Top_0}},
//     //    {typeof(MainTaskUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/MainTaskUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(MainTaskStageUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/MainTaskStageUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(BattleResultUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/BattleResultUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(TitleBarUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TitleBarUI.prefab" , showLayer = UIShowLayer.Middle_0}},
//     //    {typeof(TeamRoomListUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TeamRoomListUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(TeamRoomInfoUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TeamRoomInfoUI.prefab" , showLayer = UIShowLayer.Floor_0}},
//     //    {typeof(TipsUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TipsUI.prefab" , showLayer = UIShowLayer.Top_0}},
//     //};
//
//
//     public static Dictionary<Type, UIConfigInfo> configDic = new Dictionary<Type, UIConfigInfo>()
//     {
//         {typeof(LoginUI), new UIConfigInfo() { resId =  ResIds.LoginUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(LobbyUIPre), new UIConfigInfo() { resId =  ResIds.LobbyUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(HeroListUIPre), new UIConfigInfo() { resId =  ResIds.HeroListUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(HeroInfoUIPre), new UIConfigInfo() { resId =  ResIds.HeroInfoUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(BattleUIPre), new UIConfigInfo() { resId =  ResIds.BattleUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(ConfirmUIPre), new UIConfigInfo() {resId =  ResIds.ConfirmUI , showLayer = UIShowLayer.Top_0}},
//         {typeof(MainTaskUIPre), new UIConfigInfo() { resId =  ResIds.MainTaskUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(MainTaskStageUIPre), new UIConfigInfo() { resId =  ResIds.MainTaskStageUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(BattleResultUIPre), new UIConfigInfo() { resId =  ResIds.BattleResultUI , showLayer = UIShowLayer.Top_0}},
//         {typeof(TitleBarUIPre), new UIConfigInfo() {resId =  ResIds.TitleBarUI , showLayer = UIShowLayer.Middle_0}},
//         {typeof(TeamRoomListUIPre), new UIConfigInfo() { resId =  ResIds.TeamRoomListUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(TeamRoomInfoUIPre), new UIConfigInfo() { resId =  ResIds.TeamRoomInfoUI , showLayer = UIShowLayer.Floor_0}},
//         {typeof(TipsUIPre), new UIConfigInfo() { resId =  ResIds.TipsUI , showLayer = UIShowLayer.Top_0}},
//         {typeof(SelectHeroUIPre), new UIConfigInfo() { resId =  ResIds.SelectHeroUI , showLayer = UIShowLayer.Top_0}},
//         {typeof(LoadingUIPre), new UIConfigInfo() { resId =  ResIds.LoadingUI , showLayer = UIShowLayer.Top_0}},
//     };
//
//
//
//
//     public static UIConfigInfo GetInfo<T>()
//     {
//         var type = typeof(T);
//         if (configDic.ContainsKey(type))
//         {
//             return configDic[typeof(T)];
//         }
//         else
//         {
//             //Logx.LogError("the ui is not found : type : " + type);
//             return null;
//         }
//
//     }
//
//
// }
//
