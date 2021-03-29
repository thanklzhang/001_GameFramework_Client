using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoadAssetObjRequest : LoadObjectRequest
{
    string[] pathList;
    HashSet<string> currNeedLoadSet = new HashSet<string>();

    public bool CheckFinish()
    {
        return 0 == currNeedLoadSet.Count;
    }

    public void Start(string[] pathList)
    {
        for (int i = 0; i < pathList.Length; i++)
        {
            var currPath = pathList[i];
            currNeedLoadSet.Add(currPath);

            AssetManager.Instance.Load(currPath, (asset) =>
            {
                this.OnOneResLoadFinish(currPath, asset);
            });
        }
    }

    public void OnOneResLoadFinish(string currPath, UnityEngine.Object asset)
    {
        if (currNeedLoadSet.Contains(currPath))
        {
            currNeedLoadSet.Remove(currPath);
        }
        else
        {
            Logx.LogzError("the currPath is not correct : " + currPath);
        }


    }

    public void Finish()
    {

    }
}

