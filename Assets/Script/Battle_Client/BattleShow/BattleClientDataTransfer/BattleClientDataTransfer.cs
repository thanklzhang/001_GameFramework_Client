﻿using Google.Protobuf.Collections;
using NetProto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Battle_Client
{
    //这些都是战斗相关的输出输入的统一收敛数据

    //客户端创建战斗所需参数战斗
    public class BattleClient_CreateBattleArgs
    {
        public int guid;
        public int configId;
        public int roomId;
        public List<BattleClient_ClientPlayer> clientPlayers;
        public List<BattleClientMsg_Entity> entityList;
        public int mapSizeX;
        public int mapSizeZ;
    }

    public class BattleClient_ClientPlayer
    {
        public int playerIndex;
        public int team;
        public long uid;
        public int ctrlHeroGuid;
    }


    public class BattleClientMsg_InitArg
    {
        public int battleGuid;
        public int battleTableId;
        public int battleRoomId;

        public Dictionary<int, BattleClientMsg_ClientPlayer> players;

    }

    public class BattleClientMsg_ClientPlayer
    {
        public int playerIndex;
        public int team;
        public int uid;
        public int ctrlHeroGuid;
    }

    public class BattleClientMsg_Entity
    {
        public int guid;
        public int configId;
        public int playerIndex;
        public int level;
        public UnityEngine.Vector3 position;

        public List<BattleClientMsg_Skill> skills;

    }

    public class BattleClientMsg_Skill
    {
        public int configId;
        public int level;
    }

    public class BattleClientMsg_BattleAttr
    {
        public EntityAttrType type;
        public int value;
    }

    public class BattleClientMsg_BattleValue
    {
        public EntityCurrValueType type;
        public int value;
    }


}