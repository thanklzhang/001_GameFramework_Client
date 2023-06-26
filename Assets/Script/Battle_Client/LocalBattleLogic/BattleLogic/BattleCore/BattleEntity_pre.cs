//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Battle.BattleTrigger.Runtime;

//namespace Battle
//{
//    public enum EntityState
//    {
//        Null = 0,
//        Idle = 1,
//        Move = 2,
//        //使用技能中 包括 前摇 释放中 后摇
//        UseSkill = 3,
//        Dead = 4
//    }

//    //实体当前数据值类型
//    public enum EntityCurrValueType
//    {
//        CurrHealth = 1,
//        CurrMagic = 2,
//    }

//    public class BattleEntity
//    {
//        public int configId;

//        //人物等级
//        public int level;

//        //attr 
//        //人物基础属性组
//        EntityAttrGroup baseAttrGroup;
//        //等级属性组
//        EntityAttrGroup levelAttrGroup;
//        ////星级属性组
//        //EntityAttrGroup starAttrGroup;
//        ////装备属性组(放到单个装备中)
//        //EntityAttrGroup equipmentAttrGroup;
//        ////装备上的宝石属性组(放到单个装备中)
//        //EntityAttrGroup gemAttrGroup;

//        //buff 属性组 Dic
//        Dictionary<int, EntityAttrGroup> buffAttrGroupDic;

//        //最终属性组
//        public EntityAttrGroup FinalAttrGroup
//        {
//            get
//            {
//                //效率问题 应该是储存一个永驻的 group 然后动态增删
//                var resultGroup = new EntityAttrGroup();

//                resultGroup += baseAttrGroup;
//                resultGroup += levelAttrGroup;

//                foreach (var item in buffAttrGroupDic)
//                {
//                    resultGroup += item.Value;
//                }

//                return resultGroup;
//            }
//        }

//        public float Attack => FinalAttrGroup.GetValue(EntityAttrType.Attack);
//        public float Defence => FinalAttrGroup.GetValue(EntityAttrType.Defence);
//        public float MaxHealth => FinalAttrGroup.GetValue(EntityAttrType.MaxHealth);
//        public float AttackSpeed => FinalAttrGroup.GetValue(EntityAttrType.AttackSpeed);
//        public float MoveSpeed => FinalAttrGroup.GetValue(EntityAttrType.MoveSpeed) * (1 + FinalAttrGroup.GetValue(EntityAttrType.MoveSpeed_Permillage) / 1000.0f);
//        public float AttackRange => FinalAttrGroup.GetValue(EntityAttrType.AttackRange);
//        public int CurrHealth;


//        public int Team
//        {
//            get
//            {
//                var playerInfo = battle.FindPlayerByPlayerIndex(this.playerIndex);
//                return playerInfo.team;
//            }
//        }

//        public EntityState EntityState
//        {
//            get => entityState;

//            set
//            {
//                //_Battle_Log.Log("entity guid : " + this.guid + " : change state " + entityState + " -> " + value);
//                entityState = value;
//            }
//        }



//        public Vector3 position;

//        public Vector3 dir;

//        public int playerIndex;

//        public int guid;

//        public ulong uid;

//        private EntityState entityState;

//        public IEntityInfoConfig infoConfig;

//        Dictionary<int, Skill> skillDic = new Dictionary<int, Skill>();

//        public Vector3 moveTargetPos;

//        //BaseAI ai;

//        Dictionary<int, BuffEffect> buffDic = new Dictionary<int, BuffEffect>();
//        Dictionary<int, PassiveEffect> passiveEffectDic = new Dictionary<int, PassiveEffect>();

//        //SkillReleaseReadyInfo skillReleaseReadyInfo;

//        //碰撞等待标记
//        private bool isCollisionWaiting;
//        public bool IsCollisionWaiting { get => isCollisionWaiting; set => isCollisionWaiting = value; }

//        public float collisionWaitTimer;
//        public const float CollisionWaitMaxTime = 0.15f;

//        //是否是玩家控制
//        public bool isPlayerCtrl = false;
//        public void Init(EntityInit entityInit)
//        {
//            //entityInit 填充
//            this.configId = entityInit.configId;
//            this.level = entityInit.level;

//            this.position = entityInit.position;

//            this.isPlayerCtrl = entityInit.isPlayerCtrl;
//            //from config
//            //this.baseMoveSpeed = 5;

//            infoConfig = this.battle.ConfigManager.GetById<IEntityInfoConfig>(this.configId);

//            if (null == infoConfig)
//            {
//                _Battle_Log.LogError("BattleEntity : Init : infoConfig is not found : configId : " + configId);
//                return;
//            }

//            this.InitAttrs();

//            CurrHealth = (int)this.MaxHealth;

//            EntityState = EntityState.Idle;

