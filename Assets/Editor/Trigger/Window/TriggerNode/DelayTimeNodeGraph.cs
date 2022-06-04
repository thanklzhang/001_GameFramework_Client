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
    [Serializable]
    public class DelayTimeNodeGraph : TriggerNodeGraph
    {
        public float delayTime;

        public override void OnParse(JsonData nodeJsonData)
        {
            delayTime = (float.Parse(nodeJsonData["delayTime"]["value"].ToString()));
        }

        public override void OnCreate()
        {
            delayTime = 2.00f;
        }

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = "等待 " + delayTime + " 秒";
            return str;
        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("等待", style);
            delayTime = EditorGUILayout.FloatField(delayTime, new GUILayoutOption[] { GUILayout.Width(50) });
            GUILayout.Label("秒", style);
            GUILayout.EndHorizontal();
        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new DelayTimeNodeGraph();
            var delayTimeNode = node as DelayTimeNodeGraph;
            delayTimeNode.delayTime = this.delayTime;

            return delayTimeNode;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["delayTime"] = new JsonData();
            jd["delayTime"]["value"] = delayTime;
            return jd;
        }

    }
}