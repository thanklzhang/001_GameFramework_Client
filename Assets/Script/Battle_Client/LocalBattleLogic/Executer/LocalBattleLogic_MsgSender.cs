using Battle;
using Battle_Client;
using System.Collections.Generic;
using JetBrains.Annotations;
using Table;
using BattleItem = Battle.BattleItem;
using BuffEffect = Battle.BuffEffect;
using Skill = Battle.Skill;

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

        public void NotifyAll_PlayerReadyState(int uid, bool isReady)
        {
            BattleManager.Instance.MsgReceiver.On_PlayerReadyState(uid, isReady);
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

            foreach (var entity in logicList)
            {
                BattleClientMsg_Entity entityArg = new BattleClientMsg_Entity()
                {
                    guid = entity.guid,
                    configId = entity.configId,
                    playerIndex = entity.playerIndex,
                    level = entity.level,
                    position = new UnityEngine.Vector3(entity.position.x, entity.position.y, entity.position.z)
                };

                //skill list
                // entityArg.skills = new List<BattleClientMsg_Skill>();
                // foreach (var netSkill in item.GetAllSkills())
                // {
                //     var skill = new BattleClientMsg_Skill()
                //     {
                //         configId = netSkill.Value.configId,
                //         level = netSkill.Value.level
                //     };
                //     entityArg.skills.Add(skill);
                //
                // }

                //item list
                entityArg.itemList = new List<BattleClientMsg_Item>();
                foreach (var netItem in entity.itemList)
                {
                    var battleItem = new BattleClientMsg_Item()
                    {
                        configId = netItem.configId,
                        count = netItem.count
                    };
                    entityArg.itemList.Add(battleItem);
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

        public void NotifyAll_OnEntityReleaseSkill(int guid, int skillConfigId)
        {
            BattleManager.Instance.MsgReceiver.On_EntityUseSkill(guid, skillConfigId);
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

        public void NotifyAll_SyncEntityAttr(int guid, Dictionary<Battle.EntityAttrType, float> dic)
        {
            List<BattleClientMsg_BattleAttr> attrs = new List<BattleClientMsg_BattleAttr>();
            foreach (var kv in dic)
            {
                var value = kv.Value;
                BattleClientMsg_BattleAttr attr = new BattleClientMsg_BattleAttr();
                var type = kv.Key;
                attr.type = (EntityAttrType)(int)kv.Key;
                if (type == Battle.EntityAttrType.AttackSpeed)
                {
                    //attr.value = (int)(option.value * 1000.0f);
                    attr.value = value;
                }
                else if (type == Battle.EntityAttrType.MoveSpeed)
                {
                    attr.value = value;
                }
                else if (type == Battle.EntityAttrType.AttackRange)
                {
                    attr.value = value;
                }
                else
                {
                    attr.value = (int)value;
                }

                attrs.Add(attr);
            }

            BattleManager.Instance.MsgReceiver.On_SyncEntityAttr(guid, attrs);
        }

        public void NotifyAll_SyncEntitySkills(int guid, Dictionary<int, Skill> dic)
        {
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

        public void NotifyAll_NotifySkillInfoUpdate(Skill skill)
        {
            var entityGuid = skill.releser.guid;
            var skillConfigId = skill.configId;
            var currCDTime = skill.GetCurrCDTimer();
            var maxCDTime = skill.GetCDMaxTime();
            BattleManager.Instance.MsgReceiver.On_SkillInfoUpdate(entityGuid, skillConfigId, currCDTime, maxCDTime);
        }

        public void NotifyAll_NotifyItemInfoUpdate(BattleItem item)
        {
            if (null == item.owner)
            {
                return;
            }

            var entityGuid = item.owner.guid;
            var itemConfigId = item.configId;
            var itemIndex = item.owner.GetItemIndex(item);
            var itemCount = item.count;
            if (item.isDiscard)
            {
                itemCount = 0;
            }

            var currCDTime = item.skill.GetCurrCDTimer();
            var maxCDTime = item.skill.GetCDMaxTime();
            BattleManager.Instance.MsgReceiver.On_ItemInfoUpdate(entityGuid, itemIndex, itemConfigId, itemCount,
                currCDTime, maxCDTime);
        }

        public void NotifyAll_NotifySkillItemInfoUpdate(BattleItem item)
        {
            if (null == item.owner)
            {
                return;
            }

            var entityGuid = item.owner.guid;
            var itemConfigId = item.configId;
            var itemIndex = item.owner.GetItemIndex(item);
            var itemCount = item.count;
            if (item.isDiscard)
            {
                itemCount = 0;
            }

            float currCDTime = 0;
            float maxCDTime = 0;
            if (item.skill != null)
            {
                currCDTime = item.skill.GetCurrCDTimer();
                maxCDTime = item.skill.GetCDMaxTime();
            }


            BattleManager.Instance.MsgReceiver.On_SkillItemInfoUpdate(entityGuid, itemIndex, itemConfigId, itemCount,
                currCDTime, maxCDTime);
        }


        public void NotifyAll_NotifyUpdateBuffInfo(BuffEffectInfo buffInfo)
        {
            BattleManager.Instance.MsgReceiver.On_BuffInfoUpdate(buffInfo);
        }

        public void NotifyAll_NotifySkillTrackStart(Skill skill, int skillTrackId)
        {
            BattleClientMsg_CreateSkillTrack create = new BattleClientMsg_CreateSkillTrack();
            //var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skill.configId);
            ////var ids = StringConvert.ToIntList(skillConfig.SkillTrackList, ',');
            create.trackConfigId = skillTrackId;
            create.releaserEntityGuid = skill.releser.guid;
            create.targetPos = BattleConvert.ConvertToVector3(skill.targetPos);
            create.targetEntityGuid = skill.targetGuid;
            BattleManager.Instance.MsgReceiver.On_SkillTrackStart(create);
        }

        public void NotifyAll_NotifySkillTrackEnd(Skill skill, int skillTrackId)
        {
            var releaserGuid = skill.releser.guid;

            BattleManager.Instance.MsgReceiver.On_SkillTrackEnd(releaserGuid, skillTrackId);
        }

        // public void NotifyAll_NotifyOpenBox(BattleBox box)
        // {
        //     BattleManager.Instance.MsgReceiver.On_OpenBox(releaserGuid, skillTrackId);
        // }

        public void NotifyAll_NotifyOpenBox(Battle.BattleBox box)
        {
            BattleClientMsg_BattleBox netBox = new BattleClientMsg_BattleBox();
            netBox.selections = new List<BattleClientMsg_BattleBoxSelection>();
            netBox.openEntityGuid = box.entity.guid;
            for (int i = 0; i < box.selectionGroup.Count; i++)
            {
                var boxSelection = box.selectionGroup[i];

                BattleClientMsg_BattleBoxSelection netSelection = new BattleClientMsg_BattleBoxSelection();
                netSelection.rewardConfigId = boxSelection.rewardConfig.Id;
                netSelection.intValueList = boxSelection.GetRealRewardIntValueList();

                netBox.selections.Add(netSelection);
            }

            BattleManager.Instance.MsgReceiver.On_OpenBox(netBox);
        }

        public void NotifyAll_NotifyBoxInfoUpdate(int entityGuid,List<Battle.BattleBox> netBoxList)
        {
            List<BattleClientMsg_BattleBox> list = new List<BattleClientMsg_BattleBox>();
            foreach (var netBox in netBoxList)
            {
                BattleClientMsg_BattleBox box = new BattleClientMsg_BattleBox();

                box.selections = new List<BattleClientMsg_BattleBoxSelection>();
                if (netBox.selectionGroup != null)
                {
                    foreach (var netSelection in netBox.selectionGroup)
                    {
                        BattleClientMsg_BattleBoxSelection selecion = new BattleClientMsg_BattleBoxSelection();
                        selecion.rewardConfigId = netSelection.rewardConfig.Id;
                        selecion.intValueList = netSelection.realityReward.GetIntValueList();
                        box.selections.Add(selecion);
                    }

                }
                
                list.Add(box);
            }


            BattleManager.Instance.MsgReceiver.On_BoxInfoUpdate(entityGuid,list);
        }

        public void NotifyAll_NotifySelectBoxReward(int index)
        {
            throw new System.NotImplementedException();
        }


        public void SendMsgToClient(int uid, int cmd, byte[] bytes)
        {
            throw new System.NotImplementedException();
        }
    }
}