//            //初始化 skill
//            this.InitSkill(entityInit.skillInitList);
//            //skillReleaseReadyInfo = new SkillReleaseReadyInfo();
//            //skillReleaseReadyInfo.Init(this.battle);


//        }

//        void InitSkill(List<SkillInit> skillInitList)
//        {
//            this.skillDic = new Dictionary<int, Skill>();


//            if (this.playerIndex >= 0)
//            {
//                //玩家控制的实体:

//                var index = 0;
//                foreach (var initSkill in skillInitList)
//                {
//                    Skill skill = new Skill();
//                    skill.configId = initSkill.configId;
//                    skill.level = initSkill.level;
//                    skill.Init(this);
//                    this.skillDic.Add(skill.configId, skill);
//                    if (0 == index)
//                    {
//                        //第一个技能算是普通攻击
//                        skill.isNormalAttack = true;
//                    }
//                    index = index + 1;
//                }
//            }
//            else
//            {
//                //非玩家控制实体 从表里取
//                var index = 0;
//                foreach (var skillId in infoConfig.SkillIds)
//                {
//                    // Table.TableManager.Instance.GetById<Table.Skill>(skillId);
//                    Skill skill = new Skill();
//                    skill.configId = skillId;
//                    skill.level = 1;
//                    skill.Init(this);
//                    this.skillDic.Add(skill.configId, skill);
//                    if (0 == index)
//                    {
//                        //第一个技能算是普通攻击
//                        skill.isNormalAttack = true;
//                    }
//                    index = index + 1;
//                }

//            }

//        }

//        //
//        bool isShow;
//        internal void SetShowState(bool isShow)
//        {
//            this.isShow = isShow;
//        }


//        //之后换成属性管理器
//        void InitAttrs()
//        {
//            //人物基础属性
//            InitEntityBaseAttr();

//            //人物等级属性
//            InitEntityLevelAttr();

//            //人物上动态的 buff 属性
//            InitEntityBuffAttr();
//            //----------------------

//        }


//        void InitEntityBaseAttr()
//        {
//            baseAttrGroup = new EntityAttrGroup();
//            //var baseInfo = Table.TableManager.Instance.GetById<Table.EntityAttrBase>(this.infoConfig.BaseAttrId);
//            var baseInfo = battle.ConfigManager.GetById<IEntityAttrBaseConfig>(this.infoConfig.BaseAttrId);
//            baseAttrGroup.SetValue(EntityAttrType.Attack, baseInfo.Attack);
//            baseAttrGroup.SetValue(EntityAttrType.Defence, baseInfo.Defence);
//            baseAttrGroup.SetValue(EntityAttrType.MaxHealth, baseInfo.Health);
//            baseAttrGroup.SetValue(EntityAttrType.AttackSpeed, baseInfo.AttackSpeed / 1000.0f);
//            baseAttrGroup.SetValue(EntityAttrType.MoveSpeed, baseInfo.MoveSpeed / 1000.0f);
//            baseAttrGroup.SetValue(EntityAttrType.AttackRange, baseInfo.AttackRange / 1000.0f);
//        }

//        void InitEntityLevelAttr()
//        {
//            levelAttrGroup = new EntityAttrGroup();
//            //var levelInfo = EntityAttrLevel_Tool.GetByLevel(this.configId, this.level);

//            var allData = battle.ConfigManager.GetList<IEntityAttrLevelConfig>();
//            IEntityAttrLevelConfig levelInfo = null;
//            foreach (var item in allData)
//            {
//                if (item.TemplateId == this.infoConfig.LevelAttrId && item.Level == level)
//                {
//                    levelInfo = item;
//                }
//            }

//            if (levelInfo != null)
//            {
//                levelAttrGroup.SetValue(EntityAttrType.Attack, levelInfo.Attack);
//                levelAttrGroup.SetValue(EntityAttrType.Defence, levelInfo.Defence);
//                levelAttrGroup.SetValue(EntityAttrType.MaxHealth, levelInfo.Health);
//                levelAttrGroup.SetValue(EntityAttrType.AttackSpeed, levelInfo.AttackSpeed / 1000.0f);
//                levelAttrGroup.SetValue(EntityAttrType.MoveSpeed, levelInfo.MoveSpeed / 1000.0f);
//                levelAttrGroup.SetValue(EntityAttrType.AttackRange, levelInfo.AttackRange / 1000.0f);
//            }
//            else
//            {
//                _Battle_Log.LogError("the levelInfo is not found : " + this.configId + " " + level);
//            }
//        }

//        internal Dictionary<int, BuffEffect> GetBuffs()
//        {
//            return this.buffDic;
//        }

//        internal BuffEffect GetBuffByConfigId(int effectConfigId)
//        {
//            foreach (var item in this.buffDic)
//            {
//                if (item.Value.configId == effectConfigId)
//                {
//                    return item.Value;
//                }
//            }

