using System.Collections.Generic;
using Battle;

namespace Battle_Client
{
    public class WarehouseItemCellData_Client
    {
        public int index;
        public BattleItemData_Client itemData;

        public void Init(int index)
        {
            this.index = index;
        }

        public void SetData(BattleItemData_Client itemData)
        {
            this.itemData = itemData;
        }
    }

    public class BattleItemData_Client
    {
        public int configId;
        public int count;
    }

    public class ItemWarehouse
    {
        public List<WarehouseItemCellData_Client> itemCellList;

        public int maxCellCount = 100;

        public void Init()
        {
            itemCellList = new List<WarehouseItemCellData_Client>();
            for (int i = 0; i < maxCellCount; i++)
            {
                var cell = new WarehouseItemCellData_Client();
                cell.Init(i);
                itemCellList.Add(cell);
            }
        }

        public WarehouseItemCellData_Client GetCell(int index)
        {
            return itemCellList[index];
        }

        // public void SetItemWarehouseData(List<WarehouseItemCellData_Client> list)
        // {
        //     foreach (var cell in list)
        //     {
        //         var currCell = GetCell(cell.index);
        //         //update
        //         currCell.itemData = cell.itemData;
        //     }
        //
        //     //EventDispatcher.Broadcast(EventIDs.OnUpdateBattleCurrencyInfo);
        // }

        public void SetWarehouseItemData(BattleItemData_Client itemData,int index)
        {
            var cell = GetCell(index);
            cell.itemData = itemData;
            
            EventDispatcher.Broadcast<WarehouseItemCellData_Client>(EventIDs.OnUpdateWarehouseItemData,
                cell);
            
        }
    }
}