//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(ProceduralTextureGeneration))]
//public class ProgramTextureEditor : Editor
//{
//    SerializedObject obj;
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        EditorGUILayout.BeginVertical();
//        if (GUILayout.Button("refresh"))
//        {
//            var t = target as ProceduralTextureGeneration;
//            t._UpdateMaterial();

//            EditorUtility.SetDirty(target);

//        }
//        EditorGUILayout.EndVertical();


//    }
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
