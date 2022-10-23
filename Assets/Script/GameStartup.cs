using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//正常游戏流程启动
public class GameStartup : MonoBehaviour
{
    public bool isLocalBattle = false;
    public ServerTypeUI serverTypeUI;
    public UpdateResourceUI updateResourceUI;
    public GameObject initUIRoot;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        serverTypeUI.startUp = this;
        //this.resourceUpdate = this.GetComponent<ResourceUpdate>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Startup();
        initUIRoot.gameObject.SetActive(true);
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

        //资源更新完成
        initUIRoot.gameObject.SetActive(false);

        //正式开始游戏 开始加载游戏资源
        var gameInitPrefab = Resources.Load("GameInit") as GameObject;
        var go = Instantiate(gameInitPrefab);
        var gameMain = go.GetComponent<GameMain>();
        StartCoroutine(gameMain.GameInit(() =>
        {
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
