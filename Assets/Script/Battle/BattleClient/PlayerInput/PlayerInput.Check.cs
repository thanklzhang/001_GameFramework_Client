﻿using System.Collections.Generic;
using Battle;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public partial class PlayerInput
    {
        public BattleEntity_Client currDragEntity;
        public Vector3 dragEntityOriginPos;
        public Vector3 dragEntityOffset;

        public void CheckInput()
        {
            var battleState = BattleManager.Instance.BattleState;
            if (battleState != BattleState.Running)
            {
                return;
            }

            //判断用户键盘输入
            CheckKeyboardEvent(out int eventType);

            //判断用户鼠标输入
            var isSelectSkillState = skillDirectModule.GetSelectState();
            var isMouseLeftButtonDown = Input.GetMouseButtonDown(0);
            var isMouseRightButtonDown = Input.GetMouseButtonDown(1);
            var isMouseLeftButtonUp = Input.GetMouseButtonUp(0);
            if (isSelectSkillState)
            {
                //选择技能中
                if (BattleManager.Instance.IsIntelligentRelease && eventType > 1)
                {
                    //智能施法(除了普通攻击) 直接执行释放技能操作
                    HandleReleaseSkill();
                }
                else if (isMouseLeftButtonDown)
                {
                    //选择了目标或者点 开始执行释放技能操作
                    HandleReleaseSkill();
                }
                else if (isMouseRightButtonDown)
                {
                    //取消技能选择操作 
                    skillDirectModule.FinishSelect();
                    willUseItemIndex = -1;
                }
                else
                {
                    //还没有下一步操作 继续技能目标选择中状态
                    CheckNoEventBySkillState();
                }
            }
            else
            {
                //无技能释放动作
                if (isMouseLeftButtonDown)
                {
                    if (TryToGetRayOnEntity(out var entityGuidList))
                    {
                        var battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
                        EventDispatcher.Broadcast(EventIDs.OnSelectEntity, battleEntity);

                        if (battleEntity.guid != BattleManager.Instance.GetLocalCtrlHeroGuid())
                        {
                            currDragEntity = battleEntity;
                            dragEntityOriginPos = currDragEntity.gameObject.transform.position;
                            var isInArea = TryToGetRayTargetPos(out var hit, new List<string>()
                            {
                                GlobalConfig.Ground,
                                GlobalConfig.UnderstudyArea
                            });
                            if (isInArea)
                            {
                                // this.dragEntityOffset = dragEntityOriginPos - hit.point;
                            }
                            else
                            {
                                //?????? 到底在哪呢
                            }
                        }
                    }
                    else
                    {
                        //仅仅是左键点击了某处
                        EventDispatcher.Broadcast(EventIDs.OnCancelSelectEntity);
                    }
                }
                else if (isMouseRightButtonDown)
                {
                    //检查右键普通攻击
                    var entity = CheckNormalAttackByRightMouseKey();

                    if (null == entity)
                    {
                        //移动到某处
                        if (TryToGetRayTargetPos(out var hit))
                        {
                            this.OnPlayerClickGround(hit.point);
                        }
                    }
                }
                else if (isMouseLeftButtonUp)
                {
                    CheckMouseLeftUp();
                }
                else
                {
                    //啥也没干 检测持续性行为
                    CheckNoEventWithoutSkillState();
                }
            }
        }

        public void CheckMouseLeftUp()
        {
            // List<int> entityGuidList;
            // BattleEntity_Client battleEntity = null;

            // //检测是否有射线碰到的单位
            // var isRayToEntity = TryToGetRayOnEntity(out entityGuidList);
            // if (isRayToEntity)
            // {
            //     battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
            //     if (battleEntity != null)
            //     {
            //         if (battleEntity != currDragEntity)
            //         {
            //             var targetUnderstudyIndex =  UnderstudyManager_Client.Instance.GetLocationByEntityGuid(battleEntity.guid);
            //             if (targetUnderstudyIndex >= 0)
            //             {
            //                 //目标是替补单位 忽略 由下面区域触发即可
            //                 
            //             }
            //             else
            //             {
            //                 //发送信息：用当前拖动的单位 替换 目标单位
            //                 Logx.Log(LogxType.Zxy, "drag : replace target entity guid : "
            //                                        + battleEntity.guid);
            //                 
            //                 BattleManager.Instance.MsgSender.Send_OperateHero(new OperateHeroArg()
            //                 {
            //                     // opType = OperateHeroType.Replace,
            //                     opHeroGuid = currDragEntity.guid,
            //                     // targetHeroGuid = battleEntity.guid
            //                 });
            //             
            //                 currDragEntity = null;
            //                 return;
            //             }
            //         }
            //     }
            // }

            //检测区域
            var isColliderUnderstudy = TryToGetRayTargetPos(out var hit, GlobalConfig.UnderstudyArea);
            if (isColliderUnderstudy)
            {
                //检测是否扔到了替补位
                if (currDragEntity != null)
                {
                    var position = hit.transform.position;
                    var midPos = new Vector3(position.x, hit.point.y, position.z);
                    var pos = midPos;
                    // currDragEntity.SetPosition(pos);
                    var index = hit.transform.GetSiblingIndex();

                    //发送信息：改变布阵位置 到替补
                    Logx.Log(LogxType.Zxy, $"drag : move entity , entity guid : {currDragEntity.guid}"
                                           + $" isUnderstudyArea : {true}");

                    BattleManager.Instance.MsgSender.Send_OperateHeroByArraying(currDragEntity.guid, pos, index);

                    currDragEntity = null;
                    return;
                }
            }
            else
            {
                //检测是否扔到了战场上
                var isColliderGround = TryToGetRayTargetPos(out hit, GlobalConfig.Ground);
                if (isColliderGround)
                {
                    if (currDragEntity != null)
                    {
                        // var pos = new Vector3(resultPos.x,
                        //     currDragEntity.gameObject.transform.position.y, resultPos.z);

                        var pos = hit.point + dragEntityOffset;
                        // currDragEntity.SetPosition(pos);
                        //发送信息：改变布阵位置 
                        //参数：entityGuid,isUnderstudyArea(目标)，pos，index（替补区域位置）
                        Logx.Log(LogxType.Zxy, $"drag : move entity , entity guid : {currDragEntity.guid}"
                                               + $" isUnderstudyArea : {false}");

                        BattleManager.Instance.MsgSender.Send_OperateHeroByArraying(currDragEntity.guid, pos, -1);

                        currDragEntity = null;
                        return;
                    }
                }
                else
                {
                    //扔到了既不是替补位，也不是交换单位又不是地面
                }
            }

            //恢复原位
            if (currDragEntity != null)
            {
                currDragEntity.SetPosition(dragEntityOriginPos);
            }

            currDragEntity = null;
        }

        //该点是否在替补区域中
        public bool IsInUnderstudyArea(Vector3 pos)
        {
            return false;
        }

        //检查键盘输入 eventType：1：普通攻击，2：技能，3：道具
        void CheckKeyboardEvent(out int eventType)
        {
            eventType = -1;
            if (Input.GetKeyDown(KeyCode.A))
            {
                eventType = 1;
                this.OnUseSkill(0);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                eventType = 2;
                this.OnUseSkill(1);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                eventType = 2;
                this.OnUseSkill(2);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                eventType = 2;
                this.OnUseSkill(3);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                eventType = 2;
                this.OnUseSkill(4);
            }


            for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha6; i++)
            {
                var index = i - (int)KeyCode.Alpha1;
                var key = (KeyCode)(i);
                if (Input.GetKeyDown(key))
                {
                    eventType = 3;
                    this.OnUseItem(index);
                }
            }
        }

        BattleEntity_Client CheckNormalAttackByRightMouseKey()
        {
            var localEntity = BattleManager.Instance.GetLocalCtrlHero();
            GameObject gameObject = null;
            List<int> entityGuidList;
            BattleEntity_Client battleEntity = null;
            if (TryToGetRayOnEntity(out entityGuidList))
            {
                //遍历寻找 效率低下 之后更改
                //只找一个
                battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
            }

            if (battleEntity != null)
            {
                var targetGuid = battleEntity.guid;
                var targetPos = Vector3.right;
                //先排除自己
                if (localEntity.collider.gameObject.GetInstanceID() !=
                    battleEntity.collider.gameObject.GetInstanceID())
                {
                    var skill = localEntity.GetNormalAttackSkill();
                    if (skill != null)
                    {
                        var skillId = skill.configId;
                        SuccessRelease(targetGuid, targetPos, skillId);
                    }
                }
            }

            return battleEntity;
        }

        void CheckNoEventBySkillState()
        {
            var localEntity = BattleManager.Instance.GetLocalCtrlHero();
            var isColliderGround = TryToGetRayTargetPos(out var hit);
            skillDirectModule.UpdateMousePosition(hit.point);

            GameObject gameObject = null;
            List<int> entityGuidList;
            BattleEntity_Client battleEntity = null;
            if (TryToGetRayOnEntity(out entityGuidList))
            {
                battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);

                var entityModelRootGo = battleEntity.gameObject; //.transform.parent.gameObject
                //判断当前鼠标是否检测到是敌人

                var relationType = BattleEntity_Client.GetRelation(localEntity, battleEntity);
                if (relationType == Battle_Client.EntityRelationType.Enemy)
                {
                    OperateViewManager.Instance.cursorModule.SetCursor(CursorType.SelectAttack);
                    OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo,
                        enemyOutlineColor,
                        true);
                }
                else
                {
                    OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Select);
                    if (relationType == Battle_Client.EntityRelationType.Me)
                    {
                        OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo,
                            myOutlineColor,
                            true);
                    }
                    else
                    {
                        OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo,
                            friendOutlineColor,
                            true);
                    }
                }
            }
            else
            {
                OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Select);
                OperateViewManager.Instance.modelOutlineModule.CloseAllModelOutline();
            }
        }

        void CheckNoEventWithoutSkillState()
        {
            GameObject gameObject = null;
            List<int> entityGuidList;
            BattleEntity_Client battleEntity = null;
            if (TryToGetRayOnEntity(out entityGuidList))
            {
                //遍历寻找 效率低下 之后更改
                battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);

                var entityModelRootGo = battleEntity.gameObject; //.transform.parent.gameObject
                //判断当前鼠标是否检测到是敌人
                var localEntity = BattleManager.Instance.GetLocalCtrlHero();
                var relationType = BattleEntity_Client.GetRelation(localEntity, battleEntity);

                if (relationType == Battle_Client.EntityRelationType.Enemy)
                {
                    OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Attack);
                    OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo,
                        enemyOutlineColor,
                        true);
                }
                else
                {
                    OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Normal);

                    if (relationType == Battle_Client.EntityRelationType.Me)
                    {
                        OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo,
                            myOutlineColor,
                            true);
                    }
                    else
                    {
                        OperateViewManager.Instance.modelOutlineModule.OpenOutline(entityModelRootGo,
                            friendOutlineColor,
                            true);
                    }
                }
            }
            else
            {
                OperateViewManager.Instance.cursorModule.SetCursor(CursorType.Normal);
                OperateViewManager.Instance.modelOutlineModule.CloseAllModelOutline();
            }


            //检测拖动释放
            if (currDragEntity != null)
            {
                var isColliderGround = TryToGetRayTargetPos(out var hit);
                if (isColliderGround)
                {
                    // var pos = new Vector3(resultPos.x,
                    //     currDragEntity.gameObject.transform.position.y, resultPos.z);

                    var pos = hit.point + dragEntityOffset;
                    // Logx.Log(LogxType.Zxy,$"hit.point:{hit.point} " +
                    //                       $"dragEntityOffset:{dragEntityOffset} pos:{pos}");

                    currDragEntity.SetPosition(pos);
                }
            }
        }
    }
}