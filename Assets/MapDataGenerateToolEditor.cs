using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MapDataGenerateTool))]
public class MapDataGenerateToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("���ɵ�ͼ��ײ�ļ�"))
        {
            GenCollsionInfo();
        }

        if (GUILayout.Button("�����ͼ��ײ�ļ�"))
        {
            var tool = (MapDataGenerateTool)this.target;
            tool.ClearMap();
        }

        base.OnInspectorGUI();
    }

    void GenCollsionInfo()
    {
        var tool = (MapDataGenerateTool)this.target;
        tool.GenCollisionInfo();
    }
}


