using Google.Protobuf;
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
        v3Proto.X = (int)position.x;
        v3Proto.Y = (int)position.y;
        v3Proto.Z = (int)position.z;
        return v3Proto;

    }

    public static Vector3 ConverToVector3(Vector3Proto vector3Proto)
    {
        Vector3 vector3 = new Vector3(vector3Proto.X, vector3Proto.Y, vector3Proto.Z);
        return vector3;
    }
}

