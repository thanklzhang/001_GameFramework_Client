/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleBox : BaseConfig
    {
        
        /// <summary>
        ///名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///图标显示资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///奖励选项数
        /// </summary>
        private int selectionCount; 
        
        /// <summary>
        ///奖励池
        /// </summary>
        private int poolId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int SelectionCount { get => selectionCount; }     
        
        public int PoolId { get => poolId; }     
        

    } 
}