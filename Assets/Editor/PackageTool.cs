using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;

public class PackageTool
{
    //static List<AssetBundleBuild> bundleList = new List<AssetBundleBuild>();

    [MenuItem("BuildResource/Build iPhone Resource", false, 100)]
    public static void BuildiPhoneResource()
    {
        BuildTarget target;
#if UNITY_5
        target = BuildTarget.iOS;
#else
        target = BuildTarget.iOS;
#endif
        BuildAssetResource(target);
    }

    [MenuItem("BuildResource/Build Android Resource", false, 101)]
    public static void BuildAndroidResource()
    {
        BuildAssetResource(BuildTarget.Android);
    }

    [MenuItem("BuildResource/Build Windows Resource", false, 102)]
    public static void BuildWindowsResource()
    {
        BuildAssetResource(BuildTarget.StandaloneWindows64);
    }

    public static void BuildAssetResource(BuildTarget target)
    {
        StartBuildProgress();
    }

    // public class AssetPakageInfo
    // {
    //     public string path = null;

    //     public List<string> dependencies = new List<string>();

    //     public List<string> beDependencies = new List<string>();

    //     public void AddDependence(string path)
    //     {
    //         if (dependencies.Contains(path))
    //         {
    //             Debug.LogError("zxy : AddDependence : has exist path : " + path);
    //             return;
    //         }
    //         dependencies.Add(path);
    //     }

    //     public void AddBeDependence(string path)
    //     {
    //         if (beDependencies.Contains(path))
    //         {
    //             Debug.LogError("zxy : AddBeDependence : has exist path : " + path);
    //             return;
    //         }
    //         beDependencies.Add(path);
    //     }

    // }
    //public static Dictionary<string, AssetPakageInfo> assetInfoDic = new Dictionary<string, AssetPakageInfo>();

    //public static void CollectAsset(string path)
    //{
    //    if (!Directory.Exists(path))
    //    {
    //        Debug.LogWarning("zxy : CollectAsset : the path doesnt exist : " + path);
    //        return;
    //    }
    //    string[] filePaths = Directory.GetFiles(path);
    //    string[] dirs = Directory.GetDirectories(path);
    //    for (int i = 0; i < filePaths.Length; i++)
    //    {
    //        var filePath = filePaths[i].Replace("\\", "/");
    //        var ext = Path.GetExtension(filePath);
    //        if (IsCanPackageAsset(ext))
    //        {
    //            //AssetPakageInfo info = new AssetPakageInfo();
    //            //info.path = filePath;
    //            //assetInfoDic.Add(filePath, info);
    //        }
    //    }

    //    for (int i = 0; i < dirs.Length; i++)
    //    {
    //        var dirName = dirs[i];
    //        CollectAsset(dirName);
    //    }
    //}


    //public static void CollectDependencies()
    //{
    //    //这里可以进行优化：
    //    //1 去掉多余的依赖 如果资源的依赖中有重复项 比如 A 依赖 B,C  B 依赖 C 那么就可以去掉 A 依赖 C 这条线 
    //    //2 合包策略 只引用一次的资源向上合并
    //    for (int i = 0; i < assetInfoDic.Count; i++)
    //    {
    //        var kv = assetInfoDic.ElementAt(i);
    //        var path = kv.Key;
    //        var info = kv.Value;

    //        var deps = AssetDatabase.GetDependencies(path, false);
    //        foreach (var dep in deps)
    //        {
    //            var ext = Path.GetExtension(dep);
    //            if (IsCanPackageAsset(ext))
    //            {
    //                info.AddDependence(dep);
    //                if (assetInfoDic.ContainsKey(dep))
    //                {
    //                    assetInfoDic[dep].AddBeDependence(path);
    //                }
    //                else
    //                {
    //                    Debug.LogWarning("zxy : dep doesnt exist in info collect : " + dep);
    //                }
    //            }

    //        }
    //    }
    //}

