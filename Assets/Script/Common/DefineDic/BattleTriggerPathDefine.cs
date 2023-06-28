/*
 * generate by tool
*/
using System.Collections;
using System;
using System.Collections.Generic;
namespace Table
{

  public class BattlrTriggerPathDefine
    {
        public static List<string> GetTriggerPathList(string key)
        {
            if (triggerPathDic.ContainsKey(key))
            {
                return triggerPathDic[key];
            }
            else
            {
                return new List<string>();   
            }
            
        }
		static Dictionary<string,List<string>> triggerPathDic = new Dictionary<string,List<string>>()
		{
			
			{"BattleTrigger/MainTaskBattle/MainTaskBattle_001",new List<string>()
				{
					
				}		
				
			},
			
			{"BattleTrigger/TeamBattle/TeamBattle_001",new List<string>()
				{
					
				}		
				
			},
			
			{"BattleTrigger/TestTrigger",new List<string>()
				{
					
					"BattleTrigger/TestTrigger/new_trigger0.json",
					
					"BattleTrigger/TestTrigger/test_trigger_gen_boss 1.json",
					
					"BattleTrigger/TestTrigger/test_trigger_gen_boss.json",
					
					"BattleTrigger/TestTrigger/test_trigger_gen_monster.json",
					
				}		
				
			},
			
		
		};
       
    }
	
	
	
}