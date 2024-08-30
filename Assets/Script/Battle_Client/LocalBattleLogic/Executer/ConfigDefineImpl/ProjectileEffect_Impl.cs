/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Table;
namespace Battle
{
    
    
 
    public class ProjectileEffect_Impl : IProjectileEffect
    {
        private Table.ProjectileEffect config;
        
        public void Init(int id)
        {
            config = TableManager.Instance.GetById<Table.ProjectileEffect>(id);
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
        ///施加效果的目标类型
        /// </summary>
        public int EffectTargetType => config.EffectTargetType;
        
        /// <summary>
        ///是否跟随
        /// </summary>
        public int IsFollow => config.IsFollow;
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        public int LastTime => config.LastTime;
        
        /// <summary>
        ///偏转角度(适用非跟随)
        /// </summary>
        public int DeflectionAngle => config.DeflectionAngle;
        
        /// <summary>
        ///速度（* 1000）
        /// </summary>
        public int Speed => config.Speed;
        
        /// <summary>
        ///碰撞半径(*1000)
        /// </summary>
        public int CollisionRadius => config.CollisionRadius;
        
        /// <summary>
        ///是否穿透
        /// </summary>
        public int IsThrough => config.IsThrough;
        
        /// <summary>
        ///是否飞行最大距离(只看飞行时间,不看是否到达技能目标点)
        /// </summary>
        public int IsFlyMaxRange => config.IsFlyMaxRange;
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        public List<int> StartEffectList => config.StartEffectList;
        
        /// <summary>
        ///碰到物体时触发的效果列表
        /// </summary>
        public List<int> CollisionEffectList => config.CollisionEffectList;
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        public List<int> EndEffectList => config.EndEffectList;
        
        /// <summary>
        ///碰到物体时触发的对应实体类型(待加)
        /// </summary>
        public int CollisionEffectEntityType => config.CollisionEffectEntityType;
        
        /// <summary>
        ///结束时转向次数
        /// </summary>
        public int EndRedirectCount => config.EndRedirectCount;
        
        /// <summary>
        ///结束时转向类型
        /// </summary>
        public int EndRedirectType => config.EndRedirectType;
        
        /// <summary>
        ///结束时转向参数
        /// </summary>
        public string EndRedirectParam => config.EndRedirectParam;
        
        /// <summary>
        ///结束时转向之后持续时间
        /// </summary>
        public int EndRedirectLastTime => config.EndRedirectLastTime;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}