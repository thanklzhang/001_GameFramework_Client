/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Table
{
    
    
    
       
    public class ProjectileEffect : BaseTable
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///施加效果的目标类型
        /// </summary>
        private int effectTargetType; 
        
        /// <summary>
        ///是否跟随
        /// </summary>
        private int isFollow; 
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        private int lastTime; 
        
        /// <summary>
        ///偏转角度(适用非跟随)
        /// </summary>
        private int deflectionAngle; 
        
        /// <summary>
        ///速度（* 1000）
        /// </summary>
        private int speed; 
        
        /// <summary>
        ///碰撞半径(*1000)
        /// </summary>
        private int collisionRadius; 
        
        /// <summary>
        ///是否穿透
        /// </summary>
        private int isThrough; 
        
        /// <summary>
        ///是否飞行最大距离(只看飞行时间,不看是否到达技能目标点)
        /// </summary>
        private int isFlyMaxRange; 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        private string startEffectList; 
        
        /// <summary>
        ///碰到物体时触发的效果列表
        /// </summary>
        private string collisionEffectList; 
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        private string endEffectList; 
        
        /// <summary>
        ///碰到物体时触发的对应实体类型(待加)
        /// </summary>
        private int collisionEffectEntityType; 
        
        /// <summary>
        ///结束时转向次数
        /// </summary>
        private int endRedirectCount; 
        
        /// <summary>
        ///结束时转向类型
        /// </summary>
        private int endRedirectType; 
        
        /// <summary>
        ///结束时转向参数
        /// </summary>
        private string endRedirectParam; 
        
        /// <summary>
        ///结束时转向之后持续时间
        /// </summary>
        private int endRedirectLastTime; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int EffectTargetType { get => effectTargetType; }     
        
        public int IsFollow { get => isFollow; }     
        
        public int LastTime { get => lastTime; }     
        
        public int DeflectionAngle { get => deflectionAngle; }     
        
        public int Speed { get => speed; }     
        
        public int CollisionRadius { get => collisionRadius; }     
        
        public int IsThrough { get => isThrough; }     
        
        public int IsFlyMaxRange { get => isFlyMaxRange; }     
        
        public string StartEffectList { get => startEffectList; }     
        
        public string CollisionEffectList { get => collisionEffectList; }     
        
        public string EndEffectList { get => endEffectList; }     
        
        public int CollisionEffectEntityType { get => collisionEffectEntityType; }     
        
        public int EndRedirectCount { get => endRedirectCount; }     
        
        public int EndRedirectType { get => endRedirectType; }     
        
        public string EndRedirectParam { get => endRedirectParam; }     
        
        public int EndRedirectLastTime { get => endRedirectLastTime; }     
        
        public int EffectResId { get => effectResId; }     
        

    } 
}