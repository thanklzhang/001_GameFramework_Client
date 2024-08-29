/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Table
{
    
    
    
       
    public class MainTaskChapter : BaseTable
    {
        
        /// <summary>
        ///章节名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///描述
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///关卡
        /// </summary>
        private List<int> stageList; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public List<int> StageList { get => stageList; }     
        

    } 
}