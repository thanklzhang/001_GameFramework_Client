//using Assets.Script.Combat;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using UnityEngine;

//public class CombatNet : Singleton<CombatNet>
//{
//    UDPClient udpClient;
//    EndPoint serverEndPoint;
//    public void HandleMsg(MsgPack pack)
//    {
//        var msgId = pack.msgId;
//        var data = pack.data;

//        switch (msgId)
//        {
//            case (int)SS2GC.MsgId.Ss2GcFrameAction:
//                FrameAction(data);

//                break;
//        }
//    }

//    public void SetServerEndPoint(string ip,int port)
//    {
//        serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
//    }

//    public void Init(UDPClient udp)
//    {
//        this.udpClient = udp;
//    }

//    /// <summary>
//    /// 收到帧的事件
//    /// </summary>
//    void FrameAction(byte[] data)
//    {
//        SS2GC.FrameAction frameAction = SS2GC.FrameAction.Parser.ParseFrom(data);

//        //Debug.Log("receive currFrame : " + ra.CurrFrame.Id);

//        CombatManager.Instance.HandleFrames(frameAction);

//    }

//    public void SendTo(int msgHeader, byte[] dataContent)
//    {
//        var data = BuildData(msgHeader, dataContent);
//        this.udpClient.Send(serverEndPoint, data);
//    }

//    public virtual byte[] BuildData(int msgHeader, byte[] dataContent)// int userId,
//    {
//        byte[] data = null;
//        MemoryStream ms = null;
//        using (ms = new MemoryStream())
//        {
//            BinaryWriter writer = new BinaryWriter(ms);
//            writer.Write(msgHeader);
//            writer.Write(dataContent);
//            writer.Flush();

//            data = ms.ToArray();
//            writer.Close();
//        }

//        return data;

//    }


//}
