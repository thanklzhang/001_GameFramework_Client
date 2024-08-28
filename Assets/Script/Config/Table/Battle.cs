/*
 * generate by tool
*/
//using System.Collections;
using System.Collections.Generic;
namespace Table
{
    
    
    
       
    public class Battle : BaseTable
    {
        
        /// <summary>
        ///战斗名称
        /// </summary>
        private string name; 
        
        /// <summary>
        ///战斗介绍
        /// </summary>
        private string describe; 
        
        /// <summary>
        ///活动 id
        /// </summary>
        private int activityId; 
        
        /// <summary>
        ///地图 id
        /// </summary>
        private int mapId; 
        
        /// <summary>
        ///战斗触发器id
        /// </summary>
        private int triggerId; 
        
        /// <summary>
        ///强制玩家控制某一个实体(|分割玩家 ,分割索引和实体id)
        /// </summary>
        private string forceUseHeroList; 
        
        /// <summary>
        ///玩家控制实体初始位置(|分割玩家 ,分割坐标轴)(长度作为玩家数最大数目)
        /// </summary>
        private string initPos; 
        
        /// <summary>
        ///boss限时击杀时间(*1000 微妙)
        /// </summary>
        private int bossLimitTime; 
        
        /// <summary>
        ///关卡流程id
        /// </summary>
        private int processId; 
        
        /// <summary>
        ///战斗配置id
        /// </summary>
        private int battleConfigId; 
        
        /// <summary>
        ///队伍信息(| 分割队伍  ,分割玩家索引)
        /// </summary>
        private string teamInfo; 
        

        
        public string Name { get => name; }     
        
        public string Describe { get => describe; }     
        
        public int ActivityId { get => activityId; }     
        
        public int MapId { get => mapId; }     
        
        public int TriggerId { get => triggerId; }     
        
        public string ForceUseHeroList { get => forceUseHeroList; }     
        
        public string InitPos { get => initPos; }     
        
        public int BossLimitTime { get => bossLimitTime; }     
        
        public int ProcessId { get => processId; }     
        
        public int BattleConfigId { get => battleConfigId; }     
        
        public string TeamInfo { get => teamInfo; }     
        

    } 
}