using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//资源更新模块
public class UpdateResourceUI : MonoBehaviour
{
    public class ResInfo
    {
        public string path;
        public string md5;
    }
    public Text progressText;

    void Awake()
    {
        progressText = transform.Find("progress").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Startup();
    }
    List<ResInfo> needUpdatePathList;

    Dictionary<string, string> localFileDic;
    public IEnumerator CheckPersistentResource()
    {

        if (!Const.isUseAB)
        {
            yield break;
        }
        yield return null;

        var persistentPath = Const.AssetBundlePath;
        var localVersionPath = Const.AssetBundlePath + "/" + "version.txt";
        //热更
        var isExist = Directory.Exists(persistentPath) && File.Exists(localVersionPath);
        if (isExist)
        {
            //有本地资源文件 开始检测资源版本
            var fileListStr = "";

            var localBigVer = 0;
            var localSmallVer = 0;

            //检查当前版本资源是否正确(待定)

            //取得自身 ip
            var localIP = NetTool.GetHostIp();

            //得到服务端资源版本信息
            var url = string.Format("http://{0}:{1}/get_res_version", localIP, 8080);
            Logx.Log("请求服务器 http : " + url);

            UnityWebRequest request = UnityWebRequest.Get(url);

            DownloadHandlerBuffer Download = new DownloadHandlerBuffer();
            request.downloadHandler = Download;

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Logx.Log("isHttpError : " + request.error);
                yield break;
            }

            var json = request.downloadHandler.text;
            LitJson.JsonData jd = LitJson.JsonMapper.ToObject(json);

            Logx.Log("服务端回包 get_version : " + json);

            var serBigVer = int.Parse(jd["big_version"].ToString());
            var serSmallVer = int.Parse(jd["small_version"].ToString());
            //Logx.Log("serBigVer : " + serBigVer);
            //Logx.Log("serSmallVer : " + serSmallVer);


            //得到本地资源版本信息

            if (File.Exists(localVersionPath))
            {
                var verTxtStr = FileTool.GetTextFromFile(localVersionPath);
                var ver = verTxtStr.Split('v')[1];
                var verParamStr = ver.Split('.');
                localBigVer = int.Parse(verParamStr[0]);
                localSmallVer = int.Parse(verParamStr[1]);

                //Logx.Log("localBigVer : " + localBigVer);
                //Logx.Log("localSmallVer : " + localSmallVer);
            }
            else
            {
                Debug.Log("缺少 version.txt 文件");
                yield break;
            }

            if (localBigVer < serBigVer)
            {
                Debug.Log("版本过低 请下载最新客户端");
                yield break;
            }

            if (localBigVer > serBigVer || localSmallVer > serSmallVer)
            {
                Debug.Log("版本错误 高出服务器资源版本 请重新下载客户端包");
                yield break;
            }

            if (localSmallVer == serSmallVer)
            {
                Debug.Log("资源已经是最新版本 无需再更新");
                yield break;
            }

            var localVerStr = "v" + localBigVer + "." + localSmallVer;
            var serVerStr = "v" + serBigVer + "." + serSmallVer;
            Debug.Log("当前版本 : " + localVerStr + " , 服务器版本 : " + serVerStr + " , 需要更新资源");

            //传给服务器自己的版本 1.0 , 取服务器最新版本号 例如 1.3 , 并获得服务端返回的需要更新的文件列表
            url = string.Format("http://{0}:{1}/get_res_file?big_version={2}&small_version={3}",
                localIP, 8080, localBigVer, localSmallVer);
            Logx.Log("请求服务端 http : " + url);

            request = UnityWebRequest.Get(url);

            Download = new DownloadHandlerBuffer();
            request.downloadHandler = Download;

            yield return request.SendWebRequest();

            //得到服务端资源版本信息 服务端返回的需要更新的文件（1.0 一次更新到 1.3 的版本所需要的资源）
            Dictionary<string, string> serFileDic = new Dictionary<string, string>();


            json = request.downloadHandler.text;
            Logx.Log("服务端回包 : get_res_file : " + json);
            jd = LitJson.JsonMapper.ToObject(json);
            foreach (JsonData jsonD in jd)
            {
                var path = jsonD["path"].ToString();
                var md5 = jsonD["md5"].ToString();
                serFileDic.Add(path, md5);

                //Logx.Log("ser : path : " + path);
                //Logx.Log("ser : md5 : " + md5);
            }

            //得到本地当前资源列表信息
            localFileDic = new Dictionary<string, string>();
            var localResFileListPath = Const.AssetBundlePath + "/" + "file_list.txt";
            if (File.Exists(localResFileListPath))
            {
                var fileTxtStr = FileTool.GetTextFromFile(localResFileListPath, true);
                //Logx.Log("local : fileTxtStr : " + fileTxtStr);
                var filesStr = fileTxtStr.Split('\n');
                foreach (var option in filesStr)
                {
                    //Logx.Log("local : option : " + option);

                    var paramsStr = option.Split('|');
                    if (paramsStr.Length <= 1)
                    {
                        //可能是版本信息
                        continue;
                    }
                    var path = paramsStr[0];
                    var md5 = paramsStr[1];
                    //Logx.Log("local : filesStr : " + filesStr);
                    //Logx.Log("local : path : " + path);
                    //Logx.Log("local : md5 : " + md5);
                    if (localFileDic.ContainsKey(path))
                    {
                        //localFileDic[path] = md5;
                        Logx.Log("the path has exist in localResFileListPath : path : " + path);
                    }
                    else
                    {
                        localFileDic.Add(path, md5);
                    }
                }
            }
            else
            {
                Debug.Log("缺少 file_list.txt 文件");
                yield break;
            }

            needUpdatePathList = new List<ResInfo>();
            foreach (var ser in serFileDic)
            {
                string serPath = ser.Key;
                string serMd5 = ser.Value;
                bool isNeedDownload = true;

                foreach (var local in localFileDic)
                {
                    string localPath = local.Key;
                    string localMd5 = local.Value;

                    if (serPath == localPath)
                    {
                        if (serMd5 != localMd5)
                        {
                            isNeedDownload = true;
                        }
                        else
                        {
                            //找到了 但是 md5 一样 不用下载
                            isNeedDownload = false;
                        }
                        break;
                    }
                }

                //如果在本地没找到 那么就是没有该资源 需要下载
                if (isNeedDownload)
                {
                    var resInfo = new ResInfo()
                    {
                        path = serPath,
                        md5 = serMd5
                    };
                    needUpdatePathList.Add(resInfo);
                }
            }

            if (needUpdatePathList.Count > 0)
            {
                Logx.Log("有需要更新的资源 文件数量为 : " + needUpdatePathList.Count);
            }
            else
            {
                Logx.Log("没有需要更新的资源");
            }

            ////根据 needUpdatePathList 进行逐个文件的下载
            //foreach (var path in needUpdatePathList)
            //{
            //    Logx.Log("need update path : " + path);
            //}

            //对比服务器返回的文件列表 进行更新 localFileDic vs serFileDic
            //每个文件下完了就更新 file_list.txt 保证文件为单位的断点续传
            for (int i = 0; i < needUpdatePathList.Count; i++)
            {
                var downPath = needUpdatePathList[i];
                Logx.Log("开始下载资源 : " + downPath);

                yield return DownloadFile(downPath);

                Logx.Log("完成下载资源 : " + downPath);
            }


            //这里这两步其实可以跟 asset 放到一起更新
            ////下载 StreamingAssets 引用文件
            //var dep_streaming_str = "StreamingAssets";
            //Logx.Log("开始下载 '资源引用信息' 资源 : " + dep_streaming_str);
            //yield return DownloadFile(dep_streaming_str);
            //Logx.Log("完成下载 '资源引用信息' 资源 : " + dep_streaming_str);

            ////下载 asset 和 ab 对应文件
            //var dep_asset_to_ab_str = "AssetToAbFileData.json";
            //Logx.Log("开始下载 '资源引用信息' 资源 : " + dep_asset_to_ab_str,false);
            //yield return DownloadFile(dep_asset_to_ab_str);
            //Logx.Log("完成下载 '资源引用信息' 资源 : " + dep_asset_to_ab_str, false);


            //全部文件都一致了就更新 version.txt 
            Logx.Log("开始更新 version.txt 文件");
            var localVersionFileListPath = Const.AssetBundlePath + "/" + "version.txt";
            var str = "v" + serBigVer + "." + serSmallVer;
            FileTool.SaveToFile(localVersionFileListPath, str);
            Logx.Log("完成更新 version.txt 文件");


            //完成更新过程
            Logx.Log("所有更新过程完成");
        }
        else
        {
            Logx.Log("第一次进入游戏 , 开始解压游戏资源");

            //没有本地资源文件 复制包内中的现有资源复制到本地
            Directory.CreateDirectory(persistentPath);

            //复制所有文件从 streaming 到 persistent 中
            var streamingPath = Const.AppStreamingAssetPath;
            var allFiles = System.IO.Directory.GetFiles(streamingPath, "*.*", SearchOption.AllDirectories);
            //过滤文件
            var files = allFiles.Where(f =>
            {
                var isMetaFile = f.EndsWith(".meta");
                //file_list 本地只存储所有最新资源 只有一版 所以不从服务端复制
                //var isFileListTxt = f.Contains("file_list.txt");

                return !isMetaFile;//&& !isFileListTxt
            }).ToList();
            //

            var totalProgress = files.Count + 0.0f;
            var span_1 = 1.0f / totalProgress;
            var currFinishCount = 0;
            for (int i = 0; i < files.Count; i++)
            {
                var filePath = files[i];
                //Logx.Log("file : " + filePath);

                var index = filePath.IndexOf(streamingPath);
                var partFilePath = filePath.Substring(streamingPath.Length + 1);
                var persistentFullPath = persistentPath + "\\" + partFilePath;

                UnityWebRequest request = UnityWebRequest.Get(filePath);
                request.SendWebRequest();

                var lastProgress = currFinishCount / totalProgress;
                while (true)
                {
                    yield return null;
                    if (request.isDone)
                    {
                        if (request.isHttpError || request.isNetworkError)
                        {
                            Logx.Log(request.error);
                        }
                        else
                        {
                            var directory = Path.GetDirectoryName(persistentFullPath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            if (!File.Exists(persistentFullPath))
                            {
                                File.Create(persistentFullPath).Dispose();
                            }
                            var downBytes = request.downloadHandler.data;

                            //file_list.txt 本地只存储所有最新资源 只有一版 所以不从服务端复制
                            var isFileListTxt = filePath.Contains("file_list.txt");
                            if (isFileListTxt)
                            {
                                var fileListStr = Encoding.UTF8.GetString(downBytes);
                                var str = FilterToNewestRes(fileListStr);
                                downBytes = Encoding.UTF8.GetBytes(str);
                            }

                            File.WriteAllBytes(persistentFullPath, downBytes);
                        }

                        var currProgress = lastProgress + span_1 * request.downloadProgress;
                        var progressStr = Math.Round((currProgress * 100), 2);
                        //Logx.Log("finish a file : now progress is : " + progressStr + "%");
                        progressText.text = "progress : " + progressStr + "%";

                        break;
                    }
                }

                currFinishCount += 1;

            }

            Logx.Log("解压游戏资源完成");

        }


    }


