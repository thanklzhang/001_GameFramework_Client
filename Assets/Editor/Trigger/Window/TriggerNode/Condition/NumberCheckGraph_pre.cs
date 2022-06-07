//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Linq;
//using System.Text;
//using LitJson;
//using System;
//using System.Reflection;
//using PlotDesigner.Runtime;

//namespace BattleTrigger.Editor
//{
//    public class NumberCheckGraph : ConditionCheckNodeGraph
//    {
//        float a;
//        ConditionCompareType op;
//        float b;
        
//        public override void OnParse(JsonData nodeJsonData)
//        {
//            base.OnParse(nodeJsonData);

//            a = (float.Parse(nodeJsonData["a"]["value"].ToString()));
//            op = (ConditionCompareType)(int.Parse(nodeJsonData["op"]["value"].ToString()));
//            b = (float.Parse(nodeJsonData["b"]["value"].ToString()));
//        }

//        public override void OnCreate()
//        {
//            base.OnCreate();

           

//        }

//        public override string GetDrawContentStr()//Rect childRect
//        {
//            var str = string.Format("如果 {0} {1} {2}  ", a, ConditionCheckNodeGraph.GetCompareTypeStr(op), b);
//            return str;

//        }
     

//        public override void DrawSelectInfo()
//        {
//            var style = new GUIStyle();
//            style.stretchWidth = false;
//            style.normal.textColor = Color.white;

//            GUILayout.BeginHorizontal();
//            GUILayout.Label("如果 ", style);
//            a = EditorGUILayout.FloatField(a, new GUILayoutOption[] { GUILayout.Width(40) });
//            op = (ConditionCompareType)EditorGUILayout.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(100) });
//            b = EditorGUILayout.FloatField(b, new GUILayoutOption[] { GUILayout.Width(40) });
//            GUILayout.EndHorizontal();
//        }

//        public override TriggerNodeGraph OnClone()
//        {
//            NumberCheckGraph node = new NumberCheckGraph();

//            node.aExecuteGroup = this.aExecuteGroup.Clone();
//            node.bExecuteGroup = this.bExecuteGroup.Clone();
//            node.aShowGraph = this.aShowGraph.Clone();
//            node.bShowGraph = this.bShowGraph.Clone();

//            node.a = this.a;
//            node.op = this.op;
//            node.b = this.b;

//            return node;
//        }

//        public override JsonData OnToJson(JsonData jd)
//        {
//            var currJd = base.OnToJson(jd);
//            currJd["a"] = new JsonData();
//            currJd["a"]["value"] = a;
//            currJd["op"] = new JsonData();
//            currJd["op"]["value"] = (int)op;
//            currJd["b"] = new JsonData();
//            currJd["b"]["value"] = b;

//            return jd;
//        }

      
//    }
//}