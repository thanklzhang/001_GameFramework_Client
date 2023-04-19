using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Table;
using UnityEngine;
using UnityEngine.UI;


public class SkillDirectorModule
{
    BattleCtrl battleCtrl;
    bool selectState;
    private Table.Skill skillConfig;

    public Dictionary<int, SkillDirectorGroup> directorGroupDic;
    SkillDirectorGroup currDirectorGroup;

    public void Init(BattleCtrl ctrl)
    {
        this.battleCtrl = ctrl;
        directorGroupDic = new Dictionary<int, SkillDirectorGroup>();
    }

    public void StartSelect(int skillId, GameObject originGameObject)
    {
        selectState = true;

        //生成投掷物指示器
        if (directorGroupDic.ContainsKey(skillId))
        {
            currDirectorGroup = directorGroupDic[skillId];
        }
        else
        {
            this.skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);

            var pDirector = GenNewDirector(SkillDirectorType.DirectorProjectile);
            var rDirector = GenNewDirector(SkillDirectorType.DirectorReleaserTerminal);
            var tDirector = GenNewDirector(SkillDirectorType.DirectorTargetTerminal);

            SkillDirectorGroup group = new SkillDirectorGroup();
            group.Init(pDirector, rDirector, tDirector);


            directorGroupDic.Add(skillId, group);
            currDirectorGroup = group;
        }

        currDirectorGroup.Show(originGameObject);

        OperateViewManager.Instance.SetCursor(CursorType.Select);


    }

    public BaseSkillDirector GenNewDirector(SkillDirectorType type)
    {
        BaseSkillDirector skillDirector = null;
        if (type == SkillDirectorType.DirectorProjectile)
        {
            skillDirector = new SkillDirectorProjectile();
            skillDirector.Init(skillConfig.SkillDirectorProjectileType, skillConfig.SkillDirectorProjectileParam);
        }
        else if (type == SkillDirectorType.DirectorReleaserTerminal)
        {
            skillDirector = new SkillDirectorTerminal();
            skillDirector.Init(skillConfig.SkillReleaserDirectType, skillConfig.SkillReleaserDirectParam);
        }
        else if (type == SkillDirectorType.DirectorTargetTerminal)
        {
            skillDirector = new SkillDirectorTerminal();
            skillDirector.Init(skillConfig.SkillTargetDirectType, skillConfig.SkillTargetDirectParam);

        }

        return skillDirector;
    }

    public void FinishSelect()
    {
        selectState = false;

        currDirectorGroup.Hide();

        OperateViewManager.Instance.SetCursor(CursorType.Normal);

    }

    public void Update(float deltaTime)
    {
        foreach (var item in directorGroupDic)
        {
            var directorGroup = item.Value;
            directorGroup.Update(deltaTime);
        }
    }

    internal void UpdateMousePosition(Vector3 resultPos)
    {
        foreach (var item in directorGroupDic)
        {
            var directorGroup = item.Value;
            directorGroup.UpdateMousePosition(resultPos);
        }
    }

    public bool GetSelectState()
    {
        return this.selectState;
    }


    public void Release()
    {
        foreach (var item in directorGroupDic)
        {
            item.Value.Release();
        }
    }


}
