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
    Destroy = 3,
    Destory = 4
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

    //应该在 load temp obj 上加这个 ， 现在这里加上
    public Collider collider;

    //技能信息
    List<BattleSkillInfo> skills;

    public void Init(int guid, int configId)
    {
        this.guid = guid;
        this.configId = configId;

        //load
        //先拿一个简易的模型暂时放这 然后等真正模型下载好之后在替换即可
        var asset = GameMain.Instance.tempModelAsset;
        gameObject = GameObject.Instantiate(asset);

        // get path
        var heroConfig = Table.TableManager.Instance.GetById<Table.EntityInfo>(this.configId);

        var heroResTable = Table.TableManager.Instance.GetById<Table.ResourceConfig>(heroConfig.ModelId);
        //临时组路径 之后会打进 ab 包
        path = "Assets/BuildRes/" + heroResTable.Path + "/" + heroResTable.Name + "." + heroResTable.Ext;

        isFinishLoad = false;

        //this.StartLoadModel();
    }

    //开始自行加载(主要用于创建 entity 的时候自己自行异步加载 )
    public void StartSelfLoadModel()
    {
        Logx.Log("StartLoadModel");
        isFinishLoad = false;
        ResourceManager.Instance.GetObject<GameObject>(path, (obj) =>
        {
            OnLoadModelFinish(obj);
        });
    }

    public void OnLoadModelFinish(GameObject obj)
    {
        Logx.Log("BattleEntity : OnLoadModelFinish");
        isFinishLoad = true;
        var position = gameObject.transform.position;
        GameObject.Destroy(gameObject);
        gameObject = obj;
        gameObject.transform.position = position;
        //gameObject = 
        collider = gameObject.GetComponentInChildren<Collider>();
    }

    internal void SetSkillList(List<BattleSkillInfo> skills)
    {
        this.skills = skills;
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

    internal Vector3 GetPosition()
    {
        return this.gameObject.transform.position;
    }

    public void Destroy()
    {
        this.state = BattleEntityState.Destroy;
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

    internal void ReleaseSkill()
    {
        //play animation
        Logx.Log("entity release skill : " + this.guid);
    }

    internal int GetSkillIdByIndex(int index)
    {
        if (skills.Count > 0)
        {
            return skills[index].configId;
        }
        else
        {
            Logx.LogWarning("the count of skills is 0 : index : " + index);
            return -1;
        }

    }
}
