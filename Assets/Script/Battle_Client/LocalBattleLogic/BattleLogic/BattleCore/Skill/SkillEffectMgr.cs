using System.Collections.Generic;

namespace Battle
{
    public enum EffectPosType
    {
        //受击挂点
        Hit_Pos = 0,
        //自定义挂点
        Custom_Pos = 1,
    }

    public class CreateEffectInfo
    {
        public int guid;
        public int resId;
        public EffectPosType effectPosType;

        public Vector3 createPos;
        public bool isAutoDestroy;
        public int followEntityGuid;

        public BuffEffectInfo buffInfo;
    }

    public class SkillEffectMgr
    {
        Dictionary<int, SkillEffect> skillEffectDic;
        Battle battle;
        public void Init(Battle battle)
        {
            this.battle = battle;
            skillEffectDic = new Dictionary<int, SkillEffect>();
        }


        int maxGuid = 1;
        public int GenGuid()
        {
            return maxGuid++;
        }

        public void Update(float timeDelta)
        {
            foreach (var item in skillEffectDic)
            {
                var effect = item.Value;
                effect.Update(timeDelta);
            }

            foreach (var effect in willCreateEffectList)
            {
                effect.Init();
                this.skillEffectDic.Add(effect.guid, effect);
            }
            willCreateEffectList.Clear();

            this.HandleWillStartStateEffect();
            this.HandleWillEndStateEffect();
        }

        public void HandleWillStartStateEffect()
        {
            foreach (var item in skillEffectDic)
            {
                var guid = item.Key;
                var effect = item.Value;
                if (effect.state == SkillEffectState.InitFinish)
                {
                    effect.Start();
                }
            }
        }

        public void HandleWillEndStateEffect()
        {
            List<int> willEndList = new List<int>();
            foreach (var item in skillEffectDic)
            {
                var guid = item.Key;
                var effect = item.Value;
                if (effect.state == SkillEffectState.WillEnd)
                {
                    willEndList.Add(guid);
                }
            }

            foreach (var guid in willEndList)
            {
                var effect = skillEffectDic[guid];
                effect.End();
                effect.Destroy();
                skillEffectDic.Remove(guid);

                //展示效果不通知
                var isNotify = !effect.isAutoDestroy;//!(effect is CalculateEffect);
                if (isNotify)
                {
                    this.battle.OnSkillEffectDestroy(guid);
                }
            }
        }

        List<SkillEffect> willCreateEffectList = new List<SkillEffect>();

