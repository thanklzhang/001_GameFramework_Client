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
    
    
    
       
    public class EntityAttrBase : BaseTable
    {
        
        /// <summary>
        ///介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///攻击力
        /// </summary>
        private int attack; 
        
        /// <summary>
        ///防御值
        /// </summary>
        private int defence; 
        
        /// <summary>
        ///生命值
        /// </summary>
        private int health; 
        
        /// <summary>
        ///魔法值
        /// </summary>
        private int magic; 
        
        /// <summary>
        ///攻击速度(*1000) 1 秒中攻击次数
        /// </summary>
        private int attackSpeed; 
        
        /// <summary>
        ///移动速度(*1000)
        /// </summary>
        private int moveSpeed; 
        
        /// <summary>
        ///攻击距离(*1000)
        /// </summary>
        private int attackRange; 
        
        /// <summary>
        ///承受伤害千分比
        /// </summary>
        private int inputDamageRate; 
        

        
        public string Describe { get => describe; }     
        
        public int Attack { get => attack; }     
        
        public int Defence { get => defence; }     
        
        public int Health { get => health; }     
        
        public int Magic { get => magic; }     
        
        public int AttackSpeed { get => attackSpeed; }     
        
        public int MoveSpeed { get => moveSpeed; }     
        
        public int AttackRange { get => attackRange; }     
        
        public int InputDamageRate { get => inputDamageRate; }     
        

    } 
}