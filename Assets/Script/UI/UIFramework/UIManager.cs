using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class UIArg
// {
//     public string name;
//     public string resPath;
//     public UIType type;
// }

public enum UIType
{
    Normal,//最底层的ui
    Title,//标题栏
    Pop,//弹窗的ui
    //Top//不受限制的弹窗ui
}

public class UIName
{
    public const string HeroListUI = "HeroListUI";
    public const string HeroInfoUI = "HeroInfoUI";
    public const string ItemListUI = "ItemListUI";
    public const string EquipmentListUI = "EquipmentListUI";

    public const string TitleUI = "TitleUI";

    public const string PopAUI = "PopAUI";
    public const string PopBUI = "PopBUI";
    public const string PopCUI = "PopCUI";
}



public class UIManager : Singleton<UIManager>
{

    /// <summary>
    /// ////// will delete
    /// </summary>
    /// <returns></returns>
    public static List<string> GetAllNameList()
    {
        return new List<string>()
    {
        UIName.HeroListUI,
        UIName.ItemListUI,
        UIName.EquipmentListUI,
        UIName.PopAUI,
        UIName.PopBUI,
        UIName.PopCUI,
    };
    }
    /// <summary>
    /// ///////////
    /// </summary>
    public List<BaseUI> uiCacheList = new List<BaseUI>();
    private Transform uiRoot;
    public Transform normalRoot;
    public Transform titleRoot;
    public Transform popRoot;
    public BaseUI currActiveNormalUI;
    public BaseUI titleUI;
    public void Init(Transform uiRoot)
    {
        this.uiRoot = uiRoot;

        normalRoot = this.uiRoot.Find("NormalUIRoot");
        titleRoot = this.uiRoot.Find("TitleUIRoot");
        popRoot = this.uiRoot.Find("PopUIRoot");
    }

    public BaseUI OpenUI(string uiName,Action loadFinishCallback = null)
    {
        BaseUI currUI = null;
        //先寻找当前所有缓存的界面中 是否有将要打开的ui
        var findUI = FindCacheUI(uiName);

        if (findUI != null)
        {
            //找到了自身的 ui 并提到最前
            RemoveCacheUI(findUI);

            findUI.gameObject.transform.SetAsLastSibling();

            if (findUI.type == UIType.Normal)
            {
                //隐藏前面显示出来的所有 UI
                InactiveAllUI();
                currActiveNormalUI = findUI;
                RefreshTitleUI();
            }
            AppendCacheUI(findUI);
            ActiveCacheUI(findUI);

            currUI = findUI;
        }
        else
        {
            //没有找到 创建新的ui
            var newUI = CreateUIClass(uiName);

            newUI.Init();
            newUI.StartLoad();
            //加载 UI 预设 这里之后会采取先加载下一个 UI 然后再关闭上一个 UI 的方案
            //这样能保证不会有闪背景的情况出现
            LoadUI(newUI.resPath, (uiGameObject) =>
            {
                //加载完成
                newUI.LoadFinish(uiGameObject);

                //UI 对应不同层级
                if (newUI.type == UIType.Normal)
                {
                    SetNewNormalUI(newUI, uiGameObject);
                }
                else if (newUI.type == UIType.Pop)
                {
                    SetNewPopUI(newUI, uiGameObject);
                }

                loadFinishCallback?.Invoke();
            });

            currUI = newUI;
        }
        return currUI;
    }

    public void LoadUI(string path, Action<GameObject> action)
    {
        AssetManager.Instance.Load(path, (asset) =>
         {
             var prefab = asset as GameObject;
             var obj = GameObject.Instantiate(prefab);
             action?.Invoke(obj);
         }, false);
        //var gameObject = Resources.Load<GameObject>(path);
        //GameObject obj = GameObject.Instantiate(gameObject);
        //action?.Invoke(obj);
    }

    void ActiveCacheUI(BaseUI ui)
    {
        if (ui.state == UIState.Inactive)
        {
            ui.Active();
        }
        ui.Refresh();
    }

    public BaseUI CreateUIClass(string uiName)
    {

        Type classType = Type.GetType(uiName.ToString());
        if (null == classType)
        {
            Debug.LogError("zxy : the ui is not found as prefab : uiName : " + uiName);
            return null;
        }
        var newUI = (BaseUI)Activator.CreateInstance(classType);
        return newUI;
    }


    public void SetNewNormalUI(BaseUI newUI, GameObject uiGameObject)
    {
        InactiveAllUI();
        uiGameObject.transform.SetParent(normalRoot, false);
        currActiveNormalUI = newUI;
        uiGameObject.transform.SetAsLastSibling();
        AppendCacheUI(newUI);
        //注意顺序问题 这里是根据最上面 normal UI 配置信息来进行刷新标题栏的
        RefreshTitleUI();
        newUI.Open();
        ActiveCacheUI(newUI);

    }

