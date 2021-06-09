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

    public void Init()
    {
        
    }

    public void ConnectToLoginServer()
    {
        tcpNetClient = new TcpNetClient();
        tcpNetClient.connectAction += this.OnConnectToLoginServerFinish;
        tcpNetClient.Connect("127.0.0.1", 5151);
    }

    public void OnConnectToLoginServerFinish(bool isSuccess)
    {
        Logx.Log("net", "NetworkManager : OnConnectToLoginServerFinish : " + isSuccess);
    }

    public void SendMsg()
    {
        //clientNet.SendMsg();
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
