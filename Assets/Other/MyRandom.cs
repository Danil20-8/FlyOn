using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Other
{
    public class MyRandom
    {
        public static T OneOf<T>(params T[] items)
        {
            return items[Random.Range(0, items.Length)];
        }
        public static T OneOf<T>(params LimitedProperty<T>[] items)
        {
            var ps = items.Where(p => p.IsAble()).ToArray();
            return ps[Random.Range(0, ps.Length)].Value;
        }
    }
    public class MyRandom<T>
    {
        T[] values;
        public MyRandom(params T[] values)
        {
            this.values = values;
        }
        public static implicit operator T(MyRandom<T> random)
        {
            return MyRandom.OneOf(random.values);
        }
    }

    public class LimitedRandomList<T>: IEnumerable<T>
    {
        LimitedProperty<T>[] items;
        public int count { get { return items.Sum(i => i.Count); } }
        public LimitedRandomList(LimitedProperty<T>[] items)
        {
            this.items = items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int n = count;
            for(int i = 0; i < n; i++)
            {
                yield return MyRandom.OneOf(items);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
