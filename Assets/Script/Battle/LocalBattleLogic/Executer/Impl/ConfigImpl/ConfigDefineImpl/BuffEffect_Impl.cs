/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class BuffEffect_Impl : IBuffEffect
    {
        private Config.BuffEffect config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.BuffEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///buff介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///图标资源
        /// </summary>
        public int IconResId => config.IconResId;
        
        /// <summary>
        ///施加效果的目标类型
        /// </summary>
        public int EffectTargetType => config.EffectTargetType;
        
        /// <summary>
        ///异常状态类型列表(,分割)
        /// </summary>
        public List<int> AbnormalStateTypeList => config.AbnormalStateTypeList;
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        public int LastTime => config.LastTime;
        
        /// <summary>
        ///是否能够被驱散
        /// </summary>
        public int IsCanBeClear => config.IsCanBeClear;
        
        /// <summary>
        ///初始层数
        /// </summary>
        public int InitLayerCount => config.InitLayerCount;
        
        /// <summary>
        ///满层数
        /// </summary>
        public int MaxLayerCount => config.MaxLayerCount;
        
        /// <summary>
        ///叠层类型
        /// </summary>
        public int AddLayerType => config.AddLayerType;
        
        /// <summary>
        ///是否满层移除
        /// </summary>
        public int IsMaxLayerRemove => config.IsMaxLayerRemove;
        
        /// <summary>
        ///满层触发效果列表
        /// </summary>
        public List<int> MaxLayerTriggerEffectList => config.MaxLayerTriggerEffectList;
        
        /// <summary>
        ///满层触发时施加效果的目标类型
        /// </summary>
        public int MaxLayerTriggerTargetType => config.MaxLayerTriggerTargetType;
        
        /// <summary>
        ///效果间隔时间(ms) 
        /// </summary>
        public int IntervalTime => config.IntervalTime;
        
        /// <summary>
        ///持续时间附加组(ms)
        /// </summary>
        public List<List<int>> LastTimeAddedGroup => config.LastTimeAddedGroup;
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        public List<int> StartEffectList => config.StartEffectList;
        
        /// <summary>
        ///间隔触发的效果列表
        /// </summary>
        public List<int> IntervalEffectList => config.IntervalEffectList;
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        public List<int> EndEffectList => config.EndEffectList;
        
        /// <summary>
        ///增加的属性组(,分割)
        /// </summary>
        public List<int> AddedAttrGroup => config.AddedAttrGroup;
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        public List<List<int>> AddedValueGroup => config.AddedValueGroup;
        
        /// <summary>
        ///结束的时候移除的效果列表
        /// </summary>
        public List<int> EndRemoveEffectList => config.EndRemoveEffectList;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}