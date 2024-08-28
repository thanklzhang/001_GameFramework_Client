/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Table
{
    
    
    
       
    public class PassiveEffect : BaseTable
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
        ///触发的时机类型
        /// </summary>
        private int triggerTimeType; 
        
        /// <summary>
        ///触发参数
        /// </summary>
        private string triggerParam; 
        
        /// <summary>
        ///触发几率(千分比)
        /// </summary>
        private int triggerChance; 
        
        /// <summary>
        ///触发目标类型
        /// </summary>
        private int triggerTargetType; 
        
        /// <summary>
        ///触发效果列表
        /// </summary>
        private string triggerEffectList; 
        
        /// <summary>
        ///触发后移除的效果列表
        /// </summary>
        private string afterTriggerRemoveEffectList; 
        
        /// <summary>
        ///触发CD(毫秒)
        /// </summary>
        private int triggerCD; 
        
        /// <summary>
        ///触发时候的效果资源id（自身）
        /// </summary>
        private int triggerEffectResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int TriggerTimeType { get => triggerTimeType; }     
        
        public string TriggerParam { get => triggerParam; }     
        
        public int TriggerChance { get => triggerChance; }     
        
        public int TriggerTargetType { get => triggerTargetType; }     
        
        public string TriggerEffectList { get => triggerEffectList; }     
        
        public string AfterTriggerRemoveEffectList { get => afterTriggerRemoveEffectList; }     
        
        public int TriggerCD { get => triggerCD; }     
        
        public int TriggerEffectResId { get => triggerEffectResId; }     
        

    } 
}