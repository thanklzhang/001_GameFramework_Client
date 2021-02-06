using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public enum ActionNodeType
{
    Move = 1,
    PlayAnimation = 2,
    AddEffect = 3
}
public class TimelineSequence 
{
    public List<BaseNode> nodeList = new List<BaseNode>();
}