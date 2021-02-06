//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Assets.Script.Combat;
//using FixedPointy;
//using UnityEngine;

///// <summary>
///// 效果属性类型 既用在效果的属性上 也用到人物的属性上 
///// </summary>
//public enum EffectAttributeType
//{
//    None = 0,
//    Attack = 1,//攻击力
//    Defence = 2,//防御
//    MaxHp = 3,//最大生命值
//    MaxMp = 4,//最大魔法值
//    CurrHp = 5,//当前生命值
//    CurrMp = 6,//当前魔法值
//    MoveSpeed = 7,//移动速度
//    AttackSpeed = 8,//攻击速度
//    AttackRange = 9,//攻击距离


//    NormalDamage = 10,//普通伤害
//    TrueDamage = 11,//真实伤害
//    Stun = 12,//击晕
//    DamageReduceNum = 13,//伤害减免值
//    DamageReducePercent = 14,//伤害减免百分比
//    Fly = 15,//击飞
//    End,
//}

///// <summary>
///// 效果计算加成类型
///// </summary>
//public enum EffectCalculateAddedType
//{
//    None = 0,
//    SelfAttack = 1,
//    SelfDefence = 2,
//    SelfMaxHp = 3,
//    SelfMaxMp = 4,
//    SelfCurrHp = 5,
//    SelfCurrMp = 6,
//    SelfLostHpThousand = 7,//(每损失 1 / 1000)
//    SelfLostMpThousand = 8,//(每损失 1 / 1000)
//    SelfMoveSpeed = 9,
//    SelfAttackSpeed = 10,

//    TargetAttack = 11,
//    TargetDefence = 12,
//    TargetMaxHp = 13,
//    TargetMaxMp = 14,
//    TargetCurrHp = 15,
//    TargetCurrMp = 16,
//    TargetLostHpThousand = 17,//(每损失 1 / 1000)
//    TargetLostMpThousand = 18,//(每损失 1 / 1000)
//    TargetMoveSpeed = 19,
//    TargetAttackSpeed = 20,

//    End

//}


//public class CalculateEffectLogic : SkillEffectLogic
//{
//    public Config.SkillCalculateConfig config;

//    FixVec3 sendInitPos;
//    public override void Init(int skillEffectSN, CombatLogicEntity releaser, CombatLogicEntity target, FixVec3 targetPos)
//    {
//        config = Config.ConfigManager.Instance.GetBySN<Config.SkillCalculateConfig>(skillEffectSN);
//        base.Init(skillEffectSN, releaser, target, targetPos);
//    }

//    public override void Start()
//    {
//        base.Start();
//        var effectAttributeType = (EffectAttributeType)config.effectAttributeType;



//        Fix currValue = 0;

//        //先算固定值----------------------
//        currValue += (Fix)config.effectValue;

//        var addedInfo = config.attributeCalculateAddedInfo;
//        if ("0" == addedInfo || null == addedInfo)
//        {
//            addedInfo = "";
//        }

//        var options = addedInfo.Split(',');
//        if (1 == options.Length)
//        {
//            var arg = options[0];
//            if (arg.Length < 2)
//            {
//                //可以确定 这里计算加成
//                Debug.Log("the calculate effect is not exist : " + config.SN);
//                return;
//            }
//        }

//        //到这里表示肯定有一个以上的计算加成
//        options.ToList().ForEach(option =>
//        {
//            var args = option.Split(':');

//            var type = (EffectCalculateAddedType)int.Parse(args[0]);
//            int value = int.Parse(args[1]);

//            Fix thousandValue = value / 1000;

//            Fix currArgValue = 0;

//            if (type == EffectCalculateAddedType.SelfAttack)
//            {
//                currArgValue = releaser.GetAttribute(EffectAttributeType.Attack);
//            }

//            if (type == EffectCalculateAddedType.SelfMaxMp)
//            {
//                currArgValue = releaser.GetAttribute(EffectAttributeType.MaxHp);
//            }

//            currValue += currArgValue * thousandValue;

//        });



//        //再算千分比----------------------
//        //Fix currThousandValue = 0;
//        //currThousandValue += config.effectThousandValue;
//        //var thousandAddedInfo = config.attributeCalculateAddedThousandInfo;
//        //if ("0" == thousandAddedInfo || null == thousandAddedInfo)
//        //{
//        //    thousandAddedInfo = "";
//        //}

//        //var thousandOptions = thousandAddedInfo.Split(',');
//        //if (1 == thousandOptions.Length)
//        //{
//        //    var arg = thousandOptions[0];
//        //    if (arg.Length < 2)
//        //    {
//        //        //可以确定 这里计算加成
//        //        Debug.Log("the calculate effect is not exist : " + config.SN);
//        //        return;
//        //    }
//        //}

//        ////到这里表示肯定有一个以上的计算加成
//        //thousandOptions.ToList().ForEach(option =>
//        //{
//        //    var args = option.Split(':');

//        //    var type = (EffectCalculateAddedType)int.Parse(args[0]);
//        //    int value = int.Parse(args[1]);

//        //    Fix thousandValue = value / 1000;

//        //    Fix currArgValue = 0;

//        //    if (type == EffectCalculateAddedType.SelfAttack)
//        //    {
//        //        //例如 1000 / 1000 表示 1000 攻击力 增加 1000 个千分比
//        //        currArgValue = releaser.GetAttribute(EffectAttributeType.Attack);
//        //    }

//        //    if (type == EffectCalculateAddedType.SelfMaxMp)
//        //    {
//        //        currArgValue = releaser.GetAttribute(EffectAttributeType.MaxHp);
//        //    }

//        //    currThousandValue += currArgValue * thousandValue;

//        //});



//        //普通伤害计算
//        if (effectAttributeType == EffectAttributeType.NormalDamage)
//        {
//            target.BeHurt(releaser, currValue);
//        }
//        else
//        {
//            if (config.effectValue > 0)
//            {
//                //固定值
//                releaser.ChangeAttributeValue(effectAttributeType, currValue);
//            }
//            else
//            {
//                releaser.ChangeAttributeThousandValue(effectAttributeType, currValue);
//            }



//            //Fix baseValue = 0;
//            ////状态的改变 如 增加 20% 攻击力等
//            ////switch (effectAttributeType)
//            ////{
//            ////    case EffectAttributeType.Attack:
//            ////        baseValue = releaser.GetAttribute(effectAttributeType);
//            ////        break;
//            ////    case EffectAttributeType.Defence:
//            ////        break;
//            ////    case EffectAttributeType.AttackSpeed:
//            ////        break;
//            ////    case EffectAttributeType.MoveSpeed:
//            ////        break;
//            ////}
//            //baseValue = releaser.GetAttribute(effectAttributeType);
//        }
//    }

//    public override void Update()
//    {
//        base.Update();
//    }

//    public void DamageCalculate()
//    {

//    }

//}

