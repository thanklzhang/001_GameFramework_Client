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
#if UNITY_EDITOR
        //unity 系统路径只能再主线程访问 所以先储存
        Const.AppStreamingAssetPath = Application.streamingAssetsPath;
        Const.AssetBundlePath = Application.persistentDataPath + "/" + Const.AppName + "";
#endif

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

    //资源文本 变为 内存中的 dic 
    public static List<VersionInfo> StringToDic(string resStr)
    {
        var strs = resStr.Split('\n');
        List<VersionInfo> verInfoList = new List<VersionInfo>();
        for (int i = 0; i < strs.Length; i++)
        {
            var lineStr = strs[i];
            if (lineStr.Equals(""))
            {
                continue;
            }
            var resIndex = lineStr.IndexOf('|');
            bool isVer = resIndex < 0;
            if (isVer)
            {
                //版本行
                var verStr = lineStr.Split('v')[1];
                var optionStr = verStr.Split('.');
                int bigVer = int.Parse(optionStr[0]);
                int smallVer = int.Parse(optionStr[1]);

                VersionInfo verInfo = new VersionInfo()
                {
                    bigVer = bigVer,
                    smallVer = smallVer,
                    resList = new List<ResInfo>()
                };
                verInfoList.Add(verInfo);
            }
            else
            {
                //资源行
                var resOptionStr = lineStr.Split('|');
                var path = resOptionStr[0];
                var md5 = resOptionStr[1];

                var newestVerInfo = verInfoList[verInfoList.Count - 1];

                ResInfo resInfo = new ResInfo()
                {
                    path = path,
                    md5 = md5
                };
                newestVerInfo.resList.Add(resInfo);
            }
        }

        return verInfoList;
    }

    public static List<ResInfo> GetResList(List<VersionInfo> verInfoList)
    {
        //每次将最新资源列表再储存成另一个文件 这里不用每次收集(待定)
        //这里先按照每次打包来计算

        Dictionary<string, ResInfo> dic = new Dictionary<string, ResInfo>();
        for (int i = 0; i < verInfoList.Count; i++)
        {
            var verInfo = verInfoList[i];
            for (int j = 0; j < verInfo.resList.Count; j++)
            {
                var resInfo = verInfo.resList[j];
                var path = resInfo.path;
                var md5 = resInfo.md5;
                if (dic.ContainsKey(path))
                {
                    dic[path].md5 = md5;
                }
                else
                {
                    dic.Add(path, resInfo);
                }
            }

        }

        return dic.Select((v) => v.Value).ToList();
    }

    //获得目标下所有文件
    public static List<ResInfo> GetAllFilesInfo(string sourceFolder)
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

    //获得目标下所有资源(ab)文件
    public static List<ResInfo> GetAllFileResInfo(string sourceFolder)
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

            //if (isDepPath)
            //{
            //    Logx.Log("isDepPath true : " + file);
            //}

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

    public static List<ResInfo> GetDifferentResList(List<ResInfo> oldResList, List<ResInfo> newResList)
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

    public static void BuildAssetBundle()
    {

        //先取出当前的版本资源信息
        var versionPath = Const.AppStreamingAssetPath + "/" + "version.txt";
        var fileListPath = Const.AppStreamingAssetPath + "/" + "file_list.txt";
        var oldBigVer = 0;
        var oldSmallVer = 0;
        var filelistStr = "";
        List<ResInfo> oldResList = new List<ResInfo>();
        if (File.Exists(versionPath))
        {
            var oldVersionStr = FileTool.ReadAllText(versionPath);
            var optionsStr = oldVersionStr.Split('v');
            var verStr = optionsStr[1].Split('.');
            oldBigVer = int.Parse(verStr[0]);
            oldSmallVer = int.Parse(verStr[1]);
            //Logx.Log("oldBigVer : " + oldBigVer);
            //Logx.Log("oldSmallVer : " + oldSmallVer);

            //读取 file_list.txt 
            filelistStr = FileTool.ReadAllText(fileListPath);
            List<VersionInfo> verList = StringToDic(filelistStr);

            //获得本地当前版本资源列表
            oldResList = GetResList(verList);
            //oldResList.ForEach(d =>
            //{
            //    Logx.Log("old : " + d.path + " " + d.md5);
            //});

        }

        //开始 bundle 打包到 streamingAsset 路径中
        var outPath = Const.AppStreamingAssetPath;
        var abManifest = BuildPipeline.BuildAssetBundles(outPath, bundleBuildList.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        //var deletePath = Const.AssetBundlePath;

        AssetDatabase.Refresh();

        //比对 生成 fileList.txt 和 version.txt
        //是否全量更新 如果全量 那么所有资源全部替换最新的
        bool isFullUpdate = false;
        //var filelistStr = "";
        var difList = new List<ResInfo>();
        //Logx.Log("versionPath : " + versionPath);
        if (File.Exists(versionPath))
        {
            //存在 本本文件 开始比对资源 升级资源版本


            //获取打包后的资源
            List<ResInfo> newestResList = GetAllFileResInfo(Const.AppStreamingAssetPath);
            //oldResList.ForEach(d =>
            //{
            //    Logx.Log("old : " + d.path + " " + d.md5);
            //});
            //newestResList.ForEach(d =>
            //{
            //    Logx.Log("new : " + d.path + " " + d.md5);
            //});


            //比对资源文件
            difList = GetDifferentResList(oldResList, newestResList);

            difList.ForEach(d =>
            {
                Logx.Log("diffrent : path : " + d.path + "   |   md5 : " + d.md5);
            });

            bool isUpperVer = difList.Count > 0;
            if (isUpperVer)
            {
                Logx.Log("有新资源 升级资源版本");
                var newBigVer = oldBigVer;
                var newSmallVer = oldSmallVer + 1;
                //fileListDic save to fileList.txt , update version.txt

                var oldStr = filelistStr;
                var newStr = oldStr;

                var verStr = "v" + newBigVer + "." + newSmallVer;

                newStr += "\n";
                newStr += verStr;
                newStr += "\n";
                for (int i = 0; i < difList.Count; i++)
                {
                    var res = difList[i];
                    newStr += res.path + "|" + res.md5;

                    if (i < difList.Count - 1)
                    {
                        newStr += "\n";
                    }
                }

                FileTool.SaveToFile(fileListPath, newStr);

                FileTool.SaveToFile(versionPath, verStr);
            }
            else
            {
                Logx.Log("当前没有变化的资源 无需升级资源版本");
            }
        }
        else
        {
            Logx.Log("version.txt 没有找到 将进行第一次打资源包");
            //不存在 版本文件 那么当作第一次的包
            var newBigVer = 1;
            var newsmallVer = 0;
            string versionStr = "v" + newBigVer + "." + newsmallVer;

            //version.txt
            var saveVerPath = Const.AppStreamingAssetPath + "/" + "version.txt";
            FileTool.SaveToFile(saveVerPath, versionStr);

            //file_list.txt
            var fileListStr = versionStr + "\n";
            List<string> pathList = new List<string>();
            bundleBuildList.ForEach((b) =>
            {
                pathList.Add(b.assetBundleName.ToLower());
            });

            //资源引用文件 特殊加入
            string[] depInfoPathList = new string[]
            {
                "StreamingAssets","AssetToAbFileData.json"
            };
            pathList.AddRange(depInfoPathList);

            for (int i = 0; i < pathList.Count; i++)
            {
                var path = pathList[i];
                var abPath = path;

                //var filePath = Application.dataPath.Replace("/Assets", "") + "/" + path;
                var filePath = Const.AppStreamingAssetPath + "/" + path;
                //var bytes = FileTool.ReadAllBytes(filePath);

                var md5 = EncryptionTool.GetMD5HashFromFile(filePath);
                fileListStr += abPath + "|" + md5;

                if (i < pathList.Count - 1)
                {
                    fileListStr += "\n";
                }

                difList.Add(new ResInfo()
                {
                    path = abPath,
                    md5 = md5
                });
            }


            //foreach (var depPath in depInfoPathList)
            //{
            //    var filePath = Const.AppStreamingAssetPath + "/" + depPath;
            //    var md5 = EncryptionTool.GetMD5HashFromFile(filePath);

            //    difList.Add(new ResInfo()
            //    {
            //        path = depPath,
            //        md5 = md5
            //    });
            //}

            //更新 file_list.txt
            var saveFileListPath = Const.AppStreamingAssetPath + "/" + "file_list.txt";
            FileTool.SaveToFile(saveFileListPath, fileListStr);
        }

        //include res,fileList.txt,version.txt

        AssetDatabase.Refresh();

        //打资源后 更改过的或者新的的资源会上传到 ftp 服务器
        if (difList.Count > 0)
        {
            UploadUpdateRes(difList);
        }
        //

    }

    public static async void UploadUpdateRes(List<ResInfo> difList)
    {
        await Task.Run(() =>
        {
            Logx.Log("有资源更改 开始资源上传");

            UploadByResInfoList(difList);
            UploadResFlagInfo();


            Logx.Log("完成 更新资源上传");

        });
    }

    public static void UploadResFlagInfo()
    {
        List<string> strs = new List<string>();
        strs.Add("version.txt");
        strs.Add("file_list.txt");

        //strs.Add("AssetToAbFileData.json");
        //strs.Add("StreamingAssets");

        //这个不用
        //strs.Add("StreamingAssets.manifest");

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

    [MenuItem("BuildResource/upload res to ftp server", false, 104)]
    public static async void UploadToFtp()
    {
#if UNITY_EDITOR
        //unity 系统路径只能再主线程访问 所以先储存
        Const.AppStreamingAssetPath = Application.streamingAssetsPath;
        Const.AssetBundlePath = Application.persistentDataPath + "/" + Const.AppName + "";
#endif
        await Task.Run(() =>
        {
            Logx.Log("开始 所有资源上传");

            List<ResInfo> newestResList = GetAllFilesInfo(Const.AppStreamingAssetPath);
            UploadByResInfoList(newestResList);
            UploadResFlagInfo();

            Logx.Log("完成 所有资源上传");
        });


    }



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

    static string tssss;
    [MenuItem("BuildResource/test", false, 105)]
    public static void test()
    {
       

        //string json = "{'ss':[1,2,3]}";

        //var jd = LitJson.JsonMapper.ToObject(json);

        //var oo = jd["ss"];




        ////var ss = LitJson.JsonMapper.ToObject(jd["ss"]);

        //var obj = LitJson.JsonMapper.ToObject<byte[]>(jd["ss"].ToJson());
        ////var sss = jd["ss"].ToString();
        //Debug.Log("ss : " + obj.Length);


        //tssss = Const.AppStreamingAssetPath;


        //test2();
    }



    public static async void test2()
    {
        await Task.Run(() =>
        {
            var xx = tssss;
            Debug.Log(xx);

        }); ;
    }


}
