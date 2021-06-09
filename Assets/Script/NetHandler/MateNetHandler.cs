//using Google.Protobuf;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class MateNetHandler : NetHandler
//{
//    public Action startMateAction;
//    public Action combatInitInfoAction;
//    public override void Init()
//    {
//        base.Init();
        
//        AddListener(ProtoMsgIds.GC2CS_StartMateCombat, RespStartMateCombat);
//        AddListener(ProtoMsgIds.GC2SS_SyncCombatInitInfo, RespCombatInitInfo);
        
//    }

//    public void ReqStartMateCombat(Action action = null, Action combatInitAction = null)
//    {
//        GC2CS.reqStartMateCombat req = new GC2CS.reqStartMateCombat();

//        startMateAction = action;
//        combatInitInfoAction = combatInitAction;
//        //send message
//        this.SendMsgToCS(ProtoMsgIds.GC2CS_StartMateCombat, req);
//    }

//    public void RespStartMateCombat(byte[] data)
//    {
//        GC2CS.respStartMateCombat resp = GC2CS.respStartMateCombat.Parser.ParseFrom(data);

//        if (resp.Ret != ResultCode.Success)
//        {
//            Debug.Log("ret : " + resp.Ret);
//        }
        
//        startMateAction?.Invoke();
        
//    }

//    //匹配战斗创建成功 客户端在此先进行战斗初始化的操作
//    public void RespCombatInitInfo(byte[] data)
//    {
//        SyncCombatInitInfo initInfo = SyncCombatInitInfo.Parser.ParseFrom(data);

//        //CombatData.Instance.SetCombatInitData(initInfo);

//        CombatManager.Instance.CreateCombat(initInfo);

//        combatInitInfoAction?.Invoke();
        
//    }


//}

