//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;
//using UnityEngine;

//public class MsgPack
//{
//    public int msgId;
//    //public int clientId;//转发消息的时候需要
//    public byte[] data;
//    public static MsgPack Create(int msgId, byte[] data)
//    {
//        var p = new MsgPack();
//        p.msgId = msgId;
//        //p.clientId = clientId;
//        p.data = data;
//        return p;
//    }

//}

//public enum NetState
//{
//    Null,
//    Connect,
//    Close,
//}

//public class TcpClient : ITcpClient
//{
//    //protected Socket socket;
//    //ITcpServer server;//作为服务端中客户端的连接类时使用
//    public int clientId;
//    public Socket socket;

//    private byte[] buffer = new byte[1024];
//    public byte[] dataBuffer;
//    public NetState netState;

//    int fixedHead = 8;//4 4 X (消息 token(目前先用 userId) 长度 包体)
//    int fixedHeadWithoutLen = 4;

//    public DateTime preHeartBeatTime;//作为服务端连接的时候 当作接收客户端请求的 socket 的上一次心跳时间

//    //ublic HeartBeatService heartBeatService;

//    //event
//    public Action<bool> connectAction;
//    public Action<int> closeAction;

//    public TcpClient()
//    {

//        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        Init();
//    }
//    public TcpClient(Socket s)
//    {

//        this.socket = s;
//        Init();
//    }

//    void Init()
//    {
//        //作为服务端时候接受 client 的心跳
//        //ResetHeartBeat();

//        //客户端心跳服务
//        //heartBeatService = new HeartBeatService(Const.heartBeatInterval, this);
//    }
//    public void Connect(string ip, int port)
//    {
//        try
//        {
//            IPAddress mIp = IPAddress.Parse(ip);
//            IPEndPoint ip_end_point = new IPEndPoint(mIp, port);
//            socket.BeginConnect(ip_end_point, (e) =>
//            {
//                if (socket.Connected)
//                {
//                    netState = NetState.Connect;
//                    StartReceive();
//                }
//                //heartBeatService.Start();
//                connectAction?.Invoke(socket.Connected);
//            }, null);
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            ChangeToCloseState();
//        }


//    }

//    public virtual void HandleMsg(MsgPack pack)
//    {

//        //作为客户端
//        //if ((ushort)NetCommon.MsgId.HeartBeatHandshake == pack.msgId)
//        //{
//        //    heartBeatService.Start();
//        //}

//        //if (Const.receiveHeartBeatMsgId == pack.msgId)
//        //{
//        //    heartBeatService.ResetTimeout();
//        //}

//        //作为服务端的 client 连接
//        //if ((ushort)NetCommon.MsgId.HeartBeatSend == pack.msgId)
//        //{
//        //    ResetHeartBeat();
//        //}


//    }

//    public Action<TcpClient, MsgPack> ReceiveMsgAction;

//    public void Send(MsgPack msg)
//    {
//        Send(msg.msgId, msg.data);
//    }

//    //public void Send(int msgId, byte[] data)
//    //{
//    //    Send(msgId,  data);
//    //}

//    public void Send(int msgId, byte[] data)
//    {
//        try
//        {
//            byte[] resultData = BuildData(msgId, data);
//            socket.BeginSend(resultData, 0, resultData.Length, SocketFlags.None, SendMsg, null);
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            ChangeToCloseState();
//        }

//    }

//    private void SendMsg(IAsyncResult ar)
//    {
//        //var clientSocket = (ar.AsyncState as Socket);
//        socket.EndSend(ar);

//        //Console.WriteLine("finish!");

//    }
//    /// <summary>
//    /// 默认 4 4 x
//    /// </summary>
//    /// <param name="msg"></param>
//    public virtual byte[] BuildData(int msgId, byte[] dataContent)
//    {
//        byte[] data = null;
//        MemoryStream ms = null;
//        using (ms = new MemoryStream())
//        {
//            BinaryWriter writer = new BinaryWriter(ms);
//            writer.Write(msgId);
//            //writer.Write(clientId);
//            writer.Write(dataContent.Length);
//            writer.Write(dataContent);
//            writer.Flush();

