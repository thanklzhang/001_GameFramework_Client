﻿using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalData
{
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
}

