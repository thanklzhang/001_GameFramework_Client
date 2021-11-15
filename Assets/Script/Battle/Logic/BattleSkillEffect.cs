using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum BattleSkillEffectState
{
    Idle = 0,
    Move = 1,
    WillDestroy = 2,
    Destroy = 3
}

public class BattleSkillEffect
{
    public int guid;
    //public int configId;
    public int resId;

    //public Vector3 position;

    public GameObject gameObject;

    public BattleSkillEffectState state = BattleSkillEffectState.Idle;

    //加载相关
    public bool isFinishLoad = false;
    public string path;

    //move
    Vector3 moveTargetPos;
    float moveSpeed;
    private int targetGuid;

    ////技能信息
    //List<BattleSkillInfo> skills;

    float currLastTime = 0.0f;
    float totalTotalTime;
    bool isLoop;

    public void Init(int guid, int resId)
    {
        this.guid = guid;
        this.resId = resId;

        //load
        //先拿一个简易的模型暂时放这 然后等真正模型下载好之后在替换即可
        var asset = GameMain.Instance.tempModelAsset;
        gameObject = GameObject.Instantiate(asset);

        // get path
        //var heroConfig = Table.TableManager.Instance.GetById<Table.EntityInfo>(this.configId);

        var resTable = Table.TableManager.Instance.GetById<Table.ResourceConfig>(this.resId);
        //临时组路径 之后会打进 ab 包
        path = "Assets/BuildRes/" + resTable.Path + "/" + resTable.Name + "." + resTable.Ext;

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
        if (this.state == BattleSkillEffectState.Destroy)
        {
            ResourceManager.Instance.ReturnObject(path, gameObject);
            return;
        }
        Logx.Log("BattleSkillEffect : OnLoadModelFinish");
        isFinishLoad = true;
        var position = gameObject.transform.position;
        GameObject.Destroy(gameObject);
        gameObject = obj;
        gameObject.transform.position = position;
        //gameObject = 

        //获取持续时长
        var particles = obj.GetComponentsInChildren<ParticleSystem>();
        if (particles != null && particles.Length > 0)
        {
            var particle = particles[0];
            this.isLoop = particle.main.loop;
            totalTotalTime = particle.main.duration;
            particle.Play();
        }

    }

    //internal void SetSkillList(List<BattleSkillInfo> skills)
    //{
    //    this.skills = skills;
    //}

    public void SetPosition(Vector3 pos)
    {
        //this.position = pos;
        gameObject.transform.position = pos;
    }

    public void Update(float timeDelta)
    {
        if (this.state == BattleSkillEffectState.WillDestroy &&
               this.state == BattleSkillEffectState.Destroy)
        {
            return;
        }

        if (state == BattleSkillEffectState.Move)
        {
            var targetPos = moveTargetPos;

            if (targetGuid > 0)
            {
                //跟随
                var entity = BattleEntityManager.Instance.FindEntity(targetGuid);
                if (entity != null)
                {
                    targetPos = entity.GetPosition();
                }

            }

            var moveVector = targetPos - this.gameObject.transform.position;
            var speed = this.moveSpeed;
            var dir = moveVector.normalized;
            //var dis = moveVector.magnitude;

            var currPos = this.gameObject.transform.position;

            this.gameObject.transform.position = currPos + dir * speed * timeDelta;

        }

        if (isFinishLoad)
        {

            if (!isLoop)
            {
                currLastTime = currLastTime + timeDelta;
                if (currLastTime >= this.totalTotalTime)
                {
                    this.SetWillDestoryState();
                    //BattleSkillEffectManager.Instance.DestorySkillEffect(this.guid);
                }
            }
        }
    }

    public void SetWillDestoryState()
    {
        this.state = BattleSkillEffectState.WillDestroy;
    }

    public void Destroy()
    {
        this.state = BattleSkillEffectState.Destroy;
        if (isFinishLoad)
        {
            ResourceManager.Instance.ReturnObject(path, gameObject);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }

    }

    internal void StartMove(Vector3 targetPos, int targetGuid, float moveSpeed)
    {
        Logx.Log("skill effect start move : " + this.guid + " will move to : " + targetPos + " by speed : " + moveSpeed);
        state = BattleSkillEffectState.Move;

        this.moveTargetPos = targetPos;
        this.moveSpeed = moveSpeed;
        this.targetGuid = targetGuid;



    }

    public void StopMove(Vector3 endPos)
    {
        state = BattleSkillEffectState.Idle;

        this.SetPosition(endPos);
    }

    //internal void ReleaseSkill()
    //{
    //    //play animation
    //    Logx.Log("entity release skill : " + this.guid);
    //}

    //internal int GetSkillIdByIndex(int index)
    //{
    //    if (skills.Count > 0)
    //    {
    //        return skills[index].configId;
    //    }
    //    else
    //    {
    //        Logx.LogWarning("the count of skills is 0 : index : " + index);
    //        return -1;
    //    }

    //}
}
