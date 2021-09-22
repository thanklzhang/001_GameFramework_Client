
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum SelectShowObjType
{
    Null,
    Type1,  //将要进行技能的实体
    Type2   //将要成为技能目标的实体
}

public class SelectShowObj
{
    public GameObject obj;
    public SelectShowObjType type;
    public int entityGuid;
}


public class CombatShow : Singleton<CombatShow>
{
    public Transform root;
    List<SelectShowObj> showObjs;

    //List<SelectShowObj> targetsShowObj;
    private bool isPlay = false;
    public void Init(Transform root)
    {
        this.root = root;

        showObjs = new List<SelectShowObj>();
        //targetsShowObj = new List<SelectShowObj>();

        //EventManager.AddListener<int>((int)GameEvent.SelectEntity, OnSelectReleaserEntity);
        //EventManager.AddListener<int>((int)GameEvent.CancelSelectEntity, OnCancelSelectReleaserEntity);
        //EventManager.AddListener<TimelineSequence>((int)GameEvent.CombatRoundShowStart, OnSyncTimelineSequenceStart);



    }


    public void Release()
    {
        root = null;
        //EventManager.RemoveListener<int>((int)GameEvent.SelectEntity, OnSelectReleaserEntity);
        //EventManager.RemoveListener<int>((int)GameEvent.CancelSelectEntity, OnCancelSelectReleaserEntity);
        //EventManager.RemoveListener<TimelineSequence>((int)GameEvent.CombatRoundShowStart, OnSyncTimelineSequenceStart);
    }


    public void OnSelectReleaserEntity(int guid)
    {
        // SelectShowObj showObj = new SelectShowObj();
        // var resId = 1000030000;
        // ResourceManager.Instance.CreateGameObjectById(resId, (isSuccess, obj) =>
        // {
        //     showObj.obj = obj;
        // }, false);
        // showObj.type = SelectShowObjType.Type1;
        // showObj.entityGuid = guid;
        // var entity = CombatManager.Instance.GetEntityByGuid(guid);
        // if (entity != null)
        // {
        //     showObj.obj.transform.position = entity.GetGameObject().transform.position;
        // }

        // showObjs.Add(showObj);
    }

    public void OnCancelSelectReleaserEntity(int guid)
    {
        var showObj = showObjs.Find(r => r.entityGuid == guid);
        if (showObj != null)
        {
            GameObject.Destroy(showObj.obj);
            showObjs.Remove(showObj);
        }
    }
    TimelineSequence currSequence;
    float currTimer = 0.0f;
    private void OnSyncTimelineSequenceStart(TimelineSequence sequence)
    {
        isPlay = true;
        currTimer = 0.0f;
        this.currSequence = sequence;
        for (int i = 0; i < currSequence.nodeList.Count; i++)
        {
            Debug.Log("zxy : currSequence " + currSequence.nodeList[i]);
        }
    }

    public void Update(float deltaTime)
    {
        if (isPlay)
        {
            currTimer += deltaTime;

            for (int i = 0; i < currSequence.nodeList.Count; i++)
            {
                var currNode = currSequence.nodeList[i];
                if (currNode.IsEnd())
                {
                    continue;
                }

                if (!currNode.IsStart())
                {
                    if (currTimer >= currNode.startTime)
                    {
                        currNode.Start();
                    }
                }
                else
                {
                    currNode.Update(deltaTime);

                    if (currTimer >= (currNode.startTime + currNode.lastTime))
                    {
                        currNode.End();
                    }
                }
            }

            var count = currSequence.nodeList.Count;
            if (count > 0)
            {
                var finalNode = currSequence.nodeList[count - 1];
                if (finalNode.IsEnd())
                {
                    this.OnTimelineSequenceEnd();
                }
            }


        }
    }

    public void OnTimelineSequenceEnd()
    {
        isPlay = false;
        currTimer = 0.0f;
    }
}