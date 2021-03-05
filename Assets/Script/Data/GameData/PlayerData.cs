using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UserData : Singleton<UserData>
{
    //public string account;
    //public string nickName;
    //public int level;
    //public int coin;
    //public string portraitURL;
    //public string UserId;
    public string userAccount;
    public string token;

    internal void SetAccount(string userAccount, string token)
    {
        this.userAccount = userAccount;
        this.token = token;
    }

    //public void SetPlayerAttrInfo()
    //{
    //    //send event ...

    //    //set data
    //    //需要公共显示的用事件分发
    //    EventManager.Broadcast((int)GameEvent.SyncPlayerBaseInfo,1);
    //}

    //public void SetHeroListInfo()
    //{

    //}

    //public List<Hero> ownHeroes = new List<Hero>();
    //public List<Item> ownItems = new List<Item>();
    //public List<Drawing> ownDrawings = new List<Drawing>();
    //public void SetUserBaseData(GS2GC.PlayerBaseInfo info)
    //{
    //    account = info.Account;
    //    nickName = info.NickName;
    //    level = info.Level;
    //    coin = info.Coin;
    //    portraitURL = info.PortraitURL;
    //    //ownHeroes
    //    //ownItems
    //    //ownDrawings
    //    EventManager.Broadcast(GameEvent.SyncPlayerBaseInfo);

    //}

    //public void SetHeroes(GS2GC.HeroListInfo info)
    //{
    //    ownHeroes.Clear();
    //    foreach (var hero in info.Heroes)
    //    {
    //        ownHeroes.Add(Hero.Create(hero));
    //    }
    //}

    //public void SetItems(GS2GC.ItemListInfo info)
    //{
    //    Debug.Log(info.Items.Count);
    //    ownItems.Clear();
    //    foreach (var item in info.Items)
    //    {
    //        Debug.Log(item.Quality);
    //        ownItems.Add(Item.Create(item));
    //    }
    //}

    //public void SetDrawings(GS2GC.DrawingListInfo info)
    //{
    //    ownDrawings.Clear();
    //    foreach (var item in info.Drawings)
    //    {
    //        ownDrawings.Add(Drawing.Create(item));
    //    }
    //}
}

