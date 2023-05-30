using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;



//过后抽象出来
public class Logx
{
    public static bool enable = true;
    public static bool isShowFrame = true;
    public enum LogType
    {
        Normal = 1,
        Error = 2,
        Warning = 3,

        Net = 50,
        ClientBattle = 51,

        Zxy = 100,
        AB = 200,
        Asset = 300,
        Key = 400,//流程关键点

    }



    public static void Log(object obj)
    {
        if (!enable)
        {
            return;
        }
        if (isShowFrame)
        {
#if UNITY_EDITOR
            Debug.Log(obj);
#else
            var currBattleFrameNum = GameMain.Instance.currBattleFrameNum;
            Debug.Log("[frame:" + currBattleFrameNum + "] " + obj);
#endif
        }
        else
        {
            Debug.Log(obj);
        }


    }

    public static void LogWarning(object obj)
    {
        //if (!enable)
        //{
        //    return;
        //}
        Debug.LogWarning(obj);
    }

    public static void LogError(object obj)
    {
        if (!enable)
        {
            return;
        }
        Debug.LogError(obj);
    }


    public static void Log(string flag, object obj)
    {
        if (!enable)
        {
            return;
        }

        Log(flag + ":" + obj);
    }
    public static void LogWarning(string flag, object obj)
    {
        LogWarning(flag + ":" + obj);
    }
    public static void LogError(string flag, object obj)
    {
        LogError(flag + ":" + obj);
    }

    public static void LogNet(string obj)
    {
        Log(LogType.Net.ToString(), obj);
    }


    public static void LogBattle(string obj)
    {
        Log(LogType.ClientBattle.ToString(), obj);
    }

    public static void LogException(Exception e)
    {
        Logx.LogError(e.Message + "\n" + e.StackTrace);
    }

    //-------------
    //Custom



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
