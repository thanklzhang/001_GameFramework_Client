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
//    public enum Number_GetValueFromType
//    {
//        FloatFixed = 0,
//        IntFixed = 1,
//        CalculateExpression = 2,
//        UnitAttr = 10,
//    }

//    public class Number_ConditionCheck : ConditionCheck
//    {
//        Number_GetValueFromType aGetType;
//        NumberVar a;
//        ConditionCompareType op;
//        Number_GetValueFromType bGetType;
//        NumberVar b;

//        public override void OnParse(JsonData nodeJsonData)
//        {
//            a = NumberVar.ParseNumberValue(nodeJsonData["a"]);
//            op = (ConditionCompareType)(int.Parse(nodeJsonData["op"].ToString()));
//            b = NumberVar.ParseNumberValue(nodeJsonData["b"]);
//            aGetType = (Number_GetValueFromType)(int.Parse(nodeJsonData["aGetType"].ToString()));
//            bGetType = (Number_GetValueFromType)(int.Parse(nodeJsonData["bGetType"].ToString()));
//        }

//        public override void OnCreate()
//        {
//            a = new FloatFixedVar();
//            a.Create();
//            op = ConditionCompareType.Equal;
//            b = new FloatFixedVar();
//            b.Create();
//            aGetType = Number_GetValueFromType.FloatFixed;
//            bGetType = Number_GetValueFromType.FloatFixed;
//        }

//        public override string GetDrawContentStr()//Rect childRect
//        {
//            var str = string.Format("如果 {0} {1} {2}  ", a.GetDrawContentStr(), ConditionNodeGraph.GetCompareTypeStr(op), b.GetDrawContentStr());
//            return str;
//        }
//        public override void DrawSelectInfo()
//        {
//            var style = new GUIStyle();
//            style.stretchWidth = false;
//            style.normal.textColor = Color.white;

//            aGetType = (Number_GetValueFromType)EditorGUILayout.EnumPopup(aGetType, new GUILayoutOption[] { GUILayout.Width(100) });

//            //A 根据选择的 numberGetType 来进行各自的输入显示
//            var aNumberType = GetNumberClassType(aGetType);
//            if (null != aNumberType)
//            {
//                //if (!a.GetType().IsSubclassOf(numberType))
//                if (aNumberType != a.GetType())
//                {
//                    a = null;
//                    a = Activator.CreateInstance(aNumberType) as NumberVar;
//                    a.Create();
//                }
//            }

//            a.DrawSelectInfo();

//            op = (ConditionCompareType)EditorGUILayout.EnumPopup(op, new GUILayoutOption[] { GUILayout.Width(100) });

//            bGetType = (Number_GetValueFromType)EditorGUILayout.EnumPopup(bGetType, new GUILayoutOption[] { GUILayout.Width(100) });

//            //B 根据选择的 numberGetType 来进行各自的输入显示
//            var bNumberType = GetNumberClassType(bGetType);
//            if (null != bNumberType)
//            {
//                //if (!b.GetType().IsSubclassOf(bNumberType))
//                if (bNumberType != b.GetType())
//                {
//                    b = null;
//                    b = Activator.CreateInstance(bNumberType) as NumberVar;
//                    b.Create();
//                }
//            }

//            b.DrawSelectInfo();
//        }

//        public Type GetNumberClassType(Number_GetValueFromType enumType)
//        {
//            var enumName = enumType.ToString();
//            Logx.Log("aEnumName:" + enumName);
//            var enumfullName = NameSpaceName + "." + enumName + "Var";
//            var numberType = Type.GetType(enumfullName);
//            if (null == numberType)
//            {
//                Logx.LogError("the type of numberVar is not found : " + enumfullName);
//                return null;
//            }
//            return numberType;
//        }

//        public override ConditionCheck OnClone()
//        {
//            Number_ConditionCheck judge = new Number_ConditionCheck();
//            judge.a = this.a.Clone();
//            judge.op = this.op;
//            judge.b = this.b.Clone();

//            return judge;
//        }

//        public override JsonData OnToJson()
//        {
//            JsonData jd = new JsonData();
//            var fullTypeName = this.GetType().ToString();
//            var splits = fullTypeName.Split('.');
//            var typeName = splits[splits.Length - 1];

//            jd["__TYPE__"] = typeName;
//            jd["aGetType"] = (int)this.aGetType;
//            jd["a"] = a.ToJson();
//            jd["op"] = (int)op;
//            jd["bGetType"] = (int)this.bGetType;
//            jd["b"] = b.ToJson();
//            return jd;
//        }
//    }
//}