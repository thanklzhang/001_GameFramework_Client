/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Table
{
    
    
    
       
    public class CalculateEffect : BaseTable
    {
        
        /// <summary>
        ///技能名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///技能介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///施加效果的目标类型
        /// </summary>
        private int effectTargetType; 
        
        /// <summary>
        ///附加伤害组
        /// </summary>
        private List<List<int>> addedValueGroup; 
        
        /// <summary>
        ///最终的效果类型 1 物理伤害 2 法强伤害 
        /// </summary>
        private int finalEffectType; 
        
        /// <summary>
        ///效果资源id
        /// </summary>
        private int effectResId; 
        
        /// <summary>
        ///产生效果点位置类型
        /// </summary>
        private int effectPosType; 
        
        /// <summary>
        ///特效是否跟随目标
        /// </summary>
        private int isEffectFollowTarget; 
        
        /// <summary>
        ///效果点节点名称
        /// </summary>
        private string effectPosName; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int EffectTargetType { get => effectTargetType; }     
        
        public List<List<int>> AddedValueGroup { get => addedValueGroup; }     
        
        public int FinalEffectType { get => finalEffectType; }     
        
        public int EffectResId { get => effectResId; }     
        
        public int EffectPosType { get => effectPosType; }     
        
        public int IsEffectFollowTarget { get => isEffectFollowTarget; }     
        
        public string EffectPosName { get => effectPosName; }     
        

    } 
}