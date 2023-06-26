using System.Collections.Generic;
using System.Linq;
namespace Battle
{
    //init
    public class BattlePlayerInit
    {
        public int playerIndex;
        public long uid;
        public int team;

        public bool isPlayerCtrl;

    }

    public class BattlePlayerInitArg
    {
        public List<BattlePlayerInit> battlePlayerInitList;
    }

    //////////////////////// runtime

    public class BattlePlayer
    {
        public int playerIndex;
        public long uid;
        public int team;

        public int ctrlHeroGuid;

        //temp data
        private int progress;//thousand
        public int Progress { get => progress; set => progress = value; }

        private bool isReadyFinish;
        internal bool IsReadyFinish { get => isReadyFinish; set => isReadyFinish = value; }

        //客户端剧情播放完成
        private bool isPlotEnd;
        public bool IsPlotEnd { get => isPlotEnd; set => isPlotEnd = value; }

        private bool isPlayerCtrl;
        internal bool IsPlayerCtrl { get => isPlayerCtrl; set => isPlayerCtrl = value; }


        internal void SetCtrlHeroGuid(int entityGuid)
        {
            ctrlHeroGuid = entityGuid;
        }
    }

}

