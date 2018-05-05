using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Algoriphms;
using MyLib.Modern;
using Assets.Other.Special;

using Random = System.Random;

namespace Assets.Other
{
    public static class Algs
    {
        public static int[] Split(int lenght, params float[] weights)
        {
            float[] r = new float[weights.Length];
            var s = weights.Sum();
            float d = lenght / s;
            for (int i = 0; i < weights.Length; i++)
                r[i] = d * weights[i];

            int[] ri = r.Select(f => (int)f).ToArray();
            int sr = ri.Sum();
            if(lenght != sr)
            {
                int miss = lenght - sr;
                var fi = r.Select(ri, (f, i) => f - i).ToArray();
                var et = fi.Select((f, i) => new Tuple<float, int>(f, i)).OrderBy(t => t.Item1).ToArray();
                for (int i = 0; i < miss; i++)
                    ri[et[i].Item2] += 1;
            }
            return ri;
        }

        public static T[] Sqare<T>(this T[] self, int width, int height, int index, int size)
        {
            List<T> r = new List<T>();
            int x = (index % width) - size;
            int y = index / width - size;
            size = size * 2 + 1;
            for(int i = 0; i < size; i++)
                for(int j = 0; j < size; j++)
                {
                    int px = x + i;
                    int py = y + j;
                    if (px >= 0 && px < width && py >= 0 && py < height)
                        r.Add(self[px + width * py]);
                }
            return r.ToArray();
        }
        public static float Mid(this float[] self)
        {
            return self.Sum() / self.Length;
        }
        public static float[] Esum(this float[][] self)
        {
            var it = self.Select(e => e.GetEnumerator()).ToList();
            float[] r = new float[self[0].Length];
            int i = 0;
            while(it.All(e => e.MoveNext()))
            {
                r[i] = it.Sum(v => (float)v.Current);
                i++;
            }
            return r;
        }
        public static void IncreseArray<T>(ref T[] arr, int new_size)
        {
            T[] t = new T[new_size];
            for (int i = 0; i < arr.Length; i++)
                t[i] = arr[i];
            arr = t;
        }

        public static void ShakerSort<T>(T[] array, float[] keys)
        {
            int left = 0;
            int right = array.Length - 1;

            float t_key;
            T t_element;
            int t_next;
            while (left <= right)
            {
                for (int i = left; i < right; i++)
                {
                    t_next = i + 1;
                    if (keys[i] > keys[t_next])
                    {
                        t_key = keys[i];
                        keys[i] = keys[t_next];
                        keys[t_next] = t_key;

                        t_element = array[i];
                        array[i] = array[t_next];
                        array[t_next] = t_element;
                    }
                }
                right--;

                for (int i = right; i > left; i--)
                {
                    t_next = i - 1;
                    if (keys[t_next] > keys[i])
                    {
                        t_key = keys[i];
                        keys[i] = keys[t_next];
                        keys[t_next] = t_key;

                        t_element = array[i];
                        array[i] = array[t_next];
                        array[t_next] = t_element;
                    }
                }
                left++;
            }
        }

        public static bool IsNaN(this Vector3 v)
        {
            return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
        }

        public static Color[] GenerateColorArray(Color[] colors, int width, int height, int smooth, Random random)
        {
            var t = Enumerable.Range(0, width * height).Select(i => Enumerable.Range(0, colors.Length).Select(j => random.NextDouble()).ToArray()).ToArray();
            return Enumerable.Range(0, width * height).Select(i => t.Sqare(width, height, i, 3).ESum(es => es.Sum() / es.Count()).ToArray())
                .Select(w => colors[w.IndexWithMax(f => f)]).ToArray();
        }
    }

    public struct DistributionCube
    {
        public float width, height, depth;
        public int widthN, heightN, depthN;

        public float widthD { get { return width / widthN; } }
        public float heightD { get { return height / heightN; } }
        public float depthD { get { return depth / depthN; } }

        public Vector3 size { get { return new Vector3(width, height, depth); } }
        public Vector3 distribution { get { return new Vector3(widthN, heightN, depthN); } }
        public Vector3 interval { get { return new Vector3(widthD, heightD, depthD); } }

        public int cubesAmount { get { return widthN * heightN * depthN; } }

        public IEnumerable<Vector3> Distribute(Vector3 position, Vector3 up, Vector3 right, Vector3 forward)
        {
            return new CustomEnumerable<Vector3>(GetEnumerator(position, up, right, forward));
        }

        IEnumerator<Vector3> GetEnumerator(Vector3 position, Vector3 up, Vector3 right, Vector3 forward)
        {
            Vector3 xInterval = widthD * right;
            Vector3 yInterval = heightD * up;
            Vector3 zInterval = depthD * forward;

            Vector3 cAx = position;
            for (int i = 0; i < widthN; i++, cAx += xInterval)
            {
                Vector3 cAxAy = cAx;
                for (int j = 0; j < heightN; j++, cAxAy += yInterval)
                {
                    Vector3 cAxAyAz = cAxAy;
                    for (int k = 0; k < depthN; k++, cAxAyAz += zInterval)
                    {
                        yield return cAxAyAz;
                    }
                }
            }
        }

        public void Distribute(Vector3 position, Vector3 up, Vector3 right, Vector3 forward, Action<int, int, int, Vector3> action)
        {
            var pos = GetEnumerator(position, up, right, forward);
            for (int i = 0; i < widthN; i++)
                for (int j = 0; j < heightN; j++)
                    for (int k = 0; k < depthN; k++)
                    {
                        pos.MoveNext();
                        action(i, j, k, pos.Current);
                    }
        }

        public DistributionCube(float width, float height, float depth, int widthN, int heightN, int depthN)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;

            this.widthN = widthN;
            this.heightN = heightN;
            this.depthN = depthN;
        }

        public DistributionCube(Vector3 size, Vector3 xyzAmount) : this(size.x, size.y, size.z, (int)xyzAmount.x, (int)xyzAmount.y, (int)xyzAmount.z)
        { }
    }
}
