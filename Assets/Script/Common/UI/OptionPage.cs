using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionPage : MonoBehaviour
{
    List<Toggle> optionList;
    public Transform pageRoot;
    // Use this for initialization
    void Start()
    {
        optionList = transform.GetComponentsInChildren<Toggle>().ToList();

        for (int i = 0; i < optionList.Count; ++i)
        {
            var option = optionList[i];
            var page = pageRoot.GetChild(i).gameObject;
            option.onValueChanged.AddListener((isOn) =>
            {
                page.SetActive(isOn);
            });

        }

        //默认
        if (optionList.Count > 0 && pageRoot.childCount > 0)
        {
            optionList[0].isOn = true;
            pageRoot.GetChild(0).gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
