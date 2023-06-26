using System.Collections.Generic;

namespace Battle
{
    public interface IPlayerMsgReceiver
    {
        void On_PlayerLoadProgress(long uid, int progress);
        void On_BattleReadyFinish(long uid);
        void On_ClientPlotEnd(long uid);

        void On_MoveEntity(int entityGuid, Vector3 targetPos);
        void On_UseSkill(int releaserGuid, int skillId, int targetGuid, Vector3 targetPos);
    }
}

