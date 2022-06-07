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
//    public enum NumberCheckType
//    {
//        Int,
//        Float,
//        Bool
//    }

//    public class NumberCheckGraph : ConditionCheckNodeGraph
//    {
//        NumberVar a;
//        ConditionCompareType op;
//        NumberVar b;



//        Int_GetValueFromType int_from_type;

//        public override void OnParse(JsonData nodeJsonData)
//        {
//            base.OnParse(nodeJsonData);

//            a = NumberVar.ParseNumberValue(nodeJsonData["a"]);
//            op = (ConditionCompareType)(int.Parse(nodeJsonData["op"].ToString()));
//            b = NumberVar.ParseNumberValue(nodeJsonData["b"]);

//            if (a is IntVar)
//            {
//                int_from_type = Int_GetValueFromType.Fixed;
//            }
//        }

//        public override void OnCreate()
//        {
//            base.OnCreate();

//            a = new IntVar();
//            op = ConditionCompareType.Equal;
//            b = new IntVar();

//            if (a is IntVar)
//            {
//                int_from_type = Int_GetValueFromType.Fixed;
//            }
//        }

//        public override string GetDrawContentStr()//Rect childRect
//        {
//            var str = string.Format("如果 {0} {1} {2}  ", a.GetShowStr(), ConditionCheckNodeGraph.GetCompareTypeStr(op), b.GetShowStr());
//            return str;
//        }

//        public override void DrawSelectInfo()
//        {
//            //var style = new GUIStyle();
//            //style.stretchWidth = false;
//            //style.normal.textColor = Color.white;

//            //GUILayout.BeginHorizontal();
//            //GUILayout.Label("如果 ", style);
//            //a = EditorGUILayout.FloatField(a, new GUILayoutOption[] { GUILayout.Width(40) });
//            //op = (ConditionCompareType)EditorGUILayout.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(100) });
//            //b = EditorGUILayout.FloatField(b, new GUILayoutOption[] { GUILayout.Width(40) });
//            //GUILayout.EndHorizontal();



//            var style = new GUIStyle();
//            style.stretchWidth = false;
//            style.normal.textColor = Color.white;


//            DrawAGetValueFrom();
//            a.DrawSelectInfo();
//            op = (ConditionCompareType)EditorGUILayout.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(100) });
//            b.DrawSelectInfo();

//            //GUILayout.BeginHorizontal();
//            //GUILayout.Label("如果 ", style);
//            //a = EditorGUILayout.FloatField(a, new GUILayoutOption[] { GUILayout.Width(40) });
//            //op = (ConditionCompareType)EditorGUILayout.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(100) });
//            //b = EditorGUILayout.FloatField(b, new GUILayoutOption[] { GUILayout.Width(40) });
//            //GUILayout.EndHorizontal();

//        }



//        public void DrawAGetValueFrom()
//        {
//            if (a is IntVar)
//            {
//                int_from_type = (Int_GetValueFromType)EditorGUILayout.EnumPopup(int_from_type new GUILayoutOption[] { GUILayout.Width(100) });
//            }

//        }

//        public override TriggerNodeGraph OnClone()
//        {
//            NumberCheckGraph node = new NumberCheckGraph();

//            node.aExecuteGroup = this.aExecuteGroup.Clone();
//            node.bExecuteGroup = this.bExecuteGroup.Clone();
//            node.aShowGraph = this.aShowGraph.Clone();
//            node.bShowGraph = this.bShowGraph.Clone();

//            node.a = this.a.Clone();
//            node.op = this.op;
//            node.b = this.b.Clone();

//            return node;
//        }

//        public override JsonData OnToJson(JsonData jd)
//        {
//            jd["a"] = a.ToJson();
//            jd["op"] = (int)op;
//            jd["b"] = b.ToJson();

//            base.OnToJson(jd);

//            return jd;
//        }


//    }
//}