        public void Add(int effectConfigId, SkillEffectContext context)
        {
            SkillEffect effect = null;
            var type = SkillEffectFactory.GetTypeByConfigId(effectConfigId);
            //根据类型不同 创建不同的 effect
            //_Battle_Log.Log("skillEffectMgr : add effect : configId : " + effectConfigId + " , type : " + type);

            int resId = -1;
            Vector3 position = Vector3.zero;

            var followEntityGuid = -1;

            float lastTime = 0;

            bool isAutoDestroy = false;

            EffectPosType effectPosType = EffectPosType.Custom_Pos;

            BuffEffectInfo buffInfo = null;

            //投掷效果 effect
            if (type == SkillEffectType.ProjectileEffect)
            {
                effect = new ProjectileEffect();
                ProjectileEffect pEffect = (ProjectileEffect)effect;
                //var config = TableManager.Instance.GetById<Table.ProjectileEffect>(effectConfigId);
                var config = battle.ConfigManager.GetById<IProjectileEffectConfig>(effectConfigId);
                pEffect.targetGuid = context.fromSkill.targetGuid;
                pEffect.targetPos = context.fromSkill.targetPos;
                pEffect.speed = config.Speed / 1000.0f;
                //pEffect.lastTime = config.LastTime;
                pEffect.isFollow = config.IsFollow;

                resId = config.EffectResId;
                position = context.fromSkill.releser.position;


            }

            //区域 effect
            if (type == SkillEffectType.AreaEffect)
            {
                effect = new AreaEffect();
                AreaEffect pEffect = (AreaEffect)effect;
                var config = battle.ConfigManager.GetById<IAreaEffectConfig>(effectConfigId);

                if (config.CenterType == CenterType.SkillReleaser)
                {
                    resId = config.EffectResId;
                    if (resId > 0)
                    {
                        isAutoDestroy = true;
                    }
                    position = context.fromSkill.releser.position;
                }
            }

            //move effect
            if (type == SkillEffectType.MoveEffect)
            {
                effect = new MoveEffect();
                MoveEffect pEffect = (MoveEffect)effect;

            }

            //buff effect
            if (type == SkillEffectType.BuffEffect)
            {

                if (context.selectEntities.Count > 0)
                {
                    followEntityGuid = context.selectEntities[0].guid;
                }
                else
                {
                    _Battle_Log.LogError(" : the this.context.selectEntities of count is 0 : in buff , config : " + effectConfigId);
                    return;
                }

                var entity = battle.FindEntity(followEntityGuid);
                if (null == entity)
                {
                    _Battle_Log.LogError("SkillEffectMgr Add : the entity is not found , config : " + effectConfigId);
                    return;
                }

                //var config = TableManager.Instance.GetById<Table.BuffEffect>(effectConfigId);
                var config = battle.ConfigManager.GetById<IBuffEffectConfig>(effectConfigId);
                resId = config.EffectResId;
                lastTime = config.LastTime;

                buffInfo = new BuffEffectInfo();
                //buffInfo.guid = 0;
                buffInfo.targetEntityGuid = followEntityGuid;
                
                // if (0 == config.LastTime)
                // {
                //     buffInfo.currCDTime = -1;
                //     buffInfo.maxCDTime = buffInfo.currCDTime;
                // }
                // else
                // {
                //     //刷新 CD 时间
                //     buffInfo.maxCDTime = (int)(config.LastTime * 1000);
                //     buffInfo.currCDTime = buffInfo.maxCDTime;
                // }

                buffInfo.statckCount = 1;
                //buffInfo.iconResId = config.IconResId;
                buffInfo.configId = effectConfigId;



                var buff = entity.GetBuffByConfigId(effectConfigId);
                if (buff != null)
                {
                    var isAddLayer = buff.tableConfig.AddLayerType == AddLayerType.AddLayerAndEffect ||
                        buff.tableConfig.AddLayerType == AddLayerType.AddLayerWithoutEffect;
                    if (isAddLayer)
                    {
                        var stackCount = 1;
                        buff.AddStack(stackCount);

                        buffInfo.statckCount = buff.GetCurrStackCount();
                        buffInfo.guid = buff.guid;
                        buffInfo.maxCDTime = (int)(buff.GetMaxLastTime() * 1000);
                        buffInfo.currCDTime = (int)(buff.GetCurrLastTime() * 1000);

                        battle.OnUpdateBuffInfo(buffInfo);

                        return;
                    }
                }
                else
                {
                    buffInfo.maxCDTime = (int) (config.LastTime);
                    buffInfo.currCDTime = buffInfo.maxCDTime;
                    
                }

                effect = new BuffEffect();
                BuffEffect pEffect = (BuffEffect)effect;


            }

            //结算伤害 effect
            if (type == SkillEffectType.CalculateEffect)
            {
                isAutoDestroy = true;
                effect = new CalculateEffect();
                CalculateEffect pEffect = (CalculateEffect)effect;
                //var config = TableManager.Instance.GetById<Table.CalculateEffect>(effectConfigId);
                var config = battle.ConfigManager.GetById<ICalculateEffectConfig>(effectConfigId);
                pEffect.targetGuid = context.fromSkill.targetGuid;

                //填充伤害结算信息
                DamageCalculate damageCalculate = new DamageCalculate();
                damageCalculate.damageSrcGuid = context.fromSkill.releser.guid;

                damageCalculate.calculateOptionList = new List<CalculateEffectAddedOption>();
                //待优化 不能总是分割字符串 所以存起来或者表格导出那更改逻辑
                var addedValueGroup = config.AddedValueGroup;
                foreach (var addedValueOption in addedValueGroup)
                {
                    var addedType = addedValueOption.valueType;
                    var addedValue = addedValueOption.value;
                    var addedDamageType = addedValueOption.effectDamageType;

                    //_Battle_Log.Log(string.Format("addedType:{0} addedValue:{1} addedDamageType:{2}  {3}",
                    //    addedType, addedValue, addedDamageType, config.Name));

                    CalculateEffectAddedOption calOption = new CalculateEffectAddedOption();
                    calOption.addedValueType = addedType;
                    calOption.value = addedValue;
                    calOption.effectDamageType = addedDamageType;

                    damageCalculate.calculateOptionList.Add(calOption);
                }

                damageCalculate.finalEffectDamageType = (EffectDamageType)config.FinalEffectType;

                pEffect.SetDamageCalculate(damageCalculate);

                resId = config.EffectResId;
                //if (context.selectEntities.Count > 0)
                //{
                //    position = context.selectEntities[0].position;

                //}



                if (context.selectEntities.Count > 0)
                {

                    var selectEntity = context.selectEntities[0];

                    effectPosType = EffectPosType.Hit_Pos;

                    if (config.isEffectFollowTarget)
                    {
                        followEntityGuid = selectEntity.guid;
                    }
                }
            }

            if (type == SkillEffectType.CollisionGroupEffect)
            {
                effect = new CollisionGroupEffect();
                CollisionGroupEffect pEffect = (CollisionGroupEffect)effect;
            }

            if (type == SkillEffectType.PassiveEffect)
            {
                effect = new PassiveEffect();
                PassiveEffect pEffect = (PassiveEffect)effect;

                var skillTargetType = context.fromSkill.infoConfig.SkillTargetType;
                if (skillTargetType == SkillTargeType.SkillReleaser)
                {
                    context.selectEntities = new List<BattleEntity>();
                    context.selectEntities.Add(context.fromSkill.releser);
                }

            }

            if (effect != null)
            {
                effect.configId = effectConfigId;

                var guid = this.GenGuid();
                effect.guid = guid;
                //effect.SetFromSkill(fromSkill);
                effect.SetContext(context);
                //effect.Init(fromSkill);
                willCreateEffectList.Add(effect);
                //skillEffectDic.Add(guid, effect);



                effect.isAutoDestroy = isAutoDestroy;
                //if (resId > 0)
                //{
                //    CreateEffectInfo createInfo = new CreateEffectInfo();
                //    createInfo.guid = guid;
                //    createInfo.resId = resId;
                //    createInfo.effectPosType = effectPosType;
                //    if (createInfo.effectPosType == EffectPosType.Custom_Pos)
                //    {
                //        createInfo.createPos = position;
                //    }
                //    createInfo.followEntityGuid = followEntityGuid;
                //    createInfo.isAutoDestroy = isAutoDestroy;
                //    battle.OnCreateSkillEffect(createInfo);
                //}

                CreateEffectInfo createInfo = new CreateEffectInfo();
                createInfo.guid = guid;
                createInfo.resId = resId;
                createInfo.effectPosType = effectPosType;
                if (createInfo.effectPosType == EffectPosType.Custom_Pos)
                {
                    createInfo.createPos = position;
                }
                createInfo.followEntityGuid = followEntityGuid;
                createInfo.isAutoDestroy = isAutoDestroy;

                if (buffInfo != null)
                {
                    buffInfo.guid = guid;
                }
                createInfo.buffInfo = buffInfo;

                battle?.OnCreateSkillEffect(createInfo);

            }
            else
            {
                _Battle_Log.LogError(string.Format("the type is not found : {0}", type));
            }


        }

        public void DeleteAllBuffsFromEntity(int entityGuid)
        {
            var entity = battle.FindEntity(entityGuid);
            if (entity != null)
            {
                var buffs = entity.GetBuffs();
                foreach (var item in buffs)
                {
                    item.Value.ForceDelete();
                }
            }

        }

        public void DeleteBuffFromEntity(int entityGuid, int effectConfigId)
        {
            var entity = battle.FindEntity(entityGuid);
            if (entity != null)
            {
                var buff = entity.GetBuffByConfigId(effectConfigId);
                if (buff != null)
                {
                    buff.ForceDelete();
                }
            }
        }
    }

    public class BuffEffectInfo
    {
        public int guid = 0;
        public int configId = 0;
        public int targetEntityGuid = 0;
        public int currCDTime = 0;
        public int maxCDTime = 0;
        public int statckCount = 0;

        //public int iconResId;
    }
}


