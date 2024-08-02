using Google.Protobuf.Collections;
using NetProto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Battle;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public partial class BattleEntity_Client
    {
        //拥有的宝箱配置id list
        // public List<int> boxConfigIdList;
        private List<BattleClientMsg_BattleBox> boxList;
        //当前打开的宝箱
        public BattleClientMsg_BattleBox currOpenBox;

        public void SetBoxList(List<BattleClientMsg_BattleBox> boxList)
        {
            if (BattleManager.Instance.GetLocalCtrlHero().guid == this.guid)
            {
                Logx.Log(LogxType.BattleBox,"BattleEntity_Client : SetBoxList : count : " + boxList.Count);
            }

            this.boxList = boxList;
            EventDispatcher.Broadcast(EventIDs.OnUpdateBoxInfo);
        }

        public void OnOpenBox(BattleClientMsg_BattleBox box)
        {
            Logx.Log(LogxType.BattleBox,"BattleEntity_Client : OnOpenBox : box.selections?.Count : " + box.selections?.Count);
            currOpenBox = box;
          
            EventDispatcher.Broadcast(EventIDs.OnBoxOpen);
        }

        // public void OnSelectBox(int index)
        // {
        //     
        // }

        public int GetBoxCount()
        {
            if (null == boxList)
            {
                return 0;
            }

            return boxList.Count;
        }

        
        public void TryOpenBox()
        {
           
            var boxCount = this.GetBoxCount();
            
            Logx.Log(LogxType.BattleBox,"BattleEntity_Client : TryOpenBox : boxCount : " + boxCount);
            
            if (boxCount > 0)
            {
                //有箱子
                BattleManager.Instance.MsgSender.Send_OpenBox();
            }
            else
            {
                //没有箱子
                Debug.Log("no box");
            }
        }


    }

}