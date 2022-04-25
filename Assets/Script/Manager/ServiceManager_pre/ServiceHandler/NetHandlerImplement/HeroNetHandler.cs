using System;
using System.Collections;
using System.Collections.Generic;

public class HeroNetHandler : HeroServiceHandler
{
    public override void Init()
    {
        //注册 net 事件
        //add upgrade level (ProtoCmdId) -> OnUpgradeHeroLevel
        //EventDispatcher.AddListener<HeroData>(ProtoIDs.UpgradeHeroLevel, OnUpgradeHeroLevel);
    }

    public override void SendUpgradeHeroLevel(int heroId)
    {
        //int msgId = (int)ProtoIDs.UpgradeHeroLevel;
        //byte[] bb = new byte[] { };
        //NetworkManager.Instance.SendMsg(msgId, bb);

        ////simulate
        //var preData = GameDataManager.Instance.HeroGameDataStore.GetDataById(heroId);
        //HeroData heroData = new HeroData();
        //heroData.id = heroId;
        //heroData.level = preData.level + 1;
        //this.OnUpgradeHeroLevel(heroData);
    }
}
