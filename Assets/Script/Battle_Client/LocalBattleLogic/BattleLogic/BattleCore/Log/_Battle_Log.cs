using System;

namespace Battle
{
    public class _Battle_Log
    {
        static IBattleLog log;

        public static void RegisterLog(IBattleLog _log)
        {
            log = _log;
            if (null == log)
            {
                log = new DefaultBattleLog();
            }
        }
        public static void Log(string str)
        {
            //log.Log(str);
        }
        public static void LogWarning(string str)
        {
            log.LogWarning(str);
        }
        public static void LogError(string str,Exception e = null)
        {
            var resultStr = str;
            
            log.LogError(str);
        }
        public static void LogException(Exception e = null)
        {
            var str = "";
            if (e != null)
            {
                str = e.Message + "\n" + e.StackTrace;
            }

            log.LogError(str);
        }
    }

}

