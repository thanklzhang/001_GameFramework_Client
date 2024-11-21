/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Config
{
    
    
    
       
    public class BuffEffect : BaseConfig
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///buff介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///图标资源
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///施加效果的目标类型
        /// </summary>
        private int effectTargetType; 
        
        /// <summary>
        ///异常状态类型列表(,分割)
        /// </summary>
        private List<int> abnormalStateTypeList; 
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        private int lastTime; 
        
        /// <summary>
        ///是否能够被驱散
        /// </summary>
        private int isCanBeClear; 
        
        /// <summary>
        ///影响类型
        /// </summary>
        private int affectType; 
        
        /// <summary>
        ///初始层数
        /// </summary>
        private int initLayerCount; 
        
        /// <summary>
        ///满层数
        /// </summary>
        private int maxLayerCount; 
        
        /// <summary>
        ///叠层类型
        /// </summary>
        private int addLayerType; 
        
        /// <summary>
        ///是否满层移除
        /// </summary>
        private int isMaxLayerRemove; 
        
        /// <summary>
        ///满层触发效果列表
        /// </summary>
        private List<int> maxLayerTriggerEffectList; 
        
        /// <summary>
        ///满层触发时施加效果的目标类型
        /// </summary>
        private int maxLayerTriggerTargetType; 
        
        /// <summary>
        ///效果间隔时间(ms) 
        /// </summary>
        private int intervalTime; 
        
        /// <summary>
        ///持续时间附加组(ms)
        /// </summary>
        private List<List<int>> lastTimeAddedGroup; 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        private List<int> startEffectList; 
        
        /// <summary>
        ///间隔触发的效果列表
        /// </summary>
        private List<int> intervalEffectList; 
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        private List<int> endEffectList; 
        
        /// <summary>
        ///增加的属性组(,分割)
        /// </summary>
        private List<int> addedAttrGroup; 
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        private List<List<int>> addedValueGroup; 
        
        /// <summary>
        ///是否是持续性改变类型的属性
        /// </summary>
        private List<int> isAddedAttrGroupContinuous; 
        
        /// <summary>
        ///结束的时候移除的效果列表
        /// </summary>
        private List<int> endRemoveEffectList; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        
        /// <summary>
        ///显示类型
        /// </summary>
        private int showType; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int EffectTargetType { get => effectTargetType; }     
        
        public List<int> AbnormalStateTypeList { get => abnormalStateTypeList; }     
        
        public int LastTime { get => lastTime; }     
        
        public int IsCanBeClear { get => isCanBeClear; }     
        
        public int AffectType { get => affectType; }     
        
        public int InitLayerCount { get => initLayerCount; }     
        
        public int MaxLayerCount { get => maxLayerCount; }     
        
        public int AddLayerType { get => addLayerType; }     
        
        public int IsMaxLayerRemove { get => isMaxLayerRemove; }     
        
        public List<int> MaxLayerTriggerEffectList { get => maxLayerTriggerEffectList; }     
        
        public int MaxLayerTriggerTargetType { get => maxLayerTriggerTargetType; }     
        
        public int IntervalTime { get => intervalTime; }     
        
        public List<List<int>> LastTimeAddedGroup { get => lastTimeAddedGroup; }     
        
        public List<int> StartEffectList { get => startEffectList; }     
        
        public List<int> IntervalEffectList { get => intervalEffectList; }     
        
        public List<int> EndEffectList { get => endEffectList; }     
        
        public List<int> AddedAttrGroup { get => addedAttrGroup; }     
        
        public List<List<int>> AddedValueGroup { get => addedValueGroup; }     
        
        public List<int> IsAddedAttrGroupContinuous { get => isAddedAttrGroupContinuous; }     
        
        public List<int> EndRemoveEffectList { get => endRemoveEffectList; }     
        
        public int EffectResId { get => effectResId; }     
        
        public int ShowType { get => showType; }     
        

    } 
}