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
//    public class CalculateExpressionVar : NumberVar
//    {
//        public NumberVar a;
//        public CalculateVarType opType;
//        public NumberVar b;

//        public override float Get()
//        {
//            float result = 0.0f;
//            if (opType == CalculateVarType.Plus)
//            {
//                result = a.Get() + b.Get();
//            }
//            else if (opType == CalculateVarType.Minus)
//            {
//                result = a.Get() - b.Get();
//            }
//            else if (opType == CalculateVarType.Multi)
//            {
//                result = a.Get() - b.Get();
//            }
//            else if (opType == CalculateVarType.Divide)
//            {
//                result = a.Get() - b.Get();
//            }

//            return result;
//        }

//        public override void OnParse(JsonData nodeJsonData)
//        {
//            a = ParseNumberValue(nodeJsonData["a"]);
//            opType = (CalculateVarType)int.Parse(nodeJsonData["opType"].ToString());
//            b = ParseNumberValue(nodeJsonData["b"]);
//        }

//        public override void OnCreate()
//        {
//            a = new FloatFixedVar();
//            a.Create();

//            b = new FloatFixedVar();
//            b.Create();
//        }

//        public override JsonData OnToJson(JsonData jd)
//        {
//            jd["a"] = a.ToJson();
//            jd["opType"] = (int)opType;
//            jd["b"] = b.ToJson();
//            return jd;
//        }

//        public override NumberVar OnClone()
//        {
//            CalculateExpressionVar numVar = new CalculateExpressionVar();
//            numVar.a = this.a.Clone();
//            numVar.opType = this.opType;
//            numVar.b = this.b.Clone();
//            return numVar;
//        }

//        public override string GetDrawContentStr()
//        {
//            var str = "";
//            var aShowStr = a.GetDrawContentStr();
//            var opStr = NumberVar.GetCompareTypeStr(this.opType);
//            var bShowStr = b.GetDrawContentStr();

//            str = string.Format("( {0} {1} {2} )",aShowStr,opStr,bShowStr);

//            return str;
//        }

//        public override void DrawSelectInfo()
//        {
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("(");
//            a.DrawSelectInfo();
//            GUILayout.EndHorizontal();
            
//            opType = (CalculateVarType)EditorGUILayout.EnumPopup(opType, new GUILayoutOption[] { GUILayout.Width(100) });

//            GUILayout.BeginHorizontal();
//            b.DrawSelectInfo();
//            GUILayout.Label(")");
//            GUILayout.EndHorizontal();
//        }
//    }
//}