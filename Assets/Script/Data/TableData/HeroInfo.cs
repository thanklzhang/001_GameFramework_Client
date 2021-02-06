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
       
    public class HeroInfo : ConfigData
    {
        //fileds
        /// <summary>
        ///英雄名称
        /// </summary>
        public string name;
 		
        
        /// <summary>
        ///英雄介绍
        /// </summary>
        public string describe;
 		
        
        /// <summary>
        ///基础攻击力
        /// </summary>
        public int baseAttack;
 		
        
        /// <summary>
        ///基础护甲
        /// </summary>
        public int baseDefence;
 		
        
        /// <summary>
        ///基础最大生命
        /// </summary>
        public int baseMaxHealth;
 		
        
        /// <summary>
        ///行动速度
        /// </summary>
        public int baseActionSpeed;
 		
        
        /// <summary>
        ///战斗模型Id
        /// </summary>
        public int combatModelId;
 		
        
        /// <summary>
        ///技能 id 组
        /// </summary>
        public string skillIds;
 		
        
                

        
        
    } 
}