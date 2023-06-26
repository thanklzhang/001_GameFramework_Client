using System;
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
    }
}