    //public static void GenerateAssetFileData()
    //{
    //    //后缀改为 ab
    //    Dictionary<string, AssetPakageInfo> dic = new Dictionary<string, AssetPakageInfo>();
    //    foreach (var item in assetInfoDic)
    //    {
    //        var resultInfo = ReplaceExtToAB(item.Value);
    //        dic.Add(resultInfo.path, resultInfo);
    //    }
    //    var json = JsonMapper.ToJson(dic);
    //    var configPath = "Assets/StreamingAssets/AssetFileData.json";
    //    if (File.Exists(configPath))
    //    {
    //        File.Delete(configPath);
    //    }
    //    using (var writer = File.CreateText(configPath))
    //    {
    //        writer.Write(json);
    //    }



    //}

    public static void GenerateAssetToAbDicFileData()
    {
        //assetToAbDic
        var json = JsonMapper.ToJson(assetToAbDic);
        var configPath = "Assets/StreamingAssets/AssetToAbFileData.json";
        if (File.Exists(configPath))
        {
            File.Delete(configPath);
        }
        using (var writer = File.CreateText(configPath))
        {
            writer.Write(json);
        }
    }

    public static string ReplacePathPreToAB(string path)
    {
        return path.Replace("Assets/" + Const.buildPath + "/", "");
    }

    //public static AssetPakageInfo ReplaceExtToAB(AssetPakageInfo assetPackInfo)
    //{
    //    {
    //        var ext = Path.GetExtension(assetPackInfo.path);
    //        var abPath = ReplacePathPreToAB(assetPackInfo.path.Replace(ext, Const.ExtName));
    //        assetPackInfo.path = abPath;
    //    }


    //    List<string> deps = new List<string>();
    //    foreach (var item in assetPackInfo.dependencies)
    //    {
    //        var ext = Path.GetExtension(item);
    //        var abPath = ReplacePathPreToAB(item.Replace(ext, Const.ExtName));
    //        deps.Add(abPath);
    //    }
    //    assetPackInfo.dependencies.Clear();
    //    assetPackInfo.dependencies.AddRange(deps);

    //    List<string> beDeps = new List<string>();
    //    foreach (var item in assetPackInfo.beDependencies)
    //    {
    //        var ext = Path.GetExtension(item);
    //        var abPath = ReplacePathPreToAB(item.Replace(ext, Const.ExtName));
    //        beDeps.Add(abPath);
    //    }

    //    assetPackInfo.beDependencies.Clear();
    //    assetPackInfo.beDependencies.AddRange(beDeps);

    //    return assetPackInfo;

