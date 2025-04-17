/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleReward : BaseConfig
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
        ///获得时机类型
        /// </summary>
        private int gainTimingType; 
        
        /// <summary>
        ///类型
        /// </summary>
        private int type; 
        
        /// <summary>
        ///参数值列表
        /// </summary>
        private List<int> valueList; 
        
        /// <summary>
        ///奖励中数值的权重
        /// </summary>
        private List<int> weightList; 
        
        /// <summary>
        ///最大获得次数
        /// </summary>
        private int maxGainTimesType; 
        
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
        
        public int GainTimingType { get => gainTimingType; }     
        
        public int Type { get => type; }     
        
        public List<int> ValueList { get => valueList; }     
        
        public List<int> WeightList { get => weightList; }     
        
        public int MaxGainTimesType { get => maxGainTimesType; }     
        
        public int Count { get => count; }     
        
        public int MakeSureRewardOccasion { get => makeSureRewardOccasion; }     
        

    } 
}