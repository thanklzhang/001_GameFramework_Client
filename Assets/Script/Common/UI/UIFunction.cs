using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIFunction
{

    //public static void SetObjPoolByUIPath(int dataNum, Transform root, string pathUI, Action<GameObject, int> stepAction)
    //{

    //}

    /// <summary>
    /// 在界面中 可以用这个进行物品池局部创建
    /// </summary>
    public static void SetObjPool(int dataNum, Transform root, GameObject prefab, Action<GameObject, int> stepAction)
    {
        int i = 0;
        for (; i < dataNum; ++i)
        {
            GameObject obj = null;
            if (i < root.childCount)
            {
                obj = root.GetChild(i).gameObject;
            }
            else
            {
                obj = GameObject.Instantiate(prefab, root, false);
            }

            obj.SetActive(true);
            stepAction?.Invoke(obj, i);
        }

        for (; i < root.childCount; ++i)
        {
            root.GetChild(i).gameObject.SetActive(false);
        }
    }
}
