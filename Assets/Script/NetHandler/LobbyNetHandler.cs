//using Google.Protobuf;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class LobbyNetHandler : NetHandler
//{
//    public Action<bool> askEnterGameAction;
//    public override void Init()
//    {
//        base.Init();

//        AddListener(ProtoMsgIds.GC2CS_EnterGameService, AskEnterGameServer);

//    }

   
//    public void AskEnterGameServer(Action<bool> action)
//    {
//        ////send message
//        //GC2CS.reqEnterGameService req = new GC2CS.reqEnterGameService();
//        //req.Account = UserData.Instance.userAccount;
//        //req.Token = UserData.Instance.token;

//        //this.askEnterGameAction = action;

//        //this.SendMsgToCS(ProtoMsgIds.GC2CS_EnterGameService, req);

//    }

//    private void AskEnterGameServer(byte[] bytes)
//    {
//        var resp = GC2CS.respEnterGameService.Parser.ParseFrom(bytes);

//        if (resp.Err == ResultCode.Success)
//        {
//            this.askEnterGameAction?.Invoke(true);
//        }
//        else
//        {
//            Debug.Log("err : " + resp.Err.ToString());
//            this.askEnterGameAction?.Invoke(false);
//        }

//        this.askEnterGameAction = null;
//    }


//}

