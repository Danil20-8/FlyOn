using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public static class Blender
    {
        public static void Blend<T1, T2>(this T1[] left, T2[] right, Action<T1, T2> action)
        {
            for (int i = 0; i < left.Length; i++)
                action(left[i], right[i]);
        }
        public static void Blend<T1, T2>(this List<T1> left, List<T2> right, Action<T1, T2> action)
        {
            for (int i = 0; i < left.Count; i++)
                action(left[i], right[i]);
        }
        public static void Blend<T1, T2>(this IEnumerable<T1> left, IEnumerable<T2> right, Action<T1, T2> action)
        {
            for (int i = 0; i < left.Count(); i++)
                action(left.ElementAt(i), right.ElementAt(i));
        }
        public static IEnumerable<T> Select<T, T1, T2>(this IEnumerable<T1> left, IEnumerable<T2> right, Func<T1, T2, T> func)
        {
            return (IEnumerable<T>)new TwoSelect<T1, T2, T>(left, right, func);
        }
        public static IEnumerable<Tresult> ESum<T, Tresult>(this IEnumerable<IEnumerable<T>> self, Func<IEnumerable<T>, Tresult> selector)
        {
            List<Tresult> r = new List<Tresult>();
            List<T> temp = new List<T>();
            var nums = self.Select(e => e.GetEnumerator()).ToList();
            bool more = true;
            while (true)
            {
                more = false;
                foreach (var i in nums)
                    if (i.MoveNext())
                    {
                        temp.Add(i.Current);
                        more = true;
                    }
                if (!more)
                    break;
                r.Add(selector(temp));
                temp.Clear();
            }
            foreach (var n in nums)
                n.Dispose();
            return r;
        }
    }

    public class TwoSelect<T1, T2, T> : IEnumerable<T>
    {

        IEnumerable<T1> left;
        IEnumerable<T2> right;
        Func<T1, T2, T> func;

        public TwoSelect(IEnumerable<T1> left, IEnumerable<T2> right, Func<T1, T2, T> func)
        {
            this.left = left;
            this.right = right;
            this.func = func;
        }

        public IEnumerator GetEnumerator()
        {
            var l = left.GetEnumerator();
            var r = right.GetEnumerator();
            while (l.MoveNext() && r.MoveNext())
                yield return func(l.Current, r.Current);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var l = left.GetEnumerator();
            var r = right.GetEnumerator();
            while (l.MoveNext() && r.MoveNext())
                yield return func(l.Current, r.Current);
        }
    }

    public static class MyEnumerable
    {
        public static T FirstWN<T>(this IEnumerable<T> self) where T : class
        {
            var e = self.GetEnumerator();
            if (e.MoveNext())
                return e.Current;
            else
                return null;
        }
    }
}
