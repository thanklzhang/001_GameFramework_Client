using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//正常游戏流程启动
public class GameStartup : MonoBehaviour
{
    public bool isLocalBattle = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Startup();
    }

    public void Startup()
    {


        //TODO : 热更

        var gameInitPrefab = Resources.Load("GameInit") as GameObject;
        var go = Instantiate(gameInitPrefab);
        var gameMain = go.GetComponent<GameMain>();
        StartCoroutine(gameMain.GameInit(()=>
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
