using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerNetHandler : NetHandler
{
    public override void Init()
    {
        base.Init();

        AddListener(ProtoMsgIds.GC2LS_AskLogin, SyncPlayerAttrInfo);
        AddListener(ProtoMsgIds.GC2LS_AskLogin, SyncHeroListInfo);

    }

    public void AskLogin()
    {
        //send message
    }

    public void SyncPlayerAttrInfo(byte[] data)
    {
        //PlayerData.Instance.SetPlayerAttrInfo();

    }

    public void SyncHeroListInfo(byte[] data)
    {
        //PlayerData.Instance.SetHeroListInfo();

    }
}

