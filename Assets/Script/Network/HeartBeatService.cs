using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class HeartBeatService
{
    int interval;
    public int timeout;
    Timer timer;
    DateTime lastTime;

    // Protocol protocol;
    TcpClient client;
    public HeartBeatService(int interval, TcpClient client)
    {
        this.interval = interval;
        this.client = client;
    }

    public void ResetTimeout()
    {
        this.timeout = 0;
        lastTime = DateTime.Now;
        Debug.Log("ResetTimeout");
    }

    public void SendHeartBeat(object source, ElapsedEventArgs e)
    {
        TimeSpan span = DateTime.Now - lastTime;
        timeout = (int)span.TotalMilliseconds;

        //check timeout
        if (timeout > interval * 2)
        {
            //protocol.getPomeloClient().disconnect();
            //protocol.close();

            client.ChangeToCloseState();
            //stop();
            return;
        }

        //Send heart beat
        //protocol.send(PackageType.PKG_HEARTBEAT);
        client.Send(Const.sendHeartBeatMsgId, new byte[] { });
        //Debug.Log("send heartBeat");
    }

    public void Start()
    {
        //    if (interval < 1000) return;
        Debug.Log("the heartBeat is start");
        //start hearbeat
        this.timer = new Timer();
        timer.Interval = interval;
        timer.Elapsed += new ElapsedEventHandler(SendHeartBeat);
        timer.Enabled = true;

        //Set timeout
        timeout = 0;
        lastTime = DateTime.Now;
    }

    public void Stop()
    {
        if (this.timer != null)
        {
            this.timer.Enabled = false;
            this.timer.Dispose();
        }
    }
}