//            return null;
//        }

//        void InitEntityBuffAttr()
//        {
//            buffAttrGroupDic = new Dictionary<int, EntityAttrGroup>();
//            //buffAttrGroup.SetValue(EntityAttrType.Attack, 0);
//            //buffAttrGroup.SetValue(EntityAttrType.Defence, 0);
//            //buffAttrGroup.SetValue(EntityAttrType.MaxHealth, 0);
//            //buffAttrGroup.SetValue(EntityAttrType.AttackSpeed, 0 / 1000.0f);
//            //buffAttrGroup.SetValue(EntityAttrType.MoveSpeed, 0 / 1000.0f);
//            //buffAttrGroup.SetValue(EntityAttrType.AttackRange, 0 / 1000.0f);

//        }

//        public void AddBuffAttrs(int buffGuid, List<BuffAttr> buffAttrList)
//        {

//            EntityAttrGroup attrGroup = new EntityAttrGroup();
//            baseAttrGroup.id = buffGuid;
//            for (int i = 0; i < buffAttrList.Count; i++)
//            {
//                var buffAttr = buffAttrList[i];
//                //buffAttr.calculateTarget
//                var valueType = buffAttr.addedValueType;
//                //待定 AddedValueType 的计算逻辑看是否放到传递的时候 buff 来计算
//                if (valueType == AddedValueType.Fixed)
//                {
//                    attrGroup.AddValue(buffAttr.entityAttrType, buffAttr.value);
//                }
//                else if (valueType == AddedValueType.PhysicAttack_Permillage)
//                {
//                    var resultValue = this.FinalAttrGroup.GetValue(EntityAttrType.Attack) * (buffAttr.value / 1000.0f);
//                    attrGroup.AddValue(buffAttr.entityAttrType, resultValue);
//                }

//            }
//            buffAttrGroupDic.Add(buffGuid, attrGroup);
//            this.OnSyncAllAttr();

//        }

//        public void RemoveBuffAttrs(int buffGuid)
//        {
//            if (buffAttrGroupDic.ContainsKey(buffGuid))
//            {
//                buffAttrGroupDic.Remove(buffGuid);
//                this.OnSyncAllAttr();
//            }
//            else
//            {
//                _Battle_Log.LogError("the buffGuid is not found : " + buffGuid);
//            }

//        }

//        //如果实体在战斗开始前就已经有了 那么就会调用这个函数
//        public void NotifyBattleData()
//        {
//            this.OnSyncAllAttr();
//            this.OnSyncValue();
//            this.OnSyncSkillInfo();
//        }



//        //受到伤害(或者治疗)
//        internal void BeHurt(int damageValue, Skill damageSrcSkill)
//        {
//            if (this.EntityState == EntityState.Dead)
//            {
//                return;
//            }

//            var preHp = CurrHealth;
//            int resultDamage = 0;
//            if (damageValue > 0)
//            {
//                //伤害
//                resultDamage = (int)(damageValue - Defence);
//            }
//            else
//            {
//                //治疗
//                resultDamage = damageValue;
//            }

//            CurrHealth -= resultDamage;

//            CurrHealth = Math.Max(0, CurrHealth);
//            CurrHealth = Math.Min((int)MaxHealth, CurrHealth);

//            var attackerName = "NotFound_Error";
//            var attacker = damageSrcSkill.releser;
//            if (attacker != null)
//            {
//                attackerName = attacker.infoConfig.Name;
//            }

//            var beHurtName = this.infoConfig.Name;

//            _Battle_Log.Log(string.Format("{0} be hurt by {1} ,damage : {2}, after defence calculate , result damage : {3}" +
//                " hp : {4} -> {5}", beHurtName, attackerName, damageValue, resultDamage, preHp, CurrHealth));

//            this.OnSyncCurrHealth(attacker.guid);

//            if (resultDamage > 0)
//            {
//                attacker.OnHurtToOtherSuccess();
//                if (damageSrcSkill.isNormalAttack)
//                {
//                    attacker.OnNormalAttackToOtehrSuccess(this, resultDamage, damageSrcSkill);
//                }
//                else
//                {
//                    attacker.OnSkillToOtehrSuccess();
//                }
//            }

//            if (CurrHealth <= 0)
//            {
//                this.OnDead();
//            }
//        }

//        //当给别人伤害的时候
//        public void OnHurtToOtherSuccess()
//        {

//        }

//        //当普通攻击别人命中时
//        public void OnNormalAttackToOtehrSuccess(BattleEntity other, int resultDamage, Skill damageSrcSkill)
//        {
//            //检测被动技能
//            foreach (var item in this.passiveEffectDic)
//            {
//                item.Value.OnNormalAttackToOtherSuccess(other, resultDamage, damageSrcSkill);
//            }
//        }

