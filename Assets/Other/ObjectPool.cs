using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other.Special;
using Assets.Other;
namespace Assets.Other
{
    public class ObjectPool<T> : IEnumerable<T> where T : class, ICloneable<T>
    {
        T prototype;
        T[] pool;
        int length;

        public T this[int index] { get { return pool[index]; } }

        public ObjectPool(T prototype, int capacity)
        {
            this.prototype = prototype;
            pool = new T[capacity];
            InitNew();
        }

        public T Get()
        {
            if (length == pool.Length)
            {
                Algs.IncreseArray(ref pool, (length + 1) * 2);
                InitNew();
            }

            return pool[length++];
        }
        public IEnumerable<T> Get(int amount)
        {
            if(length + amount - 1 >= pool.Length)
            {
                Algs.IncreseArray(ref pool, (length + amount) + (length + 1) * 2);
                InitNew();
            }
            int begin = length;
            length += amount;
            return new CustomEnumerable<T>(GetEnumerator(begin));
        }
        void InitNew()
        {
            for (int i = length; i < pool.Length; i++)
                pool[i] = prototype.Clone();
        }
        public void Release(int index)
        {
            var t = pool[index];
            pool[index] = pool[--length];
            pool[length] = t;
        }

        public bool Release(T obj)
        {
            int index = IndexOf(obj);
            if(index != -1)
            {
                Release(index);
                return true;
            }
            return false;
        }

        public int IndexOf(T obj)
        {
            for (int i = 0; i < length; i++)
                if (pool[i] == obj)
                    return i;
            return -1;
        }

        public void ReleaseAll()
        {
            length = 0;
        }

        public void Clear()
        {
            pool = new T[0];
            length = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < length; i++)
                yield return pool[i];
        }
        public IEnumerator<T> GetEnumerator(int begin)
        {
            for (int i = begin; i < length; i++)
                yield return pool[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
