//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FixedPointy;
//using NetCommon;
//using UnityEngine;

//public enum OperateType
//{

//    Move,
//    Attack,
//    Skill
//}

////public enum HeroState
////{
////    Idle,
////    Move,
////    Attack,
////    Die
////}

//public enum SkillReleaseTargetType
//{
//    NoTarget,
//    Unit,
//    Pos,

//}

//public enum EntityState
//{
//    Idle,
//    Run,
//    PreSkill,
//}

//namespace Assets.Script.Combat
//{
//    //public enum SkillType
//    //{
//    //    Attack,
//    //    //Skill0,
//    //    //Skill1,
//    //    //Skill2,
//    //    //Skill3
//    //    Skill,


//    //}

//    /// <summary>
//    /// 准备动作 信息
//    /// </summary>
//    public class PreAction
//    {
//        // public SkillType skillType;
//        public int skillSN;

//        Fix preTimer;
//        Fix lastTime = Fix.Ratio(250, 1000);
//        public CombatLogicEntity releaser;
//        public CombatLogicEntity targetUnit;
//        public FixVec3 targetPos;
//        public bool IsFinish()
//        {
//            return preTimer >= lastTime;
//        }

//        public void Update()
//        {
//            Debug.Log("the preAction Update : " + preTimer);
//            preTimer += Fix.Ratio(Const.frameTime, 1000);
//        }

//        public void Init(int skillSN, CombatLogicEntity releaser, CombatLogicEntity targetUnit, FixVec3 targetPos)
//        {
//            this.skillSN = skillSN;
//            this.releaser = releaser;
//            this.targetUnit = targetUnit;
//            this.targetPos = targetPos;

//            var skillModel = releaser.GetSkill(skillSN);
//            preTimer = (Fix)skillModel.config.preTime;
//        }

//        public void Reset()
//        {
//            preTimer = 0;
//            //lastTime = Fix.Ratio(250, 1000);
//            targetUnit = null;
//        }
//    }

//    public class CombatLogicEntity
//    {
//        public int seat;
//        public int team;
//        public int SN;

//        //public GameObject gameObj;
//        public EntityState state;
//        public FixVec3 position;


//        public Action<FixVec3> moveAction;
//        public Action<FixVec3> rotateAction;
//        public Action stopMoveAction;
//        public Action<int> preSkillAction;
//        //public Action<Fix> onChangeHp;
//        public Action<EffectAttributeType, Fix> onChangeAttribute;

//        public int guid;

//        private FixVec3 forward;
//        public FixVec3 right;
//        public FixVec3 up;

//        public FixVec3 Forward
//        {
//            get
//            {
//                return forward;
//            }

//            set
//            {
//                forward = value.Normalize();
//                right = Forward.Cross(up).Normalize();
//                up = right.Cross(Forward).Normalize();

//                rotateAction?.Invoke(forward);
//            }
//        }


//        //skill 
//        List<SkillModel> skills = new List<SkillModel>();

//        internal SkillModel GetSkillBySN(int skillSN)
//        {
//            return skills.Find(s => s.config.SN == skillSN);
//        }

//        public SkillModel GetAttackSkill()
//        {
//            return skills.Find(s => s.config.isNormalAttack);
//        }

//        public SkillModel GetSkill(int index)
//        {
//            return skills[index];
//        }

//        //property 

//        //之后会变成属性带有加成的形式  
//        //这里将会变成的是最终的初始值 也就是说从配置表读出的数值 + 符文信息 + 其他关于初始值的加成
//        //public Fix attack = 10;
//        //public Fix hp = 100;
//        //public Fix maxHp = 100;
//        //public Fix defence;

//        //public Fix attackRange = 2;

//        //public Fix moveSpeed;
//        //public Fix attackSpeed = 1;//由 skill 提供

//        //attribute



//        public CombatLogicEntity()
//        {
//            up = new FixVec3(0, 1, 0);
//            right = new FixVec3(1, 0, 0);
//            Forward = new FixVec3(0, 0, 1);


//            attributeManager = new AttributeManager();
//            attributeManager.Init(this);


//            //先只有攻击
//            //preActionList = new List<PreAction>()
//            //{
//            //    //new PreAction()
//            //    //{
//            //    //     skillType =  SkillType.Attack
//            //    //},
//            //    //new PreAction()
//            //    //{
//            //    //     skillType =  SkillType.Skill
//            //    //}
//            //};
//        }

//        AttributeManager attributeManager;

//        public void SetInitAttribute(EffectAttributeType type, Fix baseValue)
//        {
//            attributeManager.InitAttribute(type, baseValue);
//        }

//        public Fix GetAttribute(EffectAttributeType type)
//        {
//            return attributeManager.GetValue(type);
//        }

//        public void ChangeAttributeValue(EffectAttributeType type, Fix value)
//        {
//            attributeManager.ChangeFixedValue(type, value);
//            onChangeAttribute?.Invoke(type, GetAttribute(type));
//        }

//        public void ChangeAttributeThousandValue(EffectAttributeType type, Fix value)
//        {
//            attributeManager.ChangeThousandValue(type, value);
//            onChangeAttribute?.Invoke(type, GetAttribute(type));
//        }

//        //List<PreAction> preActionList;
//        PreAction currPreAction;
//        //SkillType currPreSkillType;//当前正在进入准备阶段的技能类型

//        public void Init(int SN)
//        {
//            //

//            //init skill
//            //skills.Add();

//        }

//        public void Update()
//        {
//            if (this.state == EntityState.PreSkill)
//            {
//                currPreAction.Update();

