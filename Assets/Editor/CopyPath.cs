using UnityEngine;
using UnityEditor;

public static class CopyPath
{
    // Ϊ��Դ�ļ�����Ҽ��˵���
    [MenuItem("Assets/Copy Node Path")]
    private static void CopyAssetPath()
    {
        // ��ȡ��ǰѡ�е���Դ·��
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(path))
        {
            // ���Ƶ�������
            EditorGUIUtility.systemCopyBuffer = path;
            // Debug.Log("Copied Asset Path: " + path);
        }
    }

    // Ϊ�����еĽڵ�����Ҽ��˵���
    [MenuItem("GameObject/Copy Node Path", true)]
    private static bool ValidateCopyNodePath()
    {
        // ����Ƿ���ѡ�е�GameObject
        return Selection.activeGameObject != null;
    }

    [MenuItem("GameObject/Copy Node Path")]
    private static void CopyGameObjectPath()
    {
        // ��ȡ��ǰѡ�е�GameObject
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            // ��ȡGameObject������·��
            string path = GetFullPath(selectedObject.transform);
            // ���Ƶ�������
            EditorGUIUtility.systemCopyBuffer = path;
            // Debug.Log("Copied GameObject Path: " + path);
        }
    }

    // �ݹ��ȡGameObject������·��
    private static string GetFullPath(Transform t)
    {
        if (t.parent == null)
            return t.name;
        return GetFullPath(t.parent) + "/" + t.name;
    }
}