using System.Collections.Generic;
using UnityEngine;

namespace Battle_Client
{
    //布阵相关
    public partial class ClientPlayer
    {
        public void OnOperateHero(OperateHeroByArraying_RecvMsg_Arg arg)
        {
          
            Vector3 pos = arg.targetPos; //
            var opHeroGuid = arg.opHeroGuid; //
            int toUnderstudyIndex = arg.toUnderstudyIndex;

            Logx.Log(LogxType.Zxy,$"client : recv msg : pos:{pos} opHeroGuid:{opHeroGuid} toUnderstudyIndex:{toUnderstudyIndex}");
            
            if (opHeroGuid > 0)
            {
                var targetEntity = BattleEntityManager.Instance.FindEntity(opHeroGuid);
                if (targetEntity != null)
                {
                    if (toUnderstudyIndex >= 0)
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
            else
            {
                if (toUnderstudyIndex >= 0)
                {
                    UnderstudyManager_Client.Instance.SetLocation(null, toUnderstudyIndex);
                }
            }

           
        }
    }
}