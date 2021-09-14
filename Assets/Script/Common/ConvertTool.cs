//using FixedPointy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//public class ConvertTool
//{

//    public static Vector3Proto ToVector3Proto(Vector3 vector3)
//    {
//        Vector3Proto v = new Vector3Proto();
//        v.X = (int)(vector3.x * 1000.0f);
//        v.Y = (int)(vector3.y * 1000.0f);
//        v.Z = (int)(vector3.z * 1000.0f);

//        return v;
//    }

//    public static Vector3 ToVector3(Vector3Proto vDto)
//    {
//        Vector3 v = new Vector3();
//        v.x = vDto.X / 1000.0f;
//        v.y = vDto.Y / 1000.0f;
//        v.z = vDto.Z / 1000.0f;

//        return v;
//    }

//    //public static QuaternionDTO ToQuaternionDTO(Quaternion qua)
//    //{

//    //    QuaternionDTO q = new QuaternionDTO();
//    //    q.X = qua.x;
//    //    q.Y = qua.y;
//    //    q.Z = qua.z;
//    //    q.W = qua.w;

//    //    return q;
//    //}


//    //public static Vector3 ToVector3(FixVec3 vec3)
//    //{
//    //    //这里的转换可以之后换成在 Vector3 类中 或者在 FixVec3 中增加扩展函数
//    //    Vector3 vector3 = new Vector3((float)vec3.X, (float)vec3.Y, (float)vec3.Z);
//    //    return vector3;
//    //}


//}