    public void SetNewPopUI(BaseUI newUI, GameObject uiGameObject)
    {
        uiGameObject.transform.SetParent(popRoot, false);
        uiGameObject.transform.SetAsLastSibling();
        AppendCacheUI(newUI);
        newUI.Open();
        ActiveCacheUI(newUI);
    }


    public void RefreshTitleUI()
    {
        if (null == titleUI)
        {
            string titleName = UIName.TitleUI.ToString();
            var newUI = CreateUIClass(titleName);
            newUI.Init();
            newUI.StartLoad();

            LoadUI(newUI.resPath, (uiGameObject) =>
            {
                titleUI = newUI;
                uiGameObject.transform.SetParent(titleRoot, false);
                newUI.LoadFinish(uiGameObject);
                newUI.Open();
                ActiveCacheUI(newUI);
            });
        }
        else
        {
            ActiveCacheUI(titleUI);
        }
    }



    void CloseUI(BaseUI ui)
    {
        if (ui != null)
        {
            if (ui == titleUI)
            {
                //标题栏特殊关闭
                CloseTitleUI();
                return;
            }

            //正常流程 UI
            var name = ui.name;
            this.CloseUI(name);
        }
        else
        {
            Debug.Log("zxy : CloseUI : the ui is null");
        }

    }
    public void CloseUI(string name)
    {
        var findUI = FindCacheUI(name);
        if (null == findUI)
        {
            Debug.LogWarning("zxy : CloseUI the ui is not found : " + name);
            return;
        }

        if (findUI.type == UIType.Normal)
        {
            var topNormalUI = GetTopNormalUI();
            if (topNormalUI != null)
            {
                if (topNormalUI.name != name)
                {
                    Debug.LogError("zxy : CloseUI the ui is not current : " + name);
                    return;
                }
                //关闭包括该 normal 以内和上层的所有 pop
                for (int i = this.uiCacheList.Count - 1; i >= 0; i--)
                {
                    var currUI = this.uiCacheList[i];
                    currUI.Inactive();
                    currUI.Close();
                    currUI.Dispose();
                    RemoveCacheUI(currUI);

                    if (currUI == topNormalUI)
                    {
                        break;
                    }
                }

                //打开 上一个normal 和 该 normal 和 将要关闭 normal 之间的 pop
                for (int i = this.uiCacheList.Count - 1; i >= 0; i--)
                {
                    var currUI = this.uiCacheList[i];
                    ActiveCacheUI(currUI);
                    if (currUI.type == UIType.Normal)
                    {
                        currActiveNormalUI = currUI;
                        RefreshTitleUI();
                        break;
                    }
                }
            }
        }
        else if (findUI.type == UIType.Pop)
        {
            findUI.Inactive();
            findUI.Close();
            findUI.Dispose();
            RemoveCacheUI(findUI);
        }

        //如果一个 Normal 都没有了 那么关闭 titleUI 
        //不过从设计上应该是至少一个 Normal 这点注意
        var currTopNormalUI = GetTopNormalUI();
        if (null == currTopNormalUI)
        {
            this.CloseTitleUI();
        }

    }


    public void CloseCurrNormalUI()
    {
        this.CloseUI(currActiveNormalUI);
    }

    public void CloseTitleUI()
    {
        if (titleUI != null)
        {
            titleUI.Inactive();
            titleUI.Close();
            titleUI.Dispose();
            titleUI = null;
        }
        else
        {
            Debug.Log("zxy : CloseUI : the titleUI is null");
        }
    }


    public void InactiveAllUI()
    {
        for (int i = uiCacheList.Count - 1; i >= 0; i--)
        {
            var currUI = uiCacheList[i];
            if (currUI.state == UIState.Active)
            {
                currUI.Inactive();
            }
        }
    }


    BaseUI FindCacheUI(string uiName)
    {
        BaseUI ui = null;
        for (int i = uiCacheList.Count - 1; i >= 0; i--)
        {
            var currUI = uiCacheList[i];
            if (currUI.name == uiName.ToString())
            {
                ui = currUI;
                break;
            }
        }

        return ui;
    }

    void AppendCacheUI(BaseUI ui)
    {
        if (!uiCacheList.Contains(ui))
        {
            uiCacheList.Add(ui);
        }
        else
        {
            Debug.LogError("zxy : the ui will add has exist : " + ui.name);
        }
    }

    void RemoveCacheUI(BaseUI ui)
    {
        if (ui != null)
        {
            uiCacheList.Remove(ui);
        }
        else
        {
            Debug.Log("zxy : the ui is null ");
        }
    }


    public string GetCurrActiveNormalUIName()
    {
        if (this.currActiveNormalUI != null)
        {
            return this.currActiveNormalUI.name;
        }
        return "";
    }

    BaseUI GetTopNormalUI()
    {
        for (int i = this.uiCacheList.Count - 1; i >= 0; i--)
        {
            var currUI = this.uiCacheList[i];

            if (currUI.type == UIType.Normal)
            {
                return currUI;
            }
        }
        return null;
    }
}