//        //当技能命中别人时
//        public void OnSkillToOtehrSuccess()
//        {

//        }

//        //当被别人普通攻击命中时
//        public void OnBeNormalAttackByOtherSuccess()
//        {

//        }

//        void OnSyncValue()
//        {
//            OnSyncCurrHealth();
//        }

//        void OnSyncSkillInfo()
//        {
//            foreach (var item in this.skillDic)
//            {
//                var skill = item.Value;
//                this.battle.OnSkillInfoUpdate(skill);
//            }
//        }

//        Battle battle;
//        public void SetBattle(Battle battle)
//        {
//            this.battle = battle;
//        }

//        public Battle GetBattle()
//        {
//            return this.battle;
//        }

//        public void Update(float timeDelta)
//        {
//            //技能更新
//            foreach (var item in this.skillDic)
//            {
//                var skill = item.Value;
//                skill.Update(timeDelta);
//            }

//            ////ai 更新
//            //ai?.Update(timeDelta);


//            if (this.EntityState == EntityState.Dead)
//            {
//                return;
//            }

//            if (isCollisionWaiting)
//            {
//                collisionWaitTimer += timeDelta;
//                if (collisionWaitTimer >= CollisionWaitMaxTime)
//                {
//                    collisionWaitTimer = 0;
//                    isCollisionWaiting = false;
//                    battle.EntityContinueFindPath(this);
//                }
//                return;
//            }


//            //移动更新
//            if (EntityState == EntityState.Move)
//            {
//                var isCanMove = this.IsCanMove();

//                if (!isCanMove)
//                {
//                    _Battle_Log.Log(string.Format("{0} Update move : no move ! ", this.infoConfig.Name));
//                    return;
//                }

//                ////检测边界 目前只是当作矩形检测
//                //Vector3 outPos;
//                //if (CheckBorder(moveTargetPos, out outPos))
//                //{
//                //    //this.SetPosition(moveTargetPos);
//                //    //Logx.Log(" this.SetPosition : " + moveTargetPos);
//                //    //this.ChangeToIdle();
//                //    this.StartMoveToPos(outPos);
//                //    return;
//                //}


//                var vector = moveTargetPos - this.position;
//                var dir = vector.normalized;
//                var speed = this.MoveSpeed;

//                var currFramePos = this.position;
//                var nextFramePos = this.position + dir * speed * battle.TimeDelta;

//                var dotValue = Vector3.Dot(nextFramePos - moveTargetPos, moveTargetPos - currFramePos);

//                if (dotValue >= 0)
//                {
//                    //到达目的地
//                    this.SetPosition(moveTargetPos);

//                    //_G.Log("battle", "entity of guid : " + this.guid + " , reach to target pos : " + moveTargetPos);


//                    //this.ChangeToIdle();

//                    battle.OnMoveToCurrTargetPosFinish(this.guid);
//                }
//                else
//                {
//                    var moveDelta = dir * speed * battle.TimeDelta;
//                    var nextPos = this.position + moveDelta;
//                    this.SetPosition(nextPos);

//                    //碰撞检测
//                    BattleEntity collisionEntity;
//                    bool isNeedCollisionWait;
//                    var isCollision = battle.CheckCollision(this, out collisionEntity, out isNeedCollisionWait);
//                    if (isCollision)
//                    {
//                        var collisionGuid = collisionEntity.guid;

//                        //this.ChangeToIdle();

//                        battle.OnEntityCollision(this.guid, collisionGuid, isNeedCollisionWait);
//                    }
//                }
//            }

//            ////判断下是否有准备释放的技能
//            //var isWillReleaseSkill = this.skillReleaseReadyInfo.IsHaveWillReleseSkill();
//            //if (isWillReleaseSkill)
//            //{
//            //    var sId = this.skillReleaseReadyInfo.skillConfigId;
//            //    var tGuid = this.skillReleaseReadyInfo.targetEntityGuid;
//            //    var tPos = this.skillReleaseReadyInfo.willReleaseSkillTargetPos;
//            //    this.ReleaseSkill(sId, tGuid, tPos);
//            //}


//        }

//        public bool CheckBorder(Vector3 pos, out Vector3 outPos)
//        {
//            bool isBorder = false;
//            if (pos.x < 0)
//            {
//                pos.x = 0;
//                isBorder = true;
//            }
//            if (pos.x > this.battle.GetMapSizeX())
//            {
//                pos.x = this.battle.GetMapSizeX();
//                isBorder = true;
//            }
//            if (pos.z < 0)
//            {
//                pos.z = 0;
//                isBorder = true;
//            }
//            if (pos.z > this.battle.GetMapSizeZ())
//            {
//                pos.z = this.battle.GetMapSizeZ();
//                isBorder = true;
//            }

//            outPos = pos;
//            return isBorder;
//        }

