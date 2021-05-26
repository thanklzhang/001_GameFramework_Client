using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayAnimationNode : BaseNode
{
    internal int targetrGuid;
    internal string animationName;

    private CombatEntity moveEntity;

    public override void OnStart()
    {
        Debug.Log("zxy : " + "PlayAnimationNode start");
        moveEntity = CombatManager.Instance.GetEntityByGuid(targetrGuid);
        moveEntity.PlayAnimation(animationName);
    }

    public override void OnUpdate(float deltaTime)
    {
       
    }

    public override void OnEnd()
    {
        
    }
}

