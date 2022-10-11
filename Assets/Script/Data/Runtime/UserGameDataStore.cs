using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GameData
{
    public class UserGameDataStore : GameDataStore
    {
        private ulong uid;

        public ulong Uid { get => uid; set => uid = value; }
    }


}