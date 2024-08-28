/*
 * generate by tool
*/
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using LitJson;
//using FixedPointy;
namespace Table
{
    
    
    
       
    public class CollisionGroupEffect : BaseTable
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
        ///包含的技能效果id列表(表示这些效果碰撞到的实体只有第一次是正常效果)
        /// </summary>
        private string skillEffectIds; 
        
        /// <summary>
        ///影响效果类型
        /// </summary>
        private int affectType; 
        
        /// <summary>
        ///影响效果参数
        /// </summary>
        private string affectParam; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public string SkillEffectIds { get => skillEffectIds; }     
        
        public int AffectType { get => affectType; }     
        
        public string AffectParam { get => affectParam; }     
        
        public int EffectResId { get => effectResId; }     
        

    } 
}