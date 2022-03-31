using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
namespace PlotDesigner.Runtime
{
    public class PlotAnimationTrackPlayer : PlotTrackPlayer
    {
        AnimationTrack animationTrack;

        public override void Init(Track trackData)
        {
            base.Init(trackData);
            this.animationTrack = (AnimationTrack)trackData;
        }

        public override void Execute()
        {

        }

        public override void Update(float timeDelta)
        {

        }
    }
}