//        public void SetPosition(Vector3 targetPos)
//        {
//            this.position = targetPos;
//            this.position.y = 0;
//        }

//        ////只供外部玩家行为调用
//        //public void AskReleaseSkill(int skillId, int skillTargetGuid, Vector3 skillTargetPos)
//        //{
//        //    ReleaseSkill(skillId, skillTargetGuid, skillTargetPos);
//        //}

//        public void ChangeToIdle()
//        {
//            if (this.EntityState != EntityState.Idle)
//            {
//                this.EntityState = EntityState.Idle;
//                battle.OnEntityStopMove(this.guid, this.position);
//            }
//        }

//        ////外界调用
//        //public void AskMoveToPos(Vector3 targetPos)
//        //{
//        //    this.skillReleaseReadyInfo.Clear();

//        //    this.StartMoveToPos(targetPos);
//        //}

//        //开始移动(按照路径移动)
//        public void StartMoveByPath(List<Vector3> posList)
//        {
//            if (posList.Count > 0)
//            {
//                //this.ChangeToIdle();

//                var startPos = posList[0];
//                bool isSuccess = MoveToPos(startPos);
//                if (isSuccess)
//                {
//                    battle.OnEntityStartMoveByPath(this.guid, posList, this.MoveSpeed);
//                }
//            }
//        }

//        //移动到某点 
//        public bool MoveToPos(Vector3 targetPos)
//        {
//            var isCanMove = this.IsCanMove();

//            if (!isCanMove)
//            {
//                _Battle_Log.Log(string.Format("{0} StartMoveToPos move : no move ! ", this.infoConfig.Name));
//                return false;
//            }


//            //Vector3 outPos;
//            //if (CheckBorder(moveTargetPos, out outPos))
//            //{
//            //    targetPos = outPos;
//            //    _Battle_Log.Log(string.Format("moveTest : battleLogic , will out of border  {0}", targetPos));
//            //}

//            ////寻路
//            //Vector3 findPos;
//            //if (!pathFinder.FindPath(targetPos, out findPos))
//            //{
//            //    _Battle_Log.Log(string.Format("moveTest : battleLogic , not found path : targetPos : {0}", targetPos));
//            //    return;
//            //}
//            //this.moveTargetPos = findPos;


//            this.moveTargetPos = targetPos;

//            this.dir = (this.moveTargetPos - this.position).normalized;

//            EntityState = EntityState.Move;
//            _Battle_Log.Log(string.Format("moveTest : battleLogic {0} StartMoveToPos by speed : {1}", this.infoConfig.Name, this.MoveSpeed));

//            ////计算方向
//            //var preDir = dir;
//            //var nowDirV3 = moveTargetPos - this.position;
//            //nowDirV3.y = 0;
//            ////如果目标点和本身重合 那么就取之前的方向(或者不变即可)
//            //if (0 == nowDirV3.sqrMagnitude)
//            //{
//            //    nowDirV3 = preDir;
//            //}
//            //dir = nowDirV3.normalized;
//            //dir.y = 0;

//            //battle.OnEntityStartMove(this.guid, this.moveTargetPos, dir, this.MoveSpeed);
//            return true;
//        }

//        public bool IsInSkillReleaseRange(int skillId, int targetGuid, Vector3 targetPos)
//        {

//            var isCanReleaseSkill = this.IsCanReleaseSkill();

//            if (!isCanReleaseSkill)
//            {
//                //_G.Log(string.Format("{0} move : no release skill ! ", this.infoConfig.Name));
//                return false;
//            }


//            bool isInRange = false;
//            var skill = FindSkillByConfigId(skillId);

//            if (null == skill)
//            {
//                _Battle_Log.LogWarning("the skillId is not found : " + skillId);
//                return false;
//            }

//            if (null == skill.infoConfig)
//            {
//                _Battle_Log.LogWarning("the infoConfig is null , the skill id is : " + skillId);
//                return false;
//            }

//            var releaseTargetType = (SkillReleaseTargeType)skill.infoConfig.SkillReleaseTargeType;
//            if (releaseTargetType == SkillReleaseTargeType.Entity || releaseTargetType == SkillReleaseTargeType.Point)
//            {
//                //判断当前距离是否能够释放技能
//                float sqrtDis = 99999999;
//                if (targetGuid > 0)
//                {
//                    var targetEntity = this.battle.FindEntity(targetGuid);
//                    if (targetEntity != null)
//                    {
//                        sqrtDis = Vector3.SqrtDistance(this.position, targetEntity.position);
//                    }
//                }
//                else
//                {
//                    targetPos = new Vector3(targetPos.x, 0, targetPos.z);
//                    sqrtDis = Vector3.SqrtDistance(this.position, targetPos);
//                }

//                //_G.Log(string.Format("{0} 's state : {1}", this.infoConfig.Name, this.entityState.ToString()));

