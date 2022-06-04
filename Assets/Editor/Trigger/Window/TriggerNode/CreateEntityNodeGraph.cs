using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;
using System.Reflection;
using PlotDesigner.Runtime;

namespace BattleTrigger.Editor
{
    public class CreateEntityType
    {
        public int configId;
    }
    [Serializable]
    public class CreateEntityNodeGraph : TriggerNodeGraph
    {
        public int count;
        //public CreateEntityType createEntityType;
        public int configId;
        public Vector3 createPosition;

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = string.Format("创建 {0} 个 '配置 id 为 {1}' 的实体 在 {2} 点上", count, configId, createPosition);
            return str;
        }

        public override void OnParse(JsonData nodeJsonData)
        {

            count = (int.Parse(nodeJsonData["count"]["value"].ToString()));
            configId = (int.Parse(nodeJsonData["entityType"]["configId"].ToString()));

            var x = (float.Parse(nodeJsonData["createPosition"]["x"]["value"].ToString()));
            var y = (float.Parse(nodeJsonData["createPosition"]["y"]["value"].ToString()));
            var z = (float.Parse(nodeJsonData["createPosition"]["z"]["value"].ToString()));
            createPosition = new Vector3(x, y, z);
        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("创建 ", style);
            count = EditorGUILayout.IntField(count, new GUILayoutOption[] { GUILayout.Width(40) });
            GUILayout.Label(" 个", style);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("配置 id 为", style);
            configId = EditorGUILayout.IntField(configId, new GUILayoutOption[] { GUILayout.Width(120) });
            GUILayout.Label("的实体", style);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("在", style);
            createPosition = EditorGUILayout.Vector3Field("", createPosition);
            GUILayout.Label("点上", style);
            GUILayout.EndHorizontal();
        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new CreateEntityNodeGraph();
            var createEntityNode = node as CreateEntityNodeGraph;
            createEntityNode.count = this.count;
            createEntityNode.configId = this.configId;
            createEntityNode.createPosition = this.createPosition;
            return createEntityNode;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["count"] = new JsonData();
            jd["count"]["value"] = count;

            jd["entityType"] = new JsonData();
            jd["entityType"]["configId"] = configId;

            jd["createPosition"] = new JsonData();
            jd["createPosition"]["x"] = new JsonData();
            jd["createPosition"]["x"]["value"] = createPosition.x;
            jd["createPosition"]["y"] = new JsonData();
            jd["createPosition"]["y"]["value"] = createPosition.y;
            jd["createPosition"]["z"] = new JsonData();
            jd["createPosition"]["z"]["value"] = createPosition.z;

            return jd;
        }

    }
}