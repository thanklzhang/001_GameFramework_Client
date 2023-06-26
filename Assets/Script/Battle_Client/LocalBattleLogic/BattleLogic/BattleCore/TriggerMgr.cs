using System;
using System.Collections.Generic;
using Battle.BattleTrigger.Runtime;

namespace Battle
{

    public class TriggerMgr
    {
        List<Trigger> triggers;

        Battle battle;

        public void Init(Battle battle)
        {
            this.battle = battle;

            triggers = new List<Trigger>();

        }

        //public void Load(List<string> triggerJsons)
        //{
        //    foreach (var triggerJson in triggerJsons)
        //    {
        //        var trigger = Trigger.ParseFromJson(triggerJson);
        //        trigger.Init(this.battle);
        //        triggers.Add(trigger);
        //    }

        //}

        public void Load(TriggerSourceResData srcData)
        {
            triggers = battle.TriggerReader.GetTriggers(srcData);

            foreach (var item in triggers)
            {
                item.Init(this.battle);
            }

            //foreach (var triggerJson in triggerJsons)
            //{
            //    var trigger = Trigger.ParseFromJson(triggerJson);
            //    trigger.Init(this.battle);
            //    triggers.Add(trigger);
            //}

        }

        public void Register()
        {
            foreach (var trigger in triggers)
            {
                //trigger.RegisterTimePassEvent();
                //trigger.RegisterEntityTypeDeadEvent();
                //trigger.RegisterBattleStartEvent();
                trigger.RegisterEvent();
            }
        }

        public void Clear()
        {
            foreach (var trigger in triggers)
            {
                trigger.RemoveEvent();
            }
        }
        List<Trigger> willDelteList = new List<Trigger>();
        internal void Update(float timeDelta)
        {
            willDelteList.Clear();
            for (int i = 0; i < triggers.Count; i++)
            {
                var trigger = triggers[i];
                trigger.Update(timeDelta);

                if (trigger.state == TriggerState.Finish)
                {
                    willDelteList.Add(trigger);
                }
            }

            //移除完成的节点
            for (int i = 0; i < willDelteList.Count; i++)
            {
                var delTri = willDelteList[i];
                triggers.Remove(delTri);
            }
        }
    }
}

