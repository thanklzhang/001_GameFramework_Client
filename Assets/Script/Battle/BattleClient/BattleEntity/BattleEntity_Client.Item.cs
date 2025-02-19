using System.Collections.Generic;
using Config;
using UnityEngine;

namespace Battle_Client
{
    //道具相关
    public partial class BattleEntity_Client
    {
        //List<BattleItemInfo> itemList = new List<BattleItemInfo>();

        private List<ItemBarCellData_Client> itemBarCellList = new List<ItemBarCellData_Client>();

        // public List<BattleItemInfo> GetItems()
        // {
        //     return itemList;
        // }

        public List<ItemBarCellData_Client> GetItemBarCells()
        {
            return itemBarCellList;
        }

        //道具初始化
        void InitItemList()
        {
            // itemList = new List<BattleItemInfo>();
            // for (int i = 0; i < 6; i++)
            // {
            //     BattleItemInfo item = new BattleItemInfo();
            //     item.index = i;
            //     item.ownerGuid = this.guid;
            //
            //
            //     itemList.Add(item);
            // }

            itemBarCellList = new List<ItemBarCellData_Client>();
            
            var config = ConfigManager.Instance.GetById<Config.BattleCommonParam>(1);
            var maxCellCount = config.MaxEntityItemBarCellCount;
            var initCellCount = config.InitEntityItemBarCellUnlockCount;
            
            for (int i = 0; i < maxCellCount; i++)
            {
                var itemCell = new ItemBarCellData_Client();
                itemCell.index = i;
                itemCell.itemData = null;
                itemCell.isUnlock = i < initCellCount;
                itemBarCellList.Add(itemCell);
            }
        }


        public void UpdateItemBarCellList(List<ItemBarCellData_Client> incomeList)
        {
            List<ItemBarCellData_Client> changeList = new List<ItemBarCellData_Client>();
            for (int i = 0; i < incomeList.Count; i++)
            {
                var incomeBarCell = incomeList[i];
                var currCell = FindItemBarCell(incomeBarCell.index);
                if (currCell != null)
                {
                    currCell.itemData = incomeBarCell.itemData;
                    currCell.isUnlock = incomeBarCell.isUnlock;
                    
                    changeList.Add(currCell);
                }
            }

            EventDispatcher.Broadcast(EventIDs.OnEntityItemsUpdate,this,changeList);
        }

        public ItemBarCellData_Client FindItemBarCell(int index)
        {
            return this.itemBarCellList.Find(cell => cell.index == index);
        }

        public void UpdateItemBarItem(int index,BattleItemData_Client data,bool isUnlock)
        {
            var cell = FindItemBarCell(index);
            if (cell != null)
            {
                cell.itemData = data;
                cell.isUnlock = isUnlock;
            }
            
            EventDispatcher.Broadcast(EventIDs.OnEntityItemInfoUpdate,this,cell);
        }

        // public BattleItemInfo FindItem(int index)
        // {
        //     var item = this.itemList.Find((s) => { return s.index == index; });
        //
        //     return item;
        // }


        // internal void UpdateItemInfo(int index, int configId, int count, float currCDTime, float maxCDTime)
        // {
        //     Logx.Log(LogxType.BattleItem, "entity (client) UpdateItemInfo : UpdateItemInfo , index : " + index +
        //                                   " , configId : " + configId + " , count : " + count);
        //
        //     var item = this.FindItem(index);
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
        //
        //         EventDispatcher.Broadcast(EventIDs.OnItemInfoUpdate, item);
        //     }
        // }
    }

    // public class BattleItemInfo
    // {
    //     public int ownerGuid;
    //     public int index;
    //
    //     public int configId;
    //     public int count;
    //
    //     public float maxCDTime;
    //     public float currCDTime;
    //
    //     internal void UpdateInfo(int configId, int count, float currCDTime, float maxCDTime)
    //     {
    //         this.configId = configId;
    //         this.count = count;
    //         this.currCDTime = currCDTime;
    //         this.maxCDTime = maxCDTime;
    //     }
    // }


    //道具栏数据
    public class ItemBarCellData_Client
    {
        public int index;
        public BattleItemData_Client itemData;
        public bool isUnlock = true;
    }

    //道具数据
    public class BattleItemData_Client
    {
        public int configId;
        public int count;
    }
}