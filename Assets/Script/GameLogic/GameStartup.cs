using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

//正常游戏流程启动
public class GameStartup : MonoBehaviour
{
    public bool isLocalBattleTest = false;
    public bool isUseAB = false;
    public bool isUseInternalAB;

    public ServerTypeUI serverTypeUI;

    public GameObject initOperateRoot;

    public GameObject updateResourceRootGo;

    UpdateResourceModule updateResModule;
    UpdateResourceUI updateResourceUI;



    //public Texture2D selectCursor;
    void Awake()
    {
        Logx.Log(LogxType.Game,"Init");
        
        DontDestroyOnLoad(this.gameObject);

        serverTypeUI.startUp = this;
        //this.resourceUpdate = this.GetComponent<ResourceUpdate>();

        updateResModule = new UpdateResourceModule();

        updateResourceUI = new UpdateResourceUI();
        updateResourceUI.Init(updateResourceRootGo, updateResModule);

      
        
    }

    // Start is called before the first frame update
    void Start()
    {
        HandlePreCompileVar();
     
        //// test http
        //StartCoroutine(xxx());
        //return;
        ////

      
        //Startup();
        initOperateRoot.gameObject.SetActive(true);
        var formal = false;
        if (formal)
        {
            Logx.Log(LogxType.Game,"mode : Formal Game");
            
            //正式环境
            Startup();
        }
        else
        {
            if (Const.isLocalBattleTest)
            {
                //本地战斗 供测试使用
                Logx.Log(LogxType.Game,"mode : local battle test");
                this.Startup();
            }
            else
            {
                //局域网模式 本地需要开启所有本地服务器
                // Const.isLANServer = true;
                Logx.Log(LogxType.Game,"mode : LAN Game");
                
                serverTypeUI.gameObject.SetActive(true);
                //this.Startup(); 
            }
        }
    }

    //处理预编译变量
    void HandlePreCompileVar()
    {
        
        Const.isLocalBattleTest = false;
        Const.isUseAB = false;
        Const.isUseInternalAB = false;
        
        
#if UNITY_EDITOR
        Const.isLocalBattleTest = this.isLocalBattleTest;
        Const.isUseAB = this.isUseAB;
        Const.isUseInternalAB = this.isUseInternalAB;
#else 
    #if IS_LOCAL_BATTLE
        Const.isLocalBattleTest = true;
    #endif
            
    #if IS_USE_AB
        Const.isUseAB = true;
    #endif
            
    #if IS_USE_INTERNAL_AB
        Const.isUseInternalAB = true;
    #endif
#endif
        

        
    }

    IEnumerator xxx()
    {
        yield return yyy();
        Debug.Log("xxx 1");

    }

    IEnumerator yyy()
    {
        Debug.Log("yyy 1");
        yield break;
    }

    public void Startup()
    {
        StartCoroutine(_Startup());
    }
    public IEnumerator _Startup()
    {
        //updateResourceUI.gameObject.SetActive(true);

        updateResourceUI.Show();
        //检查游戏资源并更新
        Logx.Log(LogxType.Game,"update resource start");
        UpdateResError updateResError = new UpdateResError();
        yield return updateResModule.CheckResource(updateResError);

        if (updateResError.IsError())
        {
            Logx.LogError("更新资源过程中出错 停止更新 : 原因 : " + updateResError.errInfo);
            updateResModule.TriggerStateEvent(UpdateResStateType.Error, updateResError.errInfo);
            yield break;
        }
        
        Logx.Log(LogxType.Game,"update resource finish");

        //资源更新完成
        initOperateRoot.gameObject.SetActive(false);

        //正式开始游戏 开始加载游戏资源
        var gameInitPrefab = Resources.Load("GameInit") as GameObject;
        var go = Instantiate(gameInitPrefab);
        var gameMain = go.GetComponent<GameMain>();

      
        
        yield return gameMain.GameInit(() =>
        {
          
            //游戏初始化完毕 开始游戏逻辑
            if (!Const.isLocalBattleTest)
            {
                gameMain.StartToLogin();
            }
            else
            {
                gameMain.StartLocalBattle();
            }
        });

        //gameMain.selectCursor = selectCursor;
    }

    private void OnDestroy()
    {
        this.updateResourceUI.Release();
    }
}
