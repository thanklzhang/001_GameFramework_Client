using Battle;
using Battle_Client;
using System.Collections.Generic;

namespace Battle_Client
{
    //战斗逻辑中发送给玩家的发送器
    public class LocalBattleLogic_MsgSender : IPlayerMsgSender
    {
        Battle.Battle battle;
        public void NotifyAllPlayerMsg(int cmd, byte[] bytes)
        {
            throw new System.NotImplementedException();
        }

        public void NotifyAll_AllPlayerLoadFinish()
        {
            BattleManager.Instance.MsgReceiver.On_AllPlayerLoadFinish();
        }

        public void NotifyAll_AllPlayerPlotEnd(string plotName)
        {
            BattleManager.Instance.MsgReceiver.On_PlotEnd();
        }

        public void NotifyAll_BattleStart()
        {
            BattleManager.Instance.MsgReceiver.On_StartBattle();
        }

        public void NotifyAll_CreateEntities(List<Battle.BattleEntity> logicList)
        {
            List<BattleClientMsg_Entity> entityList = new List<BattleClientMsg_Entity>();

            foreach (var item in logicList)
            {
                BattleClientMsg_Entity entityArg = new BattleClientMsg_Entity()
                {
                    guid = item.guid,
                    configId = item.configId,
                    playerIndex = item.playerIndex,
                    level = item.level,
                    position = new UnityEngine.Vector3(item.position.x, item.position.y, item.position.z)
                };

                entityArg.skills = new List<BattleClientMsg_Skill>();
                foreach (var netSkill in item.GetAllSkills())
                {
                    var skill = new BattleClientMsg_Skill()
                    {
                        configId = netSkill.Value.configId,
                        level = netSkill.Value.level
                    };
                    entityArg.skills.Add(skill);

                }

                entityList.Add(entityArg);
            }

            BattleManager.Instance.MsgReceiver.On_CreateEntities(entityList);
        }

        public void NotifyAll_CreateSkillEffect(CreateEffectInfo createInfo)
        {

            var pos = new UnityEngine.Vector3(createInfo.createPos.x, createInfo.createPos.y, createInfo.createPos.z);
            //var lastTimeInt = (int)(lastTime * 1000);
            BattleManager.Instance.MsgReceiver.On_CreateSkillEffect(createInfo);
        }

        public void NotifyAll_EntityAddBuff(int guid, BuffEffect buff)
        {

        }

        public void NotifyAll_EntityDead(Battle.BattleEntity battleEntity)
        {
            BattleManager.Instance.MsgReceiver.On_EntityDead(battleEntity.guid);
        }

        //public void NotifyAll_EntityStartMove(int guid, Vector3 targetPos, Vector3 dir, float finalMoveSpeed)
        //{
        //    var pos = new UnityEngine.Vector3(targetPos.x, targetPos.y, targetPos.z);
        //    var uDir = new UnityEngine.Vector3(dir.x, dir.y, dir.z);
        //    BattleManager.Instance.MsgReceiver.On_EntityStartMove(guid, pos, uDir, finalMoveSpeed);
        //}

        public void NotifyAll_EntityStartMoveByPath(int guid, List<Vector3> pathList, float finalMoveSpeed)
        {
            List<UnityEngine.Vector3> unityPosList = new List<UnityEngine.Vector3>();
            foreach (var pos in pathList)
            {
                var resultPos = new UnityEngine.Vector3(pos.x, pos.y, pos.z);
                unityPosList.Add(resultPos);
            }
            BattleManager.Instance.MsgReceiver.On_EntityStartMoveByPath(guid, unityPosList, finalMoveSpeed);
        }

        public void NotifyAll_EntityStopMove(int guid, Vector3 position)
        {
            var pos = new UnityEngine.Vector3(position.x, position.y, position.z);
            BattleManager.Instance.MsgReceiver.On_EntityStopMove(guid, pos);
        }

        public void NotifyAll_SyncEntityDir(int guid, Vector3 dir)
        {
            var pos = new UnityEngine.Vector3(dir.x, dir.y, dir.z);
            BattleManager.Instance.MsgReceiver.On_EntitySyncDir(guid, pos);
        }

        public void NotifyAll_OnEntityStartSkillEffect(int guid, int skillConfig)
        {
            BattleManager.Instance.MsgReceiver.On_EntityUseSkill(guid, skillConfig);
        }

        public void NotifyAll_PlayPlot(string name)
        {
            BattleManager.Instance.MsgReceiver.On_PlayPlot(name);
        }

        public void NotifyAll_SetEntitiesShowState(List<int> entityGuids, bool isShow)
        {
            BattleManager.Instance.MsgReceiver.On_SetEntitiesShowState(entityGuids, isShow);
        }

        public void NotifyAll_SkillEffectDestroy(int guid)
        {
            BattleManager.Instance.MsgReceiver.On_DestroySkillEffect(guid);
        }

        public void NotifyAll_SkillEffectStartMove(EffectMoveArg effectMoveArg)
        {
            var guid = effectMoveArg.effectGuid;
            var targetPos = new UnityEngine.Vector3(effectMoveArg.targetPos.x, effectMoveArg.targetPos.y,
                 effectMoveArg.targetPos.z);
            var targetGuid = effectMoveArg.targetGuid;
            var moveSpeed = effectMoveArg.moveSpeed;
            BattleManager.Instance.MsgReceiver.On_SkillEffectStartMove(guid, targetPos, targetGuid, moveSpeed);
        }

        public void NotifyAll_SyncEntityAttr(int guid, EntityAttrGroup finalAttrGroup)
        {
            List<BattleClientMsg_BattleAttr> attrs = new List<BattleClientMsg_BattleAttr>();
            foreach (var kv in finalAttrGroup.OptionDic)
            {
                var option = kv.Value;
                BattleClientMsg_BattleAttr attr = new BattleClientMsg_BattleAttr();
                attr.type = (Battle_Client.EntityAttrType)(int)option.attrType;
                if (option.attrType == Battle.EntityAttrType.AttackSpeed)
                {
                    //attr.value = (int)(option.value * 1000.0f);
                    attr.value = option.value;
                }
                else if (option.attrType == Battle.EntityAttrType.MoveSpeed)
                {
                    attr.value = option.value;
                }
                else if (option.attrType == Battle.EntityAttrType.AttackRange)
                {
                    attr.value = option.value;
                }
                else
                {
                    attr.value = (int)option.value;
                }
                attrs.Add(attr);
            }

            BattleManager.Instance.MsgReceiver.On_SyncEntityAttr(guid, attrs);
        }

        public void NotifyAll_SyncEntityCurrHealth(int guid, int hp, int fromEntityGuid)
        {
            List<BattleClientMsg_BattleValue> values = new List<BattleClientMsg_BattleValue>();
            BattleClientMsg_BattleValue v = new BattleClientMsg_BattleValue()
            {
                type = EntityCurrValueType.CurrHealth,
                value = hp,
                fromEntityGuid = fromEntityGuid
            };
            values.Add(v);
            BattleManager.Instance.MsgReceiver.On_SyncEntityValue(guid, values);
        }



        public void SendMsgToClient(int uid, int cmd, byte[] bytes)
        {
            throw new System.NotImplementedException();
        }
    }
}