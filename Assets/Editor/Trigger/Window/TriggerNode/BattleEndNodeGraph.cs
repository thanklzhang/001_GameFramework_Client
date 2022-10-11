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
    public class BattleEndNodeGraph : TriggerNodeGraph
    {
        public NumberVarField winTeam;

        public override void OnParse(JsonData nodeJsonData)
        {
            //delayTime = (float.Parse(nodeJsonData["delayTime"]["value"].ToString()));
            winTeam = NumberVarField.ParseNumberVarField(nodeJsonData["winTeam"]);
        }

        public override void OnCreate()
        {
            winTeam = new NumberVarField();
            winTeam.Create();
        }

        public override string GetDrawContentStr()//Rect childRect
        {
            var str = "队伍 " + winTeam.GetDrawContentStr() + " 获胜";
            return str;
        }

        public override void DrawSelectInfo()
        {
            var style = new GUIStyle();
            style.stretchWidth = false;
            style.normal.textColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("队伍 ", style);

            winTeam.DrawSelectInfo();

            GUILayout.Label(" 获胜", style);
            GUILayout.EndHorizontal();
        }

        public override TriggerNodeGraph OnClone()
        {
            TriggerNodeGraph node = new BattleEndNodeGraph();
            var _node = node as BattleEndNodeGraph;
            _node.winTeam = (NumberVarField)this.winTeam.Clone();

            return _node;
        }

        public override JsonData OnToJson(JsonData jd)
        {
            jd["winTeam"] = winTeam.ToJson();
            return jd;
        }

    }
}