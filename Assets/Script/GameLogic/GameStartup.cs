using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

//正常游戏流程启动
public class GameStartup : MonoBehaviour
{
    public bool isLocalBattle = false;
    public bool isUseAB = false;

    public ServerTypeUI serverTypeUI;

    public GameObject initOperateRoot;

    public GameObject updateResourceRootGo;

    UpdateResourceModule updateResModule;
    UpdateResourceUI updateResourceUI;



    //public Texture2D selectCursor;
    void Awake()
    {
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
        //// test http
        //StartCoroutine(xxx());
        //return;
        ////

        Const.isUseAB = this.isUseAB;
        //Startup();
        initOperateRoot.gameObject.SetActive(true);
        var formal = false;
        if (formal)
        {
            //正式环境
            Startup();
        }
        else
        {
            serverTypeUI.gameObject.SetActive(true);
        }

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
        UpdateResError updateResError = new UpdateResError();
        yield return updateResModule.CheckResource(updateResError);

        if (updateResError.IsError())
        {
            Logx.LogError("更新资源过程中出错 停止更新 : 原因 : " + updateResError.errInfo);
            updateResModule.TriggerStateEvent(UpdateResStateType.Error, updateResError.errInfo);
            yield break;
        }

        //资源更新完成
        initOperateRoot.gameObject.SetActive(false);

        //正式开始游戏 开始加载游戏资源
        var gameInitPrefab = Resources.Load("GameInit") as GameObject;
        var go = Instantiate(gameInitPrefab);
        var gameMain = go.GetComponent<GameMain>();

        yield return gameMain.GameInit(() =>
        {
            //游戏初始化完毕 开始游戏逻辑
            if (!isLocalBattle)
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
