using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(MapDataGenerateTool))]
public class MapDataGenerateToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("生成地图碰撞文件"))
        {
            GenCollsionInfo();
        }

        if (GUILayout.Button("清理地图碰撞文件"))
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


