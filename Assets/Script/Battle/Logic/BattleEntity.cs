using Google.Protobuf.Collections;
using NetProto;
using System;
using System.Collections;
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
    Destroy = 4
}

public enum EntityAttrType
{
    Null = 0,

    //数值
    Attack = 1,
    Defence = 2,
    MaxHealth = 3,
    AttackSpeed = 4,
    MoveSpeed = 5,

    //千分比
    Attack_Permillage = 1001,
    Defence_Permillage = 1002,
    MaxHealth_Permillage = 1003,
    AttackSpeed_Permillage = 1004,
    MoveSpeed_Permillage = 1005,

}

//实体当前数据值类型
public enum EntityCurrValueType
{
    CurrHealth = 1,
    CurrMagic = 2,
}

public class BattleEntityAttr
{
    public int attack;
    public int defence;
    public int maxHealth;
    public float moveSpeed;
}

public class BattleEntity
{
    public int guid;
    public int configId;

    public GameObject gameObject;

    public GameObject tempModel;
    public GameObject model;

    public BattleEntityState state = BattleEntityState.Idle;

    //加载相关
    public bool isFinishLoad = false;
    public string path;

    //move
    Vector3 moveTargetPos;
    //float moveSpeed;

    //应该在 load temp obj 上加这个 ， 现在这里加上
    public Collider collider;

    public int level;
    public int CurrHealth;
    public int MaxHealth
    {
        get
        {
            return attr.maxHealth;
        }

    }
    List<BattleSkillInfo> skills;
    public BattleEntityAttr attr = new BattleEntityAttr();

    public Animation animation;

    public void Init(int guid, int configId)
    {
        this.guid = guid;
        this.configId = configId;

        //load
        //先拿一个简易的模型暂时放这 然后等真正模型下载好之后在替换即可
        var asset = GameMain.Instance.tempModelAsset;
        gameObject = GameObject.Instantiate(asset);

        tempModel = gameObject.transform.Find("Cube").gameObject;

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
        //var position = gameObject.transform.position;
        //GameObject.Destroy(gameObject);
        tempModel.SetActive(false);

        model = obj;
        model.transform.SetParent(this.gameObject.transform);
        model.transform.localPosition = Vector3.zero;

        //gameObject.transform.position = position;
        //gameObject = 
        collider = gameObject.GetComponentInChildren<Collider>();
        animation = gameObject.GetComponentInChildren<Animation>();
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
            var speed = this.attr.moveSpeed;
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
            ResourceManager.Instance.ReturnObject(path, model);
            model.transform.SetParent(null);
        }

        GameObject.Destroy(gameObject);

        EventDispatcher.Broadcast(EventIDs.OnEntityDestroy, this);

    }

    internal void StartMove(Vector3 targetPos, float moveSpeed)
    {
        Logx.Log("entity start move : " + this.guid + " will move to : " + targetPos + " by speed : " + moveSpeed);
        state = BattleEntityState.Move;

        this.moveTargetPos = targetPos;
        this.attr.moveSpeed = moveSpeed;
        PlayAnimation("walk");
    }

    public void StopMove(Vector3 endPos)
    {
        state = BattleEntityState.Idle;
        PlayAnimation("free");
        this.SetPosition(endPos);
    }

    internal void ReleaseSkill()
    {
        //play animation
        PlayAnimation("skill");
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

    internal void SyncAttr(RepeatedField<BattleEntityAttrProto> attrs)
    {
        foreach (var item in attrs)
        {
            var type = (EntityAttrType)item.Type;
            if (type == EntityAttrType.Attack)
            {
                this.attr.attack = item.Value;
            }
            else if (type == EntityAttrType.MaxHealth)
            {
                this.attr.maxHealth = item.Value;
            }
            else if (type == EntityAttrType.MoveSpeed)
            {
                // /1000
                this.attr.moveSpeed = item.Value / 1000.0f;
            }

            Logx.Log("sync entity attr : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.Value);
        }
        EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this);
    }

    internal void SyncValue(RepeatedField<BattleEntityValueProto> values)
    {
        foreach (var item in values)
        {
            var type = (EntityCurrValueType)item.Type;
            var value = item.Value;
            if (type == EntityCurrValueType.CurrHealth)
            {
                this.CurrHealth = value;
            }


            Logx.Log("sync entity curr value : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.Value);
        }
        EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this);

    }

    public void PlayAnimation(string aniName)
    {
        if (isFinishLoad && animation != null)
        {
            animation.Play(aniName);
        }
    }

    public void Dead()
    {
        PlayAnimation("death");
        state = BattleEntityState.Dead;
        CoroutineManager.Instance.StartCoroutine(RemoveSelf());

    }

    public IEnumerator RemoveSelf()
    {
        yield return new WaitForSeconds(2);

        //这里应该是设置成标志 然后删除
        BattleEntityManager.Instance.DestoryEntity(this.guid);
    }
}
