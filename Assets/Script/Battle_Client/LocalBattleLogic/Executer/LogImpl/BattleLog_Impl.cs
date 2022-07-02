using System;

namespace Battle
{
    public class BattleLog_Impl : IBattleLog
    {
        public void Log(string str)
        {
            Logx.Log(str);
        }

        public void LogError(string str)
        {
            Logx.LogWarning(str);
        }

        public void LogWarning(string str)
        {
            Logx.LogWarning(str);
        }
    }

}