    //}
    static Dictionary<string, string> assetToAbDic;
    public static void BuildAssetBundle()
    {
        //assetToAbDic = new Dictionary<string, string>();

        //var bundleBuildList = new List<AssetBundleBuild>();
        //foreach (var assetInfo in assetInfoDic)
        //{
        //    var info = assetInfo.Value;
        //    string path = info.path;

        //    var build = new AssetBundleBuild();

        //    string assetName = path;
        //    //目前用一个文件打成一个包的策略
        //    //-> 去掉 build 前面的路径
        //    var bundleWithPrePath = path.Replace("Assets/" + Const.buildPath + "/", "");
        //    var dir = Path.GetDirectoryName(bundleWithPrePath).Replace("\\", "/");
        //    string abName = Path.GetFileNameWithoutExtension(bundleWithPrePath) + Const.ExtName;
        //    var bundleName = "";
        //    if ("" == dir)
        //    {
        //        bundleName = abName;
        //    }
        //    else
        //    {
        //        bundleName = dir + "/" + abName;
        //    }

        //    build.assetBundleName = bundleName;
        //    build.assetNames = new string[] { assetName };

        //    bundleBuildList.Add(build);

        //    // asset 和 ab 的对应关系
        //    var assetResultName = Path.GetFileNameWithoutExtension(assetName);
        //    //Debug.Log(assetResultName);
        //    assetToAbDic.Add(assetResultName, bundleName);
        //}

        //var outPath = Const.AppStreamingAssetPath;
        //BuildPipeline.BuildAssetBundles(outPath, bundleBuildList.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        //AssetDatabase.Refresh();

        var deletePath = Const.AppStreamingAssetPath;
        FileTool.DeleteAllFile(deletePath);

        PackageStrategy packStrategy = new PackageStrategy();
        var bundleBuildList = packStrategy.StartParse();

        ////收集打包信息
        //assetInfoDic = new Dictionary<string, AssetPakageInfo>();

        //收集 asset ab 对应 
        assetToAbDic = new Dictionary<string, string>();
              
        for (int i = 0; i < bundleBuildList.Count; i++)
        {
            var buildBundle = bundleBuildList[i];
            for (int j = 0; j < buildBundle.assetNames.Length; j++)
            {
                var assetName = buildBundle.assetNames[j];
                if (assetToAbDic.ContainsKey(assetName))
                {
                    Debug.LogWarning("zxy : PackageToole : BuildAssetBundle : the assetName is exist : " + assetName);
                }
                else
                {
                    assetToAbDic.Add(assetName, buildBundle.assetBundleName);
                }
                
            }
        }

        GenerateAssetToAbDicFileData();

        //
        var outPath = Const.AppStreamingAssetPath;
        var abManifest = BuildPipeline.BuildAssetBundles(outPath, bundleBuildList.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        AssetDatabase.Refresh();

        

    }

    public static void StartBuildProgress()
    {
        BuildAssetBundle();
    }
    ////打 AssetBundle 包基本流程
    //public static void StartBuildProgress_pre()
    //{
    //    assetInfoDic?.Clear();

    //    //收集所有资源 目前先按照一个文件一个 ab 策略处理
    //    string assetRootPath = "Assets/" + Const.buildPath;
    //    CollectAsset(assetRootPath);
    //    Debug.Log("zxy : finish collect dependencies");

    //    // foreach (var item in assetInfoDic)
    //    // {
    //    //     Debug.Log("zxy : collect asset : " + item.Key + " -> " + item.Value.path);
    //    // }

    //    //根据收集的资源信息来设置依赖关系
    //    CollectDependencies();
    //    Debug.Log("zxy : finish set dependencies");

    //    // foreach (var item in assetInfoDic)
    //    // {
    //    //     Debug.Log("---start dep zxy :  : " + item.Key + " -> " + item.Value.path);
    //    //     item.Value.dependencies.ToList().ForEach((depName) =>
    //    //     {
    //    //         Debug.Log("zxy : " + depName);
    //    //     });
    //    //     Debug.Log("--start beDep------------");

    //    //     item.Value.beDependencies.ToList().ForEach((depName) =>
    //    //     {
    //    //         Debug.Log("zxy : " + depName);
    //    //     });
    //    //     Debug.Log("---end zxy :  : " + item.Key + " -> " + item.Value.path);
    //    // }



    //    //真正的打包
    //    BuildAssetBundle();
    //    Debug.Log("zxy : finish BuildAssetBundle");

    //    //生成资源依赖表
    //    GenerateAssetFileData();
    //    Debug.Log("zxy : finish GenerateAssetFileData");

    //    //生成资源和 ab 的对应表
    //    GenerateAssetToAbDicFileData();
    //    Debug.Log("zxy : finish GenerateAssetToAbDicFileData");

    //    AssetDatabase.Refresh();
    //}

    private static readonly List<string> IgnoredAssetTypeExtension = new List<string>{
            string.Empty,
            ".manifest",
            ".meta",
            ".assetbundle",
            ".sample",
            ".unitypackage",
            ".cs",
            ".sh",
            ".js",
            ".zip",
            ".tar",
            ".tgz",
			#if UNITY_5_6 || UNITY_5_6_OR_NEWER
			#else
			".m4v",
			#endif
		};

    public static bool IsCanPackageAsset(string fileExtension)
    {
        return !IgnoredAssetTypeExtension.Contains(fileExtension);
    }

}
