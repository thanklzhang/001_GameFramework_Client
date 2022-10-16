using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerTypeUI : MonoBehaviour
{
    public Button remoteStartBtn;
    public Button localStartBtn;
    public InputField localServerIp;
    public InputField localServerPort;
    
    public Text tips;

    private void Awake()
    {
        remoteStartBtn = transform.Find("remote").GetComponent<Button>();
        localStartBtn = transform.Find("local").GetComponent<Button>();
        localServerIp = transform.Find("Ip").GetComponent<InputField>();
        localServerPort = transform.Find("port").GetComponent<InputField>();
        tips = transform.Find("tips").GetComponent<Text>();

        remoteStartBtn.onClick.AddListener(() =>
        {
            tips.text = "暂未开放";
            Const.isLocalServer = false;
        });

        localStartBtn.onClick.AddListener(() =>
        {
            //var ip = localServerIp.text;
            //var port = int.Parse(localServerPort.text);
            Const.isLocalServer = true;
            this.gameObject.SetActive(false);

            var startUp = GameObject.Find("GameStartup").GetComponent<GameStartup>();
            startUp.Startup();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        //localServerIp.text = "192.168.3.13";
        //localServerPort.text = "" + 5556;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
