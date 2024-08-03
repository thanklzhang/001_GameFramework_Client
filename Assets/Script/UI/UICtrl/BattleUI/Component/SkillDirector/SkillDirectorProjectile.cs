using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Table;
using UnityEngine;
using UnityEngine.UI;
public enum SkillDirectorProjectileType
{
    Null = 0,
    Rectangle = 1
}

public class SkillDirectorProjectile : BaseSkillDirector
{
    string param;
    //矩形参数
    float rectXWidth;
    float rectZWidth;

    SkillDirectorProjectileType skillDirectorType;

    public override void OnInit(int _skillDirectorType, string param)
    {
        this.skillDirectorType = (SkillDirectorProjectileType)_skillDirectorType;
        this.param = param;

        ParseParam(skillDirectorType, param);
    }

    public void ParseParam(SkillDirectorProjectileType skillDirectorType, string param)
    {
        if (skillDirectorType == SkillDirectorProjectileType.Rectangle)
        {
            var paramsStrs = param.Split(',');
            this.resourceId = int.Parse(paramsStrs[0]);
            this.rectXWidth = int.Parse(paramsStrs[1]) / 1000.0f;
            this.rectZWidth = int.Parse(paramsStrs[2]) / 1000.0f;
        }
    }

    public override void OnShow()
    {
        var pre = transform.localScale;
        transform.localScale = new Vector3(this.rectXWidth, pre.y, this.rectZWidth);
    }


    public override void OnUpdate(float deltaTime)
    {
        this.transform.position = followEntity.transform.position;

        //跟随鼠标
        var mousePosition = mousePositionOnGround;
        Vector3 toMouseDir = mousePosition - this.transform.position;
        toMouseDir = new Vector3(toMouseDir.x, 0, toMouseDir.z);
        this.transform.forward = toMouseDir;
    }



}
