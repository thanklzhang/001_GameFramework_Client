using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UIConfigInfo
{
    public string path;
    public UIShowLayer showLayer;
}

public class UIConfigInfoDic
{
    //TODO:抽取出来 用表格数据来驱动
    public static Dictionary<Type, UIConfigInfo> configDic = new Dictionary<Type, UIConfigInfo>()
    {
        {typeof(LoginUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/LoginUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(LobbyUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/LobbyUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(HeroListUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/HeroListUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(HeroInfoUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/HeroInfoUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(BattleUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/BattleUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(ConfirmUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/ConfirmUI.prefab" , showLayer = UIShowLayer.Top_0}},
        {typeof(MainTaskUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/MainTaskUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(MainTaskStageUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/MainTaskStageUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(BattleResultUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/BattleResultUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(TitleBarUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TitleBarUI.prefab" , showLayer = UIShowLayer.Middle_0}},
        {typeof(TeamRoomListUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TeamRoomListUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(TeamRoomInfoUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TeamRoomInfoUI.prefab" , showLayer = UIShowLayer.Floor_0}},
        {typeof(TipsUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/TipsUI.prefab" , showLayer = UIShowLayer.Top_0}},
    };


    public static UIConfigInfo GetInfo<T>()
    {
        var type = typeof(T);
        if (configDic.ContainsKey(type))
        {
            return configDic[typeof(T)];
        }
        else
        {
            //Logx.LogError("the ui is not found : type : " + type);
            return null;
        }

    }


}

