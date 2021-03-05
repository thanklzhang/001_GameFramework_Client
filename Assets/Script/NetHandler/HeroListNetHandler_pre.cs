//using Google.Protobuf;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class HeroListNetHandler : NetHandler
//{
//    public Action heroListAction;
//    public Action addHeroLevelAction;
//    public override void Init()
//    {
//        base.Init();
        
//        AddListener(ProtoMsgIds.GC2CS_HeroList, RespServerHeroList);
//        AddListener(ProtoMsgIds.GC2CS_AddHeroLevel, RespAddHeroLevel);
//        AddListener(ProtoMsgIds.GC2CS_NotifyUpdateHeroes, RespNotifyUpdateHeroes);

//    }

//    public void ReqServerHeroList(Action action = null)
//    {
//        GC2CS.reqHeroList req = new GC2CS.reqHeroList();
//        heroListAction = action;
//        //send message
//        this.SendMsgToCS(ProtoMsgIds.GC2CS_HeroList, req);
//    }

//    public void RespServerHeroList(byte[] data)
//    {
//        GC2CS.respHeroList resp = GC2CS.respHeroList.Parser.ParseFrom(data);
//        var list = resp.HeroList.ToList();
//        //set login data
//        HeroData.Instance.UpdateHeroList(list);

//        heroListAction?.Invoke();
//        heroListAction = null;

//    }

//    public void ReqAddHeroLevel(int heroId , Action action = null)
//    {
//        GC2CS.reqAddHeroLevel req = new GC2CS.reqAddHeroLevel();
//        req.HeroId = heroId;

//        addHeroLevelAction = action;
//        //send message
//        this.SendMsgToCS(ProtoMsgIds.GC2CS_AddHeroLevel, req);
//    }

//    private void RespAddHeroLevel(byte[] data)
//    {
//        GC2CS.respAddHeroLevel resp = GC2CS.respAddHeroLevel.Parser.ParseFrom(data);
//        if (resp.Err == ResultCode.Success)
//        {
//            addHeroLevelAction?.Invoke();
//        }
//        else
//        {
//            Debug.Log("error : " + resp.Err);
//        }
       
//    }

//    private void RespNotifyUpdateHeroes(byte[] data)
//    {
//        GC2CS.respNotifyUpdateHeroes resp = GC2CS.respNotifyUpdateHeroes.Parser.ParseFrom(data);
//        HeroData.Instance.UpdateHeroList(resp.HeroInfoList.ToList());

//    }


//}

