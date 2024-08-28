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
    
    
    
       
    public class TitleBar : BaseTable
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
        ///标题资源id
        /// </summary>
        private int titleRes; 
        
        /// <summary>
        ///标题显示文本
        /// </summary>
        private string titleName; 
        
        /// <summary>
        ///标题资源列表(，分割)
        /// </summary>
        private string resList; 
        
        /// <summary>
        ///是否显示关闭按钮
        /// </summary>
        private int isShowCloseBtn; 
        
        /// <summary>
        ///是否显示背景
        /// </summary>
        private int isShowBg; 
        
        /// <summary>
        ///是否显示线
        /// </summary>
        private int isShowLine; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int TitleRes { get => titleRes; }     
        
        public string TitleName { get => titleName; }     
        
        public string ResList { get => resList; }     
        
        public int IsShowCloseBtn { get => isShowCloseBtn; }     
        
        public int IsShowBg { get => isShowBg; }     
        
        public int IsShowLine { get => isShowLine; }     
        

    } 
}