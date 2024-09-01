using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Battle_Client
{
    public enum BattleSkillEffectState
    {
        Idle = 0,
        Move = 1,
        WillDestroy = 2,
        Destroy = 3
    }

    public class BuffEffectInfo_Client
    {
        public int guid;
        public int configId;
        public int targetEntityGuid;
        public float currCDTime;
        public float maxCDTime;
        public int stackCount;
        //public int iconResId;
        internal bool isRemove;
        
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


        int followEntityGuid;
        Transform followEntityNode;

        public int GetFollowEntityGuid()
        {
            return this.followEntityGuid;
        }

        ////技能信息
        //List<BattleSkillInfo> skills;

        bool isAutoDestroy;
        float currAutoDestroyLastTime = 0.0f;
        float totalAutoDestroyTime;
        bool isLoop;


        BuffEffectInfo_Client buffInfo;


        public void Init(int guid, int resId)
        {
            this.guid = guid;
            this.resId = resId;

            //load
            //先拿一个简易的模型暂时放这 然后等真正模型下载好之后在替换即可
            var asset = GameMain.Instance.tempModelAsset;
            gameObject = GameObject.Instantiate(asset);

            // get path
            //var heroConfig = Config.ConfigManager.Instance.GetById<Config.EntityInfo>(this.configId);
            if (this.resId > 0)
            {
                var resConfig = Config.ConfigManager.Instance.GetById<Config.ResourceConfig>(this.resId);
                //临时组路径 之后会打进 ab 包
                path = "Assets/BuildRes/" + resConfig.Path + "/" + resConfig.Name + "." + resConfig.Ext;
            }
            else
            {
                gameObject.SetActive(false);
            }
            
            gameObject.transform.SetParent(GameMain.Instance.gameObjectRoot,false);


            isFinishLoad = false;

            //this.StartLoadModel();
        }

        //开始自行加载(主要用于创建 entity 的时候自己自行异步加载 )
        public void StartSelfLoadModel()
        {
            //Logx.Log("StartLoadModel");
            isFinishLoad = false;
            if (this.resId > 0)
            {
                ResourceManager.Instance.GetObject<GameObject>(path, (obj) =>
                {
                    OnLoadModelFinish(obj);
                });
            }
            else
            {
                isFinishLoad = true;
            }

        }



        internal void SetIsAutoDestroy(bool isAutoDestroy)
        {
            this.isAutoDestroy = isAutoDestroy;
        }

        //设置该特效为一直跟随实体
        internal void SetFollowEntityGuid(int followEntityGuid, string nodeName)
        {
            //StartMove(Vector3.zero, followEntityGuid, 9999999.0f);

            this.followEntityGuid = followEntityGuid;

            var entity = BattleEntityManager.Instance.FindEntity(followEntityGuid);

            if (null != entity)
            {
                followEntityNode = entity.FindModelNode(nodeName);
                this.SetPosition(followEntityNode.position);
            }
            else
            {
                //Debug.LogWarning("zxy SetFollowEntityGuid : the entity is null : followEntityGuid : " + followEntityGuid);
            }
           
        }

        public void OnLoadModelFinish(GameObject obj)
        {
            if (this.state == BattleSkillEffectState.Destroy)
            {
                ResourceManager.Instance.ReturnObject(path, gameObject);
                return;
            }
            //Logx.Log("BattleSkillEffect : OnLoadModelFinish");
            isFinishLoad = true;
            var position = gameObject.transform.position;
            GameObject.Destroy(gameObject);
            gameObject = obj;
            gameObject.transform.position = position;
            //gameObject = 

            
            //获取持续时长
            var curParticle = obj.GetComponent<ParticleSystem>();
            var particles = obj.GetComponentsInChildren<ParticleSystem>();
            // if (gameObject.name.Contains("eft_skill_projectile"))
            // {
            //     Logx.Log(" particles : " + particles.Length);
            // }

            if (particles != null && particles.Length > 0)
            {
                var particle = particles[0];
                this.isLoop = particle.main.loop;
                totalAutoDestroyTime = particle.main.duration;
                // Logx.Log(this.gameObject.name + " totalAutoDestroyTime : " + totalAutoDestroyTime);
                
                for (int i = 0; i < particles.Length; i++)
                {
                    particle = particles[i];
                    particle.Play();
                }
            }

            if (curParticle != null)
            {
                curParticle.Play();                
            }

        }

        internal void SetBuffInfo(Battle.BuffEffectInfo buffInfo)
        {
            this.buffInfo = new BuffEffectInfo_Client()
            {
                guid = buffInfo.guid,
                targetEntityGuid = buffInfo.targetEntityGuid,
                currCDTime = buffInfo.currCDTime / 1000.0f,
                maxCDTime = buffInfo.maxCDTime / 1000.0f,
                stackCount = buffInfo.statckCount,
                configId = buffInfo.configId,
                //iconResId = buffInfo.iconResId

            };

            EventDispatcher.Broadcast(EventIDs.OnBuffInfoUpdate, this.buffInfo);
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

                var moveVector = Vector3.one;
                if (targetGuid > 0)
                {
                    //跟随
                    var entity = BattleEntityManager.Instance.FindEntity(targetGuid);
                    if (entity != null)
                    {
                        targetPos = entity.GetPosition();
                        moveTargetPos = targetPos;
                    }
                    else
                    {
                        targetPos = moveTargetPos;
                    }
                    
                    moveVector = targetPos - this.gameObject.transform.position;

                }
                else
                {
                    //非跟随 一直飞行
                    moveVector = initDir;
                }


                var speed = this.moveSpeed;
                var dir = moveVector.normalized;
                //var dis = moveVector.magnitude;

                var currPos = this.gameObject.transform.position;

                var dirDis = dir * (speed * timeDelta);
                this.gameObject.transform.position = currPos + dirDis; 

                if (this.followEntityGuid <= 0)
                {
                    if (dir != Vector3.zero)
                    {
                        this.gameObject.transform.forward = dir;
                    }
                   
                }

                var preDir = moveVector;
                var nowDir = this.gameObject.transform.position - targetPos;
                if (Vector3.Dot(preDir, nowDir) < 0)
                {
                    //到了目的地 此时如果战斗结束 那么直接设置删除状态
                    //否则等待服务器删除
                    if (isBattleEnd)
                    {
                        this.SetWillDestoryState();
                    }
                }



            }

            if (this.followEntityGuid > 0)
            {
                //完全跟随目标
                var followEntity = BattleEntityManager.Instance.FindEntity(this.followEntityGuid);
                if (followEntity != null)
                {
                    var followPos = new Vector3();
                    if (followEntityNode != null)
                    {
                        //跟随某一个挂点
                        followPos = followEntityNode.position;
                    }
                    else
                    {
                        followPos = followEntity.gameObject.transform.position;
                    }

                    this.gameObject.transform.position = followPos + Vector3.up * 0.03f;

                }
            }
            else
            {

            }

            if (isFinishLoad)
            {

                //if (!isLoop)
                //{
                //    currLastTime = currLastTime + timeDelta;
                //    if (currLastTime >= this.totalTotalTime)
                //    {
                //        this.SetWillDestoryState();
                //        //BattleSkillEffectManager.Instance.DestorySkillEffect(this.guid);
                //    }
                //}

                if (this.isAutoDestroy)
                {
                    currAutoDestroyLastTime += timeDelta;

                    if (currAutoDestroyLastTime >= this.totalAutoDestroyTime)
                    {
                        this.SetWillDestoryState();
                    }
                }


                //应该没有持续时间的概念 等待消息才销毁 这个时间可以做倒计时
                //currLastTime = currLastTime + timeDelta;
                //if (currLastTime >= this.totalTotalTime)
                //{
                //    this.SetWillDestoryState();
                //    //BattleSkillEffectManager.Instance.DestorySkillEffect(this.guid);
                //}

            }
        }

        //internal void UpdateEffectInfo(float currCDTime, float maxCDTime, int stackCount)
        //{

        //}

        public void SetWillDestoryState()
        {
            this.state = BattleSkillEffectState.WillDestroy;
        }

        public void Destroy()
        {
            this.state = BattleSkillEffectState.Destroy;
            if (isFinishLoad)
            {
                if (this.resId > 0)
                {
                    ResourceManager.Instance.ReturnObject(resId, gameObject);
                }
                else
                {
                    GameObject.Destroy(gameObject);
                }
            }
            else
            {
                GameObject.Destroy(gameObject);
            }

        }

        public Vector3 initDir;
        private bool isBattleEnd;

        internal void StartMove(Vector3 targetPos, int targetGuid, float moveSpeed)
        {
            //Logx.Log("skill effect start move : " + this.guid + " will move to : " + targetPos + " by speed : " + moveSpeed);
            state = BattleSkillEffectState.Move;

            this.moveTargetPos = targetPos;
            this.moveSpeed = moveSpeed;
            this.targetGuid = targetGuid;

            if (this.targetGuid <= 0)
            {
                //非跟随
                initDir = (targetPos - this.gameObject.transform.position).normalized;
            }

        }

        public void StopMove(Vector3 endPos)
        {
            state = BattleSkillEffectState.Idle;

            this.SetPosition(endPos);
        }
        
        public void OnBattleEnd()
        {
            this.isBattleEnd = true;
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


}