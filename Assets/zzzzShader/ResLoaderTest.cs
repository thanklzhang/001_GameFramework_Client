using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResLoaderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var canvs = GameObject.Find("Canvas").transform;
        string path = "prefabs/ui/managementui/managementmainui";

        // ResourceManager.Instance.LoadPrefabByPath(path, (isSuccess, obj) =>
        // {
        //     Debug.Log("ResLoaderTest : " + isSuccess + " " + obj);

        //     obj.transform.SetParent(canvs, false);

        // }, true);

        // string path2 = "prefabs/ui/managementui/managementbenchui";

        // ResourceManager.Instance.LoadPrefabByPath(path2, (isSuccess, obj) =>
        // {
        //     Debug.Log("ResLoaderTest 2 : " + isSuccess + " " + obj);

        //     obj.transform.SetParent(canvs, false);

        // }, true);

        //ResourceManager.Instance.LoadPrefab(path, (isSuccess, obj) =>
        //{
        //    Debug.Log("ResLoaderTest : " + isSuccess + " " + obj);

        //    obj.transform.SetParent(canvs, false);

        //}, true);

        //sync
        //ResourceManager.Instance.LoadPrefab(path, (isSuccess, obj) =>
        //{
        //    Debug.Log("ResLoaderTest : " + isSuccess + " " + obj);

        //    obj.transform.SetParent(canvs, false);

        //}, false);

        //ResourceManager.Instance.LoadPrefab(path, (isSuccess, obj) =>
        //{
        //    Debug.Log("ResLoaderTest : " + isSuccess + " " + obj);

        //    obj.transform.SetParent(canvs, false);

        //}, false);
        //ResourceManager.Instance.LoadPrefab(path, (isSuccess, obj) =>
        //{
        //    Debug.Log("ResLoaderTest : " + isSuccess + " " + obj);

        //    obj.transform.SetParent(canvs, false);

        //}, false);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
