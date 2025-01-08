using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    //技能道具相关（如 技能书）
    public partial class BattleEntity_Client
    {
        // List<BattleItemInfo> skillItemList = new List<BattleItemInfo>();
        //
        // public List<BattleItemInfo> GetSkillItems()
        // {
        //     return skillItemList;
        // }
        //
        // public BattleItemInfo FindSkillItem(int index)
        // {
        //     var item = this.skillItemList.Find((s) => { return s.index == index; });
        //
        //     return item;
        // }
        //
        //
        // internal void UpdateSkillItemInfo(int index, int configId, int count, float currCDTime, float maxCDTime)
        // {
        //     Logx.Log(LogxType.BattleItem, "entity (client) : UpdateSkillItemInfo , index : " + index +
        //                                   " , configId : " + configId + " , count : " + count);
        //
        //     var item = this.FindSkillItem(index);
        //     // if (null == item)
        //     // {
        //     //     item = new BattleItemInfo()
        //     //     {
        //     //         configId = configId,
        //     //         index = index,
        //     //         count = count,
        //     //         currCDTime = currCDTime,
        //     //         maxCDTime = maxCDTime
        //     //     };
        //     //     
        //     //     this.itemList.Add(item);
        //     //     this.itemList.Sort((a,b) =>
        //     //     {
        //     //         return a.index.CompareTo(b.index);
        //     //     });
        //     // }
        //
        //     if (item != null)
        //     {
        //         item?.UpdateInfo(configId, count, currCDTime, maxCDTime);
        //         if (count <= 0)
        //         {
        //             this.skillItemList.Remove(item);
        //         }
        //     }
        //     else
        //     {
        //         item = new BattleItemInfo();
        //         item.configId = configId;
        //         item.count = count;
        //         item.currCDTime = currCDTime;
        //         item.maxCDTime = maxCDTime;
        //         item.index = index;
        //         item.ownerGuid = this.guid;
        //
        //         this.skillItemList.Add(item);
        //     }
        //
        //     EventDispatcher.Broadcast(EventIDs.OnSkillItemInfoUpdate, item);
        // }
    }
}