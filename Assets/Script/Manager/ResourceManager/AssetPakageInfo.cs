//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;


//public class AssetPakageInfo
//{
//    public string path = null;

//    public List<string> dependencies = new List<string>();

//    public List<string> beDependencies = new List<string>();

//    public void AddDependence(string path)
//    {
//        if (dependencies.Contains(path))
//        {
//            Debug.LogError("zxy : AddDependence : has exist path : " + path);
//            return;
//        }
//        dependencies.Add(path);
//    }

//    public void AddBeDependence(string path)
//    {
//        if (beDependencies.Contains(path))
//        {
//            Debug.LogError("zxy : AddBeDependence : has exist path : " + path);
//            return;
//        }
//        beDependencies.Add(path);
//    }

//}
