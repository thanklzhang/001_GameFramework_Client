/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Table;
namespace Battle
{
    
    
 
    public class BattleProcessWaveNode_Impl : IBattleProcessWaveNode
    {
        private Table.BattleProcessWaveNode config;
        
        public void Init(int id)
        {
            config = TableManager.Instance.GetById<Table.BattleProcessWaveNode>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///波次索引
        /// </summary>
        public int Index => config.Index;
        
        /// <summary>
        ///初始延迟（*1000）
        /// </summary>
        public int DelayTime => config.DelayTime;
        
        /// <summary>
        ///间隔时间（*1000）
        /// </summary>
        public int IntervalTime => config.IntervalTime;
        
        /// <summary>
        ///触发次数
        /// </summary>
        public int TriggerCount => config.TriggerCount;
        
        /// <summary>
        ///实体id
        /// </summary>
        public int EntityConfigId => config.EntityConfigId;
        
        /// <summary>
        ///实体个数
        /// </summary>
        public int EntityCount => config.EntityCount;
        
        /// <summary>
        ///生成位置类型
        /// </summary>
        public int PosType => config.PosType;
        
    } 
}