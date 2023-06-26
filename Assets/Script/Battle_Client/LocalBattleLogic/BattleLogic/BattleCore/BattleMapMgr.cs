using System;
using System.Collections.Generic;
namespace Battle
{


    public class MapInitArg
    {
        //public int[][] map;
        //public int mapSizeX;
        //public int mapSizeZ;

        public List<List<int>> mapList;

    }



    public enum MapNodeType
    {
        Null = 0,
        //正常节点 可移动
        Normal = 1,
        //障碍 不能移动
        Obstacle = 2
    }

    //public class MapNode
    //{
    //    public int x;
    //    public int y;

    //    public MapNodeType type;
    //}

    public class BattleMapMgr
    {
        int mapSizeX;
        int mapSizeZ;

        public int MapSizeX { get => mapSizeX; }
        public int MapSizeZ { get => mapSizeZ; }

        Map map;
        //MapData mapData;
        public void Init(MapInitArg arg)
        {
            //var mapInfo = arg.map;

            //mapData = new MapData();
            //mapData.Load(mapInfo);

            //this.mapSizeX = arg.mapSizeX;
            //this.mapSizeZ = arg.mapSizeZ;

            map = new Map();

            //test
            //var mapInfo = new int[][]
            //{
            //    //new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,1,},
            //    //new int[]{ 1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,},
            //    //new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},

            //    new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,},
            //    new int[]{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,},


            //};

            //显示数组转换成数据数组
            //var logicArray = arg.mapList;
            var list = arg.mapList;
            //List<List<int>> logicArray = new List<List<int>>();

            //for (int i = 0; i < list[i].Count; i++)
            //{
            //    logicArray.Add(new List<int>());
            //    for (int j = 0; j < list.Count; j++)
            //    {
            //        var state = list[j][list[i].Count - i - 1];
            //        logicArray[i].Add(state);
            //    }
            //}


            //map.Init(logicArray);

            map.Init(list);
        }

        internal void Update()
        {

        }

        internal Map GetMap()
        {
            return map;
        }

        public bool IsOutOfMap(int x, int y)
        {
            return map.IsOutOfMap(x, y);
        }

        public bool IsObstacle(int x, int y)
        {
            return map.IsObstacle(x, y);
        }
    }

}

