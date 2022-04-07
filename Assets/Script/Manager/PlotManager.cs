using PlotDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
public class PlotManager : Singleton<PlotManager>
{
    public PlotPlayer plotPlayer;

    public Transform plotRoot;
    public Transform plotMain;
    public Transform plotUIRoot;

    public void Init()
    {
        plotRoot = GameObject.Find("PlotRoot").transform;
       ;

        plotPlayer = new PlotPlayer();
        plotPlayer.Init();
        plotPlayer.SetPlotRoot(plotRoot);
    }

    public void StartPlot(string plotName)
    {
        //plog_main_task_001
        var file = plotName + ".json";
        var plotConfigFolderPath = Application.dataPath + "/BuildRes/PlotConfig";
        var path = Path.Combine(plotConfigFolderPath, file);
        var plot = JsonTool.LoadObjectFromFile<Plot>(path);
        Logx.Log("path : " + path);
        plotPlayer.StartPlot(plot);

    }

    public bool IsRunning()
    {
        return this.plotPlayer.IsRunning();
    }

    public void Update(float deltaTime)
    {
        plotPlayer.Update(deltaTime);
    }
}
