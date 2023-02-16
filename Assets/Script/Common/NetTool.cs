using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class NetTool
{
    public static string GetHostIp()
    {
        var hostName = Dns.GetHostName();
        var hostEntry = Dns.GetHostEntry(hostName);
        var addrList = hostEntry.AddressList;
        var localIp = "";
        foreach (var item in addrList)
        {
            if (item.AddressFamily == AddressFamily.InterNetwork)
            {
                localIp = item.ToString().Trim();
                break;
            }
        }

        return localIp;
    }

}

