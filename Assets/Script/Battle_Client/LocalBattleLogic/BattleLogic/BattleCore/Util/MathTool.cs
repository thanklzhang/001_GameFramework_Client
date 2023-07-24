﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class MathTool
    {
        //角度 -> 弧度
        public static float Deg2Rad = (float)(Math.PI / 180);
        //弧度 -> 角度
        public static float Rad2Deg = 1.0f / Deg2Rad;
        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                value = max;
            if (value < min)
                value = min;
            return value;
        }
        
        // 计算两点之间的距离
        public static float DistanceBetweenTwoPoints(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        // 计算点(x, y)到经过两点(x1, y1)和(x2, y2)的直线的距离
        public static float DistanceFromPointToLine(float x, float y, float x1, float y1, float x2, float y2)
        {
            float a = y2 - y1;
            float b = x1 - x2;
            float c = x2 * y1 - x1 * y2;

            //assert(Math.Abs(a) > 0.00001f || Math.Abs(b) > 0.00001f);

            return (float)(Math.Abs(a * x + b * y + c) / Math.Sqrt(a * a + b * b));
        }
        
    }
}
