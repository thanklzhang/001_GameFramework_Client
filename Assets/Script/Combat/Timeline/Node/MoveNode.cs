using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveNode : BaseNode
{
    //float lastTime;
    internal Vector3 targetPos;
    internal float speed;
    internal int releaserGuid;

    private CombatEntity moveEntity;
    private float currTimer;
    private float moveSpeed;
    private Vector3 orginPos;
    public override void OnStart()
    {
        Debug.Log("zxy : " + "MoveNode start");
        moveEntity = CombatManager.Instance.GetEntityByGuid(releaserGuid);
        currTimer = 0.0f;
        //moveSpeed = speed;
        
        orginPos = moveEntity.GetGameObject().transform.position;
        var dis = (targetPos - orginPos).magnitude;
        moveSpeed = dis / lastTime;
        Debug.Log("zxy : " + "moveSpeed" + (moveSpeed));
    }

    public override void OnUpdate(float deltaTime)
    {

        var obj = moveEntity.GetGameObject();
        var dir = (targetPos - orginPos).normalized;



        var moveDis = dir * moveSpeed * deltaTime;
        //Debug.Log("zxy : " + "targetPos pos " + (targetPos) + " " + orginPos);

        moveEntity.SetPosition(obj.transform.position + moveDis);
       
        //Debug.Log("zxy : " + "moveEntity pos " + (obj.transform.position));
    }

    public override void OnEnd()
    {
        Debug.Log("zxy : " + "MoveNode OnEnd " + " " + targetPos);
        moveEntity.SetPosition(targetPos);
    }
}

