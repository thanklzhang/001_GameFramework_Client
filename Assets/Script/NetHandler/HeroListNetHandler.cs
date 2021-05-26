using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroListNetHandler : NetHandler
{
    public Action heroListAction;
    public override void Init()
    {
        base.Init();
        AddListener(ProtoMsgIds.GC2CS_HeroList, RespServerHeroList);
    }

    public void ReqServerHeroList(Action action = null)
    {
        GC2CS.reqHeroList req = new GC2CS.reqHeroList();
        heroListAction = action;
        //send message
        this.SendMsgToCS(ProtoMsgIds.GC2CS_HeroList, req);
    }

    public void RespServerHeroList(byte[] data)
    {
        //GC2CS.respHeroList resp = GC2CS.respHeroList.Parser.ParseFrom(data);
        //var list = resp.HeroList.ToList();
        //DataManager.Instance.heroData.RefreshHeroList(list);

        ////callback 在最后置空 防止回调行为改变 callback
        //var action = heroListAction;
        //heroListAction = null;
        //action?.Invoke();
        ////heroListAction?.Invoke();
        ////heroListAction = null;
    }
    
}

