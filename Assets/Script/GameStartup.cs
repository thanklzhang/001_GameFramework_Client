using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

//正常游戏流程启动
public class GameStartup : MonoBehaviour
{
    public bool isLocalBattle = false;
    public bool isUseAB = false;
    public ServerTypeUI serverTypeUI;
    public UpdateResourceUI updateResourceUI;
    public GameObject initOperateRoot;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        serverTypeUI.startUp = this;
        //this.resourceUpdate = this.GetComponent<ResourceUpdate>();
    }

    // Start is called before the first frame update
    void Start()
    {

        ////test http

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
        yield return null;
        //UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8080/test?a=1");
        ////request.SetRequestHeader("Content-Type", "application/json");
        ////request.SetRequestHeader("authKey", "leohui");
        //var r =  request.SendWebRequest();
        //yield return r;

        //var downBytes = request.downloadHandler.data;
        //var str = Encoding.UTF8.GetString(downBytes);
        //Debug.Log("http response : " + str);
    }    

    public void Startup()
    {
        StartCoroutine(_Startup());
    }
    public IEnumerator _Startup()
    {

        //check resource
        updateResourceUI.gameObject.SetActive(true);
        yield return updateResourceUI.CheckPersistentResource();

        //TODO : 热更
        //yield break;

        yield break;

        //资源更新完成
        initOperateRoot.gameObject.SetActive(false);

        //正式开始游戏 开始加载游戏资源
        var gameInitPrefab = Resources.Load("GameInit") as GameObject;
        var go = Instantiate(gameInitPrefab);
        var gameMain = go.GetComponent<GameMain>();
        StartCoroutine(gameMain.GameInit(() =>
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
        }));

    }
}
