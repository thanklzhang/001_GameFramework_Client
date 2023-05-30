using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class TrackBean
{
    public int releaserGuid;
    public int trackConfigId;
    public int targetEntityGuid;
    public Vector3 targetPos;
}

public enum SkillTrackType
{
    Null = 0,
    Rectangle = 1,
    Circle = 2
}

public class SkillTrackModule
{
    BattleCtrl battleCtrl;
    public Dictionary<int, SkillTrackGroup> trackGroupDic;
    public void Init(BattleCtrl battleCtrl)
    {
        this.battleCtrl = battleCtrl;
        trackGroupDic = new Dictionary<int, SkillTrackGroup>();
    }

    public void AddTrack(TrackBean trackBean)
    {
        BaseSkillTrack track = null;
        SkillTrackGroup group = null;
        var guid = trackBean.releaserGuid;
        if (!trackGroupDic.ContainsKey(guid))
        {
            group = new SkillTrackGroup();
            group.Init();

            trackGroupDic.Add(guid, group);
        }
        else
        {
            group = trackGroupDic[guid];
        }

        track = CreateTrack(trackBean);
        group.AddTrack(track);

        track.Start();


    }

    BaseSkillTrack CreateTrack(TrackBean trackBean)
    {
        BaseSkillTrack skillTrack = null;
        var config = TableManager.Instance.GetById<Table.SkillTrack>(trackBean.trackConfigId);

        var type = (SkillTrackType)config.Type;
        if (type == SkillTrackType.Rectangle)
        {
            skillTrack = new RectangleSkillTrack();
            skillTrack.Init(trackBean);
        }

        return skillTrack;
    }

    public void DeleteTrack(int entityGuid, int trackConfigId)
    {
        if (trackGroupDic.ContainsKey(entityGuid))
        {
            var group = trackGroupDic[entityGuid];
            group.DeleteTrack(trackConfigId);
        }
        else
        {
            Logx.LogWarning("the tarck is not found : entityGuid : " + entityGuid);
        }
    }

    public void Update(float deltaTime)
    {
        List<int> willDeleteGuids = new List<int>();
        foreach (var item in trackGroupDic)
        {
            var trackGroup = item.Value;
            trackGroup.Update(deltaTime);

            if (trackGroup.IsEmpty())
            {
                willDeleteGuids.Add(item.Key);
            }
        }

        foreach (var guid in willDeleteGuids)
        {
            trackGroupDic.Remove(guid);
        }


    }

    public void Release()
    {

    }
}
