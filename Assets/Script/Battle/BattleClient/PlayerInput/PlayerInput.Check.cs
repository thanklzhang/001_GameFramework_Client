using System.Collections.Generic;
using Battle;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    public partial class PlayerInput
    {
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
                        EventDispatcher.Broadcast(EventIDs.OnSelectEntity,battleEntity);
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
                        Vector3 resultPos;
                        if (TryToGetRayOnGroundPos(out resultPos))
                        {
                            this.OnPlayerClickGround(resultPos);
                        }
                    }
                }
                else
                {
                    //啥也没干
                    CheckNoEventWithoutSkillState();
                }
            }
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
                        SuccessRelease(targetGuid,targetPos,skillId);
                    }
                }
            }

            return battleEntity;
        }

        void CheckNoEventBySkillState()
        {
            var localEntity = BattleManager.Instance.GetLocalCtrlHero();
            Vector3 resultPos;
            var isColliderGround = TryToGetRayOnGroundPos(out resultPos);
            skillDirectModule.UpdateMousePosition(resultPos);

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
        }

     
    }
}