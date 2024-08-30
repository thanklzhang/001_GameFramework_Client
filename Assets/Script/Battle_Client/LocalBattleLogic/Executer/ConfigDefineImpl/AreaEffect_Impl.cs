/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Table;
namespace Battle
{
    
    
 
    public class AreaEffect_Impl : IAreaEffect
    {
        private Table.AreaEffect config;
        
        public void Init(int id)
        {
            config = TableManager.Instance.GetById<Table.AreaEffect>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///技能名称
        /// </summary>
        public string Name => config.Name;
        
        /// <summary>
        ///技能介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///区域类型
        /// </summary>
        public int AreaType => config.AreaType;
        
        /// <summary>
        ///开始点类型
        /// </summary>
        public int StartPosType => config.StartPosType;
        
        /// <summary>
        ///开始点位移方向类型
        /// </summary>
        public int StartPosShiftDirType => config.StartPosShiftDirType;
        
        /// <summary>
        ///开始点位移方向的距离(*1000)
        /// </summary>
        public int StartPosShiftDistance => config.StartPosShiftDistance;
        
        /// <summary>
        ///影响范围的参数 半径 或者 长宽等  (*1000)
        /// </summary>
        public List<int> RangeParam => config.RangeParam;
        
        /// <summary>
        ///选择实体类型
        /// </summary>
        public int SelectEntityType => config.SelectEntityType;
        
        /// <summary>
        ///触发的效果列表（对每个选取单位）
        /// </summary>
        public List<int> EffectList => config.EffectList;
        
        /// <summary>
        ///效果资源id
        /// </summary>
        public int EffectResId => config.EffectResId;
        
    } 
}