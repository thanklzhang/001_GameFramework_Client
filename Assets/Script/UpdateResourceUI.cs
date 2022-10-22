using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//资源更新模块
public class UpdateResourceUI : MonoBehaviour
{
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



    public IEnumerator CheckPersistentResource()
    {
        yield return null;
      
        var persistentPath = Const.AssetBundlePath;
        //目前值判断根目录 之后热更的话需要判断 file 列表文件其中的文件列表等
        var isExist = Directory.Exists(persistentPath);
        if (!isExist)
        {
            Directory.CreateDirectory(persistentPath);
        }

        //复制所有文件从 streaming 到 persistent 中
        var streamingPath = Const.AppStreamingAssetPath;
        var allFiles = System.IO.Directory.GetFiles(streamingPath, "*.*", SearchOption.AllDirectories);
        var files = allFiles.Where(f => !f.EndsWith(".meta")).ToList();

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


       
    }


}
