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
    public enum ConditionOperateType
    {
        Plus = 0,
        Minus = 1,
        Multi = 2,
        Divide = 3
    }

    public enum ConditionCompareType
    {
        Equal = 0,
        NotEqual = 1,
        Less = 2,
        LessEqual = 3,
        Greater = 4,
        GreaterEqual = 5
    }

    public class ConditionCheckNodeGraph : TriggerNodeGraph
    {
        public TriggerGroupGraph aExecuteGroup;
        public TriggerGroupGraph bExecuteGroup;

        public TriggerNodeGraph aShowGraph;
        public TriggerNodeGraph bShowGraph;

        public bool isCollapse;

        public static string GetCompareTypeStr(ConditionCompareType compareType)
        {
            string str = "";
            if (compareType == ConditionCompareType.Equal)
            {
                str = "等于";
            }
            else if (compareType == ConditionCompareType.NotEqual)
            {
                str = "不等于";
            }
            else if (compareType == ConditionCompareType.Less)
            {
                str = "小于";
            }
            else if (compareType == ConditionCompareType.LessEqual)
            {
                str = "小于等于";
            }
            else if (compareType == ConditionCompareType.Greater)
            {
                str = "大于";
            }
            else if (compareType == ConditionCompareType.GreaterEqual)
            {
                str = "大于等于";
            }

            return str;
        }

        public override void Draw()//Rect childRect
        {
            base.Draw();
            if (!isCollapse)
            {
                aShowGraph.Draw();
                aExecuteGroup.Draw(new Rect());
                GUILayout.Space(2);
                bShowGraph.Draw();
                bExecuteGroup.Draw(new Rect());
            }
        }

        public override void OnParse(JsonData nodeJsonData)
        {
            var aGroup = nodeJsonData["aExecuteGroup"];
            var aNodeGraphList = TriggerWindow.instance.ParseTriggerNodeList(aGroup, this.floor + 1);
            aExecuteGroup = new TriggerGroupGraph();
            aExecuteGroup.Init(aNodeGraphList, this.floor + 1);

            var bGroup = nodeJsonData["bExecuteGroup"];
            var bNodeGraphList = TriggerWindow.instance.ParseTriggerNodeList(bGroup, this.floor + 1);
            bExecuteGroup = new TriggerGroupGraph();
            bExecuteGroup.Init(bNodeGraphList, this.floor + 1);


            aShowGraph = new AssShowStrGraph();
            aShowGraph.Create(this.floor + 1);
            aShowGraph.groupGraph = this.aExecuteGroup;
            aShowGraph.SetShowStr("那么-------------");

            bShowGraph = new AssShowStrGraph();
            bShowGraph.Create(this.floor + 1);
            bShowGraph.groupGraph = this.bExecuteGroup;
            bShowGraph.SetShowStr("否则-------------");
        }

        public override void OnCreate()
        {
            aExecuteGroup = new TriggerGroupGraph();
            var aNodeGraphList = new List<TriggerNodeGraph>();
            aExecuteGroup.Init(aNodeGraphList, this.floor + 1);

            bExecuteGroup = new TriggerGroupGraph();
            var bNodeGraphList = new List<TriggerNodeGraph>();
            bExecuteGroup.Init(bNodeGraphList, this.floor + 1);

            aShowGraph = new AssShowStrGraph();
            aShowGraph.Create(this.floor + 1);
            aShowGraph.groupGraph = this.aExecuteGroup;
            aShowGraph.SetShowStr("那么-------------");

            bShowGraph = new AssShowStrGraph();
            bShowGraph.Create(this.floor + 1);
            bShowGraph.groupGraph = this.bExecuteGroup;
            bShowGraph.SetShowStr("否则-------------");
        }

        //public override TriggerNodeGraph OnClone()
        //{
        //    ConditionCheckNodeGraph node = new ConditionCheckNodeGraph();
        //    node.aExecuteGroup = this.aExecuteGroup.Clone();
        //    node.bExecuteGroup = this.bExecuteGroup.Clone();
        //    node.aShowGraph = this.aShowGraph.Clone();
        //    node.bShowGraph = this.bShowGraph.Clone();
        //    return node;
        //}

        public override JsonData OnToJson(JsonData jd)
        {
            jd["aExecuteGroup"] = aExecuteGroup.ToJson();
            jd["bExecuteGroup"] = bExecuteGroup.ToJson();

            return jd;
        }

        public override void SetFloorIncludeChildren(int floor)
        {
            this.floor = floor;

            aExecuteGroup.SetFloor(this.floor + 1);
            bExecuteGroup.SetFloor(this.floor + 1);

            aShowGraph.SetFloorIncludeChildren(this.floor + 1);
            bShowGraph.SetFloorIncludeChildren(this.floor + 1);
        }

        public override void DrawLeft()
        {
            var style = new GUIStyle();
            style.normal.background = Texture2D.grayTexture;
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = new RectOffset();
            var sign = isCollapse ? "+" : "-";
            if (GUILayout.Button(sign, style, new GUILayoutOption[] { GUILayout.Height(14), GUILayout.Width(14) }))
            {
                isCollapse = !isCollapse;
            }
            GUILayout.Space(6);
        }

    }


}