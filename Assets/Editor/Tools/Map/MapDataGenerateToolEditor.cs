using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MapDataGenerateTool))]
public class MapDataGenerateToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("���ɵ�ͼ�ļ�"))
        {
            GenMapInfo();
        }

        if (GUILayout.Button("�����ͼ��ײ�ļ�"))
        {
            var tool = (MapDataGenerateTool)this.target;
            tool.ClearMap();
        }

        base.OnInspectorGUI();
    }

    void GenMapInfo()
    {
        var tool = (MapDataGenerateTool)this.target;
        tool.GenMapInfo();
    }
}


