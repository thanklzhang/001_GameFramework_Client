//using Google.Protobuf;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//class ClientConnect : TcpClient
//{
//    //public string userName;

//    //bool IsConnected;

//    private static ClientConnect instance;


//    //public string account;
//    public int userId;
//    //public string token;//之后改成持久化

//    public List<MsgPack> msgList;

//    internal static ClientConnect Instance
//    {
//        get
//        {
//            if (null == instance)
//            {
//                instance = new ClientConnect();
//            }

//            return instance;

//        }
//    }

//    //MessagerHandler msgHandler;

//    public ClientConnect()
//    {
//        //Instance = this;
//        //HandleMsgManager.Instance.Init();
//        Reset();
//    }

//    public void Reset()
//    {
//        //userName = "";
//        userId = 0;
//        //IsConnected = false;
//        msgList = new List<MsgPack>();

//        //msgHandler = new MessagerHandler();
//    }

//    public override void HandleMsg(MsgPack pack)
//    {
//        //base.HandleMsg(pack);//由于要改为轮询 所以重写

//        //作为客户端
//        //if ((ushort)NetCommon.MsgId.HeartBeatHandshake == pack.msgId)
//        //{
//        //    heartBeatService.Start();
//        //}
//        //else if ((ushort)NetCommon.MsgId.HeartBeatBack == pack.msgId)
//        //{
//        //    heartBeatService.ResetTimeout();
//        //}
//        ////作为服务端的 client 连接
//        //else if ((ushort)NetCommon.MsgId.HeartBeatSend == pack.msgId)
//        //{
//        //    ResetHeartBeat();
//        //}

//        //if (Const.receiveHeartBeatMsgId == pack.msgId)
//        //{
//        //    heartBeatService.ResetTimeout();
//        //}
//        //else
//        //{
//        //    msgList.Add(pack);
//        //}

//        msgList.Add(pack);



//    }



//    public void Update(float deltaTime)
//    {
//        if (this.netState == NetState.Close)
//        {
//            return;
//        }
//        //ReceiveUpdate();
//        //Singleton<HandleMsgManager>.Instance.Update();
//        //Debug.Log("c 0: " + msgList.Count);
//        while (msgList.Count > 0)
//        {
//            //Debug.Log("c1 : " + msgList.Count);
//            var msg = msgList[0];
//            //HandleMsg(msg);
//            //Debug.Log("c2 : " + msgList.Count);
//            //msgHandler.HandleMsg(msg);
//            var msgId = msg.msgId;
//            var data = msg.data;

         

//            //Debug.Log("c 3: " + msgList.Count);
//            msgList.RemoveAt(0);
//            Debug.Log("receive msg from server:  msgId : " + msgId);
//            EventManager.Broadcast(msgId, data);

//        }
//    }

//    public void CloseConnect()
//    {
//        base.Close();
//        Reset();

//    }

//}

