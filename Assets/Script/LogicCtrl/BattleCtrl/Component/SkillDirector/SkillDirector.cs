using Battle_Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Table;
using UnityEngine;
using UnityEngine.UI;
public enum SkillDirectorType
{
    Null = 0,
    Rectangle = 1
}

public class SkillDirector
{
    public GameObject gameObject;
    public Transform transform;

    string param;
    //矩形参数
    float rectXWidth;
    float rectZWidth;

    GameObject followEntity;

    int resourceId;

    SkillDirectorType skillDirectorType;

    bool isEnable;
    bool isFinishLoad;

    public void Init(SkillDirectorType skillDirectorType, string param)
    {
        this.skillDirectorType = skillDirectorType;
        this.param = param;

        ParseParam(skillDirectorType, param);
    }

    public void ParseParam(SkillDirectorType skillDirectorType, string param)
    {
        if (skillDirectorType == SkillDirectorType.Rectangle)
        {
            var paramsStrs = param.Split(',');
            this.resourceId = int.Parse(paramsStrs[0]);
            this.rectXWidth = int.Parse(paramsStrs[1]) / 1000.0f;
            this.rectZWidth = int.Parse(paramsStrs[2]) / 1000.0f;
        }
    }

    public void Show(GameObject followGameObject)
    {
        if (isFinishLoad)
        {
            var pre = transform.localScale;
            transform.localScale = new Vector3(this.rectXWidth, pre.y, this.rectZWidth);

            isEnable = true;

            gameObject.SetActive(true);
        }
        else
        {
            this.followEntity = followGameObject;
            StartLoad();
        }
    }

    public void StartLoad()
    {
        isFinishLoad = false;

        ResourceManager.Instance.GetObject<GameObject>(this.resourceId, OnLoadFinish);

    }

    public bool IsEnable()
    {
        return this.isEnable;
    }

    public void OnLoadFinish(GameObject go)
    {
        isFinishLoad = true;
        this.gameObject = go;

        this.transform = this.gameObject.transform;

        this.transform.position = followEntity.transform.position;

        this.Show(this.followEntity);
    }

    public void Update(float deltaTime)
    {
        this.transform.position = followEntity.transform.position;

        //跟随鼠标
        var mousePosition = new Vector3();
        Vector3 toMouseDir = mousePosition - this.transform.position;
        toMouseDir = new Vector3(toMouseDir.x, 0, toMouseDir.z);
        this.transform.forward = toMouseDir;
    }

    public void Hide()
    {
        isEnable = false;
        gameObject.SetActive(false);
    }

    public void Release()
    {
        ResourceManager.Instance.ReturnObject(this.resourceId, this.gameObject);
    }

}
