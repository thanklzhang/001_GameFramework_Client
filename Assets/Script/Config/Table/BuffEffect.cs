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
    
    
    
       
    public class BuffEffect : BaseTable
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
        private string abnormalStateTypeList; 
        
        /// <summary>
        ///持续时间(ms)
        /// </summary>
        private int lastTime; 
        
        /// <summary>
        ///是否能够被驱散
        /// </summary>
        private int isCanBeClear; 
        
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
        private string maxLayerTriggerEffectList; 
        
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
        private string lastTimeAddedGroup; 
        
        /// <summary>
        ///开始的时候触发的效果列表
        /// </summary>
        private string startEffectList; 
        
        /// <summary>
        ///间隔触发的效果列表
        /// </summary>
        private string intervalEffectList; 
        
        /// <summary>
        ///结束的时候触发的效果列表
        /// </summary>
        private string endEffectList; 
        
        /// <summary>
        ///增加的属性组(,分割)
        /// </summary>
        private string addedAttrGroup; 
        
        /// <summary>
        ///增加属性组数值(,|分割 目前只做一个属性之只受一种属性增加)
        /// </summary>
        private string addedValueGroup; 
        
        /// <summary>
        ///结束的时候移除的效果列表
        /// </summary>
        private string endRemoveEffectList; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public int EffectTargetType { get => effectTargetType; }     
        
        public string AbnormalStateTypeList { get => abnormalStateTypeList; }     
        
        public int LastTime { get => lastTime; }     
        
        public int IsCanBeClear { get => isCanBeClear; }     
        
        public int MaxLayerCount { get => maxLayerCount; }     
        
        public int AddLayerType { get => addLayerType; }     
        
        public int IsMaxLayerRemove { get => isMaxLayerRemove; }     
        
        public string MaxLayerTriggerEffectList { get => maxLayerTriggerEffectList; }     
        
        public int MaxLayerTriggerTargetType { get => maxLayerTriggerTargetType; }     
        
        public int IntervalTime { get => intervalTime; }     
        
        public string LastTimeAddedGroup { get => lastTimeAddedGroup; }     
        
        public string StartEffectList { get => startEffectList; }     
        
        public string IntervalEffectList { get => intervalEffectList; }     
        
        public string EndEffectList { get => endEffectList; }     
        
        public string AddedAttrGroup { get => addedAttrGroup; }     
        
        public string AddedValueGroup { get => addedValueGroup; }     
        
        public string EndRemoveEffectList { get => endRemoveEffectList; }     
        
        public int EffectResId { get => effectResId; }     
        

    } 
}