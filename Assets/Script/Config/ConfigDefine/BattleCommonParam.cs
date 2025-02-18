/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleCommonParam : BaseConfig
    {
        
        /// <summary>
        ///实体初始解锁实体装备栏数目
        /// </summary>
        private int initEntityItemBarCellUnlockCount; 
        
        /// <summary>
        ///实体的装备栏最大格子数量
        /// </summary>
        private int maxEntityItemBarCellCount; 
        
        /// <summary>
        ///实体装备栏解锁星级
        /// </summary>
        private List<int> entityItemBarCellUnlockStarLevel; 
        

        
        public int InitEntityItemBarCellUnlockCount { get => initEntityItemBarCellUnlockCount; }     
        
        public int MaxEntityItemBarCellCount { get => maxEntityItemBarCellCount; }     
        
        public List<int> EntityItemBarCellUnlockStarLevel { get => entityItemBarCellUnlockStarLevel; }     
        

    } 
}