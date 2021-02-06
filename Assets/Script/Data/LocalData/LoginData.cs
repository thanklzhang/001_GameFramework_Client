using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoginData : Singleton<LoginData>
{
  
    public string GateServerIp;
    public int GateServerPort;
   

    public void SetData(GC2LS.respAskLogin serverData)
    {
        this.GateServerIp = serverData.GateServerIp;
        this.GateServerPort = serverData.GateServerPort;
      
    }

    public void SetHeroListInfo()
    {

    }

}

