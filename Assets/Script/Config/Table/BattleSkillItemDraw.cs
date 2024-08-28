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
    
    
    
       
    public class BattleSkillItemDraw : BaseTable
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
        ///图片资源id
        /// </summary>
        private int iconResId; 
        
        /// <summary>
        ///消耗道具id（，分割）
        /// </summary>
        private string costItemIds; 
        
        /// <summary>
        ///消耗道具数目（，分割）
        /// </summary>
        private string costItemCounts; 
        
        /// <summary>
        ///抽取权重（，分割 蓝紫橙红）
        /// </summary>
        private string drawWeights; 
        
        /// <summary>
        ///抽取值
        /// </summary>
        private string drawValues; 
        
        /// <summary>
        ///蓝色卡池ids（不填则是所有蓝色卡池）
        /// </summary>
        private string pool1ItemIds; 
        
        /// <summary>
        ///蓝色卡池权重（不填的话就是都相同）
        /// </summary>
        private string pool1ItemWeights; 
        
        /// <summary>
        ///紫色卡池ids
        /// </summary>
        private string pool2ItemIds; 
        
        /// <summary>
        ///紫色卡池权重
        /// </summary>
        private string pool2ItemWeights; 
        
        /// <summary>
        ///橙色卡池ids
        /// </summary>
        private string pool3ItemIds; 
        
        /// <summary>
        ///橙色卡池权重
        /// </summary>
        private string pool3ItemWeights; 
        
        /// <summary>
        ///红色卡池ids
        /// </summary>
        private string pool4ItemIds; 
        
        /// <summary>
        ///红色卡池权重
        /// </summary>
        private string pool4ItemWeights; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int IconResId { get => iconResId; }     
        
        public string CostItemIds { get => costItemIds; }     
        
        public string CostItemCounts { get => costItemCounts; }     
        
        public string DrawWeights { get => drawWeights; }     
        
        public string DrawValues { get => drawValues; }     
        
        public string Pool1ItemIds { get => pool1ItemIds; }     
        
        public string Pool1ItemWeights { get => pool1ItemWeights; }     
        
        public string Pool2ItemIds { get => pool2ItemIds; }     
        
        public string Pool2ItemWeights { get => pool2ItemWeights; }     
        
        public string Pool3ItemIds { get => pool3ItemIds; }     
        
        public string Pool3ItemWeights { get => pool3ItemWeights; }     
        
        public string Pool4ItemIds { get => pool4ItemIds; }     
        
        public string Pool4ItemWeights { get => pool4ItemWeights; }     
        

    } 
}