    //过滤出唯一的最新资源列表
    public string FilterToNewestRes(string fileTxtStr)
    {
        var localFileDic = new Dictionary<string, string>();
        var filesStr = fileTxtStr.Split('\n');
        foreach (var option in filesStr)
        {
            //Logx.Log("local : option : " + option);

            var paramsStr = option.Split('|');
            if (paramsStr.Length <= 1)
            {
                //可能是版本信息
                continue;
            }
            var path = paramsStr[0];
            var md5 = paramsStr[1];
            if (localFileDic.ContainsKey(path))
            {
                localFileDic[path] = md5;
                //Logx.Log("the path has exist in localResFileListPath : path : " + path);
            }
            else
            {
                localFileDic.Add(path, md5);
            }
        }

        return FileDicToStr(localFileDic);
    }

    public IEnumerator DownloadFile(ResInfo serResInfo)
    {
        //得到服务端资源版本信息
        var localIP = NetTool.GetHostIp();

        var url = string.Format("http://{0}:{1}/download_file/{2}", localIP, 8080, serResInfo.path);
        Logx.Log("请求服务端 http : " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);

        DownloadHandlerBuffer Download = new DownloadHandlerBuffer();
        request.downloadHandler = Download;

        var req = request.SendWebRequest();

        while (true)
        {
            yield return null;
            Logx.Log("progress : " + req.progress);
            if (req.isDone)
            {
                break;
            }
        }

        if (request.isNetworkError || request.isHttpError)
        {
            Logx.Log("isHttpError : " + request.error);
            yield break;

        }

        //var json = request.downloadHandler.text;
        var bytes = request.downloadHandler.data;

        Logx.Log("服务端回包 : bytes 长度 : " + bytes.Length);

        //var jd = LitJson.JsonMapper.ToObject(json);
        //var serPath = jd["path"].ToString();
        //var serMD5 = jd["md5"].ToString();
        //var bytes = LitJson.JsonMapper.ToObject<byte[]>(jd["data"].ToJson());

        //var bytes = jd["data"];

        var serFileData = bytes;//Encoding.UTF8.GetBytes(bytes);

        var savePath = Const.AssetBundlePath + "/" + serResInfo.path;
        var tempDir = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }
        FileTool.SaveBytesToFile(savePath, serFileData);

        //var md5 = EncryptionTool.ComputeHashByMD5(bytes, true);

        //判断是否保存到 file_list.txt 中
        //if (isSaveToFileList)
        {
            if (localFileDic.ContainsKey(serResInfo.path))
            {
                localFileDic[serResInfo.path] = serResInfo.md5;
            }
            else
            {
                //Logx.Log("the filePath is not found : " + filePath);
                //yield break;

                localFileDic.Add(serResInfo.path, serResInfo.md5);
            }

            //下载完一个 更新 file_list.txt
            var localResFileListPath = Const.AssetBundlePath + "/" + "file_list.txt";
            if (File.Exists(localResFileListPath))
            {
                var str = FileDicToStr(localFileDic);
                FileTool.SaveToFile(localResFileListPath, str);
            }
            else
            {
                Debug.Log("缺少 file_list.txt 文件");
            }

        }



    }

    public string FileDicToStr(Dictionary<string, string> localFileDic)
    {
        string str = "";
        foreach (var item in localFileDic)
        {
            var path = item.Key;
            var md5 = item.Value;
            str += path + "|" + md5 + "\n";
        }

        str = str.Substring(0, str.Length - 1);

        return str;
    }
}
