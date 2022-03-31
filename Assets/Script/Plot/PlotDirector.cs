using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    //剧情总指导
    public class PlotDirector
    {

        PlotPlayer plotPlayer;

        public void Init()
        {
            plotPlayer = new PlotPlayer();
            plotPlayer.Init();
        }

        public void StartPlot(Plot plot)
        {
            plotPlayer.SetPlot(plot);

            LoadRes();
        }

        public void LoadRes()
        {

        }

        //所需资源加载完毕
        public void OnLoadFinish()
        {
            Execute();
        }

        public void Execute()
        {
            plotPlayer.Execute();
        }

        public void Update(float timeDelta)
        {
            
        }

    }
}

