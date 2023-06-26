namespace Battle.BattleTrigger.Runtime
{
    public class BaseVarField
    {
        public void Parse(ITriggerDataNode nodeJsonData)
        {
            this.OnParse(nodeJsonData);
        }

        public virtual void OnParse(ITriggerDataNode nodeJsonData)
        {

        }        
    }
}