//                var releaseRange = skill.GetReleaseRange();
//                //_G.Log(string.Format("sqr dis : {0} relaseRange : {1}", sqrtDis, releaseRange * releaseRange));
//                if (sqrtDis <= releaseRange * releaseRange)
//                {
//                    isInRange = true;
//                }
//                else
//                {
//                    isInRange = false;
//                }
//            }
//            else
//            {
//                isInRange = true;
//            }

//            return isInRange;
//        }

//        ////共外界调用
//        //public void AskReleaseSkill(int skillId, int targetGuid, Vector3 targetPos)
//        //{
//        //    ReleaseSkill(skillId, targetGuid, targetPos);
//        //}

//        public void CheckAndReleaseSkill(int skillId, int targetGuid, Vector3 targetPos, bool isForce = false)
//        {

//            if (this.EntityState == EntityState.Dead)
//            {
//                return;
//            }


//            //if (this.EntityState == EntityState.UseSkill)
//            //{
//            //    return;
//            //}

//            var skill = FindSkillByConfigId(skillId);

//            if (skill.infoConfig.IsPassive)
//            {
//                return;
//            }

//            var isRedayRelease = skill.IsReadyRelease();
//            if (!isRedayRelease)
//            {
//                //_Battle_Log.Log(string.Format("ReleaseSkill fail , not RedayRelease state : guid : {0} skillId : {1}", guid, skillId));
//                return;
//            }

//            //TODO: 根据配表检测目标是否符合,目前先按照只有敌对关系
//            var isSuitTargetType = false;
//            if (skill.infoConfig.SkillReleaseTargeType == SkillReleaseTargeType.Entity)
//            {
//                if (targetGuid > 0)
//                {
//                    var targetEntity = this.battle.FindEntity(targetGuid);
//                    if (targetEntity != null)
//                    {
//                        if (this.Team != targetEntity.Team)
//                        {
//                            isSuitTargetType = true;
//                        }
//                    }
//                }
//            }
//            else
//            {
//                isSuitTargetType = true;
//            }


//            if (!isSuitTargetType)
//            {
//                return;
//            }

//            //检测范围
//            var isInRange = IsInSkillReleaseRange(skillId, targetGuid, targetPos);

//            if (!isInRange)
//            {
//                //_Battle_Log.Log(string.Format("ReleaseSkill fail , not in range : guid : {0} skillId : {0}", guid, skillId));

//                //TryToMoveByReleaseSkill(skillId, targetGuid, targetPos);
//                return;
//            }

//            SuccessToReleaseSkill(skill, targetGuid, targetPos);

//            ////判断技能释放队列
//            //var isReleaseImmediately = IsCanReleaseSkillImmediately();
//            //if (isReleaseImmediately)
//            //{
//            //    StartSkillEffect(skill, targetGuid, targetPos);
//            //}
//            //else
//            //{
//            //    if (isForce && this.skillReleaseQueue.Count >= 1)
//            //    {
//            //        var s = this.skillReleaseQueue[0];
//            //        StartSkillEffect(s.skill, s.targetGuid, s.targetPos);
//            //    }
//            //    ReleaseSkillBean bean = new ReleaseSkillBean()
//            //    {
//            //        skill = skill,
//            //        targetGuid = targetGuid,
//            //        targetPos = targetPos
//            //    };
//            //    TryToAddToReleaseSkillQueue(bean);
//            //}

//        }

//        //List<ReleaseSkillBean> skillReleaseQueue = new List<ReleaseSkillBean>();
//        //bool IsCanReleaseSkillImmediately()
//        //{
//        //    var isHaveReleasingSkill = skillReleaseQueue.Count >= 1;
//        //    return !isHaveReleasingSkill;
//        //}


//        //void TryToAddToReleaseSkillQueue(ReleaseSkillBean relaseSkillBean)
//        //{
//        //    //数组第一个代表正在前摇(先不考虑释放中)
//        //    //数组第二个代表缓存的技能 理论上不会有第三个 因为之后的操作会替换缓存技能（数组第二个）
//        //    if (0 == skillReleaseQueue.Count)
//        //    {
//        //        skillReleaseQueue.Add(relaseSkillBean);
//        //    }
//        //    else
//        //    {
//        //        //有技能正在前摇
//        //        var currBean = skillReleaseQueue[0];
//        //        if (currBean.skill.configId != relaseSkillBean.skill.configId)
//        //        {
//        //            if (skillReleaseQueue.Count >= 2)
//        //            {
//        //                //目前已经有缓存技能了 直接替换
//        //                skillReleaseQueue[1] = relaseSkillBean;
//        //            }
//        //            else
//        //            {
//        //                skillReleaseQueue.Add(relaseSkillBean);
//        //            }
//        //        }
//        //    }
//        //}

