/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BattleItem_Impl : IBattleItem
    {
        private Config.BattleItem config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BattleItem>(id);
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
        ///图片资源id
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///道具类型
        /// </summary>
        public int ItemType => config.ItemType;
        
        /// <summary>
        ///道具品质
        /// </summary>
        public int ItemQuality => config.ItemQuality;
        
        /// <summary>
        ///叠加类型
        /// </summary>
        public int AddType => config.AddType;
        
        /// <summary>
        ///使用后是否销毁
        /// </summary>
        public int IsDestroyAfterUse => config.IsDestroyAfterUse;
        
        /// <summary>
        ///包含的技能id（包含主动效果和被动效果）
        /// </summary>
        public int SkillId => config.SkillId;
        
        /// <summary>
        ///增加的属性组(,分割)
        /// </summary>
        public List<int> AddedAttrGroup => config.AddedAttrGroup;
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        public List<List<int>> AddedValueGroup => config.AddedValueGroup;
        
    } 
}