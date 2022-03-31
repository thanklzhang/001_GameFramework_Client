using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    //单个剧情播放者
    public class PlotPlayer
    {
        Plot plotData;
        List<PlotTrackPlayer> trackPlayerList;

        string assembleName;
        public void Init()
        {
            trackPlayerList = new List<PlotTrackPlayer>();
            assembleName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        internal void SetPlot(Plot plot)
        {
            this.plotData = plot;

            for (int i = 0; i < plot.trackDataList.Count; i++)
            {
                var trackData = plot.trackDataList[i];

                PlotTrackPlayer trackPlayer = CreateTrackPlayerByTrackDataType(trackData.GetType());
                if (trackPlayer != null)
                {
                    trackPlayer.Init(trackData);
                    trackPlayerList.Add(trackPlayer);
                }
                else
                {
                    var trackPlayerName = "Plot" + trackData.GetType().Name + "Player";
                    Logx.LogError("PlotPlayer : SetPlot : create object fail , the type of src is : " + trackData.GetType() + ", perhaps the trackPlayer is not exist : " + trackPlayerName);
                }
            }
        }

        private PlotTrackPlayer CreateTrackPlayerByTrackDataType(Type type)
        {
            var trackPlayerName = "Plot" + type.Name + "Player";
            var nameSpaceName = "PlotDesigner.Runtime";
            var fullName = nameSpaceName + "." + trackPlayerName;
            PlotTrackPlayer trackPlayer = null;
            Logx.Log("CreateInstance : " + assembleName + " : " + fullName);
            trackPlayer = Assembly.Load(assembleName).CreateInstance(fullName) as PlotTrackPlayer;
            return trackPlayer;
        }

        internal void Execute()
        {
            for (int i = 0; i < trackPlayerList.Count; i++)
            {
                var trackPlayer = trackPlayerList[i];
                trackPlayer.Execute();
            }
        }

        public void Update(float timeDelta)
        {
            for (int i = 0; i < trackPlayerList.Count; i++)
            {
                var trackPlayer = trackPlayerList[i];
                trackPlayer.Update(timeDelta);
            }
        }


    }
}

