using Battle;
using NetProto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle_Client
{
    public class BattleSkillEffectManager : Singleton<BattleSkillEffectManager>
    {
        Dictionary<int, BattleSkillEffect> skillEffectDic;



        public void Init()
        {
            skillEffectDic = new Dictionary<int, BattleSkillEffect>();
            this.RegisterListener();
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        //收到 创建实体 的事件 ,将要进行创建一个完整的实体(包括数据和资源显示等) , 这里 entity 进行自行加载
        internal void CreateSkillEffect(CreateEffectInfo createEffectInfo)
        {
            var skillEffect = CreateSkillEffectInfo(createEffectInfo);
            skillEffect.StartSelfLoadModel();
        }


        ////收到 创建实体 的事件 ,将要进行创建一个完整的实体(包括数据和资源显示等) , 这里 entity 进行自行加载
        //public void CreateEntity(BattleEntityInfo battleEntityInfo)
        //{
        //    var entity = CreateViewEntityInfo(battleEntityInfo);
        //    entity.StartSelfLoadModel();
        //}

        //只创建技能信息 , 是创建一个技能实体的一个步骤
        internal BattleSkillEffect CreateSkillEffectInfo(CreateEffectInfo createEffectInfo)
        {
            //var guid = serverEntity.Guid;
            //var configId = serverEntity.ConfigId;

            var guid = createEffectInfo.guid;
            var resId = createEffectInfo.resId;
            var pos = new UnityEngine.Vector3(createEffectInfo.createPos.x, createEffectInfo.createPos.y,
                createEffectInfo.createPos.z);
            var followEntityGuid = createEffectInfo.followEntityGuid;
            var isAutoDestroy = createEffectInfo.isAutoDestroy;


            if (skillEffectDic.ContainsKey(guid))
            {
                Logx.LogWarning("BattleSkillEffectManager : CreateSkillEffectInfo : the guid is exist : " + guid);
                return null;
            }

            BattleSkillEffect skillEffect = new BattleSkillEffect();
            skillEffect.Init(guid, resId);
            if (createEffectInfo.effectPosType == EffectPosType.Hit_Pos)
            {
                skillEffect.SetPosition(pos);
            }
            else if (createEffectInfo.effectPosType == EffectPosType.Custom_Pos)
            {
                skillEffect.SetPosition(pos);
            }

            if (followEntityGuid > 0)
            {
                if (createEffectInfo.effectPosType == EffectPosType.Hit_Pos)
                {
                    skillEffect.SetFollowEntityGuid(followEntityGuid, "hit_pos");
                }
                else if (createEffectInfo.effectPosType == EffectPosType.Custom_Pos)
                {
                    skillEffect.SetFollowEntityGuid(followEntityGuid,"");
                }
            }
            else
            {
                skillEffect.SetPosition(pos);
            }

            //skillEffect.SetLastTime(lastTime);
            skillEffect.SetIsAutoDestroy(isAutoDestroy);

            skillEffectDic.Add(guid, skillEffect);

            //entity.StartLoadModel(loadFinishCallback);
            return skillEffect;
        }

        ///// <summary>
        ///// 返回当前战斗中所有实体加载请求
        ///// </summary>
        ///// <param name="finishCallback"></param>
        ///// <returns></returns>
        //public List<LoadObjectRequest> MakeCurrBattleAllEntityLoadRequests(Action<BattleEntity, GameObject> finishCallback)
        //{
        //    var battleEntityDic = this.skillEffectDic;
        //    return this.MakeMoreBattleEntityLoadRequests(battleEntityDic, finishCallback);
        //}
        ///// <summary>
        ///// 返回多个战斗实体的加载请求
        ///// </summary>
        //public List<LoadObjectRequest> MakeMoreBattleEntityLoadRequests(Dictionary<int, BattleEntity> battleEntityDic,
        //    Action<BattleEntity, GameObject> finishCallback)
        //{
        //    List<LoadObjectRequest> list = new List<LoadObjectRequest>();
        //    foreach (var item in battleEntityDic)
        //    {
        //        var entity = item.Value;
        //        var req = MakeBattleEntityLoadRequest(entity, finishCallback);
        //        list.Add(req);
        //    }
        //    return list;

        //}

        ///// <summary>
        ///// 返回一个战斗实体的加载请求
        ///// </summary>
        //public LoadObjectRequest MakeBattleEntityLoadRequest(BattleEntity battleEntity, Action<BattleEntity, GameObject> finishCallback)
        //{
        //    var req = new LoadBattleViewEntityRequest(battleEntity)
        //    {
        //        selfFinishCallback = finishCallback
        //    };
        //    return req;

        //}


        ////只创建实体信息 , 是创建一个完整实体的一个步骤
        //public BattleEntity CreateViewEntityInfo(BattleEntityInfo battleEntityInfo)
        //{
        //    var guid = battleEntityInfo.guid;
        //    var configId = battleEntityInfo.configId;

        //    if (entityDic.ContainsKey(guid))
        //    {
        //        Logx.LogWarning("BattleSkillEffectManager : OnCreateEntity : the guid is exist : " + guid);
        //        return null;
        //    }

        //    BattleEntity entity = new BattleEntity();
        //    entity.Init(guid, configId);
        //    entity.SetPosition(battleEntityInfo.position);
        //    entityDic.Add(guid, entity);

        //    //entity.StartLoadModel(loadFinishCallback);
        //    return entity;

        //}

        public BattleSkillEffect FindSkillEffect(int guid)
        {
            if (skillEffectDic.ContainsKey(guid))
            {
                return skillEffectDic[guid];
            }
            else
            {
                Logx.LogWarning("the guid is not found : " + guid);
            }
            return null;
        }

        public void Update(float timeDelta)
        {
            List<BattleSkillEffect> willDestroyEffects = new List<BattleSkillEffect>();
            foreach (var item in skillEffectDic)
            {
                var skillEffect = item.Value;
                skillEffect.Update(timeDelta);
                //Logx.Log("battle skill effect update");

                var isWillDestroy = skillEffect.state == BattleSkillEffectState.WillDestroy;
                if (isWillDestroy)
                {
                    willDestroyEffects.Add(skillEffect);
                }
            }

            foreach (var effect in willDestroyEffects)
            {
                effect.Destroy();
                skillEffectDic.Remove(effect.guid);
            }
        }

        public void DestorySkillEffect(int guid)
        {
            if (skillEffectDic.ContainsKey(guid))
            {
                //Logx.Log("WillDestorySkillEffect : " + guid);
                BattleSkillEffect skillEffect = skillEffectDic[guid];
                skillEffect.SetWillDestoryState();
            }
            else
            {
                Logx.LogWarning("the guid is not found : " + guid);
            }
        }

        public void RemoveListener()
        {
            //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        public void ReleaseAll()
        {
            var kvs = skillEffectDic.ToList();
            for (int i = 0; i < kvs.Count; i++)
            {
                var kv = kvs[i];
                var key = kv.Key;
                var effect = kv.Value;

                effect.Destroy();
                skillEffectDic.Remove(key);
            }
        }
    }
}