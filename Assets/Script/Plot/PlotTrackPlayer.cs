using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    //单个剧情轨道播放者
    public class PlotTrackPlayer
    {
        Track trackData;
        public virtual void Init(Track trackData)
        {
            this.trackData = trackData;
        }

        public  virtual void Execute()
        {
            
        }

        public virtual void Update(float timeDelta)
        {
            
        }
    }
}

