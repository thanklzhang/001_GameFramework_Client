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

    public void ConnectToLoginServer(Action<bool> connectCallback)
    {
        this.connectCallback = connectCallback;
        //tcpNetClient = new TcpNetClient();
        tcpNetClient.connectAction += this.OnConnectToLoginServerFinish;
        tcpNetClient.Connect("127.0.0.1", 5556);
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
        Logx.Log("Network Mgr : ReceiveMsg : cmd : " + (ProtoIDs)msg.cmdId);

        NetMsgManager.Instance.OnReceiveMsg(msg);

    }

    public void SendMsg(ProtoIDs cmd, byte[] data)
    {
        Logx.Log("Network Mgr : SendMsg : cmd : " + cmd);
        tcpNetClient.Send((int)cmd, data);
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
