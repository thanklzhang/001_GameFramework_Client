using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;
// using Battle;
using Battle.BattleTrigger.Runtime;
using Battle_Client;
using GameData;
using NetProto;
using Table;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle_Client
{
    //战斗中的用户输入
    public partial class BattleManager
    {
        //技能释放指示模块
        SkillDirectorModule skillDirectModule;

        //技能释放后的轨道模块
        SkillTrackModule skillTrackModule;


        public int willUseItemIndex;
        // public int willReleaserSkillId;

        public int willReleaserSkillIndex;

        public bool CheckLocalHeroSkillRelease(int skillId)
        {
            //检测 cd
            var skill = FindLocalHeroSkill(skillId);
            if (skill != null)
            {
                if (skill.currCDTime <= 0)
                {
                    return true;
                }
            }

            //CtrlManager.Instance.globalCtrl.ShowTips("这个技能还不能释放");

            var tips = "这个技能还不能释放";
            Tips.ShowSkillTipText(tips);
            return false;
        }


        //用户点击地面的时候(右键)
        void OnPlayerClickGround(Vector3 clickPos)
        {
            //Logx.Log("OnPlayerClickGround : clickPos : " + clickPos);
            var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();

            var myUid = GameDataManager.Instance.UserStore.Uid;

            var guid = 1; //目前这个不用发 因为 1 个玩家只控制一个英雄实体 服务器已经记录 这里先保留 entity guid
            //battleNet.SendMoveEntity(guid, clickPos);
            BattleManager.Instance.MsgSender.Send_MoveEntity(guid, clickPos);
        }

        public bool TryToGetRayOnGroundPos(out Vector3 pos)
        {
            pos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    var currHit = hits[i];
                    var tag = currHit.collider.tag;
                    if (tag == "Ground")
                    {
                        //Logx.Log("hit ground : " + currHit.collider.gameObject.name);
                        pos = currHit.point;
                        return true;

                        //this.OnPlayerClickGround(currHit.point);
                    }
                }
            }

            return false;
        }


        public bool TryToGetRayOnEntity(out List<int> entityGuidList)
        {
            entityGuidList = new List<int>();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    var currHit = hits[i];
                    var currHitGo = currHit.collider.gameObject;

                    // Logx.Log("currHitGo : " + currHitGo.name);
                    var entity =
                        BattleEntityManager.Instance.FindEntityByColliderInstanceId(currHitGo.GetInstanceID());
                    if (entity != null)
                    {
                        entityGuidList.Add(entity.guid);
                    }
                }
            }

            return entityGuidList.Count > 0;
        }

        //使用道具
        public void OnUseItem(int index)
        {
            willUseItemIndex = index;

            //TODO 根据 index 找到道具 然后从配置中找到 skillId
            var skillId = 2005001; //BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
            // willReleaserSkillId = skillId;


            //willReleaserSkillIndex = index;
            int targetGuid = 0;
            Vector3 targetPos = Vector3.zero;

            var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
            var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);

            var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
            var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
            var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

            //TODO 判断道具释放条件 现在本地检测一下
            // var isNormalAttack = 0 == index;
            // if (!isNormalAttack)
            // {
            //     var isCanRelease = CheckLocalHeroSkillRelease(skillId);
            //     if (!isCanRelease)
            //     {
            //         return;
            //     }
            // }

            var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
            if (releaseTargetType == SkillReleaseTargeType.Point)
            {
                this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
            }
            else if (releaseTargetType == SkillReleaseTargeType.Entity)
            {
                this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
            }
            else if (releaseTargetType == SkillReleaseTargeType.NoTarget)
            {
                var arg = new ItemUseArg()
                {
                    itemIndex = willUseItemIndex,
                    releaserGuid = localEntity.guid,
                    targetGuid = targetGuid,
                    targetPos = targetPos
                };
                
                Logx.Log(LogxType.Battle,"Use item : itemIndex : " + arg.itemIndex);
                
                BattleManager.Instance.MsgSender.Send_UseItem(arg);
            }

    }


    public void OnUseSkill(int index)
    {
        var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
        
        willReleaserSkillIndex = index;
        int targetGuid = 0;
        Vector3 targetPos = Vector3.zero;
        
        var battleNet = NetHandlerManager.Instance.GetHandler<BattleNetHandler>();
        var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);

        var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
        var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
        var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

        //判断释放条件 现在本地检测一下
        //普通攻击不提示


        var isNormalAttack = 1 == skillConfig.IsNormalAttack;
        if (!isNormalAttack)
        {
            var isCanRelease = CheckLocalHeroSkillRelease(skillId);
            if (!isCanRelease)
            {
                return;
            }
        }

        var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;
        if (releaseTargetType == SkillReleaseTargeType.Point)
        {
            this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
        }
        else if (releaseTargetType == SkillReleaseTargeType.Entity)
        {
            this.skillDirectModule.StartSelect(skillId, localEntity.gameObject);
        }
        else if (releaseTargetType == SkillReleaseTargeType.NoTarget)
        {
            BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid, targetPos);
        }

        //Logx.Log("use skill : skillId : " + skillId + " targetGuid : " + targetGuid + " targetPos : " + targetPos);
    }
    private Color enemyOutlineColor = new Color(1, 0.2f, 0.2f, 1);
    private Color myOutlineColor = new Color(0.5f, 1.0f, 0.5f, 1);
    private Color friendOutlineColor = new Color(0.2f, 0.6f, 1.0f, 1);


    public void CheckInput()
    {
        if (this.battleState != BattleState.Running)
        {
            return;
        }

        //判断用户输入
        var isSelectSkillState = skillDirectModule.GetSelectState();
        var isMouseLeftButtonDown = Input.GetMouseButtonDown(0);
        var isMouseRightButtonDown = Input.GetMouseButtonDown(1);

        var localCtrlHeroGameObject = BattleManager.Instance.GetLocalCtrlHeroGameObject();
        var localInstanceID = localCtrlHeroGameObject.GetInstanceID();
        var localEntity = BattleEntityManager.Instance.FindEntityByInstanceId(localInstanceID);

        if (isSelectSkillState)
        {
            //选择技能中
            var index = willReleaserSkillIndex;
             var skillId = BattleManager.Instance.GetCtrlHeroSkillIdByIndex(index);
            /*var skillId = willReleaserSkillId;*/
            var skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);
            var releaseTargetType = (SkillReleaseTargeType)skillConfig.SkillReleaseTargeType;

            if (isMouseLeftButtonDown)
            {
                if (releaseTargetType == SkillReleaseTargeType.Point)
                {
                    Vector3 resultPos;
                    var isColliderGround = TryToGetRayOnGroundPos(out resultPos);
                    //确定选择技能目标
                    if (isColliderGround)
                    {
                        skillDirectModule.FinishSelect();
                        willUseItemIndex = -1;

                        int targetGuid = 0;
                        Vector3 targetPos = resultPos;

                        if (willUseItemIndex > 0)
                        {
                            //使用道具
                            var arg = new ItemUseArg()
                            {
                                itemIndex = willUseItemIndex,
                                releaserGuid = localEntity.guid,
                                targetGuid = targetGuid,
                                targetPos = targetPos
                            };
                
                            Logx.Log(LogxType.Battle,"Use item : itemIndex : " + arg.itemIndex);
                
                            BattleManager.Instance.MsgSender.Send_UseItem(arg);
                        }
                        else
                        {
                            //技能
                            BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid,
                                targetPos);
                        }
                    }

                    skillDirectModule.FinishSelect();
                    willUseItemIndex = -1;
                }
                else if (releaseTargetType == SkillReleaseTargeType.Entity)
                {
                    //-----------------------

                    GameObject gameObject = null;
                    List<int> entityGuidList;
                    BattleEntity_Client battleEntity = null;
                    if (TryToGetRayOnEntity(out entityGuidList))
                    {
                        //遍历寻找 效率低下 之后更改
                        //只找一个
                        battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);
                        // battleEntity =
                        //     BattleEntityManager.Instance.FindEntityByColliderInstanceId(gameObject.GetInstanceID());
                    }
                    else
                    {
                        //没有目标 那么就选择最近一段距离的某个单位
                        float dis = 10.0f;
                        battleEntity = BattleEntityManager.Instance.FindNearestEntity(localEntity, dis);
                    }

                    if (battleEntity != null)
                    {
                        //Logx.Log("battle entity not null");
                        var targetGuid = battleEntity.guid;

                        var targetPos = Vector3.right;
                        //先排除自己
                        if (localEntity.collider.gameObject.GetInstanceID() !=
                            battleEntity.collider.gameObject.GetInstanceID())
                        {
                            //battleNet.SendUseSkill(skillId, targetGuid, targetPos);
                            if (willUseItemIndex > 0)
                            {
                                //使用道具
                                var arg = new ItemUseArg()
                                {
                                    itemIndex = willUseItemIndex,
                                    releaserGuid = localEntity.guid,
                                    targetGuid = targetGuid,
                                    targetPos = targetPos
                                };
                
                                Logx.Log(LogxType.Battle,"Use item : itemIndex : " + arg.itemIndex);
                
                                BattleManager.Instance.MsgSender.Send_UseItem(arg);
                            }
                            else
                            {
                                //使用技能
                                BattleManager.Instance.MsgSender.Send_UseSkill(localEntity.guid, skillId, targetGuid,
                                    targetPos);
                            }
                        }
                    }


                    skillDirectModule.FinishSelect();
                    willUseItemIndex = -1;
                }
            }
            else if (isMouseRightButtonDown)
            {
                //取消技能选择操作 
                skillDirectModule.FinishSelect();
                willUseItemIndex = -1;
            }
            else
            {
                //技能目标选择中
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
        }
        else
        {
            //无技能释放动作
            if (isMouseLeftButtonDown)
            {
                //仅仅是左键点击了某处
            }
            else if (isMouseRightButtonDown)
            {
                //移动到某处
                Vector3 resultPos;
                if (TryToGetRayOnGroundPos(out resultPos))
                {
                    this.OnPlayerClickGround(resultPos);
                }
            }
            else
            {
                //鼠标操作啥也没干
                GameObject gameObject = null;
                List<int> entityGuidList;

                BattleEntity_Client battleEntity = null;
                if (TryToGetRayOnEntity(out entityGuidList))
                {
                    //遍历寻找 效率低下 之后更改
                    battleEntity = BattleEntityManager.Instance.FindEntity(entityGuidList[0]);

                    var entityModelRootGo = battleEntity.gameObject; //.transform.parent.gameObject
                    //判断当前鼠标是否检测到是敌人
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


        if (Input.GetKeyDown(KeyCode.A))
        {
            this.OnUseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.OnUseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            this.OnUseSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            this.OnUseSkill(3);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.OnUseSkill(4);
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //TODO 找 第一个 道具栏的道具
            this.OnUseItem(0);
        }
    }

    public void OnSkillTrackStart(TrackBean trackBean)
    {
        this.skillTrackModule.AddTrack(trackBean);
    }

    public void OnSkillTrackEnd(int entityGuid, int trackId)
    {
        this.skillTrackModule.DeleteTrack(entityGuid, trackId);
    }
}

}