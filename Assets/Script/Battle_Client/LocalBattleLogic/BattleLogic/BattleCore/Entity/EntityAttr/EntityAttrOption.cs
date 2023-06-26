using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    //单位的属性
    public enum EntityAttrType
    {
        Null = 0,

        //固定值--------------------
        Attack = 1,
        Defence = 2,
        MaxHealth = 3,
        AttackSpeed = 4,
        MoveSpeed = 5,
        AttackRange = 6,
        CritRate = 7,
        CritDamage = 8,

        //输出伤害的比率(千分比)
        OutputDamageRate = 9,

        //承受伤害的比率(千分比)
        InputDamageRate = 10,
        //--------------------------


        //千分比加成--------------------

        Attack_Permillage = 1001,
        Defence_Permillage = 1002,
        MaxHealth_Permillage = 1003,
        AttackSpeed_Permillage = 1004,
        MoveSpeed_Permillage = 1005,
        AttackRange_Permillage = 1006,
        CritRate_Permillage = 1007,
        CritDamage_Permillage = 1008,
        OutputDamageRate_Permillage = 1009,
        InputDamageRate_Permillage = 1010,

        //---------------------------
    }


    public class EntityAttrOption
    {
        public EntityAttrType attrType;
        public Dictionary<int, float> valueDic;
        public float totalValue;

        public void Init(EntityAttrType type, int id, float value)
        {
            attrType = type;
            valueDic = new Dictionary<int, float>();

            //Add(id, value);
        }

        internal void Add(int id, float value)
        {
            if (!this.valueDic.ContainsKey(id))
            {
                this.valueDic.Add(id, value);

                //finalValue += value;

                //浮点不准确 暂时先重新计算
                this.totalValue = this.Calculate();
            }
            else
            {
                Logx.LogWarning("AttrOption : Add : the is exist : " + id);
            }
        }

        public void Remove(int id)
        {
            if (this.valueDic.ContainsKey(id))
            {
                //var value = this.valueDic[id];

                this.valueDic.Remove(id);

                //finalValue -= value;

                this.totalValue = this.Calculate();
            }
            else
            {
                Logx.LogWarning("AttrOption : Add : the is not exist : " + id);
            }
        }

        public float Calculate()
        {
            var sum = 0.0f;
            foreach (var item in valueDic)
            {
                sum += item.Value;
            }

            return sum;
        }

        public float GetTotalValue()
        {
            return this.totalValue;
        }

      
    }
}