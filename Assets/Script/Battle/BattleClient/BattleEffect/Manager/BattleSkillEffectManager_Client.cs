﻿using Battle;
using NetProto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle_Client
{
    public class BattleSkillEffectManager_Client : Singleton<BattleSkillEffectManager_Client>
    {
        Dictionary<int, BattleSkillEffect> skillEffectDic = new Dictionary<int, BattleSkillEffect>();

        public void Init()
        {
            skillEffectDic = new Dictionary<int, BattleSkillEffect>();
            this.RegisterListener();
        }

        public void RegisterListener()
        {
            //EventDispatcher.AddListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }

        //收到 创建技能效果 的事件 ,将要进行创建一个完整的实体(包括数据和资源显示等) , 这里 entity 进行自行加载
        internal void CreateSkillEffect(CreateEffectInfo createEffectInfo)
        {
            var skillEffect = CreateSkillEffectInfo(createEffectInfo);
            skillEffect?.StartSelfLoadModel();
        }

        //只创建技能信息 , 是创建一个技能实体的一个步骤
        internal BattleSkillEffect CreateSkillEffectInfo(CreateEffectInfo createEffectInfo)
        {
            //var guid = serverEntity.Guid;
            //var configId = serverEntity.ConfigId;
            var guid = createEffectInfo.guid;
            var resId = createEffectInfo.resId;

            if (resId <= 0)
            {
                return null;
            }

            var pos = new UnityEngine.Vector3(createEffectInfo.createPos.x, createEffectInfo.createPos.y,
                createEffectInfo.createPos.z);
            var followEntityGuid = createEffectInfo.followEntityGuid;
            var isAutoDestroy = createEffectInfo.isAutoDestroy;

            if (skillEffectDic.ContainsKey(guid))
            {
                Logx.LogWarning("BattleSkillEffectManager : CreateSkillEffectInfo : the guid is exist : " + guid);
                Debug.LogWarning("BattleSkillEffectManager : CreateSkillEffectInfo : the guid is exist : " + guid + " resId : " + resId);
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
                    skillEffect.SetFollowEntityGuid(followEntityGuid, "");
                }
            }
            else
            {
                skillEffect.SetPosition(pos);
            }
            skillEffect.SetIsAutoDestroy(isAutoDestroy);

            if (createEffectInfo.buffInfo != null && createEffectInfo.buffInfo.guid > 0)
            {
                skillEffect.SetBuffInfo(createEffectInfo.buffInfo);
            }
            skillEffectDic.Add(guid, skillEffect);
            return skillEffect;
        }

        public BattleSkillEffect FindSkillEffect(int guid)
        {
            if (skillEffectDic.ContainsKey(guid))
            {
                return skillEffectDic[guid];
            }
            else
            {
                //Logx.LogWarning("the guid is not found : " + guid);
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

        public void OnBattleEnd()
        {
            foreach (var item in skillEffectDic)
            {
                var skillEffect = item.Value;
                skillEffect.OnBattleEnd();
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
                //Logx.LogWarning("the guid is not found : " + guid);
            }
        }
        
        public void DestorySkillEffects()
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

        public void RemoveListener()
        {
            //EventDispatcher.RemoveListener<BattleEntityInfo>(EventIDs.OnCreateBattle, CreateEntity);
        }
        
        public void Clear()
        {
            RemoveListener();
            DestorySkillEffects();
        }

        public void Release()
        {
            
        }

    }
}