//                if (currPreAction.IsFinish())
//                {
//                    ApplySkill(currPreAction);
//                    currPreAction.Reset();
//                }

//                //for (int i = 0; i < preActionList.Count; ++i)
//                //{
//                //    var preAction = preActionList[i];

//                //    if (preAction.skillSN == )
//                //        3
//                //    {
//                //        preAction.Update();

//                //        if (preAction.IsFinish())
//                //        {
//                //            StartSkill(preAction);
//                //            preAction.Reset();
//                //        }
//                //    }
//                //}
//            }

//        }

//        void ApplySkill(PreAction preAction)
//        {

//            SkillEffectApply(preAction.skillSN, preAction.targetUnit, preAction.targetPos);
//            this.state = EntityState.Idle;

//            //if (preAction.skillType == SkillType.Attack)
//            //{
//            //    //var target = CombatLogicEntityManager.Instance.FindEntity(preAction.targetGuid);
//            //    if (preAction.targetUnit != null)
//            //    {
//            //        NormalAttackApply(preAction.targetUnit);
//            //        this.state = EntityState.Idle;
//            //    }
//            //    else
//            //    {
//            //        Debug.Log("the entity is not find");
//            //    }
//            //}

//            //if (preAction.skillType == SkillType.Skill)
//            //{
//            //    SkillEffectApply(preAction.skillSN, preAction.targetUnit, preAction.targetPos);
//            //    this.state = EntityState.Idle;

//            //    ////var target = CombatLogicEntityManager.Instance.FindEntity(preAction.targetGuid);
//            //    //if (preAction.targetUnit != null)
//            //    //{
//            //    //    NormalAttackApply(preAction.targetUnit);
//            //    //    this.state = EntityState.Idle;
//            //    //}
//            //    //else
//            //    //{
//            //    //    Debug.Log("the entity is not find");
//            //    //}
//            //}

//        }

//        public void Move(FixVec3 dis)
//        {
//            if (this.state == EntityState.PreSkill)
//            {
//                return;
//            }
//            //Debug.Log("move :  now : " + position.X + " " + position.Z);
//            position += dis;
//            state = EntityState.Run;

//            if (dis.GetMagnitude() > 0)
//            {
//                Forward = dis;
//            }

//            moveAction?.Invoke(dis);
//        }

//        public void Rotate(FixVec3 degrees)
//        {
//            var rotation = FixTrans3.MakeRotation(degrees);
//            Forward = rotation * Forward;
//        }



//        public void StopMove()
//        {
//            if (this.state == EntityState.PreSkill)
//            {
//                return;
//            }

//            state = EntityState.Idle;
//            stopMoveAction?.Invoke();
//        }

//        //public int currAttackTargetGuid;
//        public void SkillEffectApply(int skillSN, CombatLogicEntity targetUnit, FixVec3 targetPos)
//        {
//            var currSkill = skills.Find(skill =>
//            {
//                return skill.config.SN == skillSN;
//            });
//            if (currSkill != null)
//            {
//                currSkill.Start(targetUnit, targetPos);
//            }
//            else
//            {
//                Debug.Log("the skill is not found : " + skillSN);
//            }

//        }

//        /// <summary>
//        /// 开始技能前摇
//        /// </summary>
//        /// <param name="skillSN"></param>
//        /// <param name="targetUnit"></param>
//        /// <param name="targetPos"></param>
//        public void StartPreSkill(int skillSN, CombatLogicEntity targetUnit, FixVec3 targetPos)//int index, 
//        {
//            if (this.state == EntityState.PreSkill)
//            {
//                return;
//            }

//            this.state = EntityState.PreSkill;
//            //currAttackTargetGuid = targetGuid;
//            //currPreSkillType = SkillType.Skill;

//            //立即转向 (之后会改为根据技能更改)
//            // var target = CombatLogicEntityManager.Instance.FindEntity(targetUnitGuid);
//            if (targetUnit != null)
//            {
//                var dis = targetUnit.position - this.position;
//                var resultDis = new FixVec3(dis.X, 0, dis.Z);
//                Forward = resultDis.Normalize();
//            }

//            preSkillAction?.Invoke(skillSN);
//            //var action = this.preActionList.Find(p => p.skillType == currPreSkillType);
//            currPreAction.skillSN = skillSN;
//            currPreAction.targetUnit = targetUnit;
//            currPreAction.targetPos = targetPos;

//            //Debug.Log("NormalAttack(int targetGuid) " + targetUnitGuid);
//        }

//        public void StartPreNormalAttack(CombatLogicEntity targetUnit, FixVec3 targetPos)
//        {
//            var normalAttackSkill = skills.Find(s => s.config.isNormalAttack);
//            StartPreSkill(normalAttackSkill.config.SN, targetUnit, targetPos);
//        }

//        //public void NormalAttackApply(CombatLogicEntity target)
//        //{
//        //    Debug.Log("the entity start attck : " + this.guid);
//        //    target.BeAttacked(this);
//        //}


//        private void BeAttacked(CombatLogicEntity attacker)
//        {
//            Debug.Log("the entity is attacked : " + this.guid);
//            //ChangeHp(-20);
//        }


//        public void BeHurt(CombatLogicEntity releaser, Fix value)
//        {
//            ChangeAttributeValue(EffectAttributeType.CurrHp, value);
//        }

//        //public void ChangeHp(Fix value)
//        //{
//        //    this.hp += value;
//        //    onChangeHp?.Invoke(hp);
//        //}




//    }
//}