//            data = ms.ToArray();
//            writer.Close();
//        }

//        return data;

//    }

//    public void StartReceive()
//    {
//        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), null);

//    }

//    void Receive(IAsyncResult ar)
//    {
//        if (this.netState == NetState.Close)
//        {
//            return;
//        }

//        if (!socket.Connected)
//        {
//            return;
//        }

//        try
//        {
//            //Debug.Log(socket.Connected);
//            var length = socket.EndReceive(ar);
//            if (length == 0)
//            {
//                Console.WriteLine(socket.RemoteEndPoint + " : disconnect");
//                return;
//            }

//            //dataBuffer 加上这段数据
//            if (dataBuffer == null)
//            {
//                dataBuffer = new byte[length];
//                Array.Copy(buffer, dataBuffer, length);
//            }
//            else
//            {
//                byte[] finalB = new byte[length];
//                Array.Copy(buffer, 0, finalB, 0, length);
//                dataBuffer = dataBuffer.Concat(finalB).ToArray();
//            }

//            while (dataBuffer.Length >= fixedHead)
//            {
//                int bodyLength = BitConverter.ToInt32(dataBuffer, fixedHeadWithoutLen);

//                if (bodyLength <= dataBuffer.Length - fixedHead)
//                {
//                    byte[] currData = new byte[bodyLength + fixedHead];

//                    Array.Copy(dataBuffer, 0, currData, 0, bodyLength + fixedHead);

//                    //处理消息
//                    ParseFromMsg(currData);
                    
//                    byte[] nextData = new byte[dataBuffer.Length - fixedHead - bodyLength];
//                    Array.Copy(dataBuffer, fixedHead + bodyLength, nextData, 0, dataBuffer.Length - fixedHead - bodyLength);
//                    dataBuffer = nextData;
//                }
//                else
//                {
//                    //接收数据缓冲区满了 没到达一个数据包 所以接着接收 直到到了一个数据包的长度
//                    break;
//                }
//            }

//            if (socket.Connected)
//            {
//                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), null);

//            }

//        }
//        catch (Exception e)
//        {
//            Debug.Log(e);
//            ChangeToCloseState();
//        }
//    }

//    public void ChangeToCloseState()
//    {
//        netState = NetState.Close;
//        Debug.Log("user close : " + this.clientId);
//        Close();
//    }


//    /// <summary>
//    /// 默认 4 4 x 
//    /// </summary>
//    /// <param name="msg"></param>
//    public virtual void ParseFromMsg(byte[] msg)
//    {
//        MemoryStream ms = null;
//        using (ms = new MemoryStream(msg))
//        {
//            BinaryReader reader = new BinaryReader(ms);
//            int msgId = reader.ReadInt32();
//            //int clientId = reader.ReadInt32();
//            int len = reader.ReadInt32();

//            byte[] data = reader.ReadBytes(len);
//            var msgBytes = MsgPack.Create(msgId, data);
//            HandleMsg(msgBytes);
//            ReceiveMsgAction?.Invoke(this, msgBytes);

//            reader.Close();
//        }


//    }

//    public void ResetHeartBeat()
//    {
//        preHeartBeatTime = DateTime.Now;
//    }

//    public bool IsConnect()
//    {
//        return netState == NetState.Connect;
//    }

//    public void Close()
//    {
//        closeAction?.Invoke(this.clientId);
//        socket?.Close();
//        //Debug.Log("close ..");
//        buffer = null;
//        dataBuffer = null;
//        connectAction = null;
//        //if (heartBeatService != null)
//        //{
//        //    heartBeatService.Stop();
//        //}
//    }

//}

