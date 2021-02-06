using Assets.Script.Combat;
using FixedPointy;
using GameModelData;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    //真正的战斗开始
    public void StartCombat()
    {
        GameStateManager.Instance.ChangeState(GameState.Combat);
    }
}


//public class CombatStart : MonoBehaviour
//{

//    // Use this for initialization
//    void Awake()
//    {
//        InitCombatScene();
//    }

//    bool isInited;

//    public void InitCombatScene()
//    {
//        //int ss = (int)(Fix)7;
//        //Fix min = (Fix)7;
//        //Debug.Log(min.Raw + " " + min + " " + ss);

//        //min = Fix.Mix(2, 4, 1);
//        //Debug.Log(min.Raw + " " + min);

//        //min = new Fix(2) / new Fix(1);
//        //Debug.Log(min.Raw + " " + min);
//        //Fix timeDelta = new Fix(Const.frameTime) / new Fix(1);
//        ////Fix min = new Fix(13);
//        ////Debug.Log("test : " + min.Raw + " " + min);
//        //Debug.Log(timeDelta.Raw + " " + timeDelta);

//        LoadResource();

//        //UIManager.Instance.GoToCombat(SceneType.Main);
//        EventManager.Broadcast(GameEvent.EnterCombat);

//        //

//        UDPClient udp = new UDPClient();
//        udp.SetReceiveEndPoint(GlobaData.Instance.localUdpPort);
//        udp.ReceiveAction += ReceiveMsg;

//        CombatNet.Instance.Init(udp);
//        CombatNet.Instance.SetServerEndPoint(Const.combatServerIP, Const.combatServerPort);

//        //逻辑实体管理(逻辑层)
//        CombatLogicEntityManager.Instance.Init();

//        //表现实体管理(表现层)
//        CombatViewEntityManager.Instance.Init();

//        CombatManager.Instance.Init(GlobaData.Instance.combatInitInfo);

//        udp.StartReceive(Const.combatServerPort);



//        isInited = true;

//    }

//    void LoadResource()
//    {
//        //GameResource.Instance.LoadCombatSceneResource();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isInited)
//        {
//            CombatViewEntityManager.Instance.Update(Time.deltaTime);
//        }

//    }
//    public void ReceiveMsg(byte[] msg)
//    {

//        Loom.QueueOnMainThread(() =>
//        {

//            MemoryStream ms = null;
//            using (ms = new MemoryStream(msg))
//            {
//                BinaryReader reader = new BinaryReader(ms);
//                int msgId = reader.ReadInt32();
//                int len = msg.Length - 4;

//                byte[] data = reader.ReadBytes(len);
//                var msgPack = MsgPack.Create(msgId, data);
//                CombatNet.Instance.HandleMsg(msgPack);
//                reader.Close();
//            }


//        });
//    }

//    public void ExitCombat()
//    {
//        //GameResource.Instance.UnloadCombatResource();
//    }
//}
