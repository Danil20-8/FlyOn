using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using MyLib.Modern;

namespace Assets.Other
{
    public abstract class DoubleList<Tmain, Tsecond> : ICollection<Tmain>, IEnumerable<Tuple<Tmain, Tsecond>>
    {
        List<Tuple<Tmain, Tsecond>> list = new List<Tuple<Tmain, Tsecond>>();

        public Tuple<Tmain, Tsecond> this [int index] { get { return list[index]; } set { list[index] = value; } }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public abstract void Add(Tmain item);
        protected void Add(Tmain main, Tsecond second)
        {
            list.Add(new Tuple<Tmain, Tsecond> { Item1 = main, Item2 = second });
        }
        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Tmain item)
        {
            foreach (var e in list)
                if (e.Item1.Equals(item))
                    return true;

            return false;
        }

        public void CopyTo(Tmain[] array, int arrayIndex)
        {
            int minLength = Math.Min(array.Length - arrayIndex, list.Count);
            for (int i = 0; i < minLength; i++, arrayIndex++)
                array[arrayIndex] = list[i].Item1;
        }

        IEnumerator<Tmain> IEnumerable<Tmain>.GetEnumerator()
        {
            foreach (var e in list)
                yield return e.Item1;
        }

        public bool Remove(Tmain item)
        {
            int i = 0;
            foreach (var e in list)
            {
                if (e.Item1.Equals(item))
                {
                    list.RemoveAt(i);
                    return true;
                }
                i++;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Tuple<Tmain, Tsecond>> GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
