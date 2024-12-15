/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class EntityUpdateParam : BaseConfig
    {
        
        /// <summary>
        ///英雄每次升星所需要的升星经验
        /// </summary>
        private List<int> upgradeExpPerStarLevel; 
        
        /// <summary>
        ///英雄每个星级对应的分解星级经验
        /// </summary>
        private List<int> decomoseExpPerStarLevel; 
        

        
        public List<int> UpgradeExpPerStarLevel { get => upgradeExpPerStarLevel; }     
        
        public List<int> DecomoseExpPerStarLevel { get => decomoseExpPerStarLevel; }     
        

    } 
}