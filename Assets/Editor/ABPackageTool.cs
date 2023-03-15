using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;

public class ABPackageTool
{
    [MenuItem("BuildResource/Build IOS Resource", false, 100)]
    public static void BuildIOSResource()
    {
        BuildAssetResource(BuildTarget.iOS);
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


    public static void BuildAssetResource(BuildTarget target, string resVersion = "")
    {
        StartBuildProgress(target, resVersion);
    }


    static Dictionary<string, string> assetToAbDic;
    static List<AssetBundleBuild> bundleBuildList;

    public static void StartBuildProgress(BuildTarget target,string resVersion = "")
    {
#if UNITY_EDITOR
        //unity 系统路径只能再主线程访问 所以先储存
        Const.AppStreamingAssetPath = Application.streamingAssetsPath;
        Const.AssetBundlePath = Application.persistentDataPath + "/" + Const.AppName + "";
#endif

        CollectAssetInfo();

        GenerateAssetToAbDicFileData();

        BuildAssetBundle(target, resVersion);
    }

    public static void CollectAssetInfo()
    {
        ABPackageStrategy packStrategy = new ABPackageStrategy();
        bundleBuildList = packStrategy.StartParse();
    }

    //生成 asset 和 ab 的对应关系文件
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
                    Debug.LogWarning("zxy : PackageTool : BuildAssetBundle : the assetName is exist : " + assetName);
                }
                else
                {
                    var lowStr = assetName.ToLower();
                    assetToAbDic.Add(lowStr, buildBundle.assetBundleName);
                    //assetToAbDic.Add(assetName, buildBundle.assetBundleName);
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

    //开始打 AB 包
    public static void BuildAssetBundle(BuildTarget buildTarget, string resVersion = "")
    {
        //先取出当前的版本资源信息
        var versionPath = Const.AppStreamingAssetPath + "/" + "version.txt";
        var fileListPath = Const.AppStreamingAssetPath + "/" + "file_list.txt";
        var oldBigVer = 0;
        var oldSmallVer = 0;
        var filelistStr = "";
        List<ResInfo> oldResList = new List<ResInfo>();
        var isFirst = !File.Exists(versionPath);
        if (!isFirst)
        {
            var oldVersionStr = FileTool.ReadAllText(versionPath);
            var optionsStr = oldVersionStr.Split('v');
            var verStr = optionsStr[1].Split('.');
            oldBigVer = int.Parse(verStr[0]);
            oldSmallVer = int.Parse(verStr[1]);

            //读取 file_list.txt 
            filelistStr = FileTool.ReadAllText(fileListPath);

            //获得本地当前版本资源列表
            oldResList = StringToResInfoList(filelistStr);
        }
        else
        {
            Logx.Log("第一次打资源包");
        }

        //开始 bundle 打包到 streamingAsset 路径中
        var outPath = Const.AppStreamingAssetPath;
        var abManifest = BuildPipeline.BuildAssetBundles(outPath, bundleBuildList.ToArray(), BuildAssetBundleOptions.None, buildTarget);

        AssetDatabase.Refresh();

        var difList = new List<ResInfo>();
        //获取打包后的资源
        List<ResInfo> newestResList = GenerateFileResWithMD5(Const.AppStreamingAssetPath);

        //比对资源文件
        difList = GetNeedUpdateResList(oldResList, newestResList);

        difList.ForEach(d =>
        {
            Logx.Log("有更新的资源路径 : " + d.path + "   |   md5 : " + d.md5);
        });

        bool isUpperVer = difList.Count > 0;

        var newBigVer = 1;
        var newSmallVer = 0;

        var isForceResVersion = !resVersion.Equals("") && !resVersion.Equals("Auto");
        if (isForceResVersion)
        {
            var strs = resVersion.Split('.');
            newBigVer = int.Parse(strs[0]);
            newSmallVer = int.Parse(strs[1]);

            Logx.Log(string.Format("强行升级资源版本 ：{0}", resVersion));

            var newStr = ResListToString(newestResList);
            FileTool.SaveToFile(fileListPath, newStr);
            var verStr = "v" + newBigVer + "." + newSmallVer;
            FileTool.SaveToFile(versionPath, verStr);

            AssetDatabase.Refresh();

            UploadRes(difList);
        }
        else
        {
            if (isUpperVer)
            {
                if (!isFirst)
                {
                    newBigVer = oldBigVer;
                    newSmallVer = oldSmallVer + 1;

                    var oldVerStr = "v" + oldBigVer + "." + oldSmallVer;
                    var newVerStr = "v" + newBigVer + "." + newSmallVer;
                    Logx.Log(string.Format("有新资源 升级资源版本 ：{0} -> {1}", oldVerStr, newVerStr));
                }
                else
                {
                    //第一次打包
                    Logx.Log(string.Format("第一次打资源包的资源版本 ：{0}", "v1.0"));
                }

                var newStr = ResListToString(newestResList);
                FileTool.SaveToFile(fileListPath, newStr);
                var verStr = "v" + newBigVer + "." + newSmallVer;
                FileTool.SaveToFile(versionPath, verStr);

                AssetDatabase.Refresh();

              
                UploadRes(difList);

            }
            else
            {
                Logx.Log("当前没有变化的资源 无需升级资源版本");
            }
        }
    }

    //资源列表转换成文本
    public static string ResListToString(List<ResInfo> newestResList)
    {
        var newStr = "";
        for (int i = 0; i < newestResList.Count; i++)
        {
            var res = newestResList[i];
            newStr += res.path + "|" + res.md5;

            if (i < newestResList.Count - 1)
            {
                newStr += "\n";
            }
        }

        return newStr;
    }

    //资源列表文本读取
    public static List<ResInfo> StringToResInfoList(string resStr)
    {
        var strs = resStr.Split('\n');
        List<ResInfo> resInfoList = new List<ResInfo>();
        for (int i = 0; i < strs.Length; i++)
        {
            var lineStr = strs[i];
            if (lineStr.Equals(""))
            {
                continue;
            }
            //资源行
            var resOptionStr = lineStr.Split('|');
            var path = resOptionStr[0];
            var md5 = resOptionStr[1];
            ResInfo resInfo = new ResInfo()
            {
                path = path,
                md5 = md5
            };
            resInfoList.Add(resInfo);
        }

        return resInfoList;
    }

    //返回目录下所有需要热更的资源 并计算md5
    public static List<ResInfo> GenerateFileResWithMD5(string sourceFolder)
    {
        List<ResInfo> resList = new List<ResInfo>();
        //得到目录下的所有文件
        string[] files = System.IO.Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            var isAB = Path.GetExtension(file).Equals(Const.ABExtName);
            List<string> depPathList = new List<string>()
            {
                "StreamingAssets","AssetToAbFileData.json"
            };

            var isDepPath = false;
            foreach (var depPath in depPathList)
            {
                if (Path.GetFileName(file).Equals(depPath))
                {
                    isDepPath = true;
                    break;
                }
            }
            var ext = Path.GetExtension(file);
            var isIgnore = ext.Equals(".manifest") || ext.Equals(".meta");

            if ((isAB || isDepPath) && !isIgnore)
            {
                ResInfo resInfo = new ResInfo();
                resInfo.path = file.Replace(Const.AppStreamingAssetPath + "\\", "").Replace("\\", "/");
                resInfo.md5 = EncryptionTool.GetMD5HashFromFile(file);

                resList.Add(resInfo);
            }
        }

        return resList;
    }