//        //成功释放技能 (条件符合释放 开始技能)
//        void SuccessToReleaseSkill(Skill skill, int targetGuid, Vector3 targetPos)
//        {
//            _Battle_Log.Log(string.Format("{0} success release skill : {1}", this.infoConfig.Name, skill.infoConfig.Name));

//            this.ChangeToIdle();

//            this.EntityState = EntityState.UseSkill;

//            //设置朝向
//            var releaseTargetType = (SkillReleaseTargeType)skill.infoConfig.SkillReleaseTargeType;
//            if (releaseTargetType == SkillReleaseTargeType.Entity || releaseTargetType == SkillReleaseTargeType.Point)
//            {
//                var dir = this.dir;
//                if (targetGuid > 0)
//                {
//                    var skillTargetEntity = this.battle.FindEntity(targetGuid);
//                    if (skillTargetEntity != null)
//                    {
//                        dir = (skillTargetEntity.position - this.position).normalized;
//                    }
//                }
//                else
//                {
//                    dir = (targetPos - this.position).normalized;
//                }

//                this.dir = dir;
//                this.battle.SyncEntityDir(this.guid, dir);
//            }

//            //开始释放技能效果
//            skill.Start(targetGuid, targetPos);

//            ////假设都有前摇
//            //ReleaseSkillBean bean = new ReleaseSkillBean()
//            //{
//            //    skill = skill,
//            //    targetGuid = targetGuid,
//            //    targetPos = targetPos
//            //};
//            //TryToAddToReleaseSkillQueue(bean);

//            battle.OnEntityReleaseSkill(this.guid, skill, targetGuid, targetPos);

//        }

//        ////释放技能时候因为距离不够等原因 需要移动到距离内
//        //public void TryToMoveByReleaseSkill(int configId, int targetGuid, Vector3 targetPos)
//        //{
//        //    this.skillReleaseReadyInfo.SaveInfo(configId, targetGuid, targetPos);
//        //    Vector3 tPos;
//        //    if (skillReleaseReadyInfo.TryToGetSkillTargetPos(out tPos))
//        //    {
//        //        this.StartMoveToPos(tPos);
//        //    }
//        //}

//        public void OnSkillReleaseEnd(Skill skill)
//        {
//            if (this.EntityState == EntityState.Dead)
//            {
//                return;
//            }

//            _Battle_Log.Log(string.Format("{0} OnSkillReleaseEnd : {1}", this.infoConfig.Name, skill.infoConfig.Name));
//            this.EntityState = EntityState.Idle;
//            //ai.OnSkillReleaseEnd(skill);

//            //if (this.skillReleaseQueue.Count > 0)
//            //{
//            //    var currBean = this.skillReleaseQueue[0];
//            //    if (currBean.skill.configId == skill.configId)
//            //    {
//            //        this.skillReleaseQueue.RemoveAt(0);

//            //        if (this.skillReleaseQueue.Count > 0)
//            //        {
//            //            var nextBean = this.skillReleaseQueue[0];
//            //            this.ReleaseSkill(nextBean.skill.configId, nextBean.targetGuid, nextBean.targetPos,true);
//            //        }
//            //    }

//            //}

//        }

//        public void OnDead()
//        {
//            this.EntityState = EntityState.Dead;
//            this.battle.OnEntityDead(this);
//        }

//        public float GetEntityAtrrFinalValue(EntityAttrNumberType type)
//        {
//            float resultValue = 0;
//            switch (type)
//            {
//                case EntityAttrNumberType.Attack:
//                    resultValue = this.Attack;
//                    break;
//                case EntityAttrNumberType.Defence:
//                    resultValue = this.Defence;
//                    break;
//                case EntityAttrNumberType.CurrHealth:
//                    resultValue = this.CurrHealth;
//                    break;
//                case EntityAttrNumberType.MaxHealth:
//                    resultValue = this.MaxHealth;
//                    break;
//                case EntityAttrNumberType.AttackSpeed:
//                    resultValue = this.AttackSpeed;
//                    break;
//                case EntityAttrNumberType.AttackRange:
//                    resultValue = this.AttackRange;
//                    break;
//            }
//            return resultValue;
//        }

//        public Vector3 GetPointByType(EntityPointType type)
//        {
//            Vector3 resultValue = Vector3.zero;
//            switch (type)
//            {
//                case EntityPointType.Position:
//                    resultValue = this.position;
//                    break;
//                case EntityPointType.HeadPos:
//                    // TODO
//                    resultValue = this.position;
//                    break;
//            }
//            return resultValue;
//        }

//        public Dictionary<int, Skill> GetAllSkills()
//        {
//            return this.skillDic;
//        }

//        public Skill GetNormalAttackSkill()
//        {
//            foreach (var item in this.skillDic)
//            {
//                var skill = item.Value;
//                if (skill.isNormalAttack)
//                {
//                    return skill;
//                }
//            }
//            return null;
//        }

