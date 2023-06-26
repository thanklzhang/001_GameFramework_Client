using System.Collections.Generic;

namespace Battle
{
    //public enum SkillEffectType
    //{
    //    Null = 0,
    //    Projectile = 1,
    //    Calculate = 2
    //}

    public enum SkillEffectState
    {
        Null = 0,
        InitFinish = 1,
        Running = 2,
        WillEnd = 3,
        End = 4,
        Destroy = 5

    }

    public class SkillEffectContext
    {
        //上下文中选取的单位
        public List<BattleEntity> selectEntities;
        //归属技能
        public Skill fromSkill;
        //当前战斗
        public Battle battle;
        //碰撞组
        public CollisionGroupEffect collisonGroupEffect;
        //造成的伤害
        public float damage;
    }

    public class SkillEffect
    {
        public int guid;

        public int configId;
        //SkillEffectType type;

        //从哪个技能释放出来的
        //protected Skill fromSkill;
        protected SkillEffectContext context;
        //public void SetFromSkill(Skill skill)
        //{
        //    this.fromSkill = skill;
        //}
        public bool isAutoDestroy;
        public SkillEffectState state = SkillEffectState.Null;
        public void Init()//Skill skill
        {
            //this.fromSkill = skill;
            state = SkillEffectState.InitFinish;
            this.OnInit();
        }

        public void Start()
        {
            //on start effect
            state = SkillEffectState.Running;

            //_G.Log(string.Format("start effect configId({0}) , from skill {1}", this.configId,
            //  this.context.fromSkill.configId));

            this.OnStart();
        }

        public void Update(float timeDelta)
        {
            //update pos

            //triggerTimer over , effect

            this.OnUpdate(timeDelta);
        }

        public void End()
        {
            //on end effect
            state = SkillEffectState.End;
            this.OnEnd();
        }

        public void Destroy()
        {
            state = SkillEffectState.Destroy;
            this.OnDestroy();
        }

        /////////////////////////////////////////

        public virtual void OnInit()
        {

        }

        public virtual void OnStart()
        {

        }
        public virtual void OnUpdate(float timeDelta)
        {

        }
        public virtual void OnEnd()
        {

        }
        public virtual void OnDestroy()
        {

        }

        protected virtual void SetWillEndState()
        {
            state = SkillEffectState.WillEnd;
        }

        internal void SetContext(SkillEffectContext context)
        {
            this.context = context;
        }
    }
}


