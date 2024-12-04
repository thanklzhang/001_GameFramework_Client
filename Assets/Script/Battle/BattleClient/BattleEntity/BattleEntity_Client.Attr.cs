using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Battle_Client
{
    //实体身上的属性值和状态值等
    public partial class BattleEntity_Client
    {
        
        
        //attr
        public BattleEntityAttr attr = new BattleEntityAttr();

        public void InitAttr()
        {
            attr.Init();
        }

        internal void SyncAttr(List<BattleClientMsg_BattleAttr> attrs)
        {
            foreach (var item in attrs)
            {
                var type = (EntityAttrType)item.type;
                this.attr.SetAttr(type,item.value);
                
                if (type == EntityAttrType.MoveSpeed)
                {
                    //ani
                    var aniScale = this.attr.GetValue(EntityAttrType.MoveSpeed) / normalAnimationMoveSpeed;
                    SetAnimationSpeed(aniScale);
                }
                
                
                // if (type == EntityAttrType.Attack)
                // {
                //     this.attr.attack = (int)item.value;
                // }
                // else if (type == EntityAttrType.Defence)
                // {
                //     this.attr.defence = (int)item.value;
                // }
                // else if (type == EntityAttrType.MaxHealth)
                // {
                //     this.attr.maxHealth = (int)item.value;
                // }
                // else if (type == EntityAttrType.MoveSpeed)
                // {
                //     // /1000
                //     this.attr.moveSpeed = item.value;
                //
                //     //ani
                //     var aniScale = this.attr.moveSpeed / normalAnimationMoveSpeed;
                //     SetAnimationSpeed(aniScale);
                // }
                // else if (type == EntityAttrType.AttackSpeed)
                // {
                //     // /1000
                //     this.attr.attackSpeed = item.value;
                // }
                // else if (type == EntityAttrType.AttackRange)
                // {
                //     // /1000
                //     this.attr.attackRange = item.value;
                // }
                // else if (type == EntityAttrType.Attack_Permillage)
                // {
                //     this.attr.attack_Permillage = item.value;
                // }

                //Logx.Log("sync entity attr : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);
            }
            
            EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this, 0);
        }

        internal void SyncStateValue(List<BattleClientMsg_BattleStateValue> values)
        {
            foreach (var item in values)
            {
                var type = (EntityCurrValueType)item.type;
                var value = item.value;
                if (type == EntityCurrValueType.CurrHealth)
                {
                    this.CurrHealth = value;
                }
                //Logx.Log("sync entity curr value : guid : " + this.guid + " type : " + type.ToString() + " value : " + item.value);

                EventDispatcher.Broadcast(EventIDs.OnChangeEntityBattleData, this, item.fromEntityGuid);
            }
        }


    }
}