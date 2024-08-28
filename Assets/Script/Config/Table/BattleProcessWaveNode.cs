/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Table
{
    
    
    
       
    public class BattleProcessWaveNode : BaseTable
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///波次索引
        /// </summary>
        private int index; 
        
        /// <summary>
        ///初始延迟（*1000）
        /// </summary>
        private int delayTime; 
        
        /// <summary>
        ///间隔时间（*1000）
        /// </summary>
        private int intervalTime; 
        
        /// <summary>
        ///触发次数
        /// </summary>
        private int triggerCount; 
        
        /// <summary>
        ///实体id
        /// </summary>
        private int entityConfigId; 
        
        /// <summary>
        ///实体个数
        /// </summary>
        private int entityCount; 
        
        /// <summary>
        ///生成位置类型
        /// </summary>
        private int posType; 
        

        
        public string Describe { get => describe; }     
        
        public int Index { get => index; }     
        
        public int DelayTime { get => delayTime; }     
        
        public int IntervalTime { get => intervalTime; }     
        
        public int TriggerCount { get => triggerCount; }     
        
        public int EntityConfigId { get => entityConfigId; }     
        
        public int EntityCount { get => entityCount; }     
        
        public int PosType { get => posType; }     
        

    } 
}