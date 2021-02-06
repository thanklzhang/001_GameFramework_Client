/*
 * generate by tool
*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using FixedPointy;
namespace Config
{
       
    public class SkillEffect : ConfigData
    {
        //fileds
        /// <summary>
        ///技能名称
        /// </summary>
        public string name;
 		
        
        /// <summary>
        ///效果类型
        /// </summary>
        public int effectType;
 		
        
        /// <summary>
        ///效果目标类型
        /// </summary>
        public int effectTargetType;
 		
        
        /// <summary>
        ///效果作用范围类型
        /// </summary>
        public int effectRangeType;
 		
        
        /// <summary>
        ///效果作用范围目标数目
        /// </summary>
        public int effectRangeTargetNum;
 		
        
        /// <summary>
        ///伤害
        /// </summary>
        public int damage;
 		
        
        /// <summary>
        ///附加伤害组
        /// </summary>
        public string addedDamages;
 		
        
                

        
        
    } 
}