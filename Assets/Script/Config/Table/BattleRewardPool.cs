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
    
    
    
       
    public class BattleRewardPool : BaseTable
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
        ///固定奖励（一定产出）
        /// </summary>
        private string fixedRewardList; 
        
        /// <summary>
        ///奖励id总列表
        /// </summary>
        private string rewardIdList; 
        
        /// <summary>
        ///权重
        /// </summary>
        private string rewardWeightList; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public string FixedRewardList { get => fixedRewardList; }     
        
        public string RewardIdList { get => rewardIdList; }     
        
        public string RewardWeightList { get => rewardWeightList; }     
        

    } 
}