    //资源对比 得到需要更新的资源
    public static List<ResInfo> GetNeedUpdateResList(List<ResInfo> oldResList, List<ResInfo> newResList)
    {
        List<ResInfo> difList = new List<ResInfo>();
        for (int i = 0; i < newResList.Count; i++)
        {
            var newRes = newResList[i];

            var oldRes = oldResList.Find((_oldRes) =>
            {
                return _oldRes.path.ToLower() == newRes.path.ToLower();
            });


            if (oldRes != null)
            {
                //存在资源 
                if (!oldRes.md5.Equals(newRes.md5))
                {
                    //md5 不同 添加到 '不同 list' 中
                    difList.Add(newRes);
                }
            }
            else
            {
                //不存在资源 添加到 '不同 list' 中
                difList.Add(newRes);
            }
        }

        return difList;
    }

    //上传资源
    public static async void UploadRes(List<ResInfo> difList)
    {
        //打资源后 更改过的或者新的的资源会上传到 ftp 服务器
        //TODO : 变成可配置操作 : 1 自动更新到服务器
        //2 不更新到服务器(需要将更新的文件路径输出到控制台或者文件中 , 让其手动更新 , 或者手动全量更新)

        await Task.Run(() =>
        {
            Logx.Log("有资源更改 开始资源上传");

            UploadByResInfoList(difList);
            UploadResFlagInfo();


            Logx.Log("完成 更新资源上传");

        });
    }

