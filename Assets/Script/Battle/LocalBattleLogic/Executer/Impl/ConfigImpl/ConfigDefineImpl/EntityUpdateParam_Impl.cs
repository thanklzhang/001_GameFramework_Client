/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class EntityUpdateParam_Impl : IEntityUpdateParam
    {
        private Config.EntityUpdateParam config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.EntityUpdateParam>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///英雄每次升星所需要的升星经验
        /// </summary>
        public List<int> UpgradeExpPerStarLevel => config.UpgradeExpPerStarLevel;
        
        /// <summary>
        ///英雄每个星级对应的分解星级经验
        /// </summary>
        public List<int> DecomoseExpPerStarLevel => config.DecomoseExpPerStarLevel;
        
    } 
}