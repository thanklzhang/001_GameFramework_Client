using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroOperation
{
    public int actionType;
    public int skillId;
    public int releaserGuid;
    public List<int> targetGuids;

}

public enum CombatProgressState
{
    Null,
    RoundAction,
    RoundShow,
    End

}

public class CombatManager : Singleton<CombatManager>
{
    public int combatRoomId;
    List<CombatPlayer> players;
    int currRoundIndex;
    List<HeroOperation> currHeroOperations;

    CombatNetHandler combatNetHandler;

    CombatProgressState progressState;

    RoundActionSelectState roundActionSelectState;

    //CombatShow combatShow;
    public void CreateCombat(SyncCombatInitInfo initInfo)
    {
        this.Reset();
        this.combatRoomId = initInfo.CombatRoomId;
        initInfo.CombatPlayers.ToList().ForEach(serPlayer =>
        {
            var combatPlayer = ToCombatPlayer(serPlayer);
            this.players.Add(combatPlayer);
        });

    }

    public void Init()
    {
        this.players = new List<CombatPlayer>();
        this.currHeroOperations = new List<HeroOperation>();

        currSelectReleaserEntityGuids = new List<int>();
        currSelectSkillId = 0;
        currSelectTargetsEntityGuids = new List<int>();


        combatNetHandler = NetHandlerManager.Instance.GetHandler<CombatNetHandler>();

        EventManager.AddListener<SyncRoundActionStart>((int)GameEvent.CombatRoundActionStart, OnSyncRoundActionStart);
        //EventManager.AddListener<SyncTimelineSequenceProto>((int)GameEvent.CombatRoundShowStart, OnSyncTimelineSequenceStart);

        EventManager.AddListener<GameObject>((int)GameEvent.PlayerClickEntity, OnClickEntity);


    }


    public void Reset()
    {
        this.combatRoomId = -1;
        this.currRoundIndex = -1;
        this.players.Clear();
        this.currHeroOperations.Clear();
        progressState = CombatProgressState.Null;
        roundActionSelectState = RoundActionSelectState.Null;
        currSelectReleaserEntityGuids.Clear();
        currSelectTargetsEntityGuids.Clear();
    }

    public CombatPlayer ToCombatPlayer(CombatPlayerProto serverPlayer)
    {
        var combatPlayer = new CombatPlayer();
        combatPlayer.userId = serverPlayer.UserId;
        combatPlayer.seat = serverPlayer.Seat;
        combatPlayer.team = serverPlayer.Team;

        combatPlayer.entityDic = new Dictionary<int, CombatEntity>();
        serverPlayer.CombatHeroes.ToList().ForEach(serHero =>
        {
            Debug.Log("zxy : hero : " + serHero.Guid + " , " + serHero.ConfigId);
            CombatEntity combatHero = CombatEntity.Create(serHero);
            List<CombatSkill> skills = new List<CombatSkill>();
            serHero.Skills.ToList().ForEach(serSkill=>
            {
                CombatSkill skill = new CombatSkill();
                skill.configId = serSkill.ConfigId;
                skill.level = serSkill.Level;
                Debug.Log("zxy : add skill : " + skill.configId);
                skills.Add(skill);
            });
            combatHero.SetSkills(skills);
          
            if (!combatPlayer.entityDic.ContainsKey(serHero.Guid))
            {
                combatPlayer.entityDic.Add(serHero.Guid, combatHero);
                Debug.Log("zxy : toCombatPlayer : skill : " + combatHero.GetSkills().Count);
            }
            else
            {
                Debug.LogWarning("the guid of combatHero exists : " + serHero.Guid);
            }


        });

        return combatPlayer;
    }

    public List<CombatPlayer> GetCombatPlayers()
    {
        return players;
    }

    bool isWillRoundShow;
    /// <summary>
    /// 每回合开始 进入操作状态
    /// </summary>
    public void OnSyncRoundActionStart(SyncRoundActionStart serActionStart)
    {
        Debug.Log("zxy : " + "OnSyncRoundActionStart , roundIndex : " + serActionStart.RoundIndex);



        this.currRoundIndex = serActionStart.RoundIndex;

        if (this.progressState == CombatProgressState.RoundShow)
        {
            //show state 还没结束
            isWillRoundShow = true;

        }
        else
        {
            progressState = CombatProgressState.RoundAction;
            roundActionSelectState = RoundActionSelectState.CanSelectEntity;
        }
        
    }

