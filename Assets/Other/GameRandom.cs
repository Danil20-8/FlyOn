using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public static class GameRandom
    {
        static Random random;

        public static float value { get {  return Float / float.MaxValue; } }

        public static int range(int minValue, int maxValue) {  return Int % (maxValue - minValue) + minValue; }
        public static int range(int maxValue) { return Int % maxValue; }

        public static float range(float minValue, float maxValue) { return Float % (maxValue - minValue) + minValue; }
        public static float range(float maxValue) { return Float % maxValue; }

        public static float Float { get { return (float)Double; } }
        public static double Double { get { var r = BitConverter.ToDouble(bytes, 0); return r < 0 ? -r : r; } }
        public static int Int { get { var r = BitConverter.ToInt32(bytes, 0); return r < 0 ? -r : r; } }

        static object randomLock = new object();
        static byte[] bytes { get { lock(randomLock) { byte[] bs = new byte[4]; random.NextBytes(bs); return bs; } } }
    }
}

