using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class EntityAbnormalStateMgr
    {
        Battle battle;
        BattleEntity battleEntity;

        public Dictionary<EntityAbnormalStateType, EntityAbnormalState> abnormalStateDic;

        public void Init(BattleEntity battleEntity)
        {
            this.battleEntity = battleEntity;
            this.battle = this.battleEntity.GetBattle();

            abnormalStateDic = new Dictionary<EntityAbnormalStateType, EntityAbnormalState>();
        }

        public void Add(EntityAbnormalStateType stateType)
        {
            if (abnormalStateDic.ContainsKey(stateType))
            {
                abnormalStateDic[stateType].Add();
            }
            else
            {
                EntityAbnormalState newState = new EntityAbnormalState();
                newState.Init(stateType);
                abnormalStateDic.Add(stateType, newState);
                newState.Add();
            }
        }
        public void Remove(EntityAbnormalStateType stateType)
        {
            if (abnormalStateDic.ContainsKey(stateType))
            {
                abnormalStateDic[stateType].Remove();
            }
        }

        public bool IsExist(EntityAbnormalStateType stateType)
        {
            if (abnormalStateDic.ContainsKey(stateType))
            {
                return abnormalStateDic[stateType].IsActive();
            }

            return false;
        }

        //现在带有异常状态下 是否能主动移动
        public bool IsCanMove()
        {
            var checkStun = this.IsExist(EntityAbnormalStateType.Stun);
            var checkFreeze = this.IsExist(EntityAbnormalStateType.Freeze);
            return !checkStun && !checkFreeze;
        }

        //现在带有异常状态下 是否能释放技能(包括普通攻击)
        public bool IsCanReleaseSkill()
        {
            var checkStun = this.IsExist(EntityAbnormalStateType.Stun);
            var checkFreeze = this.IsExist(EntityAbnormalStateType.Freeze);
            return !checkStun && !checkFreeze;
        }
    }

}


public enum EntityAbnormalStateType
{
    Null = 0,

    Attack_Add = 1,
    Defence_Add = 2,
    MaxHealth_Add = 3,
    AttackSpeed_Add = 4,
    MoveSpeed_Add = 5,
    AttackRange_Add = 6,
    CritRate_Add = 7,
    CritDamage_Add = 8,
    //增伤状态 : 提升输出伤害
    OutputDamageRate_Add = 9,
    //易伤状态 : 增加受到的伤害
    InputDamageRate_Add = 10,

    Attack_Sub = 1001,
    Defence_Sub = 1002,
    MaxHealth_Sub = 1003,
    AttackSpeed_Sub = 1004,
    MoveSpeed_Sub = 1005,
    AttackRange_Sub = 1006,
    CritRate_Sub = 1007,
    CritDamage_Sub = 1008,
    //降伤状态 : 降低输出伤害
    OutputDamageRate_Sub = 1009,
    //减伤状态 : 减少受到的伤害
    InputDamageRate_Sub = 1010,

    //击晕
    Stun = 2000,
    //冰冻
    Freeze = 20001,
    //沉默
    Silence = 20002,
    //嘲讽
    Taunt = 20003,

}
