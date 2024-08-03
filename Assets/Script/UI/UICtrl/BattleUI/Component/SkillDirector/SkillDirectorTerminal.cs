using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Table;
using UnityEngine;
using UnityEngine.UI;
public enum SkillDirectorTerminalType
{
    Null = 0,
    RectangleRotate = 1,
    Circle = 2
}

public enum SkillDirectorRotateType
{
    Null = 0,
    //旋转到 释放者到目标的方向
    ReleaserToTarget = 1
}

//终端指示器 适用于施法者指示和目标指示
public class SkillDirectorTerminal : BaseSkillDirector
{
    string param;
    //矩形范围参数
    float rectXWidth;
    float rectZWidth;
    SkillDirectorRotateType rotateType;

    //圆形范围参数
    float radius;

    SkillDirectorTerminalType skillDirectorType;

    public override void OnInit(int _skillDirectorType, string param)
    {
        this.skillDirectorType = (SkillDirectorTerminalType)_skillDirectorType;
        this.param = param;

        //parse param
        if (skillDirectorType == SkillDirectorTerminalType.RectangleRotate)
        {
            var paramsStrs = param.Split(',');
            this.resourceId = int.Parse(paramsStrs[0]);
            this.rectXWidth = int.Parse(paramsStrs[1]) / 1000.0f;
            this.rectZWidth = int.Parse(paramsStrs[2]) / 1000.0f;
            this.rotateType = (SkillDirectorRotateType)int.Parse(paramsStrs[3]);
        }
        else if (skillDirectorType == SkillDirectorTerminalType.Circle)
        {
            var paramsStrs = param.Split(',');
            this.resourceId = int.Parse(paramsStrs[0]);
            this.radius = int.Parse(paramsStrs[1]) / 1000.0f;
        }
    }

    public override void OnShow()
    {
        //if (this.rotateType == SkillDirectorRotateType.Null)
        //{

        //}
        var pre = transform.localScale;

        if (skillDirectorType == SkillDirectorTerminalType.RectangleRotate)
        {
            transform.localScale = new Vector3(this.rectXWidth, pre.y, this.rectZWidth);
        }
        else if (skillDirectorType == SkillDirectorTerminalType.Circle)
        {
            transform.localScale = new Vector3(this.radius * 2, pre.y, this.radius * 2);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        this.transform.position = followEntity.transform.position;

        if (this.rotateType == SkillDirectorRotateType.Null)
        {

        }
        else if (this.rotateType == SkillDirectorRotateType.ReleaserToTarget)
        {
            //旋转到 释放者到目标的方向
            var mousePosition = mousePositionOnGround;
            Vector3 toMouseDir = mousePosition - this.transform.position;
            toMouseDir = new Vector3(toMouseDir.x, 0, toMouseDir.z);
            this.transform.forward = toMouseDir;
        }
    }


}
