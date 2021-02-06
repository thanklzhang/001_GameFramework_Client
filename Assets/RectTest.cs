using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTest : MonoBehaviour
{
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        this.rect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("zxy : position : " + this.rect.position);
        Debug.Log("zxy : local position : " + this.rect.localPosition);
        Debug.Log("zxy : anchor position : " + this.rect.anchoredPosition);
        Debug.Log("zxy : offsetMax : " + this.rect.offsetMax);
        Debug.Log("zxy : offsetMin : " + this.rect.offsetMin);
        Debug.Log("zxy : anchorMin : " + this.rect.anchorMin);
        Debug.Log("zxy : anchorMax : " + this.rect.anchorMax);
        Debug.Log("zxy : pivot : " + this.rect.pivot);

    }
}
