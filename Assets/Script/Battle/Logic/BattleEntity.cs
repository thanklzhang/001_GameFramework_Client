using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleEntity
{
    public int guid;
    public int configId;

    //public Vector3 position;

    public GameObject gameObject;

    public void Init(int guid, int configId)
    {
        this.guid = guid;
        this.configId = configId;

        //load
        //先拿一个简易的模型暂时放这 然后等真正模型下载好之后在替换即可
        var asset = GameMain.Instance.tempModelAsset;
        gameObject = GameObject.Instantiate(asset);
        this.StartLoadModel();
    }

    public void StartLoadModel()
    {

    }

    public void OnLoadModelFinish()
    {
        GameObject.Destroy(gameObject);
        gameObject = null;
        //gameObject = 
    }

    public void SetPosition(Vector3 pos)
    {
        //this.position = pos;
        gameObject.transform.position = pos;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
