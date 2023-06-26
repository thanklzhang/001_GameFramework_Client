using System;

namespace Battle
{
    public interface IBattleLog
    {
        void Log(string str);
        void LogWarning(string str);
        void LogError(string str);
    }

}

