using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MaterialTest : MonoBehaviour
{
    public int grayValue = 0;
    public Material originMat;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        // mat = this.GetComponent<Image>().material;

        mat = new Material(originMat);
        this.GetComponent<Image>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_GrayValue",grayValue);
    }
}
