﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class MsgPack
{
    public int msgId;
    //public int clientId;//转发消息的时候需要
    public byte[] data;
    public static MsgPack Create(int msgId, byte[] data)
    {
        var p = new MsgPack();
        p.msgId = msgId;
        //p.clientId = clientId;
        p.data = data;
        return p;
    }

}

/// <summary>
/// 网络状态
/// </summary>
public enum NetState
{
    Null,
    Connect,
    Close,
}

public class TcpNetClient
{
    public Socket netSocket;

    //接收消息缓冲
    private byte[] buffer = null;
    //消息数据缓冲区 目前用于解决粘包问题
    public byte[] dataBuffer;

    public NetState netState;

    int MSG_HEAD_LEN = 18;//len 4 , msgId 2 , timeStamp 4 , uid 8
    int MAX_MSG_SIZE = 1024;

    //接受消息封装包的缓存队列
    public List<MsgPack> receiveMsgQueue;

    //event
    public Action<bool> connectAction;
    public Action<int> closeAction;
    public Action<TcpNetClient, MsgPack> ReceiveMsgAction;

    public TcpNetClient()
    {

        netSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Init();
    }
    public TcpNetClient(Socket s)
    {

        this.netSocket = s;
        Init();
    }

    void Init()
    {
        buffer = new byte[MAX_MSG_SIZE];
        receiveMsgQueue = new List<MsgPack>();
    }
    public void Connect(string ip, int port)
    {
        try
        {
            Logx.Log("net", "start connect ...");
            IPAddress mIp = IPAddress.Parse(ip);
            IPEndPoint ip_end_point = new IPEndPoint(mIp, port);
            netSocket.BeginConnect(ip_end_point, OnConnectCallback, netSocket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //ChangeToCloseState();
        }

    }

    void OnConnectCallback(IAsyncResult ar)
    {
        var s = (Socket)ar.AsyncState;
        Logx.Log("net", "OnConnectCallback : on connect : " + s.Connected);
        if (s.Connected)
        {
            netState = NetState.Connect;
            s.EndConnect(ar);
           
            s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), s);
        }
        else
        {
            Logx.LogWarning("OnConnectCallback : the socket connect fail");
        }
        //heartBeatService.Start();
        bool isSuccessConnect = netSocket.Connected;
        connectAction?.Invoke(isSuccessConnect);
    }

    void OnReceiveCallback(IAsyncResult ar)
    {
        var socket = (Socket)ar.AsyncState;
        //Debug.Log(socket.Connected);
        var length = socket.EndReceive(ar);
        if (length == 0)
        {
            Console.WriteLine(socket.RemoteEndPoint + " : disconnect");
            return;
        }

        //dataBuffer 加上这段数据
        if (dataBuffer == null)
        {
            dataBuffer = new byte[length];
            Array.Copy(buffer, dataBuffer, length);
        }
        else
        {
            byte[] finalB = new byte[length];
            Array.Copy(buffer, 0, finalB, 0, length);
            dataBuffer = dataBuffer.Concat(finalB).ToArray();
        }

        while (dataBuffer.Length >= MSG_HEAD_LEN)
        {
            //第一位是 len 读出来
            int bodyLength = BitConverter.ToInt32(dataBuffer, 0);

            if (bodyLength + MSG_HEAD_LEN <= dataBuffer.Length)
            {
                //达到了消息整包大小
                byte[] currData = new byte[bodyLength + MSG_HEAD_LEN];

                Array.Copy(dataBuffer, 0, currData, 0, bodyLength + MSG_HEAD_LEN);

                //解析消息
                var msgPack = ParseFromMsg(currData);
                //加入队列
                AddMsgToReceiveQueue(msgPack);

                byte[] nextData = new byte[dataBuffer.Length - MSG_HEAD_LEN - bodyLength];
                Array.Copy(dataBuffer, MSG_HEAD_LEN + bodyLength, nextData, 0, dataBuffer.Length - MSG_HEAD_LEN - bodyLength);
                dataBuffer = nextData;
            }
            else
            {
                //没到达一个数据包 所以接着接收 直到到了一个数据包的长度
                break;
            }
        }

        if (socket.Connected)
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceiveCallback), null);

        }
    }

    public void Send(MsgPack msg)
    {
        Send(msg.msgId, msg.data);
    }

    public void Send(int msgId, byte[] data)
    {
        try
        {
            byte[] resultData = BuildData(msgId, data);
            netSocket.BeginSend(resultData, 0, resultData.Length, SocketFlags.None, new AsyncCallback(OnSendMsgCallback), netSocket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //ChangeToCloseState();
        }

    }

    private void OnSendMsgCallback(IAsyncResult ar)
    {
        var socket = (ar.AsyncState as Socket);
        socket.EndSend(ar);

        //Console.WriteLine("finish!");

    }


    /// <summary>
    /// 组装消息数据
    /// </summary>
    /// <param name="msg"></param>
    public virtual byte[] BuildData(int msgId, byte[] dataContent)
    {
        byte[] data = null;
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(dataContent.Length);
            writer.Write(msgId);
            writer.Write(dataContent);
            //TODO:
            //writer.Write(timeStemp);//时间戳
            //writer.Write(clientId);//用户唯一 id  登录之后即可拥有

            writer.Flush();
            data = ms.ToArray();
            writer.Close();
        }

        return data;

    }


    /// <summary>
    /// 解析消息
    /// </summary>
    /// <param name="msg"></param>
    public MsgPack ParseFromMsg(byte[] msg)
    {
        MemoryStream ms = null;
        using (ms = new MemoryStream(msg))
        {
            BinaryReader reader = new BinaryReader(ms);
            int len = reader.ReadInt32();
            int msgId = reader.ReadInt32();
            //int clientId = reader.ReadInt32();
            byte[] data = reader.ReadBytes(len);
            var msgPack = MsgPack.Create(msgId, data);
            reader.Close();

            return msgPack;
        }

    }

    public void AddMsgToReceiveQueue(MsgPack pack)
    {
        receiveMsgQueue.Add(pack);
    }



    public void Update()
    {
        this.UpdateReceiveQueue();
    }

    public void UpdateReceiveQueue()
    {
        for (int i = 0; i < receiveMsgQueue.Count; i++)
        {
            var msgPack = receiveMsgQueue[i];
            this.ReceiveMsgAction?.Invoke(this, msgPack);
        }
    }


    public bool IsConnect()
    {
        return netState == NetState.Connect;
    }

    public void Close()
    {
        //closeAction?.Invoke(this.clientId);
        //netSocket?.Close();
        ////Debug.Log("close ..");
        //buffer = null;
        //dataBuffer = null;
        //connectAction = null;
        ////if (heartBeatService != null)
        ////{
        ////    heartBeatService.Stop();
        ////}
    }

}

