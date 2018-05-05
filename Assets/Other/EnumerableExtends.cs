using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public static class EnumerableExtends
    {
        public static float Psum<T>(this IEnumerable<T> self, Func<T, float> selector)
        {
            var fs = self.Select(t => selector(t)).ToList();
            float m = fs.First();
            foreach (var f in fs.Skip(1))
                m *= f;
            return m / Enumerable.Range(0, fs.Count).Select(i => m / fs[i]).Sum();
        }
        public static float Psum(this IEnumerable<float> self)
        {
            float m = self.First();
            foreach (var f in self.Skip(1))
                m *= f;
            return m / Enumerable.Range(0, self.Count()).Select(i => m / self.ElementAt(i)).Sum();
        }
        public static float IPsum(this IEnumerable<float> self)
        {
            float m = self.First();
            foreach (var f in self.Skip(1))
                m *= f;
            return Enumerable.Range(0, self.Count()).Select(i => m / self.ElementAt(i)).Sum() / m;
        }
        public static CacheEnumerable<T> Cache<T>(this IEnumerable<T> self)
        {
            return new CacheEnumerable<T>(self);
        }
        public static T FirstOrNull<T>(this IEnumerable<T> self, Predicate<T> predicate) where T : class
        {
            foreach (var s in self)
                if (predicate(s))
                    return s;
            return null;
        }
        public static T FirstOrDefault<T>(this IEnumerable<T> self, Predicate<T> predicate, T defaultValue)
        {
            foreach (var s in self)
                if (predicate(s))
                    return s;
            return defaultValue;
        }
        public static bool First<T>(this IEnumerable<T> self, Predicate<T> predicate, out T result)
        {
            foreach (var s in self)
                if (predicate(s))
                {
                    result = s;
                    return true;
                }
            result = self.First();
            return false;
        }
        public static int[] GetIndeciesOf<T>(this IEnumerable<T> self, Predicate<T> predicate)
        {
            int i = 0;
            List<int> inds = new List<int>();
            foreach(var e in self)
            {
                if (predicate(e))
                    inds.Add(i);
                i++;
            }
            return inds.ToArray();
        }
    }
}
