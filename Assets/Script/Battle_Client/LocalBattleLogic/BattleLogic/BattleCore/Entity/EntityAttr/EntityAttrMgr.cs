using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public enum EntityAttrGroupType
    {
        Null = 0,

        Base = 1,
        Level = 2,
        Star = 3,
        Equipment = 4,
        Buff = 5
    }


    public class EntityAttrMgr
    {
        Battle battle;
        BattleEntity battleEntity;

        public Dictionary<EntityAttrGroupType, EntityAttrGroup> attrGroupDic;

        //总属性
        public Dictionary<EntityAttrType, float> attrTotalValueDic;
        //public Dictionary<EntityAttrType, float> attrPermillageValueDic;

        public void Init(BattleEntity battleEntity)
        {
            this.battleEntity = battleEntity;
            this.battle = this.battleEntity.GetBattle();


            attrGroupDic = new Dictionary<EntityAttrGroupType, EntityAttrGroup>();
            attrTotalValueDic = new Dictionary<EntityAttrType, float>();

            //人物基础属性
            InitEntityBaseAttr();

            //人物等级属性
            InitEntityLevelAttr();

            //人物上动态的 buff 属性
            InitEntityBuffAttr();
        }

        void InitEntityBaseAttr()
        {
            var baseInfo = battle.ConfigManager.GetById<IEntityAttrBaseConfig>(battleEntity.infoConfig.BaseAttrId);

            this.AddAttrValue(EntityAttrGroupType.Base, EntityAttrType.Attack, (int) EntityAttrType.Attack,
                baseInfo.Attack);
            this.AddAttrValue(EntityAttrGroupType.Base, EntityAttrType.Defence, (int) EntityAttrType.Defence,
                baseInfo.Defence);
            this.AddAttrValue(EntityAttrGroupType.Base, EntityAttrType.MaxHealth, (int) EntityAttrType.MaxHealth,
                baseInfo.Health);
            this.AddAttrValue(EntityAttrGroupType.Base, EntityAttrType.AttackSpeed, (int) EntityAttrType.AttackSpeed,
                baseInfo.AttackSpeed / 1000.0f);
            this.AddAttrValue(EntityAttrGroupType.Base, EntityAttrType.MoveSpeed, (int) EntityAttrType.MoveSpeed,
                baseInfo.MoveSpeed / 1000.0f);
            this.AddAttrValue(EntityAttrGroupType.Base, EntityAttrType.AttackRange, (int) EntityAttrType.AttackRange,
                baseInfo.AttackRange / 1000.0f);
            this.AddAttrValue(EntityAttrGroupType.Base, EntityAttrType.InputDamageRate, (int) EntityAttrType.InputDamageRate,
                baseInfo.InputDamageRate );
        }

        void InitEntityLevelAttr()
        {
            var allData = battle.ConfigManager.GetList<IEntityAttrLevelConfig>();
            IEntityAttrLevelConfig levelInfo = null;
            foreach (var item in allData)
            {
                if (item.TemplateId == battleEntity.infoConfig.LevelAttrId && item.Level == battleEntity.level)
                {
                    levelInfo = item;
                }
            }

            if (levelInfo != null)
            {
                this.AddAttrValue(EntityAttrGroupType.Level, EntityAttrType.Attack, (int) EntityAttrType.Attack,
                    levelInfo.Attack);
                this.AddAttrValue(EntityAttrGroupType.Level, EntityAttrType.Defence, (int) EntityAttrType.Defence,
                    levelInfo.Defence);
                this.AddAttrValue(EntityAttrGroupType.Level, EntityAttrType.MaxHealth, (int) EntityAttrType.MaxHealth,
                    levelInfo.Health);
                this.AddAttrValue(EntityAttrGroupType.Level, EntityAttrType.AttackSpeed,
                    (int) EntityAttrType.AttackSpeed, levelInfo.AttackSpeed / 1000.0f);
                this.AddAttrValue(EntityAttrGroupType.Level, EntityAttrType.MoveSpeed, (int) EntityAttrType.MoveSpeed,
                    levelInfo.MoveSpeed / 1000.0f);
                this.AddAttrValue(EntityAttrGroupType.Level, EntityAttrType.AttackRange,
                    (int) EntityAttrType.AttackRange, levelInfo.AttackRange / 1000.0f);
            }
            else
            {
                _Battle_Log.LogError("the levelInfo is not found : " + battleEntity.configId + " " +
                                     battleEntity.level);
            }
        }

        void InitEntityBuffAttr()
        {
        }


        public void AddAttrValue(EntityAttrGroupType groupType, EntityAttrType attrType,
            int id, float value) //, bool isPermillage
        {
            EntityAttrGroup group = null;
            if (attrGroupDic.ContainsKey(groupType))
            {
                group = attrGroupDic[groupType];
            }
            else
            {
                group = new EntityAttrGroup();
                attrGroupDic.Add(groupType, group);
            }

            group.AddAttr(attrType, id, value); //, isPermillage

            Calculate(groupType, attrType); //, isPermillage
        }

        public void RemoveAttrValue(EntityAttrGroupType groupType, EntityAttrType attrType,
            int id) //, bool isPermillage
        {
            EntityAttrGroup group = null;
            if (attrGroupDic.ContainsKey(groupType))
            {
                group = attrGroupDic[groupType];
                group.RemoveAttr(attrType, id); //, isPermillage

                Calculate(groupType, attrType); //, isPermillage
            }
        }

        //Dictionary<EntityAttrType, float> GetAttrDic(bool isPermillage)
        //{
        //    Dictionary<EntityAttrType, float> dic = null;

        //    if (!isPermillage)
        //    {
        //        dic = this.attrFixedValueDic;
        //    }
        //    else
        //    {
        //        dic = this.attrPermillageValueDic;
        //    }

        //    return dic;
        //}

        void Calculate(EntityAttrGroupType groupType, EntityAttrType attrType) //, bool  isPermillage
        {
            var sum = 0.0f;
            foreach (var item in attrGroupDic)
            {
                var _groupType = item.Key;
                var group = item.Value;
                var preTotalValue = GetTotalValueByGroupType(_groupType, attrType); //, isPermillage
                sum += preTotalValue;
            }

            var dic = attrTotalValueDic; // GetAttrDic(isPermillage);

            if (dic.ContainsKey(attrType))
            {
                dic[attrType] = sum;
            }
            else
            {
                dic.Add(attrType, sum);
            }
        }

        public float
            GetTotalValueByGroupType(EntityAttrGroupType groupType, EntityAttrType attrType) //, bool isPermillage
        {
            EntityAttrGroup group = null;
            if (attrGroupDic.ContainsKey(groupType))
            {
                group = attrGroupDic[groupType];
                return group.GetTotalValue(attrType); //, isPermillage
            }

            return 0;
        }

        //单一属性值的总和
        float GetTotalAttrValue(EntityAttrType attrType) //, bool isPermillage = false
        {
            var dic = attrTotalValueDic; // GetAttrDic(isPermillage);

            //TODO : 减伤和增伤有待商榷
            if (dic.ContainsKey(attrType))
            {
                return dic[attrType];
            }

            return 0;
        }

        //计算后的最终属性值
        public float GetFinalAttrValue(EntityAttrType attrType)
        {
            //if (attrType == EntityAttrType.Attack)
            //{
            //    var fixedValue = GetTotalAttrValue(attrType,false);
            //    //var permillageValue = GetTotalAttrValue(attrType, true);

            //}

            var isPer = AttrHelper.IsPermillage(attrType);
            float fixedValue = 0;
            if (!isPer)
            {
                //获取固定属性 (固定值 和 千分比 的结果)
                //固定值
                fixedValue = GetTotalAttrValue(attrType);
                
                //千分比
                EntityAttrType perType = AttrHelper.GetPermillageTypeByFixedType(attrType);
                var perValue = GetTotalAttrValue(perType) / 1000.0f;

                var resultValue = fixedValue + fixedValue * perValue;
                return resultValue;
            }
            else
            {
                //获取千分比属性
                return GetTotalAttrValue(attrType);
            }


            return GetTotalAttrValue(attrType);
        }

        public Dictionary<EntityAttrType, float> GetFinalAttrs()
        {
            return this.attrTotalValueDic;
        }

        public void Release()
        {
        }
    }
}