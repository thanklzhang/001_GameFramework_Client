using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetHandlerManager : Singleton<NetHandlerManager>
{
    Dictionary<Type, NetHandler> dic = new Dictionary<Type, NetHandler>();
    public void Init()
    {
        AddNetHandler(typeof(LoginNetHandler), new LoginNetHandler());
        AddNetHandler(typeof(BattleNetHandler), new BattleNetHandler());
        AddNetHandler(typeof(BattleEntranceHandler), new BattleEntranceHandler());

    }

    void AddNetHandler(Type type, NetHandler netHandler)
    {
        dic.Add(type, netHandler);
        netHandler.Init();
    }

    public T GetHandler<T>() where T : NetHandler
    {
        var type = typeof(T);
        if (dic.ContainsKey(type))
        {
            var netHandle = dic[type];
            return netHandle as T;
        }
        else
        {
            Debug.LogError("the net is not exist : " + type.ToString());
            return null;
        }
    }

    //add net lister


    // login success -> set data(LoginData) -> sendEvent
}

