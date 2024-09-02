using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    //消耗道具相关
    public partial class BattleEntity_Client
    {
        List<BattleItemInfo> itemList = new List<BattleItemInfo>();

        public List<BattleItemInfo> GetItems()
        {
            return itemList;
        }

        //道具初始化
        void InitItemList()
        {
            itemList = new List<BattleItemInfo>();
            for (int i = 0; i < 6; i++)
            {
                BattleItemInfo item = new BattleItemInfo();
                item.index = i;
                item.ownerGuid = this.guid;


                itemList.Add(item);
            }
        }


        public BattleItemInfo FindItem(int index)
        {
            var item = this.itemList.Find((s) => { return s.index == index; });

            return item;
        }


        internal void UpdateItemInfo(int index, int configId, int count, float currCDTime, float maxCDTime)
        {
            Logx.Log(LogxType.BattleItem, "entity (client) UpdateItemInfo : UpdateItemInfo , index : " + index +
                                          " , configId : " + configId + " , count : " + count);

            var item = this.FindItem(index);
            // if (null == item)
            // {
            //     item = new BattleItemInfo()
            //     {
            //         configId = configId,
            //         index = index,
            //         count = count,
            //         currCDTime = currCDTime,
            //         maxCDTime = maxCDTime
            //     };
            //     
            //     this.itemList.Add(item);
            //     this.itemList.Sort((a,b) =>
            //     {
            //         return a.index.CompareTo(b.index);
            //     });
            // }

            if (item != null)
            {
                item?.UpdateInfo(configId, count, currCDTime, maxCDTime);

                EventDispatcher.Broadcast(EventIDs.OnItemInfoUpdate, item);
            }
        }
    }

    public class BattleItemInfo
    {
        public int ownerGuid;
        public int index;

        public int configId;
        public int count;

        public float maxCDTime;
        public float currCDTime;

        internal void UpdateInfo(int configId, int count, float currCDTime, float maxCDTime)
        {
            this.configId = configId;
            this.count = count;
            this.currCDTime = currCDTime;
            this.maxCDTime = maxCDTime;
        }
    }
}