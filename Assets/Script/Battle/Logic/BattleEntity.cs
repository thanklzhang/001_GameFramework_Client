using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum BattleEntityState
{
    Idle = 0,
    Move = 1,
    ReleasingSkill = 2,
    Dead = 3,
}

public class BattleEntity
{
    public int guid;
    public int configId;

    //public Vector3 position;

    public GameObject gameObject;

    public BattleEntityState state = BattleEntityState.Idle;

    //加载相关
    public bool isFinishLoad = false;
    public string path;

    //move
    Vector3 moveTargetPos;
    float moveSpeed;

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
        Logx.Log("StartLoadModel");
        isFinishLoad = false;

        var heroConfig = Table.TableManager.Instance.GetById<Table.EntityInfo>(this.configId);

        var heroResTable = Table.TableManager.Instance.GetById<Table.ResourceConfig>(heroConfig.ModelId);
        //临时组路径 之后会打进 ab 包
        path = "Assets/BuildRes/" + heroResTable.Path + "/" + heroResTable.Name + "." + heroResTable.Ext;
        ResourceManager.Instance.GetObject<GameObject>(path, (obj) =>
         {
             OnLoadModelFinish(obj);
         });
    }

    public void OnLoadModelFinish(GameObject obj)
    {
        Logx.Log("OnLoadModelFinish");
        isFinishLoad = true;
        var position = gameObject.transform.position;
        GameObject.Destroy(gameObject);
        gameObject = obj;
        gameObject.transform.position = position;
        //gameObject = 
    }

    public void SetPosition(Vector3 pos)
    {
        //this.position = pos;
        gameObject.transform.position = pos;
    }

    public void Update(float timeDelta)
    {
        if (state == BattleEntityState.Move)
        {
            var moveVector = moveTargetPos - this.gameObject.transform.position;
            var speed = this.moveSpeed;
            var dir = moveVector.normalized;
            //var dis = moveVector.magnitude;

            var currPos = this.gameObject.transform.position;

            this.gameObject.transform.position = currPos + dir * speed * timeDelta;
        }
    }

    public void Destroy()
    {
        if (isFinishLoad)
        {
            ResourceManager.Instance.ReturnObject(path, gameObject);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }

    }

    internal void StartMove(Vector3 targetPos, float moveSpeed)
    {
        Logx.Log("entity start move : " + this.guid + " will move to : " + targetPos + " by speed : " + moveSpeed);
        state = BattleEntityState.Move;

        this.moveTargetPos = targetPos;
        this.moveSpeed = moveSpeed;

    }

    public void StopMove(Vector3 endPos)
    {
        state = BattleEntityState.Idle;

        this.SetPosition(endPos);
    }

}
