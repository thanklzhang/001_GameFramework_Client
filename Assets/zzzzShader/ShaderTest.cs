﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
        var image = this.GetComponent<Image>();
        Debug.Log("depth: " + image.gameObject.name + " " + image.depth);
    }
}
