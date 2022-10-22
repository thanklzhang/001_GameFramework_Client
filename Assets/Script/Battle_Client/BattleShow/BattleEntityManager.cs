using Battle_Client;
using NetProto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle_Client
{


    public class BattleSkillInfo
    {
        public int configId;
        public int level;
    }


    public class BattleEntityManager : Singleton<BattleEntityManager>
    {
        Dictionary<int, BattleEntity> entityDic;

        public void Init()
        {
            entityDic = new Dictionary<int, BattleEntity>();
            this.RegisterListener();
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        //收到 创建实体 的事件 ,将要进行创建一个完整的实体(包括数据和资源显示等) , 这里 entity 进行自行加载
        //internal BattleEntity CreateEntity(BattleEntityProto serverEntity)
        //{
        //    var entity = CreateViewEntityInfo(serverEntity);
        //    entity.StartSelfLoadModel();
        //    return entity;
        //}
        internal BattleEntity CreateEntity(BattleClientMsg_Entity msg_entity)
        {
            var entity = CreateViewEntityInfo(msg_entity);
            entity.StartSelfLoadModel();
            return entity;
        }



        ////收到 创建实体 的事件 ,将要进行创建一个完整的实体(包括数据和资源显示等) , 这里 entity 进行自行加载
        //public void CreateEntity(BattleEntityInfo battleEntityInfo)
        //{
        //    var entity = CreateViewEntityInfo(battleEntityInfo);
        //    entity.StartSelfLoadModel();
        //}

        //只创建一个简单显示实体 包括完整数据 , 是创建一个完整实体的一个步骤
        //internal BattleEntity CreateViewEntityInfo(NetProto.BattleEntityProto serverEntity)
        //{
        //    var guid = serverEntity.Guid;
        //    var configId = serverEntity.ConfigId;

        //    if (entityDic.ContainsKey(guid))
        //    {
        //        Logx.LogWarning("BattleEntityManager : OnCreateEntity : the guid is exist : " + guid);
        //        return null;
        //    }

        //    BattleEntity entity = new BattleEntity();
        //    entity.Init(guid, configId);
        //    entity.SetPosition(BattleConvert.ConverToVector3(serverEntity.Position));

        //    //填充技能
        //    List<BattleSkillInfo> skills = new List<BattleSkillInfo>();
        //    foreach (var serverSkill in serverEntity.SkillInitList)
        //    {
        //        BattleSkillInfo skill = new BattleSkillInfo()
        //        {
        //            configId = serverSkill.ConfigId,
        //            level = serverSkill.Level
        //        };
        //        skills.Add(skill);
        //    }
        //    entity.SetSkillList(skills);


        //    entityDic.Add(guid, entity);

        //    //EventDispatcher.Broadcast<BattleEntity>(EventIDs.OnCreateEntity, entity);

        //    //entity.StartLoadModel(loadFinishCallback);
        //    return entity;
        //}

        internal BattleEntity CreateViewEntityInfo(BattleClientMsg_Entity msgEntity)
        {
            var guid = msgEntity.guid;
            var configId = msgEntity.configId;

            if (entityDic.ContainsKey(guid))
            {
                Logx.LogWarning("BattleEntityManager : OnCreateEntity : the guid is exist : " + guid);
                return null;
            }

            BattleEntity entity = new BattleEntity();
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
                    level = serverSkill.level
                };
                skills.Add(skill);
            }
            entity.SetSkillList(skills);


            entityDic.Add(guid, entity);

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
                EventDispatcher.Broadcast<BattleEntity>(EventIDs.OnCreateEntity, entity);
            }
        }

        /// <summary>
        /// 返回当前战斗中所有实体加载请求
        /// </summary>
        /// <param name="finishCallback"></param>
        /// <returns></returns>
        public List<LoadObjectRequest> MakeCurrBattleAllEntityLoadRequests(Action<BattleEntity, GameObject> finishCallback)
        {
            var battleEntityDic = this.entityDic;
            return this.MakeMoreBattleEntityLoadRequests(battleEntityDic, finishCallback);
        }
        /// <summary>
        /// 返回多个战斗实体的加载请求
        /// </summary>
        public List<LoadObjectRequest> MakeMoreBattleEntityLoadRequests(Dictionary<int, BattleEntity> battleEntityDic,
            Action<BattleEntity, GameObject> finishCallback)
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
        /// 返回一个战斗实体的加载请求
        /// </summary>
        public LoadObjectRequest MakeBattleEntityLoadRequest(BattleEntity battleEntity, Action<BattleEntity, GameObject> finishCallback)
        {
            var req = new LoadBattleViewEntityRequest(battleEntity)
            {
                selfFinishCallback = finishCallback
            };
            return req;

        }

        public BattleEntity FindEntityByColliderInstanceId(int instanceID)
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

        public BattleEntity FindEntityByInstanceId(int instanceID)
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

        ////只创建实体信息 , 是创建一个完整实体的一个步骤
        //public BattleEntity CreateViewEntityInfo(BattleEntityInfo battleEntityInfo)
        //{
        //    var guid = battleEntityInfo.guid;
        //    var configId = battleEntityInfo.configId;

        //    if (entityDic.ContainsKey(guid))
        //    {
        //        Logx.LogWarning("BattleEntityManager : OnCreateEntity : the guid is exist : " + guid);
        //        return null;
        //    }

        //    BattleEntity entity = new BattleEntity();
        //    entity.Init(guid, configId);
        //    entity.SetPosition(battleEntityInfo.position);
        //    entityDic.Add(guid, entity);

        //    //entity.StartLoadModel(loadFinishCallback);
        //    return entity;

        //}

        public BattleEntity FindEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                return entityDic[guid];
            }
            else
            {
                Logx.LogWarning("the guid is not found : " + guid);
            }
            return null;
        }

        //public void CreateEntity(BattleEntity battleEntity)
        //{
        //    if (entityDic.ContainsKey(battleEntity.guid))
        //    {
        //        Logx.LogWarning("the guid is exist : " + battleEntity.guid);
        //        return;
        //    }

        //    entityDic.Add(battleEntity.guid, battleEntity);
        //}

        //public BattleEntity CreateEntity(int guid, int configId)
        //{
        //    if (entityDic.ContainsKey(guid))
        //    {
        //        Logx.LogWarning("the guid is exist : " + guid);
        //        return null;
        //    }

        //    BattleEntity entity = new BattleEntity();
        //    entity.Init(guid, configId);
        //    entityDic.Add(guid, entity);

        //    return entity;

        //    //EventDispatcher.Broadcast(EventIDs.OnCreateEntity,guid);
        //}

        public void Update(float timeDelta)
        {
            foreach (var item in entityDic)
            {
                var entity = item.Value;
                entity.Update(timeDelta);
            }
        }

        public void DestoryEntity(int guid)
        {
            if (entityDic.ContainsKey(guid))
            {
                BattleEntity entity = entityDic[guid];
                entity.Destroy();
                entityDic.Remove(guid);
            }
            else
            {
                Logx.LogWarning("the guid is not found : " + guid);
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