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
    
    
    
       
    public class BattleReward : BaseTable
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
        ///奖励图标
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///类型
        /// </summary>
        private int type; 
        
        /// <summary>
        ///参数值列表
        /// </summary>
        private string valueList; 
        
        /// <summary>
        ///权重
        /// </summary>
        private string weightList; 
        
        /// <summary>
        ///获得数目
        /// </summary>
        private int count; 
        
        /// <summary>
        ///确定实际奖励时机
        /// </summary>
        private int makeSureRewardOccasion; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int Type { get => type; }     
        
        public string ValueList { get => valueList; }     
        
        public string WeightList { get => weightList; }     
        
        public int Count { get => count; }     
        
        public int MakeSureRewardOccasion { get => makeSureRewardOccasion; }     
        

    } 
}