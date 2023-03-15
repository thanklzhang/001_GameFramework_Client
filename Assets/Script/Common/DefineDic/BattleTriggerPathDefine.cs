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
					
					"BattleTrigger/MainTaskBattle/MainTaskBattle_001/test.json",
					
					"BattleTrigger/MainTaskBattle/MainTaskBattle_001/TriggerScript/battle_end.json",
					
					"BattleTrigger/MainTaskBattle/MainTaskBattle_001/TriggerScript/create_entity_001.json",
					
					"BattleTrigger/MainTaskBattle/MainTaskBattle_001/TriggerScript/create_entity_002.json",
					
				}		
				
			},
			
			{"BattleTrigger/TeamBattle/TeamBattle_001",new List<string>()
				{
					
					"BattleTrigger/TeamBattle/TeamBattle_001/TriggerScript/battle_end.json",
					
					"BattleTrigger/TeamBattle/TeamBattle_001/TriggerScript/create_entity_001.json",
					
					"BattleTrigger/TeamBattle/TeamBattle_001/TriggerScript/create_entity_002.json",
					
				}		
				
			},
			
			{"BattleTrigger/TestTrigger",new List<string>()
				{
					
					"BattleTrigger/TestTrigger/test_trigger_gen_boss.json",
					
					"BattleTrigger/TestTrigger/test_trigger_gen_monster.json",
					
				}		
				
			},
			
		
		};
       
    }
	
	
	
}