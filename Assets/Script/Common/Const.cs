using FixedPointy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Const
{
    public static Dictionary<int, Color> colorDic = new Dictionary<int, Color>()
    {
        //{(int)EnumColor.White,Color.white },
        // {(int)EnumColor.Green,Color.green },
        // {(int)EnumColor.Blue,Color.blue },
        // {(int)EnumColor.Purple,new Color(0.55f,0,0.775f) },
        // {(int)EnumColor.Orange,new Color(1,0.47f,0.071f)},
    };

    public static Color GetColor(int index)
    {
        return colorDic[index];
    }


    //path
    public const string AppName = "Jeko";
    public static string AppStreamingAssetPath = Application.streamingAssetsPath;//游戏第一次安装的游戏内部包
                                                                                 //public static string AssetBundlePath = Application.persistentDataPath + "/" + AppName + "/Resource";
    public static string AssetBundlePath = Application.persistentDataPath + "/" + AppName + "";///Resources
    //public static string AssetBundlePath = Application.streamingAssetsPath;//方便测试
    public const string ExtName = ".ab";
    //public static bool isUpdateMode = false;//是否开启更新模式(需要开启资源服务端)
    public static bool isUseAB = true;//资源是否从 AB 中读取 不是的话 从 Resource 目录读取

    public static string projectRootPath = "Assets";
    //不用了
    public static string buildPath = "Assets/BuildRes";
    //public static string ProjectResPath = "Assets/Resources";
    //public static string AssetsPath = "Assets";

    public const string WebUrl = "http://127.0.0.1:8080/";
    public const string ip = "196.168.1.100";
    public const int port = 9595;

    public const string ResourceVersion = "1.00";

    public const string combatServerIP = "127.0.0.1";
    public const int combatServerPort = 2500;
    public const int combatClientBindPort = 2349;

    //heartBeat
    public const int heartBeatInterval = 5000;
    public const int sendHeartBeatMsgId = 100;
    public const int receiveHeartBeatMsgId = 101;

    //event name
    //public const string Login = "Login";
    //public const string ShowRoomList = "ShowRoomList";
    //public const string ShowRoom = "ShowRoom";
    //public const string ShowReady = "ShowReady";
    //public const string GetReadyInfo = "GetReadyInfo";
    //public const string ShowCombat = "ShowCombat";

    //public const string EnterMain = "EnterMain";

    public const int frameTime = 66;

    public const int floatMul = 10000;


    public static Fix timeDelta = Fix.Ratio(frameTime, 1000);

    ////--- Resource Path
    ////--------------------------------------------
    ////UIPrefab

    ////login
    //public static string LoginUIPath = "UIPrefab/LoginUI";

    ////lobby
    //public static string LobbyUIPath = "UIPrefab/LobbyUI";
    //public static string HeroListUIPath = "UIPrefab/HeroListUI";
    //public static string ItemListUIPath = "UIPrefab/ItemListUI";
    //public static string ManagementMainUIPath = "UIPrefab/ManagementMainUI";
    //public static string ManagementResultUIPath = "UIPrefab/ManagementResultUI";
    //public static string ManagementBenchUIPath = "UIPrefab/ManagementBenchUI";
    //public static string ManagementSelectDrawingUIPath = "UIPrefab/ManagementSelectDrawingUI";
    //public static string ArenaUIPath = "UIPrefab/ArenaUI";

    ////combat
    //public static string CombatUIPath = "UIPrefab/CombatUI";
    ////--------------------------------------------

    ////SpriteAtlas

    ////heroPortrait
    //public static string HeroPortraitPath = "UISprite/HeroHeadPortrait";
    ////heroBodyPic
    //public static string HeroBodyPicPath = "UISprite/HeroBodyPic";
    ////event
    //public static string EventSpritePath = "UISprite/Event";
    ////item
    //public static string ItemSpritePath = "UISprite/Item";
    ////drawing
    //public static string DrawingSpritePath = "UISprite/Drawing";

    ////--------------------------------------------
    ////Prefab
    ////这里之后可能会放到表中读取
    //public static string Entity10000Path = "Entity/Hero_10000";
    //public static string Entity10003Path = "Entity/Hero_10003";
    ////-------------------------------------------------------------------------


    ////Static

    ////font
    //public static string FontPath = "Font";

    ////sprite
    //public static string BgStaticPath = "Texture/Bg";
    //public static string CommonStaticSpritePath0 = "Texture/CommonSprite0";//图多了的话 增加 common2 3 等 (先不批量 批量不好自行管理)
    //public static string LobbyStaticPath = "Texture/LobbySprite";


    //////UI path(prefab)
    ////public const string LoginUIPath = "/UIPrefab/LoginUI";
    ////public const string LobbyUIPath = "/UIPrefab/LobbyUI";



    ////UIObjPrefab
    //public static string itemObjPath = "UIObjPrefab/item";
    //public static string HeroInfoObjPath = "UIObjPrefab/HeroInfo";


    ////combat test
    //public const string CombatStart = "CombatStart";
    //public const string SyncInfo = "SyncInfo";
    //public const string HeroIdle = "HeroIdle";
    //public const string HeroMove = "HeroMove";
    //public const string HeroReleaseSkill = "HeroReleaseSkill";
    //public const string HeroDead = "HeroDead";
    //public const string SyncHpMp = "SyncHpMp";
    //public const string SkillEffectProjectileCreate = "SkillEffectProjectileCreate";
    //public const string SkillEffectCalculateCreate = "SkillEffectCalculateCreate";
    //public const string SkillProjectileDestroy = "SkillProjectileDestroy";
    //public const string EntityInitFinish = "EntityInitFinish";
    //public const string SoldierCreate = "SoldierCreate";
    //public const string TowerCreate = "TowerCreate";
    //public const string HomeCreate = "HomeCreate";
    //public const string BuffCreate = "BuffCreate";
    //public const string BuffDestroy = "BuffDestroy";
    //public const string AttributeChange = "AttributeChange";
    //public const string SkillSwitchOpen = "SkillSwitchOpen";
    //public const string SkillSwitchClose = "SkillSwitchClose";
    //public const string SkillMoveCreate = "SkillMoveCreate";
    //public const string SkillMoveDestroy = "SkillMoveDestroy";
    //public const string SkillRangeCreate = "SkillRangeCreate";
    //public const string SkillRangeDestroy = "SkillRangeDestroy";




}
