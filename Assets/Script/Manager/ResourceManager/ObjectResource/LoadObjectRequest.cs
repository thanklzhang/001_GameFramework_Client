using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
public interface LoadObjectRequest
{
    bool CheckFinish();
    void Start(string[] pathList);
    void Finish();
}