    public void OnFinishRoundShow()
    {
        if (isWillRoundShow)
        {
            progressState = CombatProgressState.RoundAction;
            roundActionSelectState = RoundActionSelectState.CanSelectEntity;
        }

        isWillRoundShow = false;
    }

    /// <summary>
    /// 操作状态结束 进入战斗展示状态
    /// </summary>
    public void OnSyncRoundShowStart()
    {
        progressState = CombatProgressState.RoundShow;
        roundActionSelectState = RoundActionSelectState.End;
    }

    //public void Update(float deltaTime)
    //{
    //    //这里之后可以变成多个子状态机
    //    if (this.progressState == CombatProgressState.RoundShow)
    //    {
    //        currTimelineTime += deltaTime;

    //        for (int i = 0; i < currTimeline.nodeList.Count; i++)
    //        {
    //            var currNode = currTimeline.nodeList[i];
    //            if (currNode.IsEnd())
    //            {
    //                continue;
    //            }

    //            if (!currNode.IsStart())
    //            {
    //                if (currTimelineTime >= currNode.startTime)
    //                {
    //                    currNode.Start();
    //                }
    //            }

    //            currNode.Update(deltaTime);

    //        }

    //    }
    //}

    float currTimelineTime;
    TimelineSequence currTimeline;

    /// <summary>
    /// 回合展示开始
    /// </summary>
    /// <param name="timelineSequence"></param>
    public void OnSyncTimelineSequenceStart(SyncTimelineSequenceProto timelineSequence)
    {
        Debug.Log("zxy : " + "OnSyncTimelineSequenceStart , the count of nodes" + timelineSequence.Nodes);
        progressState = CombatProgressState.RoundShow;
        currTimelineTime = 0;

        currTimeline = ConvertToTimelineSequence(timelineSequence);
        EventManager.Broadcast((int)GameEvent.CombatRoundShowStart, currTimeline);

    }

    //待抽出逻辑
    TimelineSequence ConvertToTimelineSequence(SyncTimelineSequenceProto timelineSequence)
    {
        TimelineSequence sequence = new TimelineSequence();

        for (int i = 0; i < timelineSequence.Nodes.Count; i++)
        {
            BaseNode node = null;
            var nodeProto = timelineSequence.Nodes[i];
            var type = (ActionNodeType)nodeProto.Type;
            if (type == ActionNodeType.Move)
            {
                MoveActionProto moveNodeProto = MoveActionProto.Parser.ParseFrom(nodeProto.Data);
                MoveNode moveNode = new MoveNode();
                moveNode.lastTime = moveNodeProto.LastTime / 1000.0f;
                moveNode.speed = moveNodeProto.Speed / 1000.0f;
                moveNode.targetPos =  ConvertTool.ToVector3( moveNodeProto.TargetPos);
                //Debug.Log("zxy : pos ::::: " + moveNodeProto.TargetPos.X + " " + moveNodeProto.TargetPos.Z);
                moveNode.releaserGuid = moveNodeProto.ReleaserGuid;

                node = moveNode;
            }

            if (type == ActionNodeType.PlayAnimation)
            {
                PlayAnimationActionProto playerAnimationNodeProto = PlayAnimationActionProto.Parser.ParseFrom(nodeProto.Data);
                PlayAnimationNode playAnimationNode = new PlayAnimationNode();
                playAnimationNode.targetrGuid = playerAnimationNodeProto.TargetrGuid;
                playAnimationNode.animationName = playerAnimationNodeProto.AnimationName;
                
                node = playAnimationNode;
            }

            if (type == ActionNodeType.AddEffect)
            {
                AddEffectActionProto addEffectNodeProto = AddEffectActionProto.Parser.ParseFrom(nodeProto.Data);
                AddEffectNode addEffectNode = new AddEffectNode();
                addEffectNode.currHealth = addEffectNodeProto.CurrHealth;
                addEffectNode.damage = addEffectNodeProto.Damage;
                addEffectNode.effectResId = addEffectNodeProto.EffectResId;
                addEffectNode.pos = ConvertTool.ToVector3(addEffectNodeProto.Pos);
                addEffectNode.targetGuids = addEffectNodeProto.TargetrGuids.ToList();

                node = addEffectNode;
            }


            if (node != null)
            {
                node.frame = nodeProto.Frame;
                node.startTime = nodeProto.StartTime / 1000.0f;
                node.lastTime = nodeProto.LastTime / 1000.0f;
                sequence.nodeList.Add(node);
            }
           
        }

        return sequence;
    }

