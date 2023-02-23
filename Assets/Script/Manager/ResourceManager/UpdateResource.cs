//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public enum UpdateResourceStateType
//{
//    Null,
//    CheckingResource,
//    Decompressing,
//    CheckingUpdateResource,
//    UpdatingResource,
//    Finish
//}

//public class UpdateResourceInfo
//{
//    public UpdateResourceStateType currState;
//    public string currFilePath;
//    public float progress;
//    public static UpdateResourceInfo Create(string path, UpdateResourceStateType state, float pregress)
//    {
//        return new UpdateResourceInfo
//        {
//            currFilePath = path,
//            currState = state,
//            progress = pregress
//        };
//    }
//}

//public class UpdateResource : Singleton<UpdateResource>
//{
//    public Action finishCallback;
//    public Action<UpdateResourceInfo> updateCallback;
//    public void Start(Action<UpdateResourceInfo> updateCallback, Action finishCallback)
//    {
//        this.updateCallback = updateCallback;
//        this.finishCallback = finishCallback;
//        if (Const.isUseAB)
//        {
//            //检查 AB 包路径是否存在 
//            if (Directory.Exists(Const.AssetBundlePath))
//            {

//            }
//            else
//            {
//                //不存在的话从游戏包内部的数据复制到这里
//                Directory.CreateDirectory(Const.AssetBundlePath);
//                CopyResource();
//            }

//            CoroutineManager.Instance.StartCoroutine(StartUpdate());
//        }
//        else
//        {
//            //不使用 AB 的话 直接完成
//            Finish();
//        }
//    }

//    public void CopyResource()
//    {
//        Debug.Log("copy ...");
//        var sr = File.OpenText(Const.AppStreamingAssetPath + "/" + "files.txt");
//        var fileContent = sr.ReadToEnd();
//        sr.Close();

//        var filesPath = fileContent.Split('\n');
//        for (int i = 0; i < filesPath.Length; ++i)
//        {
//            var currFile = filesPath[i];
//            if (null == currFile || currFile == "")
//            {
//                continue;
//            }
//            var infos = currFile.Split('|');

//            if (infos.Length > 1)
//            {
//                var currDir = filesPath[i].Split('|')[0];
//                Debug.Log("copy : " + currDir);

//                var des = Const.AssetBundlePath + "/" + currDir;
//                CopyFile(Const.AppStreamingAssetPath + "/" + currDir, des);
//            }
//            else
//            {
//                Debug.Log("the infos format is wrong , the infos length is less than 2 ： " + currFile);
//            }

//        }

//        {
//            //拷贝 files 文件 和 version 文件

//            //files
//            Debug.Log("copy : " + "files.txt");
//            var des = Const.AssetBundlePath + "/" + "files.txt";
//            CopyFile(Const.AppStreamingAssetPath + "/" + "files.txt", des);

//            //version
//            des = Const.AssetBundlePath + "/" + "version.txt";
//            CopyFile(Const.AppStreamingAssetPath + "/" + "version.txt", des);

//        }

//        Debug.Log("copy finish");

//    }

//    void CopyFile(string src, string des)
//    {
//        var desPath = Path.GetDirectoryName(des);
//        if (!Directory.Exists(desPath))
//        {
//            Directory.CreateDirectory(desPath);
//        }

//        byte[] buffer = new byte[1024 * 1024];//1 MB buffer
//        var streamReader = new FileStream(src, FileMode.Open);
//        FileStream streamWriter = null;
//        streamWriter = new FileStream(des, FileMode.Create);
//        int length = 0;
//        do
//        {
//            length = streamReader.Read(buffer, 0, buffer.Length);
//            streamWriter.Write(buffer, 0, length);

//        } while (length == buffer.Length);

//        streamReader.Close();
//        streamWriter.Close();
//    }

//    bool CheckVersion(string downloadVersion)
//    {
//        if (downloadVersion == "")
//        {
//            return false;
//        }
//        Debug.Log("downloadVersion : " + downloadVersion);
//        string versionFile = downloadVersion;
//        var downloadVers = versionFile.Split('.');
//        var downloadBigV = int.Parse(downloadVers[0].Substring(1));
//        var downloadSmallV = int.Parse(downloadVers[1]);

//        var sr = File.OpenText(Const.AssetBundlePath + "/" + "version.txt");
//        string localFile = sr.ReadToEnd();
//        sr.Close();

//        var localVers = localFile.Split('.');
//        var localBigV = int.Parse(localVers[0].Substring(1));
//        var localSmallV = int.Parse(localVers[1]);

//        if (localBigV == downloadBigV)
//        {
//            if (localSmallV == downloadSmallV)
//            {
//                //版本相同 不必下载
//                Debug.Log("the version is the same continue...");

//            }
//            else if (localSmallV < downloadSmallV)
//            {
//                //更新
//                Debug.Log("need upgrade ...");
//                return true;
//            }
//            else
//            {
//                //本地版本比服务端大 出错
//                Debug.Log("error : local samll version is bigger than server");
//            }
//        }
//        else
//        {
//            Debug.Log("error : big version is not the same");

//        }
//        return false;
//    }

//    void CheckFiles(string downloadFile)
//    {
//        if (downloadFile == "")
//        {
//            return;
//        }
//        var sr = File.OpenText(Const.AssetBundlePath + "/" + "files.txt");
//        string localFile = sr.ReadToEnd();
//        sr.Close();

