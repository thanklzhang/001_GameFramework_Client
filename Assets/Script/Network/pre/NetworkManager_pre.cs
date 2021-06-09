//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;
//using UnityEngine;

//public class NetworkManager : Singleton<NetworkManager>
//{
//    /// <summary>
//    /// 发送同步包
//    /// </summary>
//    /// <param name="msgId"></param>
//    /// <param name="msgData"></param>
//    /// <param name="callback"></param>
//    public void SendMsg(int msgId,byte[] msgData)
//    {
        
//    }
    
//    ///////////////////////////////////////////////////////////////////////////////
//    ClientConnect client;

//    public void ConnectLoginServer()
//    {
//        client = new ClientConnect();
//        client.connectAction += ConnectLoginServerResult;
//        //连接登录服务器
//        client.Connect("127.0.0.1", 2345);
//        Debug.Log("connecting login server ...");

//    }

//    public void ConnectLoginServerResult(bool isSuccess)
//    {
//        Loom.QueueOnMainThread(() =>
//        {
//            Debug.Log("success to connect the login server");
//            if (isSuccess)
//            {
//                EventManager.Broadcast((int)GameEvent.ConnectLoginServerResult, true);
//                Debug.Log("success to connect the login server");
//            }
//            else
//            {
//                EventManager.Broadcast((int)GameEvent.ConnectLoginServerResult, false);
//                Debug.Log("cant connect the login server");
//            }
//        });
//    }

//    public void Close()
//    {
//        client?.ChangeToCloseState();
//    }

//    public void Update(float timeDelta)
//    {
//        client?.Update(timeDelta);
//    }

//    public void ConnectGateServer(string ip, int port)
//    {
//        client.CloseConnect();

//        client = new ClientConnect();
//        client.connectAction += ConnectGateServerResult;
//        client.Connect(ip, port);
//        Console.WriteLine("connecting gate server ...");

//    }

//    public void ConnectGateServerResult(bool isSuccess)
//    {
//        Loom.QueueOnMainThread(() =>
//        {
//            if (isSuccess)
//            {
//                EventManager.Broadcast((int)GameEvent.ConnectGateServerResult, true);
//                Debug.Log("success to connect the gate server");


//                //LoginController.Instance.AskEnterGameServer();

//            }
//            else
//            {
//                EventManager.Broadcast((int)GameEvent.ConnectGateServerResult, false);
//                Debug.Log("cant connect the gate server");
//            }
//        });
//    }

//    ///////////////////////


//    public void SendMsgToLS(int msg, byte[] data)//, Callback<byte[]> callback
//    {
//        client.Send(msg, data);
//    }

//    public void SendMsgToCS(int msg, byte[] data)//, Callback<byte[]> callback
//    {
//        client.Send(msg, data);
//    }

//    //这里和 CS 一样 网关服务器(GS)会根据消息转发到 SS
//    public void SendMsgToSS(int msg, byte[] data)//, Callback<byte[]> callback
//    {
//        client.Send(msg, data);
//    }



//    //需要直接回调用这个 但是注意调用顺序
//    //public void SendMsgToLS(int msg, byte[] data, Callback<byte[]> callback)
//    //{
//    //    Callback<byte[]> action = null;
//    //    action = (bytes) =>
//    //    {
//    //        callback?.Invoke(bytes);
//    //        EventManager.RemoveListener(msg, action);
//    //    };
//    //    EventManager.AddListener(msg, action);

//    //    client.Send(msg, data);
//    //}

//    //public void SendMsgToCS(int msg,byte[] data, Callback<byte[]> callback)
//    //{
//    //    Callback<byte[]> action = null;
//    //    action = (bytes) =>
//    //    {
//    //        callback?.Invoke(bytes);
//    //        EventManager.RemoveListener(msg, action);
//    //    };
//    //    EventManager.AddListener(msg, action);

//    //    client.Send(msg, data);
//    //}


//    public void SendMsgToCombatServer()
//    {
//        //同 center
//    }

//    public void SendMsgTo(int serverType,string msg)
//    {

//    }
//}
