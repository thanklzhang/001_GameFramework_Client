using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ResourceManager : Singleton<ResourceManager>
{
    public void LoadPrefab(string path, Action<GameObject> callback, bool isAsync)
    {
        Action<UnityEngine.Object> finishCallback = (obj) =>
        {
            var gameObject = obj as GameObject;
            callback?.Invoke(gameObject);
        };
        AssetManager.Instance.Load(path, finishCallback, isAsync);
    }


}
