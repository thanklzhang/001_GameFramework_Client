﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Linq;
//using System.Text;
//using LitJson;
//using System;
//using PlotDesigner.Runtime;

//namespace PlotDesigner.Editor
//{
//    [CustomEditor(typeof(TrackNodeSO),true)]
//    public class AnimationTrackNodeSOEditor : TrackNodeSOEditor
//    {
//        SerializedObject obj;
//        //SerializedProperty trackNode;
        
//        public override void OnInspectorGUI()
//        {

//            //obj = new SerializedObject(target);
//            ////trackNode = obj.FindProperty("trackNode");

//            ////base
//            //DrawPropertiesExcluding(obj);

//            ////sub
//            //var so = target as TrackNodeSO;
//            //if (so.trackNode is AnimationTrackNode)
//            //{
//            //    var ani = (AnimationTrackNode)so.trackNode;
//            //    EditorGUILayout.BeginVertical();

//            //    EditorGUILayout.BeginHorizontal();
//            //    EditorGUILayout.LabelField("aniName");
//            //    ani.aniName = EditorGUILayout.TextField(ani.aniName);
//            //    EditorGUILayout.EndHorizontal();

//            //    EditorGUILayout.BeginHorizontal();
//            //    EditorGUILayout.LabelField("isLoop");
//            //    ani.isLoop = EditorGUILayout.Toggle(ani.isLoop);
//            //    EditorGUILayout.EndHorizontal();

//            //    EditorGUILayout.EndVertical();
//            //}

//            //obj.ApplyModifiedProperties();
//        }
//    }
//}