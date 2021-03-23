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


    static Dictionary<string, string> assetToAbDic;
    static List<AssetBundleBuild> bundleBuildList;

    public static void StartBuildProgress()
    {
        var deletePath = Const.AppStreamingAssetPath;
        FileTool.DeleteAllFile(deletePath);

        CollectAssetInfo();

        GenerateAssetToAbDicFileData();

        BuildAssetBundle();
    }

    public static void CollectAssetInfo()
    {
        PackageStrategy packStrategy = new PackageStrategy();
        bundleBuildList = packStrategy.StartParse();
    }

    public static void GenerateAssetToAbDicFileData()
    {
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


    public static void BuildAssetBundle()
    {
        //开始 bundle 打包到 streamingAsset 路径中
        var outPath = Const.AppStreamingAssetPath;
        var abManifest = BuildPipeline.BuildAssetBundles(outPath, bundleBuildList.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        //var deletePath = Const.AssetBundlePath;

        AssetDatabase.Refresh();
        //拷贝 assetBundle 到 persistentDataPath 路径中
        FileTool.DeleteAllFile(Const.AssetBundlePath);

        var copySrcPath = Path.GetFullPath(outPath);
        var copyDesPath = Path.GetFullPath(Const.AssetBundlePath);
        FileTool.CopyFolder(copySrcPath, copyDesPath);
        AssetDatabase.Refresh();
    }




}
