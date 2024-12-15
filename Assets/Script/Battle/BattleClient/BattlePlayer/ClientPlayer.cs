using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    public partial class ClientPlayer
    {
        public int playerIndex;
        public int team;
        public int uid;
        public int ctrlHeroGuid;

        public void Init()
        {
            InitCurrency();
            InitBoxShop();
            InitMyBox();
            InitBattleReward();
        }

        public void OnOperateHero(OnHeroOperationMsgArg arg)
        {
            Vector3 pos = arg.targetPos; //
            var targetGuid = arg.targetGuid; //
            bool isUnderstudy = arg.isUnderstudy;
            int toUnderstudyIndex = arg.toUnderstudyIndex;

            var targetEntity = BattleEntityManager.Instance.FindEntity(targetGuid);
            if (targetEntity != null)
            {
                if (isUnderstudy)
                {
                    //设置替补点
                    UnderstudyManager_Client.Instance.SetLocation(targetEntity, toUnderstudyIndex);
                }
                else
                {
                    //去战场
                    var pre = targetEntity.GetPosition();
                    targetEntity.SetPosition(new Vector3(
                        pos.x, pre.y, pos.z));
                }
            }
        }
    }

    public class OnHeroOperationMsgArg
    {
        public Vector3 targetPos;
        public int targetGuid;

        public bool isUnderstudy;
        public int toUnderstudyIndex;
    }
}