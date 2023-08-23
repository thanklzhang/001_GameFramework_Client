using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;


public enum ABStrategyType
{
    Null = 0,
    //Ŀ¼�����е��ļ� �������в����ļ��� ÿ���ļ�һ����
    OneByOneFile = 1,

    //��Ŀ¼�����ļ����һ����
    AllInOneByFolder = 2,

    //��Ŀ¼����һ���ļ��д��һ����
    AllInOneBySubFolder = 3,
}


[System.Serializable]
public class ABStrategyOption
{
    public Object path;
    public ABStrategyType type;
}

public class ABPackageStrategySO : ScriptableObject
{
     public List<ABStrategyOption> strategyList;
}