using Battle_Client;
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
    public class BattleSkillInfo
    {
        public int releaserGuid;
        public int configId;
        public int level;
        public float maxCDTime;
        public float currCDTime;

        internal void UpdateInfo(float currCDTime, float maxCDTime)
        {
            this.currCDTime = currCDTime;
            this.maxCDTime = maxCDTime;

            EventDispatcher.Broadcast(EventIDs.OnSkillInfoUpdate, releaserGuid, this);
        }
    }


    public class BattleEntityManager : Singleton<BattleEntityManager>
    {
        //guid => entity
        Dictionary<int, BattleEntity_Client> entityDic = new Dictionary<int, BattleEntity_Client>();

        public void Init()
        {
            entityDic = new Dictionary<int, BattleEntity_Client>();
            this.RegisterListener();
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        internal BattleEntity_Client CreateEntity(BattleClientMsg_Entity msg_entity)
        {
            var entity = CreateViewEntityInfo(msg_entity);
            entity.StartSelfLoadModel();
            
            EventDispatcher.Broadcast<BattleEntity_Client>(EventIDs.OnCreateEntity, entity);
            
            return entity;
        }


        internal BattleEntity_Client CreateViewEntityInfo(BattleClientMsg_Entity msgEntity)
        {
            var guid = msgEntity.guid;
            var configId = msgEntity.configId;

            if (entityDic.ContainsKey(guid))
            {
                Logx.LogWarning("BattleEntityManager : OnCreateEntity : the guid is exist : " + guid);
                return null;
            }

            BattleEntity_Client entity = new BattleEntity_Client();
            entity.Init(guid, configId);
            entity.SetPlayerIndex(msgEntity.playerIndex);

            entity.SetPosition(msgEntity.position);

            //填充技能
            List<BattleSkillInfo> skills = new List<BattleSkillInfo>();
            msgEntity.skills = null == msgEntity.skills ? new List<BattleClientMsg_Skill>() : msgEntity.skills;
            foreach (var serverSkill in msgEntity.skills)
            {
                BattleSkillInfo skill = new BattleSkillInfo()
                {
                    configId = serverSkill.configId,
                    level = serverSkill.level,
                    maxCDTime = serverSkill.maxCDTime,
                    releaserGuid = guid
                };
                skills.Add(skill);
            }

            entity.SetSkillList(skills);


            entityDic.Add(guid, entity);

            //Debug.Log("zxy : hh : add entity guid : " + guid );

            //EventDispatcher.Broadcast<BattleEntity>(EventIDs.OnCreateEntity, entity);

            //entity.StartLoadModel(loadFinishCallback);
            return entity;
        }


        /// <summary>
        /// 通知创建当前所有已存在的实体
        /// </summary>
        public void NotifyCreateAllEntities()
        {
            foreach (var item in this.entityDic)
            {
                var entity = item.Value;
                EventDispatcher.Broadcast<BattleEntity_Client>(EventIDs.OnCreateEntity, entity);
            }
        }

        /// <summary>
        /// 返回当前战斗中所有实体加载请求
        /// </summary>
        /// <param name="finishCallback"></param>
        /// <returns></returns>
        public List<LoadObjectRequest> MakeCurrBattleAllEntityLoadRequests(
            Action<BattleEntity_Client, GameObject> finishCallback)
        {
            var battleEntityDic = this.entityDic;
            return this.MakeMoreBattleEntityLoadRequests(battleEntityDic, finishCallback);
        }

        /// <summary>
        /// 返回多个战斗实体的加载请求
        /// </summary>
        public List<LoadObjectRequest> MakeMoreBattleEntityLoadRequests(
            Dictionary<int, BattleEntity_Client> battleEntityDic,
            Action<BattleEntity_Client, GameObject> finishCallback)
        {
            List<LoadObjectRequest> list = new List<LoadObjectRequest>();
            foreach (var item in battleEntityDic)
            {
                var entity = item.Value;
                var req = MakeBattleEntityLoadRequest(entity, finishCallback);
                list.Add(req);
            }

            return list;
        }

        /// <summary>
        /// 加载现在所有实体 一般再战斗开始加载的时候调用
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadInitEntities()
        {
            int total = entityDic.Count;
            int finishCount = 0;
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                var path = entity.path;
                bool isFinish = true;
                ResourceManager.Instance.GetObject<GameObject>(path, (gameObject) =>
                {
                    //viewEntity.OnLoadModelFinish(gameObject);
                    // this.OnFinishLoadEntityObj(battleEntity, gameObject);
                    entity.OnLoadModelFinish(gameObject);
                    finishCount += 1;
                });
            }

            if (total > 0)
            {
                while (finishCount < total)
                {
                    yield return null;
                }
            }
            yield return null;
        }

        /// <summary>
        /// 返回一个战斗实体的加载请求
        /// </summary>
        public LoadObjectRequest MakeBattleEntityLoadRequest(BattleEntity_Client battleEntity,
            Action<BattleEntity_Client, GameObject> finishCallback)
        {
            var req = new LoadBattleViewEntityRequest(battleEntity)
            {
                selfFinishCallback = finishCallback
            };
            return req;
        }

        public BattleEntity_Client FindEntityByColliderInstanceId(int instanceID)
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.collider != null)
                {
                    if (entity.collider.gameObject.GetInstanceID() == instanceID)
                    {
                        return entity;
                    }
                }
            }

            return null;
        }

        public BattleEntity_Client FindNearestEntity(BattleEntity_Client originEntity, float dis)
        {
            var excludeInsId = originEntity.gameObject.GetInstanceID();
            var pos = originEntity.gameObject.transform.position;
            float minDis = 9999999;
            BattleEntity_Client battleEntity = null;

            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.collider != null)
                {
                    if (entity.CurrHealth <= 0)
                    {
                        continue;
                    }

                    var currGo = entity.collider.gameObject;

                    var vector = currGo.transform.position - pos;
                    vector = new Vector3(vector.x, 0, vector.z);
                    var currDis = vector.sqrMagnitude;

                    if (excludeInsId != entity.gameObject.GetInstanceID())
                    {
                        // TODO: 这里应该根据筛选目标来进行判断 先按照只选择敌对关系来判断
                        if (entity.Team != originEntity.Team)
                        {
                            if (currDis <= dis * dis)
                            {
                                if (currDis <= minDis)
                                {
                                    minDis = currDis;
                                    battleEntity = entity;
                                }
                            }
                        }
                    }
                }
            }

            return battleEntity;
        }

        public BattleEntity_Client FindEntityByInstanceId(int instanceID)
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.gameObject.GetInstanceID() == instanceID)
                {
                    return entity;
                }
            }

            return null;
        }


        public BattleEntity_Client FindEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                return entityDic[guid];
            }
            else
            {
                //Logx.LogWarning("the guid is not found : " + guid);
            }

            return null;
        }


        public void Update(float timeDelta)
        {
            List<BattleEntity_Client> willDeleteList = new List<BattleEntity_Client>();
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                entity.Update(timeDelta);

                if (entity.state == BattleEntityState.Destroy)
                {
                    willDeleteList.Add(entity);
                }
            }

            foreach (var entity in willDeleteList)
            {
                BattleEntityManager.Instance.DestoryEntity(entity.guid);
            }
        }

        public void DestoryEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                BattleEntity_Client entity = entityDic[guid];
                entity.Destroy();
                entityDic.Remove(guid);
            }
            else
            {
                Logx.LogWarning("the guid is not found : " + guid);
            }
        }

        public void OnBattleEnd()
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                if (entity.state != BattleEntityState.Dead)
                {
                    entity.state = BattleEntityState.Idle;
                    entity.PlayAnimation("idle");
                }
            }
        }

        public void ReleaseAllEntities()
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                entity.Destroy();
            }

            entityDic.Clear();
        }

        public void RemoveListener()
        {
            //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        public void Clear()
        {
            this.RemoveListener();
        }

        public void SetAllEntityShowState(bool isShow)
        {
            foreach (var kv in this.entityDic)
            {
                var entity = kv.Value;
                entity.SetShowState(isShow);
                EventDispatcher.Broadcast(EventIDs.OnEntityChangeShowState, entity, isShow);
            }
        }

        //这里时只是控制显隐
        internal void SetEntitiesShowState(bool isShow, List<int> entityGuids)
        {
            foreach (var guid in entityGuids)
            {
                var entity = FindEntity(guid);
                if (entity != null)
                {
                    entity.SetShowState(isShow);
                    EventDispatcher.Broadcast(EventIDs.OnEntityChangeShowState, entity, isShow);
                }
            }
        }
    }
}