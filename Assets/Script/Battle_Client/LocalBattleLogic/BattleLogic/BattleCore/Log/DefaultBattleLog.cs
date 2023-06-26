using System;

namespace Battle
{
    public class DefaultBattleLog : IBattleLog
    {
        public void Log(string str)
        {
            Console.WriteLine("battle log : " + str);
        }

        public void LogError(string str)
        {
            Console.WriteLine("battle error : " + str);
        }

        public void LogWarning(string str)
        {
            Console.WriteLine("battle warning : " + str);
        }
    }

}

