using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoadGameObjectRequest : LoadObjectRequest
{
    string[] pathList;
    HashSet<string> currNeedLoadSet = new HashSet<string>();
    private string path;
    private int count;

    public LoadGameObjectRequest(int count)
    {
        this.count = count;
    }

    public override bool CheckFinish()
    {
        return 0 == currNeedLoadSet.Count;
    }

    public void Start(string[] pathList)
    {
        this.pathList = pathList;
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
            Logx.LogError("the currPath is not correct : " + currPath);
        }


    }

    public override void Finish()
    {

    }

    public override void Release()
    {
        for (int i = this.pathList.Length - 1; i >= 0; i--)
        {
            var path = this.pathList[i];
            AssetManager.Instance.Release(path);
        }
    }

    public override void Start()
    {
        throw new NotImplementedException();
    }
}

