//using System.Collections.Generic;
//using System.Linq;

//namespace Battle
//{
//    public enum EntityAttrType
//    {
//        Null = 0,

//        //数值
//        Attack = 1,
//        Defence = 2,
//        MaxHealth = 3,
//        AttackSpeed = 4,
//        MoveSpeed = 5,
//        AttackRange = 6,

//        CritRate = 7,
//        CritDamage = 8,
//        IncreaselDamage = 9,
//        ReduceDamage = 10,

//        //千分比
//        Attack_Permillage = 1001,
//        Defence_Permillage = 1002,
//        MaxHealth_Permillage = 1003,
//        AttackSpeed_Permillage = 1004,
//        MoveSpeed_Permillage = 1005,

//    }


//    //属性组
//    public class EntityAttrGroup
//    {
//        public int id;//唯一标识id 如 获得装备时候为 装备 id 移除的时候按照这个 id 移除
//        Dictionary<EntityAttrType, AttrOption> optionDic = new Dictionary<EntityAttrType, AttrOption>();

//        public Dictionary<EntityAttrType, AttrOption> OptionDic { get => optionDic; }

//        public float GetValue(EntityAttrType type)
//        {
//            if (OptionDic.ContainsKey(type))
//            {
//                var attrOption = OptionDic[type];
//                return attrOption.value;
//            }
//            else
//            {
//                //_G.LogWarning("EntityAttrModule.Get : the type is not found : " + type);
//                return 0;
//            }
//        }

//        public void SetValue(EntityAttrType type, float value)
//        {
//            float preValue = 0;
//            if (OptionDic.ContainsKey(type))
//            {
//                var attrOption = OptionDic[type];
//                preValue = attrOption.value;
//                attrOption.value = value;
//            }
//            else
//            {
//                var attrO = new AttrOption()
//                {
//                    attrType = type,
//                    value = value
//                };
//                OptionDic.Add(type, attrO);
//            }


//        }

//        public void Init(List<AttrOption> attrOptionList)
//        {
//            optionDic = attrOptionList.ToDictionary((option) => option.attrType, (option) => option);
//        }

//        public static EntityAttrGroup operator +(EntityAttrGroup b, EntityAttrGroup c)
//        {
//            EntityAttrGroup attrGroup = new EntityAttrGroup();
//            attrGroup.SetValue(EntityAttrType.Attack, b.GetValue(EntityAttrType.Attack) + c.GetValue(EntityAttrType.Attack));
//            attrGroup.SetValue(EntityAttrType.Defence, b.GetValue(EntityAttrType.Defence) + c.GetValue(EntityAttrType.Defence));
//            attrGroup.SetValue(EntityAttrType.MaxHealth, b.GetValue(EntityAttrType.MaxHealth) + c.GetValue(EntityAttrType.MaxHealth));
//            attrGroup.SetValue(EntityAttrType.AttackSpeed, b.GetValue(EntityAttrType.AttackSpeed) + c.GetValue(EntityAttrType.AttackSpeed));
//            attrGroup.SetValue(EntityAttrType.MoveSpeed, b.GetValue(EntityAttrType.MoveSpeed) + c.GetValue(EntityAttrType.MoveSpeed));
//            attrGroup.SetValue(EntityAttrType.AttackRange, b.GetValue(EntityAttrType.AttackRange) + c.GetValue(EntityAttrType.AttackRange));

//            attrGroup.SetValue(EntityAttrType.Attack_Permillage, b.GetValue(EntityAttrType.Attack_Permillage) + c.GetValue(EntityAttrType.Attack_Permillage));
//            attrGroup.SetValue(EntityAttrType.Defence_Permillage, b.GetValue(EntityAttrType.Defence_Permillage) + c.GetValue(EntityAttrType.Defence_Permillage));
//            attrGroup.SetValue(EntityAttrType.MaxHealth_Permillage, b.GetValue(EntityAttrType.MaxHealth_Permillage) + c.GetValue(EntityAttrType.MaxHealth_Permillage));
//            attrGroup.SetValue(EntityAttrType.AttackSpeed_Permillage, b.GetValue(EntityAttrType.AttackSpeed_Permillage) + c.GetValue(EntityAttrType.AttackSpeed_Permillage));
//            attrGroup.SetValue(EntityAttrType.MoveSpeed_Permillage, b.GetValue(EntityAttrType.MoveSpeed_Permillage) + c.GetValue(EntityAttrType.MoveSpeed_Permillage));

//            return attrGroup;
//        }

//        public static EntityAttrGroup operator -(EntityAttrGroup b, EntityAttrGroup c)
//        {
//            EntityAttrGroup attrGroup = new EntityAttrGroup();
//            attrGroup.SetValue(EntityAttrType.Attack, b.GetValue(EntityAttrType.Attack) - c.GetValue(EntityAttrType.Attack));
//            attrGroup.SetValue(EntityAttrType.Defence, b.GetValue(EntityAttrType.Defence) - c.GetValue(EntityAttrType.Defence));
//            attrGroup.SetValue(EntityAttrType.MaxHealth, b.GetValue(EntityAttrType.MaxHealth) - c.GetValue(EntityAttrType.MaxHealth));
//            attrGroup.SetValue(EntityAttrType.AttackSpeed, b.GetValue(EntityAttrType.AttackSpeed) - c.GetValue(EntityAttrType.AttackSpeed));
//            attrGroup.SetValue(EntityAttrType.MoveSpeed, b.GetValue(EntityAttrType.MoveSpeed) - c.GetValue(EntityAttrType.MoveSpeed));
//            attrGroup.SetValue(EntityAttrType.AttackRange, b.GetValue(EntityAttrType.AttackRange) - c.GetValue(EntityAttrType.AttackRange));


//            attrGroup.SetValue(EntityAttrType.Attack_Permillage, b.GetValue(EntityAttrType.Attack_Permillage) - c.GetValue(EntityAttrType.Attack_Permillage));
//            attrGroup.SetValue(EntityAttrType.Defence_Permillage, b.GetValue(EntityAttrType.Defence_Permillage) - c.GetValue(EntityAttrType.Defence_Permillage));
//            attrGroup.SetValue(EntityAttrType.MaxHealth_Permillage, b.GetValue(EntityAttrType.MaxHealth_Permillage) - c.GetValue(EntityAttrType.MaxHealth_Permillage));
//            attrGroup.SetValue(EntityAttrType.AttackSpeed_Permillage, b.GetValue(EntityAttrType.AttackSpeed_Permillage) - c.GetValue(EntityAttrType.AttackSpeed_Permillage));
//            attrGroup.SetValue(EntityAttrType.MoveSpeed_Permillage, b.GetValue(EntityAttrType.MoveSpeed_Permillage) - c.GetValue(EntityAttrType.MoveSpeed_Permillage));
//            return attrGroup;
//        }

//        public void AddValue(EntityAttrType attrType, float value)
//        {
//            var preValue = GetValue(attrType);
          
//            SetValue(attrType, preValue + value);
//        }
//    }

//    //实体属性项
//    public class AttrOption
//    {
//        public EntityAttrType attrType;
//        public float value;

//        public static AttrOption Create(EntityAttrType type, float value)
//        {
//            var attrO = new AttrOption()
//            {
//                attrType = type,
//                value = value
//            };
//            return attrO;
//        }
//    }

//}
