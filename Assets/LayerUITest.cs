using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Layer1DataObj
{
    public Layer1Data data;
    public Transform layer2Root;
    public GameObject gameObject;
    public List<Layer2DataObj> layer2List = new List<Layer2DataObj>();

    public Text nameText;
    public Button btn;

    public bool isOpenLayer2;

    public float selfOrgHeight;

    public OpenState openState;

    public enum OpenState
    {
        Idle = 0,
        Go = 1,
        //Reach = 2,
        Return = 3,
    }

    public void Awake(GameObject obj)
    {


        this.gameObject = obj;

        selfOrgHeight = this.gameObject.GetComponent<RectTransform>().rect.height;

        layer2Root = this.gameObject.transform.Find("layer2Root");
        nameText = this.gameObject.transform.Find("Text").GetComponent<Text>();
        btn = this.gameObject.transform.Find("Image").GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            isOpenLayer2 = !isOpenLayer2;
            this.SetOpenState();

        });
    }

    void SetOpenState()
    {
        var rectT = this.gameObject.GetComponent<RectTransform>();
        if (isOpenLayer2)
        {
            if (openState == OpenState.Idle)
            {
                openState = OpenState.Go;
            }

        }
        else
        {
            if (openState == OpenState.Idle)
            {
                openState = OpenState.Return;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(layer2Root.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.gameObject.transform.parent.GetComponent<RectTransform>());
    }

    public float totalTime = 0.25f;
    public float currTimer = 0.0f;

    public void Update()
    {
        var rectT = this.gameObject.GetComponent<RectTransform>();



        if (openState == OpenState.Go || openState == OpenState.Return)
        {
            int dir = 1;
            if (openState == OpenState.Return)
            {
                dir = -1;
            }

            currTimer = currTimer + Time.deltaTime;

            var targetValue = layer2List.Count * 70;
            var ratio = Mathf.Clamp(currTimer / totalTime, 0, 1);
            var ratioByDir = (dir * ratio - 0.5f * (dir - 1));//x or 1 - x
            var nowValue = targetValue * ratioByDir;

            rectT.sizeDelta = new Vector2(rectT.sizeDelta.x, selfOrgHeight + nowValue);
            layer2Root.transform.localScale = new Vector3(1, ratioByDir, 1);


            if (currTimer >= totalTime)
            {
                currTimer = 0.0f;
                openState = OpenState.Idle;
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(layer2Root.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.gameObject.transform.parent.GetComponent<RectTransform>());
    }

    internal void RefreshInfo(Layer1Data l1)
    {

        this.data = l1;

        nameText.text = "" + this.data.name;

        var dataList = this.data.list;
        for (int i = 0; i < dataList.Count; i++)
        {
            var l2Data = dataList[i];
            GameObject obj = null;
            if (i < layer2Root.childCount)
            {
                obj = this.layer2Root.GetChild(i).gameObject;
            }
            else
            {
                var temp = this.layer2Root.GetChild(0).gameObject;
                obj = GameObject.Instantiate(temp, this.layer2Root, false);
            }

            Layer2DataObj dataObj = new Layer2DataObj();
            dataObj.Awake(obj);
            dataObj.RefreshInfo(l2Data);

            layer2List.Add(dataObj);
        }

        isOpenLayer2 = false;
        this.SetOpenState();
    }
}

public class Layer1Data
{
    public string name;
    public List<Layer2Data> list = new List<Layer2Data>();
}


public class Layer2Data
{
    public string name;
}

public class Layer2DataObj
{
    public GameObject gameObject;
    public Layer2Data data;
    public Text nameText;
    public void Awake(GameObject obj)
    {
        this.gameObject = obj;

        nameText = this.gameObject.transform.Find("Text").GetComponent<Text>();
    }

    internal void RefreshInfo(Layer2Data l2Data)
    {
        this.data = l2Data;

        nameText.text = this.data.name;
    }
}

public class LayerUITest : MonoBehaviour
{
    Transform layer1Root;
    List<Layer1DataObj> dataObjList = new List<Layer1DataObj>();

    private void Awake()
    {
        layer1Root = this.transform.Find("scroll/mask/layer1Root");
    }

    // Start is called before the first frame update
    void Start()
    {

        //fill data
        var dataList = new List<Layer1Data>();

        for (int i = 0; i < 10; i++)
        {
            Layer1Data l1 = new Layer1Data()
            {
                name = "Layer1_" + i
            };

            l1.list = new List<Layer2Data>();
            for (int j = 0; j < 5; j++)
            {
                Layer2Data l2 = new Layer2Data()
                {
                    name = "Layer2_" + i + "_" + j
                };
                l1.list.Add(l2);
            }

            dataList.Add(l1);
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            var l1 = dataList[i];
            var dataObj = new Layer1DataObj();

            GameObject obj = null;
            if (i < this.layer1Root.childCount)
            {
                obj = this.layer1Root.GetChild(i).gameObject;
            }
            else
            {
                obj = GameObject.Instantiate(this.layer1Root.GetChild(0).gameObject, this.layer1Root, false);
            }

            dataObj.Awake(obj);
            dataObj.RefreshInfo(l1);
            dataObjList.Add(dataObj);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in dataObjList)
        {
            item.Update();
        }
    }
}
