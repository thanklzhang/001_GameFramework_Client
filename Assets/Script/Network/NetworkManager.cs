using NetProto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public enum ServerType
{
    Login = 1,
    Gate = 2,
}

public class NetworkManager : Singleton<NetworkManager>
{
    TcpNetClient tcpNetClient;
    Action<bool> connectCallback;
    public void Init()
    {
        tcpNetClient = new TcpNetClient();
    }

    public void ConnectToLoginServer(string ip,int port,Action<bool> connectCallback)
    {
        this.connectCallback = connectCallback;
        //tcpNetClient = new TcpNetClient();
        //var ip = "192.168.3.13";
        //var ip = "120.245.26.19";
        tcpNetClient.connectAction += this.OnConnectToLoginServerFinish;
        tcpNetClient.Connect(ip, port);
        tcpNetClient.ReceiveMsgAction += this.OnReceveMsg;
    }



    public void OnConnectToLoginServerFinish(bool isSuccess)
    {
        //注意这里 要保证 loom 在 gameScene 中有实例 因为 loom 中的自动实例创建是需要在主线程的 这里不是
        Loom.QueueOnMainThread(() =>
        {
            Logx.Log("curr OnConnectToLoginServerFinish : " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            this.connectCallback?.Invoke(isSuccess);

        });
    }

    public void ConnectToGateServer(string ip, int port, Action<bool> connectCallback)
    {
        Logx.Log("connect to gate : " + ip + " " + port);

        tcpNetClient?.Close();

        this.connectCallback = connectCallback;
        tcpNetClient = new TcpNetClient();
        tcpNetClient.connectAction += this.OnConnectToGateServerFinish;

        tcpNetClient.Connect(ip, port);
        tcpNetClient.ReceiveMsgAction += this.OnReceveMsg;
    }

    public void OnConnectToGateServerFinish(bool isSuccess)
    {
        Loom.QueueOnMainThread(() =>
        {
            Logx.Log("net", "NetworkManager : OnConnectToGateServerFinish : " + isSuccess);
            this.connectCallback?.Invoke(isSuccess);
        });
    }

    private void OnReceveMsg(TcpNetClient netClient, MsgPack msg)
    {
        var now = CommonFunction.GetTimeStamp();
        var delayTime = now - lastSendTimeStamp;
        //Logx.Log("receive timeStamp : " + now);
        //Logx.Log("net delay time : " + delayTime);

        if (msg != null)
        {
            Logx.Log("Network Mgr : ReceiveMsg : cmd : " + (ProtoIDs)msg.cmdId);

            NetMsgManager.Instance.OnReceiveMsg(msg);
        }
        else
        {
            Logx.LogError("NetworkManager : OnReceveMsg : the msg is null");
        }
        

    }

    public long lastSendTimeStamp = 0;

    public void SendMsg(ProtoIDs cmd, byte[] data)
    {
        Logx.Log("Network Mgr : SendMsg : cmd : " + cmd + ":" + (int)cmd);
        lastSendTimeStamp = CommonFunction.GetTimeStamp();
        //Logx.Log("send timeStamp : " + lastSendTimeStamp);
        tcpNetClient.Send((int)cmd, data);
        //Logx.Log("Network Mgr : send time stamp : " + lastSendTimeStamp);
    }

    public void Update()
    {
        this.tcpNetClient.Update();
    }

    //public void ConnectToGateServer()
    //{
    //    clientNet = new ClientNet();
    //    clientNet.connectFinishAction += this.OnConnectToLoginServerFinish;
    //    clientNet.Connect("", 0);
    //}

    //public void OnConnect(bool isSuccess)
    //{

    //}




}