//        Debug.Log(downloadFile);
//        string[] downloadStrs = downloadFile.Split('\n');
//        Debug.Log(localFile);
//        string[] localStrs = localFile.Split('\n');
//        downloadStrs.ToList().ForEach(downStr =>
//        {
//            //从本地的 files 文件中对比
//            var dStrs = downStr.Split('|');
//            if (dStrs.Length < 2)
//            {
//                return;
//            }

//            var downDir = dStrs[0];
//            var downFileMd5 = dStrs[1];

//            var currDir = localStrs.ToList().Find(localStr =>
//            {
//                var lStrs = localStr.Split('|');
//                if (lStrs.Length < 2)
//                {
//                    return false;
//                }

//                var localDir = lStrs[0];
//                var localFileMd5 = lStrs[1];

//                return downDir == localDir;
//            });

//            if (currDir != null)
//            {
//                //找到了相应的文件 要判断 md5 值
//                var lStrs = currDir.Split('|');
//                var localDir = lStrs[0];
//                var localFileMd5 = lStrs[1];

//                if (downFileMd5 == localFileMd5)
//                {
//                    //md5 相等 不用下载
//                }
//                else
//                {
//                    //md5 不相等 需要下载
//                    needDownloadFileList.Add(downDir);
//                }
//            }
//            else
//            {
//                //没找到相应的文件
//                if (downDir != null && downDir != "")
//                {
//                    //看是否真的有这个文件(有可能是下完了但是 files 文件没有更新)

//                    var filePath = Const.AssetBundlePath + "/" + downDir;
//                    if (File.Exists(filePath))
//                    {
//                        //存在的话检查 md5
//                        var md5 = CommonFunction.md5(filePath);
//                        if (md5 == downFileMd5)
//                        {
//                            //相等不用下载
//                        }
//                        else
//                        {
//                            //不相等需要下载
//                            needDownloadFileList.Add(downDir);
//                        }
//                    }
//                    else
//                    {
//                        //不存在的话 直接下载
//                        needDownloadFileList.Add(downDir);
//                    }
//                }
//            }
//        });
//    }


//    List<string> needDownloadFileList = new List<string>();
//    IEnumerator StartUpdate()
//    {
//        Debug.Log("start download : ");

//        //先从网络下载 version 看是否需要下载
//        WWW www = new WWW("http://127.0.0.1:8080/version.txt?name=xxx");
//        yield return www;

//        Debug.Log("www : " + www.error);
//        string downloadVersion = www.text;
//        bool isNeedDownload = CheckVersion(downloadVersion);

//        if (!isNeedDownload)
//        {
//            Finish();
//            yield break;
//        }

//        //先从网络下载 file 列表文件 在本地进行比对 然后将改变的文件再次请求下载 最后更新
//        //file 列表文件
//        www = new WWW("http://127.0.0.1:8080/files.txt?name=xxx");

//        yield return www;

//        string downloadFile = www.text;

//        needDownloadFileList = new List<string>();
//        CheckFiles(downloadFile);

//        //开始进行逐个下载
//        for (int i = 0; i < needDownloadFileList.Count; ++i)
//        {
//            var file = needDownloadFileList[i];
//            Debug.Log("needDown : " + file);

//            var url = "http://127.0.0.1:8080/" + file + "?name=xxx";
//            Debug.Log("download : " + url);
//            WWW fileWWW = new WWW(url);

//            yield return fileWWW;

//            //simulate send update update state
//            var info = UpdateResourceInfo.Create("", UpdateResourceStateType.UpdatingResource, 0.5f);
//            updateCallback?.Invoke(info);

//            var des = Const.AssetBundlePath + "/" + file;
//            WriteFile(des, fileWWW.bytes);
//        }

//        //所有文件更新后 更新 files.txt 和 version.txt
//        {
//            var des = Const.AssetBundlePath + "/" + "files.txt";
//            WriteFile(des, Encoding.UTF8.GetBytes(downloadFile));

//            des = Const.AssetBundlePath + "/" + "version.txt";
//            WriteFile(des, Encoding.UTF8.GetBytes(downloadVersion));
//        }

//        Finish();
//    }

//    void WriteFile(string filePath, byte[] bytes)
//    {
//        var des = filePath;
//        var desPath = Path.GetDirectoryName(des);
//        if (!Directory.Exists(desPath))
//        {
//            Directory.CreateDirectory(desPath);
//        }

//        var streamWriter = new FileStream(des, FileMode.OpenOrCreate);
//        streamWriter.Write(bytes, 0, bytes.Length);
//        streamWriter.Close();
//    }

//    public void Finish()
//    {
//        Debug.Log("finish download : ");
//        this.finishCallback?.Invoke();



//    }
//}

////public class UpdateResourceUI : MonoBehaviour
////{
////    UpdateResource updateRes;
////    // Use this for initialization

////    public void StartCheckResource()
////    {
////        if(null == updateRes )
////        {
////            updateRes = new UpdateResource();
////            updateRes.CheckResource();
////        }

////    }

////    void Start()
////    {
////        updateRes = new UpdateResource();
////        updateRes.CheckResource();
////        //StartCoroutine
////    }

////    // Update is called once per frame
////    void Update()
////    {

////    }
////}
