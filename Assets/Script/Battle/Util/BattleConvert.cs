﻿using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetProto;
using UnityEngine;

public class BattleConvert
{
    public static Vector3Proto ConvertToVector3Proto(Vector3 position)
    {
        Vector3Proto v3Proto = new Vector3Proto();
        v3Proto.X = ToValue(position.x);
        v3Proto.Y = ToValue(position.y);
        v3Proto.Z = ToValue(position.z);
        return v3Proto;

    }

    public static Vector3 ConverToVector3(Vector3Proto vector3Proto)
    {
        var x = GetValue(vector3Proto.X);
        var y = GetValue(vector3Proto.Y);
        var z = GetValue(vector3Proto.Z);
        Vector3 vector3 = new Vector3(x, y, z);
        return vector3;
    }

    public static int ToValue(float value)
    {
        int v = (int)(value * 1000);
        return v;
    }

    public static float GetValue(int value)
    {
        return value / 1000.0f;
    }
}
