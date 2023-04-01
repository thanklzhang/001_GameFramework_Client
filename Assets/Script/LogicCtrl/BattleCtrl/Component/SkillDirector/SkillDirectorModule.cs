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

    public Dictionary<int, SkillDirector> directorDic;
    SkillDirector currDirector;

    public void Init(BattleCtrl ctrl)
    {
        this.battleCtrl = ctrl;
        directorDic = new Dictionary<int, SkillDirector>();
    }

    public void StartSelect(int skillId, GameObject originGameObject)
    {
        selectState = true;

        //生成指示器范围
        if (directorDic.ContainsKey(skillId))
        {
            currDirector = directorDic[skillId];
        }
        else
        {
            this.skillConfig = Table.TableManager.Instance.GetById<Table.Skill>(skillId);

            var skillDirector = new SkillDirector();
            skillDirector.Init((SkillDirectorType)skillConfig.SkillDirectorType, skillConfig.SkillDirectorParam);
            directorDic.Add(skillId, skillDirector);
            currDirector = skillDirector;
        }

        currDirector.Show(originGameObject);

        //生成释放者指示

        //生成目标指示

    }

    public void FinishSelect()
    {
        selectState = false;

        currDirector.Hide();

    }

    public void Update(float deltaTime)
    {
        foreach (var item in directorDic)
        {
            var director = item.Value;
            if (director.IsEnable())
            {
                director.Update(deltaTime);
            }
        }
    }


    public bool GetSelectState()
    {
        return this.selectState;
    }


    public void Release()
    {
        foreach (var item in directorDic)
        {
            item.Value.Release();
        }
    }

}
