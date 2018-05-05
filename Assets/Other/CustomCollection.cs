using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public struct CustomCollection<T> : ICollection<T>
    {
        public Func<int> count;
        public Func<bool> isReadOnly;
        public Action<T> add;
        public Action clear;
        public Func<T, bool> contains;
        public Action<T[], int> arrayIndex;
        public Func<T, bool> remove;
        public Func<IEnumerator<T>> getEnumerator;

        public int Count
        {
            get
            {
                return count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return isReadOnly();
            }
        }

        public void Add(T item)
        {
            add(item);
        }

        public void Clear()
        {
            clear();
        }

        public bool Contains(T item)
        {
            return contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return getEnumerator();
        }

        public bool Remove(T item)
        {
            return remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return getEnumerator();
        }
    }
}