    /// <summary>
    /// 增加一次英雄操作
    /// </summary>
    /// <param name="operation"></param>
    public void AddHeroOperation(HeroOperation operation)
    {
        var preOperation = currHeroOperations.Find(c => c.releaserGuid == operation.releaserGuid);
        if (preOperation != null)
        {
            currHeroOperations.Remove(preOperation);
        }

        currHeroOperations.Add(operation);

        foreach (var item in currHeroOperations)
        {
            var str = "zxy : select release skill : " + //"   type:" + item.actionType +
                "   releaseGuid:" + item.releaserGuid +
                "   skillId:" + item.skillId + "   targetGuids:";
            for (int i = 0; i < item.targetGuids.Count; i++)
            {
                str += item.targetGuids[i] + ",";
            }
            Debug.Log("" + str);
        }

        Debug.Log("zxy : operation count " + operation.targetGuids.Count);
        ResetCurrSelectInfo();
        Debug.Log("zxy : after operation count " + operation.targetGuids.Count);

    }

    public void ResetCurrSelectInfo()
    {
        RemoveAllSelectReleaseTargets();

        //currSelectReleaserEntityGuids?.Clear();
        currSelectSkillId = 0;
        currSelectTargetsEntityGuids?.Clear();
    }

    /// <summary>
    /// 完成本次回合操作(完成所有英雄的操作 如果有没有操作的英雄之后再说)
    /// </summary>
    public void FinishCurrRoundAction()
    {
        var proto = GenerateCombatPlayerOperationProto();
        //这里发消息之后可以改成会只给一个服务端发消息 然后再本地做个路由进行发送 因为客户端这边是知道要往 LS 发 ， CS 发 还是 SS 发 
        combatNetHandler.ReqPlayerFinishOperation(proto);
    }

    reqCombatPlayerOperation GenerateCombatPlayerOperationProto()
    {
        reqCombatPlayerOperation proto = new reqCombatPlayerOperation();
        proto.RoundIndex = this.currRoundIndex;
        Debug.Log("zxy :  currHeroOperations : count : " + currHeroOperations.Count);

        currHeroOperations.ForEach(heroOp =>
        {
            CombatHeroOperationProto serHeroOp = new CombatHeroOperationProto()
            {
                ActionType = heroOp.actionType,
                SkillId = heroOp.skillId,
                ReleaserGuid = heroOp.releaserGuid,

            };
            serHeroOp.TargetGuids.AddRange(heroOp.targetGuids);
            Debug.Log("zxy :  serHeroOp.TargetGuids : count : " + serHeroOp.TargetGuids.Count);
            proto.HeroOps.Add(serHeroOp);
        });

        return proto;

    }

    public CombatEntity GetEntityByInstanceId(int instanceId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            var currPlayer = players[i];
            foreach (var item in currPlayer.entityDic)
            {
                if (item.Value.gameObject.GetInstanceID() == instanceId)
                {
                    return item.Value;
                }
            }
        }

