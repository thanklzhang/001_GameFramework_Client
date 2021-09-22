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
        Logx.Log("net", "NetworkManager : OnConnectToLoginServerFinish : " + isSuccess);
        this.connectCallback?.Invoke(isSuccess);

      

    }

    public void ConnectToGateServer(string ip, int port, Action<bool> connectCallback)
    {
        tcpNetClient?.Close();

        this.connectCallback = connectCallback;
        tcpNetClient = new TcpNetClient();
        tcpNetClient.connectAction += this.OnConnectToGateServerFinish;
        Logx.Log("connect to gate : " + ip + " " + port);
        tcpNetClient.Connect(ip, port);
        tcpNetClient.ReceiveMsgAction += this.OnReceveMsg;
    }

    public void OnConnectToGateServerFinish(bool isSuccess)
    {
        Logx.Log("net", "NetworkManager : OnConnectToGateServerFinish : " + isSuccess);
        this.connectCallback?.Invoke(isSuccess);
        
    }


    private void OnReceveMsg(TcpNetClient netClient, MsgPack msg)
    {
        NetMsgManager.Instance.OnReceiveMsg(msg);

    }

    public void SendMsg(ProtoIDs cmd, byte[] data)
    {
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
