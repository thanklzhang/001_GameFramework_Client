/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Table
{
    
    
    
       
    public class EntityInfo : BaseTable
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
        ///类型(0 npc单位实例 , 1 英雄模板)
        /// </summary>
        private int type; 
        
        /// <summary>
        ///模型Id(先填资源id 之后换成另一张表格过度)
        /// </summary>
        private int modelId; 
        
        /// <summary>
        ///基础属性模板id
        /// </summary>
        private int baseAttrId; 
        
        /// <summary>
        ///等级属性模板id
        /// </summary>
        private int levelAttrId; 
        
        /// <summary>
        ///星级属性模板id
        /// </summary>
        private int starAttrId; 
        
        /// <summary>
        ///技能id列表(这个会换成技能模板)
        /// </summary>
        private List<int> skillIds; 
        
        /// <summary>
        ///等级
        /// </summary>
        private int level; 
        
        /// <summary>
        ///星级
        /// </summary>
        private int star; 
        
        /// <summary>
        ///技能等级
        /// </summary>
        private List<int> skillLevels; 
        
        /// <summary>
        ///AI脚本
        /// </summary>
        private string aiScript; 
        
        /// <summary>
        ///是否boss
        /// </summary>
        private int isBoss; 
        
        /// <summary>
        ///头像资源id
        /// </summary>
        private int avatarResId; 
        
        /// <summary>
        ///全身像资源id
        /// </summary>
        private int allBodyResId; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int Type { get => type; }     
        
        public int ModelId { get => modelId; }     
        
        public int BaseAttrId { get => baseAttrId; }     
        
        public int LevelAttrId { get => levelAttrId; }     
        
        public int StarAttrId { get => starAttrId; }     
        
        public List<int> SkillIds { get => skillIds; }     
        
        public int Level { get => level; }     
        
        public int Star { get => star; }     
        
        public List<int> SkillLevels { get => skillLevels; }     
        
        public string AiScript { get => aiScript; }     
        
        public int IsBoss { get => isBoss; }     
        
        public int AvatarResId { get => avatarResId; }     
        
        public int AllBodyResId { get => allBodyResId; }     
        

    } 
}