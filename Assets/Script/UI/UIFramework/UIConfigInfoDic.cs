using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UIConfigInfo
{
    public string path;
}

public class UIConfigInfoDic
{
    public static Dictionary<Type, UIConfigInfo> configDic = new Dictionary<Type, UIConfigInfo>()
    {
         {typeof(LoginUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/LoginUI.prefab" }},
         {typeof(LobbyUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/LobbyUI.prefab" }},
        {typeof(HeroListUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/HeroListUI.prefab" }},
        {typeof(HeroInfoUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/HeroInfoUI.prefab" }},
         {typeof(BattleUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/BattleUI.prefab" }},
          {typeof(ConfirmUI), new UIConfigInfo() { path = "Assets/BuildRes/Prefabs/UI/ConfirmUI.prefab" }},

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

