using Google.Protobuf.Collections;
using NetProto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Battle;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public class Tips
    {
        public static void ShowSkillTipText(string str)
        {
            EventDispatcher.Broadcast(EventIDs.OnSkillTips,str);
        }
    }
}