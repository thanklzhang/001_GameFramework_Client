using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class HolderItems : ScriptableObject
{
    public List<Object> selectedObject;
}

public class ABPackStrategyEditor : EditorWindow
{
    public ReorderableList renderList;

    private SerializedObject serializeObj;
    private SerializedProperty listProperty;

    [MenuItem("Tools/ABPackStrategyEditor")]
    public static void ShowWindow()
    {
        GetWindow<ABPackStrategyEditor>("ABPackStrategyEditor");
    }

    public void OnEnable()
    {
        var resPath = Const.ABPackageStrategyPath;
        var strategySO =
            AssetDatabase.LoadAssetAtPath<ABPackageStrategySO>(resPath);
        if (null == strategySO)
        {
            strategySO = ScriptableObject.CreateInstance<ABPackageStrategySO>();
            //ǰһ����������Դֻ�Ǵ����ڴ��У�����Ҫ�������浽����
            //ͨ���༭��API������һ��������Դ�ļ����ڶ�������Ϊ��Դ�ļ���AssetsĿ¼�µ�·��
            AssetDatabase.CreateAsset(strategySO, resPath);
            //���洴������Դ
            AssetDatabase.SaveAssets();
            //ˢ�½���
            AssetDatabase.Refresh();
        }

        serializeObj = new SerializedObject(strategySO);

        listProperty = serializeObj.FindProperty("strategyList");


        renderList = new ReorderableList(serializeObj, listProperty, true, true, true, true);

        renderList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "ABPackStrategyEditor");

        renderList.elementHeightCallback = (index) =>
        {
            return EditorGUIUtility.singleLineHeight + 10;
        };
        renderList.footerHeight = 10;
        renderList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var element = listProperty.GetArrayElementAtIndex(index);
          
            var pathProperty = element.FindPropertyRelative("path");
            var typeProperty = element.FindPropertyRelative("type");
            //
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.width = 200;
            
            EditorGUI.PropertyField(rect, pathProperty, GUIContent.none);
            rect.x += 250;
            EditorGUI.PropertyField(rect, typeProperty, GUIContent.none);
          
        };
      
        renderList.onAddCallback = list => { ReorderableList.defaultBehaviours.DoAddButton(list); };
        renderList.onRemoveCallback = list => { ReorderableList.defaultBehaviours.DoRemoveButton(list); };
    }
    public Vector2 fileCatalogueScrollPos;
    private void OnGUI()
    {
       
        //serializeObj.Update();

        fileCatalogueScrollPos = 
            GUILayout.BeginScrollView(fileCatalogueScrollPos, false, true, new GUILayoutOption[] { });
        renderList.DoLayoutList();
        GUILayout.EndScrollView();

        serializeObj.ApplyModifiedProperties();
    }
}