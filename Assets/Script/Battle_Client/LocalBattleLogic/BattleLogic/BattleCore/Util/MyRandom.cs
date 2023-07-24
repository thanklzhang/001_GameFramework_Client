using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    public class MyRandom
    {
        public static float Next(float a, float b, Random rand = null)
        {
            float c = b - a;
            rand = rand == null ? new Random() : rand;
            return (float)(rand.NextDouble() * c + a);

        }

        public static int Next(int a, int b, Random rand = null)
        {
            int c = b - a;
            int dir = Math.Sign(c);
            rand = rand == null ? new Random() : rand;
            var randNum = dir * rand.Next(dir * c);
            return randNum + a;
        }
    }

}