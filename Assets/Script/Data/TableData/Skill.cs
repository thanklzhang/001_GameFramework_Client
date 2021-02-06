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
       
    public class Skill : ConfigData
    {
        //fileds
        /// <summary>
        ///技能名称
        /// </summary>
        public string name;
 		
        
        /// <summary>
        ///技能介绍
        /// </summary>
        public string describe;
 		
        
        /// <summary>
        ///执行脚本 也可以是行为树生成的数据
        /// </summary>
        public string script;
 		
        
        /// <summary>
        ///技能效果 id
        /// </summary>
        public int skillEffectId;
 		
        
                

        
        
    } 
}