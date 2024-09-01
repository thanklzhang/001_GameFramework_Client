﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
using Battle_Client;
using GameData;
using NetProto;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Client
{
    public class SkillEffectStartMove_RecvMsg : ClientRecvMsg
    {
        public override void Handle()
        {
            var arg = this.msgArg as SkillEffectStartMove_RecvMsg_Arg;

            var guid = arg.EffectGuid;
            var skillEffect = BattleSkillEffect_Client_Manager.Instance.FindSkillEffect(guid);
            if (skillEffect != null)
            {
                skillEffect.StartMove(arg.TargetPos, arg.TargetGuid, arg.moveSpeed);
            }
        }
    }

    public class SkillEffectStartMove_RecvMsg_Arg : BaseClientRecvMsgArg
    {
        public int EffectGuid;
        public UnityEngine.Vector3 TargetPos;
        public int TargetGuid;
        public float moveSpeed;
    }
}