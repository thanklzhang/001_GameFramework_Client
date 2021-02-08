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
    public static void LogZxy(string flag2, object obj)
    {
        Log("zxy", flag2, obj);
    }
    public static void LogZxyWarning(string flag2, object obj)
    {
        LogWarning("zxy", flag2, obj);
    }
    public static void LogZxyError(string flag2, object obj)
    {
        LogError("zxy", flag2, obj);
    }
}
