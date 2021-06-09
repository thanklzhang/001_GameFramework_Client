using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITcpClient
{
    // Socket socket { get; set; }

    void Connect(string ip, int port);

    void Send(int msgId, byte[] data);

    void StartReceive();

    void Close();

}