        return null;
    }

    public CombatEntity GetEntityByGuid(int guid)
    {
        CombatEntity entity = null;
        for (int i = 0; i < players.Count; i++)
        {
            var currPlayer = players[i];
            if (currPlayer.entityDic.ContainsKey(guid))
            {
                entity = currPlayer.entityDic[guid];
                //Debug.Log("zxy : " + "entity count : " + entity.GetSkills().Count);
                return entity;
            }
        }
        if (entity != null)
        {
            return entity;
        }
        else
        {
            Debug.LogWarning("zxy : " + "the guid doent exist , guid : " + guid);
            return null;
        }

    }


    private void OnClickEntity(GameObject entityObj)
    {
        var instanceId = entityObj.GetInstanceID();
        var entity = GetEntityByInstanceId(instanceId);
        if (entity != null)
        {
            EventManager.Broadcast((int)GameEvent.ClickEntity, entity.guid);
            OnSelectEntity(entity.guid);
           
        }
        else
        {
            Debug.LogWarning("zxy : " + "the entity doesnt exist , instanceId : " + instanceId);
        }
    }

    List<int> currSelectReleaserEntityGuids;
    int currSelectSkillId = 0;
    List<int> currSelectTargetsEntityGuids;

    bool IsHaveSelectReleaserEntity(int guid)
    {
        return currSelectReleaserEntityGuids.Contains(guid);
    }

    void OnSelectEntity(int guid)
    {
        CombatEntity entity = GetEntityByGuid(guid);
        if (entity != null)
        {
            //只有在能操作的回合选择英雄操作才会响应
            if (this.progressState == CombatProgressState.RoundAction)
            {
                Debug.Log("zxy : " + "OnSelectEntity at " + CombatProgressState.RoundAction);
               
                if (roundActionSelectState == RoundActionSelectState.CanSelectEntity)
                {
                    currSelectTargetsEntityGuids?.Clear();

                  
                    if (IsHaveSelectReleaserEntity(guid))
                    {
                        Debug.Log("zxy : " + "cancel select releaser obj");
                        //取消选中
                        currSelectReleaserEntityGuids.Remove(guid);
                        EventManager.Broadcast((int)GameEvent.CancelSelectEntity, guid);
                    }
                    else
                    {
                        Debug.Log("zxy : " + "select releaser obj");
                        //选中(目前只会选择一个)
                        //currSelectReleaserEntityGuids?.Clear();
                        RemoveAllSelectReleaseTargets();

                        currSelectReleaserEntityGuids.Add(guid);

                        EventManager.Broadcast((int)GameEvent.SelectEntity,guid);

                    }
                }

                //当选择技能之后 就会这个状态
                if (roundActionSelectState == RoundActionSelectState.SelectTargetEntity)
                {

                    //Debug.Log("zxy : " + "OnSelectEntity at " + RoundActionSelectState.SelectTargetEntity);

                    Debug.Log("zxy : " + "select target obj");

                   

                    currSelectTargetsEntityGuids.Add(guid);

                    //这里目前先选完目标后直接认为该英雄操作完成  之后可能会等多个目标全部选完才可以认为该英雄完成
                    HeroOperation op = new HeroOperation();
                    op.actionType = 1;
                    op.releaserGuid = currSelectReleaserEntityGuids[0];
                    op.skillId = currSelectSkillId;
                    op.targetGuids = new List<int>();
                    op.targetGuids.AddRange(currSelectTargetsEntityGuids);
                    AddHeroOperation(op);

                    roundActionSelectState = RoundActionSelectState.CanSelectEntity;

                }



            }
        }
        else
        {
            Debug.LogWarning("zxy : " + "the guid doent exist , guid : " + guid);
        }
    }

    void RemoveAllSelectReleaseTargets()
    {

        for (int i = currSelectReleaserEntityGuids.Count - 1; i >= 0; i--)
        {
            var currGuid = currSelectReleaserEntityGuids[i];
            currSelectReleaserEntityGuids.Remove(currGuid);
            EventManager.Broadcast((int)GameEvent.CancelSelectEntity, currGuid);
        }

        currSelectReleaserEntityGuids.Clear();

    }

    public void OnSelectSkill(int skillId)
    {
        currSelectSkillId = skillId;
        roundActionSelectState = RoundActionSelectState.SelectTargetEntity;
    }
    
    void Release()
    {
        EventManager.RemoveListener<SyncRoundActionStart>((int)GameEvent.CombatRoundActionStart, OnSyncRoundActionStart);
        //EventManager.RemoveListener<SyncTimelineSequenceProto>((int)GameEvent.CombatRoundShowStart, OnSyncTimelineSequenceStart);
        EventManager.RemoveListener<GameObject>((int)GameEvent.PlayerClickEntity, OnClickEntity);
        
        progressState = CombatProgressState.End;
    }

}

/// <summary>
/// 操作回合的状态
/// </summary>
public enum RoundActionSelectState
{
    Null,
    CanSelectEntity,
    SelectReleaserEntity,
    //SelectSkill,
    SelectTargetEntity,
    End

    
}