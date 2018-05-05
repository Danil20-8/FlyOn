using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public class FastList<T> : ICollection<T>
    {
        T[] items;
        int length = 0;
        public int Count { get { return length; } }

        public bool IsReadOnly{get{return false;}}

        public T this [int index] { get { return items[index]; } set { items[index] = value; } }

        public FastList(int capacity)
        {
            items = new T[capacity];
        }
        public FastList()
        {
            items = new T[0];
        }

        public void Add(T item)
        {
            if(length == items.Length)
                Algs.IncreseArray(ref items, (length + 1) * 2);

            items[length++] = item;
        }
        public void Remove(int index)
        {
            items[index] = items[--length];
        }
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            else
            {
                Remove(index);
                return true;
            }
        }
        public int IndexOf(T item)
        {
            for (int i = 0; i < length; i++)
                if (item.Equals(items[i]))
                    return i;
            return -1;
        }
        public void Clear()
        {
            length = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < length; i++)
                yield return items[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            foreach (var e in this)
                if (e.Equals(item))
                    return true;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < length; i++)
                array[arrayIndex + i] = items[i];
        }
    }
}
