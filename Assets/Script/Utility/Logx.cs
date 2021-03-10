using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

//过后抽象出来
public class Logx
{
    public static void Log(object obj)
    {
        Debug.Log(obj);
    }

    public static void LogWarning(object obj)
    {
        Debug.LogWarning(obj);
    }

    public static void LogError(object obj)
    {
        Debug.LogError(obj);
    }

    public static void Log(string flag, object obj)
    {
        Debug.Log(flag + ":" + obj);
    }

    public static void Log(string flag, string flag2, object obj)
    {
        Debug.Log(flag + ":" + flag2 + ":" + obj);
    }
    public static void LogWarning(string flag, string flag2, object obj)
    {
        Debug.LogWarning(flag + ":" + flag2 + ":" + obj);
    }
    public static void LogError(string flag, string flag2, object obj)
    {
        Debug.LogError(flag + ":" + flag2 + ":" + obj);
    }

    //-------------
    //Custom
    public static void Logz(string obj)
    {
        Log("zxy", obj);
    }
    public static void LogzError(string obj)
    {
        LogErrorZxy("zxy", obj);
    }
    public static void LogZxy(string flag2, object obj)
    {
        Log("zxy", flag2, obj);
    }
    public static void LogWarningZxy(string flag2, object obj)
    {
        LogWarning("zxy", flag2, obj);
    }
    public static void LogErrorZxy(string flag2, object obj)
    {
        LogError("zxy", flag2, obj);
    }

    //[UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
    //static bool OnOpenAsset(int instanceID, int line)
    //{
    //    string stackTrace = GetStackTrace();
    //    if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("zxy:"))
    //    {
    //        System.Text.RegularExpressions.Match matches = System.Text.RegularExpressions.Regex.Match(stackTrace, @"\(at (.+)\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    //        string pathLine = "";
    //        while (matches.Success)
    //        {
    //            pathLine = matches.Groups[1].Value;
    //            Debug.Log("zxy : " + pathLine);

    //            if (!pathLine.Contains("Logx.cs"))
    //            {
    //                int splitIndex = pathLine.LastIndexOf(":");
    //                string path = pathLine.Substring(0, splitIndex);
    //                line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
    //                string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
    //                fullPath = fullPath + path;
    //                var resultPath = fullPath.Replace('/', '\\');
    //                var isSuccess = UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(resultPath, line);
    //                Debug.Log(resultPath + " " + isSuccess + " " + line);
    //                break;
    //            }
    //            matches = matches.NextMatch();
    //        }
    //        return true;
    //    }
    //    return false;
    //}

    //static string GetStackTrace()
    //{
    //    var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
    //    var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
    //    var consoleInstance = fieldInfo.GetValue(null);
    //    if (consoleInstance != null)
    //    {
    //        if ((object)UnityEditor.EditorWindow.focusedWindow == consoleInstance)
    //        {
    //            var ListViewStateType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ListViewState");
    //            fieldInfo = ConsoleWindowType.GetField("m_ListView", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
    //            var listView = fieldInfo.GetValue(consoleInstance);
    //            fieldInfo = ListViewStateType.GetField("row", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
    //            int row = (int)fieldInfo.GetValue(listView);
    //            fieldInfo = ConsoleWindowType.GetField("m_ActiveText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
    //            string activeText = fieldInfo.GetValue(consoleInstance).ToString();
    //            return activeText;
    //        }
    //    }
    //    return null;
    //}


}
