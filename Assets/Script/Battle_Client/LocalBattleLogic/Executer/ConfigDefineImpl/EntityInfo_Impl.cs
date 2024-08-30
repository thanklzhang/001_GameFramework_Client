/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Table;
namespace Battle
{
    
    
 
    public class EntityInfo_Impl : IEntityInfo
    {
        private Table.EntityInfo config;
        
        public void Init(int id)
        {
            config = TableManager.Instance.GetById<Table.EntityInfo>(id);
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
        ///类型(0 npc单位实例 , 1 英雄模板)
        /// </summary>
        public int Type => config.Type;
        
        /// <summary>
        ///模型Id(先填资源id 之后换成另一张表格过度)
        /// </summary>
        public int ModelId => config.ModelId;
        
        /// <summary>
        ///基础属性模板id
        /// </summary>
        public int BaseAttrId => config.BaseAttrId;
        
        /// <summary>
        ///等级属性模板id
        /// </summary>
        public int LevelAttrId => config.LevelAttrId;
        
        /// <summary>
        ///星级属性模板id
        /// </summary>
        public int StarAttrId => config.StarAttrId;
        
        /// <summary>
        ///技能id列表(这个会换成技能模板)
        /// </summary>
        public List<int> SkillIds => config.SkillIds;
        
        /// <summary>
        ///等级
        /// </summary>
        public int Level => config.Level;
        
        /// <summary>
        ///星级
        /// </summary>
        public int Star => config.Star;
        
        /// <summary>
        ///技能等级
        /// </summary>
        public List<int> SkillLevels => config.SkillLevels;
        
        /// <summary>
        ///AI脚本
        /// </summary>
        public string AiScript => config.AiScript;
        
        /// <summary>
        ///是否boss
        /// </summary>
        public int IsBoss => config.IsBoss;
        
        /// <summary>
        ///头像资源id
        /// </summary>
        public int AvatarResId => config.AvatarResId;
        
        /// <summary>
        ///全身像资源id
        /// </summary>
        public int AllBodyResId => config.AllBodyResId;
        
    } 
}