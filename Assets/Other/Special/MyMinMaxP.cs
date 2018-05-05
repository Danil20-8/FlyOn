using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other.Special
{
    public static class MyMinMaxP
    {
        public static T WithMin<T, Tcmp>(this T[] array, Func<T, Tcmp> selector) where Tcmp : IComparable
        {
            T min = array[0];
            Tcmp value = selector(min);
            Tcmp curr;
            for (int i = 1; i < array.Length; i++)
            {
                curr = selector(array[i]);
                if (curr.CompareTo(value) < 0)
                {
                    min = array[i];
                    value = curr;
                }
            }
            return min;
        }
        public static T WithMax<T, Tcmp>(this T[] array, Func<T, Tcmp> selector) where Tcmp : IComparable
        {
            T min = array[0];
            Tcmp value = selector(min);
            Tcmp curr;
            for (int i = 1; i < array.Length; i++)
            {
                curr = selector(array[i]);
                if (curr.CompareTo(value) > 0)
                {
                    min = array[i];
                    value = curr;
                }
            }
            return min;
        }
        public static bool WithMin<T, Tcmp>(this T[] numerable, Func<T, Tcmp> selector, Predicate<T> predicate, out T result) where Tcmp : IComparable
        {
            int i = 0;
            Tcmp curr;
            while (i < numerable.Length)
            {
                if (predicate(numerable[i]))
                    goto begin;
                i++;
            }
            result = numerable[0];
            return false;
            begin:

            T min = numerable[i];
            Tcmp value = selector(min);
            bool ok = false;
            while (i < numerable.Length)
            {
                if (predicate(numerable[i]))
                {
                    ok = true;
                    curr = selector(numerable[i]);
                    if (curr.CompareTo(value) < 0)
                    {
                        min = numerable[i];
                        value = curr;
                    }
                }
                i++;
            }
            if (ok)
            {
                result = min;
                return true;
            }
            result = numerable[0];
            return false;
        }
        public static int IndexWithMin<T, Tcmp>(this T[] array, Func<T, Tcmp> selector) where Tcmp : IComparable
        {
            int min = 0;
            Tcmp value = selector(array[0]);
            Tcmp curr;
            for (int i = 1; i < array.Length; i++)
            {
                curr = selector(array[i]);
                if (curr.CompareTo(value) < 0)
                {
                    min = i;
                    value = curr;
                }
            }
            return min;
        }
        public static int IndexWithMax<T, Tcmp>(this T[] array, Func<T, Tcmp> selector) where Tcmp : IComparable
        {
            int min = 0;
            Tcmp value = selector(array[0]);
            Tcmp curr;
            for (int i = 1; i < array.Length; i++)
            {
                curr = selector(array[i]);
                if (curr.CompareTo(value) > 0)
                {
                    min = i;
                    value = curr;
                }
            }
            return min;
        }
    }
}
