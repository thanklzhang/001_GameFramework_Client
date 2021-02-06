using DataModel;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AddEffectNode : BaseNode
{
    internal int effectResId;
    internal int currHealth;
    internal int damage;
    internal Vector3 pos;
    internal List<int> targetGuids;


    private GameObject effectObj;

    public override void OnStart()
    {
        // Debug.Log("zxy : " + "addEffectNode start");
        // ResourceManager.Instance.CreateGameObjectById(effectResId, (isSuccess, obj) =>
        // {
        //     effectObj = obj;
        // }, false);

        // for (int i = 0; i < targetGuids.Count; i++)
        // {
        //     var targetGuid = targetGuids[i];
        //     var targetEntity = CombatManager.Instance.GetEntityByGuid(targetGuid);

        //     //目前先直接放到 entity 的位置即可
        //     var obj = targetEntity.GetGameObject();
        //     effectObj.transform.position = obj.transform.position;

        // }

    }

    public override void OnUpdate(float deltaTime)
    {

    }

    public override void OnEnd()
    {

    }

}