using Google.Protobuf.Collections;
using NetProto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Battle_Client
{


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
        public float attack;
        public float defence;
        public float maxHealth;
        public float moveSpeed;
    }

    public class BattleEntity
    {
        public int guid;
        public int configId;

        public int playerIndex;

        public GameObject gameObject;

        public GameObject tempModel;
        public GameObject model;

        public BattleEntityState state = BattleEntityState.Idle;

        //加载相关
        public bool isFinishLoad = false;
        public string path;

        //move
        Vector3 moveTargetPos;
        Vector3 dirTarget;
        //float moveSpeed;

        //应该在 load temp obj 上加这个 ， 现在这里加上
        public Collider collider;

        public int level;
        public float CurrHealth;
        public float MaxHealth
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

        string modelRootName = "Model";

        internal Transform FindModelNode(string nodeName)
        {
            return this.model.transform.Find(modelRootName + "/" + nodeName);
        }

        internal void SetToward(Vector3 dir)
        {
            this.dirTarget = dir;
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
            model.transform.localRotation = Quaternion.Euler(0, 0, 0);

            //gameObject.transform.position = position;
            //gameObject = 
            collider = gameObject.GetComponentInChildren<Collider>();
            animation = gameObject.GetComponentInChildren<Animation>();
        }

        internal void SetPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
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

        //internal void StartMove(Vector3 targetPos, Vector3 dir, float moveSpeed)
        //{
        //    //Logx.Log("moveTest : battleClient : " + this.guid + " will move to : " + targetPos + " by speed : " + moveSpeed);
        //    state = BattleEntityState.Move;

        //    this.moveTargetPos = targetPos;
        //    //this.gameObject.transform.forward = dir;
        //    this.attr.moveSpeed = moveSpeed;
        //    PlayAnimation("walk");

        //    dirTarget = dir;
        //}

        public List<Vector3> movePosList;
        int movePosIndex;
        internal void StartMoveByPath(List<Vector3> pathList, float moveSpeed)
        {
            if (pathList.Count > 0)
            {
                state = BattleEntityState.Move;

                this.movePosList = pathList;
                this.attr.moveSpeed = moveSpeed;

                PlayAnimation("walk");

                movePosIndex = 0;
                this.moveTargetPos = pathList[0];

                //设置朝向
                dirTarget = (moveTargetPos - this.gameObject.transform.position).normalized;
            }
        }

        //根据消息来真实的终止移动
        public void StopMove(Vector3 endPos)
        {
            state = BattleEntityState.Idle;

            //if (this.configId == 1200001)
            //{
            //    Logx.Log("moveTest : battleClient : reach : " + endPos.x + " " + endPos.z);

            //}



            PlayAnimation("free");

            if (CheckPosIsForcePullBack(endPos))
            {
                this.SetPosition(endPos);
            }
        }

        //检查当前点和目标的距离是否需要强制拉回
        public bool CheckPosIsForcePullBack(Vector3 checkPos)
        {
            var len = (checkPos - this.GetPosition()).magnitude;

            if (len <= 0.5)
            {
                return false;
            }
            return true;
        }

        public void FakeStopMove(Vector3 endPos)
        {
            state = BattleEntityState.Idle;

            //Logx.Log("moveTest : battleClient : fake reach : " + endPos.x + " " + endPos.y);

            PlayAnimation("free");

            this.SetPosition(endPos);
        }


        internal void ReleaseSkill(int skillConfigId)
        {
            //play animation
            PlayAnimation("attack");
            Logx.Log(this.guid + " release skill : " + skillConfigId);
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

        //internal void SyncAttr(RepeatedField<BattleEntityAttrProto> attrs)
        //{
        //    foreach (var item in attrs)
        //    {
        //        var type = (EntityAttrType)item.Type;
        //        if (type == EntityAttrType.Attack)
        //        {
        //            this.attr.attack = item.Value;
        //        }
        //        else if (type == EntityAttrType.MaxHealth)
        //        {
        //            this.attr.maxHealth = item.Value;
        //        }
        //        else if (type == EntityAttrType.MoveSpeed)
        //        {
        //            // /1000
        //            this.attr.moveSpeed = item.Value / 1000.0f;
        //        }

        //        Logx.Log("sync entity attr : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.Value);
        //    }
        //    EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this);
        //}

        internal void SyncAttr(List<BattleClientMsg_BattleAttr> attrs)
        {
            foreach (var item in attrs)
            {
                var type = (EntityAttrType)item.type;
                if (type == EntityAttrType.Attack)
                {
                    this.attr.attack = (int)item.value;
                }
                else if (type == EntityAttrType.MaxHealth)
                {
                    this.attr.maxHealth = (int)item.value;
                }
                else if (type == EntityAttrType.MoveSpeed)
                {
                    // /1000
                    this.attr.moveSpeed = item.value;
                }

                Logx.Log("sync entity attr : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);

                  EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this,0);
            }
          
        }


        //internal void SyncValue(RepeatedField<BattleEntityValueProto> values)
        //{
        //    foreach (var item in values)
        //    {
        //        var type = (EntityCurrValueType)item.Type;
        //        var value = item.Value;
        //        if (type == EntityCurrValueType.CurrHealth)
        //        {
        //            this.CurrHealth = value;
        //        }


        //        Logx.Log("sync entity curr value : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.Value);
        //    }
        //    EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this);

        //}

        internal void SyncValue(List<BattleClientMsg_BattleValue> values)
        {
            foreach (var item in values)
            {
                var type = (EntityCurrValueType)item.type;
                var value = item.value;
                if (type == EntityCurrValueType.CurrHealth)
                {
                    this.CurrHealth = value;
                }
                Logx.Log("sync entity curr value : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);

                EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this, item.fromEntityGuid);

            }
          

        }

        float rotateSpeed = 540;
        int lastDir = 1;
        public void Update(float timeDelta)
        {
            //this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward.normalized, dirTarget.normalized, timeDelta * 15);

            //控制朝向

            //直接转向
            //this.gameObject.transform.forward = dirTarget;

            //缓慢转向

            if (dirTarget == Vector3.zero)
            {
                dirTarget = new Vector3(0, 0, 1);
            }

            dirTarget = new Vector3(dirTarget.x, 0, dirTarget.z);

            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, dirTarget, 30 * timeDelta);

          
            //Battle._Battle_Log.Log("zxy path test : dir : " + this.gameObject.transform.forward.x + "," + this.gameObject.transform.forward.z + " -> " + dirTarget);
            if (state == BattleEntityState.Move)
            {
                var moveVector = moveTargetPos - this.gameObject.transform.position;
                var speed = this.attr.moveSpeed;
                var dir = moveVector.normalized;
                //var dis = moveVector.magnitude;

                var currPos = this.gameObject.transform.position;
                var moveDistance = (dir * speed * timeDelta).magnitude;

                var currFramePos = currPos;
                //预测下一帧位置
                var moveDelta = dir * speed * timeDelta;
                var nextFramePos = currPos + moveDelta;

                var dotValue = Vector3.Dot(currFramePos - moveTargetPos, moveTargetPos - nextFramePos);
                if (dotValue >= 0)
                {
                    this.movePosIndex = this.movePosIndex + 1;
                    if (this.movePosIndex >= this.movePosList.Count)
                    {
                        //所有路径走完了
                        this.FakeStopMove(moveTargetPos);
                    }
                    else
                    {
                        //前往下一个路径点
                        this.moveTargetPos = this.movePosList[this.movePosIndex];

                        //设置朝向
                        dirTarget = (moveTargetPos - this.gameObject.transform.position).normalized;
                    }

                    //this.gameObject.transform.forward = dirTarget;
                }
                else
                {
                    moveDelta = dir * speed * timeDelta;
                    //Logx.Log("moveTest : battleClient : moveDis : " + moveDelta.x + " " + moveDelta.z);
                    this.gameObject.transform.position = currPos + moveDelta;

                    var prePos = this.gameObject.transform.position;
                    this.gameObject.transform.position = new Vector3(prePos.x, 0, prePos.z);

                    //this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, dirTarget, timeDelta * 15);
                }

            }
        }
        public void PlayAnimation(string aniName)
        {
            if (isFinishLoad && animation != null)
            {
                animation.CrossFade(aniName);
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

        internal void SetShowState(bool isShow)
        {
            gameObject.SetActive(isShow);
        }
    }
}