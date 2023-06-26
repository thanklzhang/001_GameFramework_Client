using System.Collections.Generic;

namespace Battle.BattleTrigger.Runtime
{
    public class TriggerSourceResData
    {
        //触发数据资源数据列表
        public List<string> dataStrList;
    }

    public interface ITriggerReader
    {
        List<Trigger> GetTriggers(TriggerSourceResData triggerSrcData);//int battleConfigId
        ExecuteGroup ParseTriggerActionExecuteGroup(ITriggerDataNode nodeJsonData, Trigger trigger, TriggerNode parent = null);
    }
}

