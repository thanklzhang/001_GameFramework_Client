using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Battle_Client
{
    //实体身上的宝箱部分
    public partial class BattleEntity_Client
    {
        // //拥有的宝箱配置id list
        // // public List<int> boxConfigIdList;
        // // private List<BattleClientMsg_BattleBox> boxList;
        // private Dictionary<RewardQuality, List<BattleClientMsg_BattleBox>> boxDic;
        //
        // //当前打开的宝箱
        // public BattleClientMsg_BattleBox currOpenBox;
        //
        // public void SetBoxList(Dictionary<RewardQuality, List<BattleClientMsg_BattleBox>> boxDic)
        // {
        //     if (BattleManager.Instance.GetLocalCtrlHero().guid == this.guid)
        //     {
        //         Logx.Log(LogxType.BattleBox, "BattleEntity_Client : SetBoxList : count : " + boxDic.Count);
        //     }
        //
        //     this.boxDic = boxDic;
        //     EventDispatcher.Broadcast(EventIDs.OnUpdateBoxInfo);
        // }
        //
        // public void OnOpenBox(BattleClientMsg_BattleBox box)
        // {
        //     Logx.Log(LogxType.BattleBox,
        //         "BattleEntity_Client : OnOpenBox : box.selections?.Count : " + box.selections?.Count);
        //     currOpenBox = box;
        //
        //     EventDispatcher.Broadcast(EventIDs.OnBoxOpen);
        // }
        //
        // // public void OnSelectBox(int index)
        // // {
        // //     
        // // }
        //
        // public int GetBoxCount(RewardQuality quality)
        // {
        //     if (null == boxDic)
        //     {
        //         return 0;
        //     }
        //
        //     if (!boxDic.ContainsKey(quality))
        //     {
        //         return 0;
        //     }
        //
        //     var list = boxDic[quality];
        //     if (null == list)
        //     {
        //         return 0;
        //     }
        //
        //
        //     return list.Count;
        // }
        //
        //
        // public void TryOpenBox(RewardQuality quality)
        // {
        //     if (boxDic.ContainsKey(quality))
        //     {
        //         var list = boxDic[quality];
        //         if (list.Count > 0)
        //         {
        //             Logx.Log(LogxType.BattleBox, "BattleEntity_Client : TryOpenBox : quality : " + quality);
        //
        //             BattleManager.Instance.MsgSender.Send_OpenBox(quality);
        //         }
        //         else
        //         {
        //             Logx.Log(LogxType.BattleBox, "BattleEntity_Client : TryOpenBox : box list count is 0 : ");
        //         }
        //     }
        //     else
        //     {
        //         Logx.Log(LogxType.BattleBox, "BattleEntity_Client : TryOpenBox : no contain quality : " + quality);
        //     }
        //
        //
        //     // var boxCount = this.GetBoxCount();
        //     //
        //     // Logx.Log(LogxType.BattleBox,"BattleEntity_Client : TryOpenBox : boxCount : " + boxCount);
        //     //
        //     // if (boxCount > 0)
        //     // {
        //     //     //有箱子
        //     //     BattleManager.Instance.MsgSender.Send_OpenBox();
        //     // }
        //     // else
        //     // {
        //     //     //没有箱子
        //     //     Debug.Log("no box");
        //     // }
        // }
    }
}