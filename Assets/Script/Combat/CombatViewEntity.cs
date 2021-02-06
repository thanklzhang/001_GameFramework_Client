//using Assets.Script.Combat;
//using FixedPointy;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CombatViewEntity
//{
//    public GameObject entityObj;

//    public CombatLogicEntity entity;

//    private Animation ani;

//    //public EntityState state;

//    public static CombatViewEntity Create(GameObject obj, Vector3 pos, CombatLogicEntity entity)
//    {
//        CombatViewEntity ctrl = new CombatViewEntity()
//        {
//            entityObj = obj,
//            entity = entity
//        };
//        ctrl.entityObj.transform.position = pos;

//        ctrl.Init();

//        return ctrl;
//    }

//    public Vector3 currTargetPos;
//    public Vector3 currForward;

//    void Init()
//    {
//        ani = entityObj.GetComponent<Animation>();
//        AddListeners();
//    }

//    void AddListeners()
//    {
//        entity.moveAction += OnMoveTo;
//        entity.rotateAction += OnRotate;//目前这个没用 因为旋转直接和 entity 中的数据在 update 中直接过渡对应
//        entity.stopMoveAction += OnStopMove;
//        //entity.onChangeHp += OnChangeHp;
//        entity.preSkillAction += OnPreAttack;
//    }

//    void OnMoveTo(FixVec3 dis)
//    {
//        var disDelta = ConvertTool.ToVector3(dis);
//        OnMoveTo(disDelta);
//    }

//    public void OnMoveTo(Vector3 disDelta)
//    {
//        currTargetPos = this.entityObj.transform.position + disDelta * 1000;//捕捉远距离影子
//        PlayMoveAnimation();
//    }

//    public void OnRotate(FixVec3 forward)
//    {
//        var currForward = ConvertTool.ToVector3(forward);

//        this.currForward = currForward;

//        // entityObj.transform.forward = currForward;
//    }

//    public void OnStopMove()
//    {
//        PlayStopAnimation();
//        SyncPosition();
//    }

//    public void OnChangeHp(Fix hp)
//    {

//    }

//    public void OnPreAttack(int skillSN)
//    {
//        PlayAttackAnimation();//目前都用这个 之后根据 SN 播放不同的动画
//    }

//    public void Update(float deltaTime)
//    {
//        //if (entity.state == EntityState.Run)
//        //{
//        //rotate : 
//        this.entityObj.transform.forward = Vector3.Lerp(this.entityObj.transform.forward, ConvertTool.ToVector3(entity.Forward), Time.deltaTime * 20.0f);

//        //pos : 

//        var sqrtLimitDis = 0.25f * 0.25f;
//        if ((currTargetPos - entityObj.transform.position).sqrMagnitude < sqrtLimitDis)
//        {
//            return;
//        }

//        var speedScale = Const.frameTime / 1000.0f / deltaTime;

//        var dir = (currTargetPos - this.entityObj.transform.position).normalized;
//        this.entityObj.transform.position += dir * (float)entity.GetAttribute(EffectAttributeType.MoveSpeed) * speedScale * deltaTime;
//        //}



//        //entityObj.transform.forward = Vector3.Lerp(entityObj.transform.forward, currForward, Time.deltaTime * 10);
//    }

//    void SyncPosition()
//    {
//        var sqrtLimitDis = 0.25f * 0.25f;
//        var pos = entity.position;

//        if ((entityObj.transform.position - ConvertTool.ToVector3(pos)).sqrMagnitude > sqrtLimitDis)
//        {
//            entityObj.transform.position = new Vector3((float)pos.X, (float)pos.Y, (float)pos.Z);
//        }
//        currTargetPos = entityObj.transform.position;
//    }

//    void PlayMoveAnimation()
//    {
//        PlayAnimation("walk");
//    }

//    void PlayStopAnimation()
//    {
//        PlayAnimation("idle");
//    }

//    void PlayAttackAnimation()
//    {
//        PlayAnimation("attack");
//    }

//    void PlayAnimation(string aniStr)
//    {
//        ani.CrossFade(aniStr);
//    }

//}