    //上传版本资源信息标记的文件
    public static void UploadResFlagInfo()
    {
        List<string> strs = new List<string>();
        strs.Add("version.txt");
        strs.Add("file_list.txt");

        List<ResInfo> resList = new List<ResInfo>();
        foreach (var str in strs)
        {
            resList.Add(new ResInfo()
            {
                path = str,
                md5 = ""
            });
        }

        UploadByResInfoList(resList);

    }

    //获得路径下需要上传的所有文件
    public static List<ResInfo> GetCanUploadAllFilesInfo(string sourceFolder)
    {
        List<ResInfo> resList = new List<ResInfo>();
        //得到目录下的所有文件
        string[] files = System.IO.Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            var ext = Path.GetExtension(file);
            if (!ext.Equals(".meta"))
            {
                ResInfo resInfo = new ResInfo();
                var _path = file.Replace(Const.AppStreamingAssetPath + "\\", "").Replace("\\", "/");
                resInfo.path = _path;
                resList.Add(resInfo);
            }
        }
        return resList;
    }

    [MenuItem("BuildResource/upload all res to ftp server", false, 104)]
    public static async void UploadAllResToFtp()
    {
        //强行更新所有资源到 ftp 服务器
        //注意这里没有增加版本号 所以客户端不会更新 这里只是更新资源
#if UNITY_EDITOR
        //unity 系统路径只能再主线程访问 所以先储存
        Const.AppStreamingAssetPath = Application.streamingAssetsPath;
        Const.AssetBundlePath = Application.persistentDataPath + "/" + Const.AppName + "";
#endif
        await Task.Run(() =>
        {
            Logx.Log("开始 所有资源上传");

            List<ResInfo> newestResList = GetCanUploadAllFilesInfo(Const.AppStreamingAssetPath);
            UploadByResInfoList(newestResList);
            UploadResFlagInfo();

            Logx.Log("完成 所有资源上传");
        });


    }

    //通过 ftp 上传资源文件
    public static void UploadByResInfoList(List<ResInfo> newestResList)
    {
        FTPHelper helper = new FTPHelper();
        helper.Init("ftp://127.0.0.1", "thanklzhang", "zhang425");

        for (int i = 0; i < newestResList.Count; i++)
        {
            var resInfo = newestResList[i];
            var localPath = Const.AppStreamingAssetPath + "/" + resInfo.path;
            var remotePath = resInfo.path;
            Debug.Log("start upload res : " + localPath + " , remote res : " + remotePath);
            helper.UpLoadFile(localPath, remotePath);
        }
    }

    public class VersionInfo
    {
        public int bigVer;
        public int smallVer;
        public List<ResInfo> resList;
    }

    public class ResInfo
    {
        public string path;
        public string md5;
    }


}
