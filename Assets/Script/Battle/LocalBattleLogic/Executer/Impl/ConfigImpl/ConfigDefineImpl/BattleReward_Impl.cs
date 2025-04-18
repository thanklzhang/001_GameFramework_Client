/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleReward_Impl : IBattleReward
    {
        private Config.BattleReward config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleReward>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///奖励图标
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///获得时机类型
        /// </summary>
        public int GainTimingType => config.GainTimingType;
        
        /// <summary>
        ///类型
        /// </summary>
        public int Type => config.Type;
        
        /// <summary>
        ///参数值列表
        /// </summary>
        public List<int> ValueList => config.ValueList;
        
        /// <summary>
        ///奖励中数值的权重
        /// </summary>
        public List<int> WeightList => config.WeightList;
        
        /// <summary>
        ///最大获得次数
        /// </summary>
        public int MaxGainTimesType => config.MaxGainTimesType;
        
        /// <summary>
        ///奖励效果是否对新生成的队友也生效（目前适用于全员加属性和buff）
        /// </summary>
        public int ApplyToNewTeamMembers => config.ApplyToNewTeamMembers;
        
        /// <summary>
        ///获得数目
        /// </summary>
        public int Count => config.Count;
        
        /// <summary>
        ///确定实际奖励时机
        /// </summary>
        public int MakeSureRewardOccasion => config.MakeSureRewardOccasion;
        
    } 
}