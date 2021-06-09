using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDPClient
{

    Socket sendSocket;
    Socket receiveSocket;
    //发送消息的目标 endPoint
    const int bufferSize = 8 * 1024;
    byte[] buffer = new byte[bufferSize];

    EndPoint myEndPoint;
    //EndPoint otherEndPoint;

    public Action<byte[]> ReceiveAction;

    public UDPClient()
    {
        receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    }
    //public void SetSendEndPoint(string targetIp, int targetPort)
    //{
    //    sendMsgTarget = new IPEndPoint(IPAddress.Parse(targetIp), targetPort);
    //}

    public void SetReceiveEndPoint(int port)
    {
        myEndPoint = new IPEndPoint(IPAddress.Any, port);
    }


    public void StartReceive( int port)
    {
        receiveSocket.Bind(myEndPoint);
        //EndPoint otherEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        EndPoint otherEndPoint = new IPEndPoint(IPAddress.Any, port);

        Debug.Log("start receive");
        receiveSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref otherEndPoint, Receive, otherEndPoint);
        Debug.Log("remote : " +  otherEndPoint.ToString());
    }

    //public void StartReceive()
    //{
    //    receiveSocket.Bind(myEndPoint);
    //    Console.WriteLine("start receive");
    //    receiveSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref otherEndPoint, Receive, null);

    //}

    void Receive(IAsyncResult e)
    {
        var ep = (EndPoint)e.AsyncState;

        var length = receiveSocket.EndReceiveFrom(e, ref ep);
        //Console.WriteLine("received from : " + otherEndPoint.ToString());
        byte[] data = new byte[length];
        Array.Copy(buffer, data, length);
        ReceiveAction?.Invoke(data);
        //Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, length));

        receiveSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ep, Receive, ep);
    }

    public void Send(EndPoint sendMsgTarget, byte[] data)
    {
        sendSocket.SendTo(data, data.Length, SocketFlags.None, sendMsgTarget);//将数据发送到指定的终结点
    }

    public void Close()
    {
        if (sendSocket != null)
        {
            sendSocket.Close();
        }

        if (receiveSocket != null)
        {
            receiveSocket.Close();
        }

        myEndPoint = null;
        //otherEndPoint = null;
        ReceiveAction = null;
    }
}
