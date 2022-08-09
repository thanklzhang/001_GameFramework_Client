using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathNodeType
{
    Normal = 0,
    Obstacle = 1
}
public class PathNode
{
    public int id;

    public int x;
    public int y;

    public PathNodeType nodeType;
}


public class Map
{
    public const float CellWidth = 1;
    public const float CellHeight = 1;

    public List<List<PathNode>> mapNodes;

    int[][] mapInfo;

    public void Init(int[][] map)
    {
        mapInfo = map;

        mapNodes = new List<List<PathNode>>();

        int width = mapInfo[0].Length;
        int height = mapInfo.Length;
        for (int i = 0; i < width; i++)
        {
            List<PathNode> nodeList = new List<PathNode>();
            for (int j = 0; j < height; j++)
            {
                PathNode node = new PathNode();
                node.x = i;
                node.y = j;

                node.nodeType = (PathNodeType)(mapInfo[mapInfo.Length - j - 1][i]);
                nodeList.Add(node);
            }
            mapNodes.Add(nodeList);
        }
    }
}
