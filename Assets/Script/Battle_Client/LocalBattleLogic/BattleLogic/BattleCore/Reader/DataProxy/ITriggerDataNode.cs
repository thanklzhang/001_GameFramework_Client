using System;

namespace Battle.BattleTrigger.Runtime
{
    public interface ITriggerDataNode
    {
        Type GetDataType();

        bool ContainsKey(string key);

        string GetString(string key);

        int GetInt(string key);

        float GetFloat(string key);

        ITriggerDataNode GetValueObj(string key);

        bool IsArray { get; }

        int Count { get; }

        ITriggerDataNode this[string key]
        {
            get;
            set;
        }

        ITriggerDataNode this[int idx]
        {
            get;
            set;
        }

        string ToString();
    }

    
}