//        //目前 configId 当作 key , 所以不要有一个英雄用相同的技能
//        public Skill FindSkillByConfigId(int configId)
//        {
//            if (skillDic.ContainsKey(configId))
//            {
//                return skillDic[configId];
//            }
//            return null;
//        }

//        //-------------------- buff
//        public void AddBuff(BuffEffect buff)
//        {
//            if (!buffDic.ContainsKey(buff.guid))
//            {
//                var isPreCanMove = this.IsCanMove();

//                this.buffDic.Add(buff.guid, buff);

//                var isNowCanMove = this.IsCanMove();

//                //检测移动
//                if (isPreCanMove && !isNowCanMove)
//                {
//                    this.ChangeToIdle();
//                }

//                battle.OnEntityAddBuff(this.guid, buff);
//            }
//            else
//            {
//                _Battle_Log.Log("entity : AddBuff : the guid is exist : " + buff.guid);
//            }
//        }

//        public void RemoveBuff(BuffEffect buff)
//        {
//            if (buffDic.ContainsKey(buff.guid))
//            {
//                this.buffDic.Remove(buff.guid);
//            }
//            else
//            {
//                _Battle_Log.Log("entity : RemoveBuff : the guid is not exist : " + buff.guid);
//            }
//        }

//        //只是移除引用
//        public void RemoveBuffsByConfigId(List<int> configIds)
//        {
//            foreach (var configId in configIds)
//            {
//                this.RemoveBuffByConfigId(configId);
//            }
//        }

//        //只是移除引用
//        public void RemoveBuffByConfigId(int configId)
//        {
//            //性能待定
//            foreach (var kv in buffDic)
//            {
//                var buff = kv.Value;
//                if (buff.configId == configId)
//                {
//                    buffDic.Remove(configId);
//                }
//            }
//        }

//        //-------------

//        //passive -------

//        public void AddPassiveEffect(PassiveEffect passiveEffect)
//        {
//            if (!passiveEffectDic.ContainsKey(passiveEffect.guid))
//            {
//                this.passiveEffectDic.Add(passiveEffect.guid, passiveEffect);
//                //battle.OnEntityAddBuff(this.guid, buff);
//            }
//            else
//            {
//                _Battle_Log.LogWarning("entity : AddPassiveEffect : the guid is exist : " + passiveEffect.guid);
//            }
//        }

//        public void RemovePassiveEffect(PassiveEffect passiveEffect)
//        {
//            if (passiveEffectDic.ContainsKey(passiveEffect.guid))
//            {
//                this.passiveEffectDic.Remove(passiveEffect.guid);
//            }
//            else
//            {
//                _Battle_Log.LogWarning("entity : RemovePassiveEffect : the guid is not exist : " + passiveEffect.guid);
//            }
//        }

//        //-------------

//        public bool IsCanMove()
//        {

//            if (this.EntityState == EntityState.Dead)
//            {
//                return false;
//            }

//            if (this.EntityState == EntityState.UseSkill)
//            {
//                return false;
//            }

//            var isBuffCanMove = true;
//            //检测 buff
//            foreach (var kv in this.buffDic)
//            {
//                var buff = kv.Value;
//                var isNoMove = buff.tableConfig.IsNoMove;
//                if (isNoMove)
//                {
//                    isBuffCanMove = false;
//                    break;
//                }
//            }

//            if (!isBuffCanMove)
//            {
//                return false;
//            }

//            return true;
//        }

//        public bool IsCanReleaseSkill()
//        {
//            var isCanReleaseSkill = true;
//            //检测 buff
//            foreach (var kv in this.buffDic)
//            {
//                var buff = kv.Value;
//                var isNoCanReleaseSkill = buff.tableConfig.IsNoReleaseSkill;
//                if (isNoCanReleaseSkill)
//                {
//                    isCanReleaseSkill = false;
//                    break;
//                }
//            }
//            return isCanReleaseSkill;
//        }

//        //-------------------- 同步

//        //同步单一属性
//        public void OnSyncAttr(EntityAttrType type)
//        {
//            var value = FinalAttrGroup.GetValue(type);
//            EntityAttrGroup group = new EntityAttrGroup();
//            group.SetValue(type, value);
//            battle.OnSyncEntityAttr(this.guid, group);
//        }

//        //同步全属性
//        public void OnSyncAllAttr()
//        {
//            battle.OnSyncEntityAttr(this.guid, FinalAttrGroup);
//        }

//        //同步当前实体当前战斗数据

//        //同步当前生命
//        public void OnSyncCurrHealth(int fromEntityGuid = 0)
//        {
//            var hp = this.CurrHealth;
//            battle.OnSyncEntityCurrHealth(this.guid, hp, fromEntityGuid);
//        }


//        //清理函数
//        internal void Clear()
//        {

//        }


//    }

//}
