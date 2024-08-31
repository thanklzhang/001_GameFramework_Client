/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
using Config;
namespace Battle
{
    
    
 
    public class EntityAttrLevel_Impl : IEntityAttrLevel
    {
        private Config.EntityAttrLevel config;
        
        public void Init(int id)
        {
            config = ConfigManager.Instance.GetById<Config.EntityAttrLevel>(id);
        }
        
        public int Id => config.Id;
        
        /// <summary>
        ///作为模板 id
        /// </summary>
        public int TemplateId => config.TemplateId;
        
        /// <summary>
        ///等级
        /// </summary>
        public int Level => config.Level;
        
        /// <summary>
        ///介绍
        /// </summary>
        public string Describe => config.Describe;
        
        /// <summary>
        ///攻击力
        /// </summary>
        public int Attack => config.Attack;
        
        /// <summary>
        ///防御值
        /// </summary>
        public int Defence => config.Defence;
        
        /// <summary>
        ///生命值
        /// </summary>
        public int Health => config.Health;
        
        /// <summary>
        ///魔法值
        /// </summary>
        public int Magic => config.Magic;
        
        /// <summary>
        ///攻击速度(*1000) 1 秒中攻击次数
        /// </summary>
        public int AttackSpeed => config.AttackSpeed;
        
        /// <summary>
        ///移动速度(*1000)
        /// </summary>
        public int MoveSpeed => config.MoveSpeed;
        
        /// <summary>
        ///攻击距离(*1000)
        /// </summary>
        public int AttackRange => config.AttackRange;
        
    } 
}