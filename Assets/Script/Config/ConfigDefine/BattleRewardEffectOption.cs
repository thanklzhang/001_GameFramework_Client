/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BattleRewardEffectOption : BaseConfig
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
        ///奖励效果是否对新生成的队友也生效（目前适用于全员加属性和buff）
        /// </summary>
        private int applyToNewTeamMembers; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int GainTimingType { get => gainTimingType; }     
        
        public int Type { get => type; }     
        
        public List<int> ValueList { get => valueList; }     
        
        public List<int> WeightList { get => weightList; }     
        
        public int MaxGainTimesType { get => maxGainTimesType; }     
        
        public int ApplyToNewTeamMembers { get => applyToNewTeamMembers; }     
        

    } 
}