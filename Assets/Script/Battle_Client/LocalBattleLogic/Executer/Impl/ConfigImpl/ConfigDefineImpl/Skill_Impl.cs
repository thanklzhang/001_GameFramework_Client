/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Table;
namespace Battle
{
    
    
 
    public class Skill_Impl : ISkill
    {
        private Table.Skill config;
        
        public void Init(int id)
        {
            config = TableManager.Instance.GetById<Table.Skill>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///技能介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///技能图标资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///等级
        /// </summary>
        public int Level => config.Level;
        
        /// <summary>
        ///技能释放目标类型
        /// </summary>
        public int SkillReleaseTargeType => config.SkillReleaseTargeType;
        
        /// <summary>
        ///技能释放类型 
        /// </summary>
        public int SkillReleaseType => config.SkillReleaseType;
        
        /// <summary>
        ///是否是被动技能
        /// </summary>
        public int IsPassiveSkill => config.IsPassiveSkill;
        
        /// <summary>
        ///技能目标类型
        /// </summary>
        public int SkillTargetType => config.SkillTargetType;
        
        /// <summary>
        ///触发的效果列表（主动释放产生的效果）
        /// </summary>
        public List<int> EffectList => config.EffectList;
        
        /// <summary>
        ///获得时候触发的效果列表（获得技能的时候立即生效）
        /// </summary>
        public List<int> EffectListOnGain => config.EffectListOnGain;
        
        /// <summary>
        ///释放距离(普通攻击不走这个 走属性)
        /// </summary>
        public int ReleaseRange => config.ReleaseRange;
        
        /// <summary>
        ///释放技能时前摇时间(毫秒)
        /// </summary>
        public int BeforeTime => config.BeforeTime;
        
        /// <summary>
        ///释放技能时后摇时间(毫秒)
        /// </summary>
        public int AfterTime => config.AfterTime;
        
        /// <summary>
        ///技能CD（毫秒）(在释放技能结束后,后摇时间开始前,则开始计时)(普通攻击不计算在这里)
        /// </summary>
        public int CdTime => config.CdTime;
        
        /// <summary>
        ///是否是普通攻击
        /// </summary>
        public int IsNormalAttack => config.IsNormalAttack;
        
        /// <summary>
        ///是否是大招
        /// </summary>
        public int IsBigSkill => config.IsBigSkill;
        
        /// <summary>
        ///是否'不能被打断' 0: 能被打断 , 1:不能被打断
        /// </summary>
        public int IsNoBreak => config.IsNoBreak;
        
        /// <summary>
        ///动画速度缩放(*1000),1 000为正常 根据前后摇校正动画 
        /// </summary>
        public int AnimationSpeedScale => config.AnimationSpeedScale;
        
        /// <summary>
        ///技能动画触发名称
        /// </summary>
        public string AnimationTriggerName => config.AnimationTriggerName;
        
        /// <summary>
        ///释放者释放技能时候在身上的特效
        /// </summary>
        public int ReleaserEffectResId => config.ReleaserEffectResId;
        
        /// <summary>
        ///技能释放 投掷物 指示器类型
        /// </summary>
        public int SkillDirectorProjectileType => config.SkillDirectorProjectileType;
        
        /// <summary>
        ///技能释放 投掷物 指示器参数
        /// </summary>
        public List<int> SkillDirectorProjectileParam => config.SkillDirectorProjectileParam;
        
        /// <summary>
        ///技能释放 释放者 指示类型
        /// </summary>
        public int SkillReleaserDirectType => config.SkillReleaserDirectType;
        
        /// <summary>
        ///释放者指示参数
        /// </summary>
        public List<int> SkillReleaserDirectParam => config.SkillReleaserDirectParam;
        
        /// <summary>
        ///技能释放 目标 指示类型
        /// </summary>
        public int SkillTargetDirectType => config.SkillTargetDirectType;
        
        /// <summary>
        ///目标指示参数
        /// </summary>
        public List<int> SkillTargetDirectParam => config.SkillTargetDirectParam;
        
        /// <summary>
        ///技能轨迹 Id 列表
        /// </summary>
        public List<int> SkillTrackList => config.SkillTrackList;
        
    } 
}