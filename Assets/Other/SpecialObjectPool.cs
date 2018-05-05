using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other.Special;
namespace Assets.Other
{
    public class SpecialObjectPool<T> : IEnumerable<T> where T : class, IPoolable<T>
    {
        T prototype;
        T[] pool;
        int length;

        public SpecialObjectPool(T prototype)
        {
            pool = new T[0];
            this.prototype = prototype;
        }

        public SpecialObjectPool(T prototype, int capacity)
        {
            this.prototype = prototype;
            pool = new T[capacity];
            InitNew();
        }

        public T Get()
        {
            if(length == pool.Length)
            {
                Algs.IncreseArray(ref pool, (length + 1) * 2);
                InitNew();
            }
            var r = pool[length++];
            r.Reset();
            return r;
        }
        public IEnumerable<T> Get(int amount)
        {
            if (length + amount - 1 >= pool.Length)
            {
                Algs.IncreseArray(ref pool, (length + amount) + (length + 1) * 2);
                InitNew();
            }
            int begin = length;
            length += amount;
            return new CustomEnumerable<T>(ResetAndGetEnumerator(begin));
        }

        public void Release(int index)
        {
            pool[index].Disable();

            var t = pool[index];
            pool[index] = pool[--length];
            pool[length] = t;
        }
        public bool Release(T obj)
        {
            int index = IndexOf(obj);
            if (index != -1)
            {
                Release(index);
                return true;
            }
            return false;
        }
        int IndexOf(T obj)
        {
            for (int i = 0; i < length; i++)
                if (pool[i] == obj)
                    return i;
            return -1;
        }
        public void ReleaseAll()
        {
            foreach (var o in pool)
                o.Disable();
            length = 0;
        }

        public void Clear()
        {
            foreach (var o in pool)
                o.Destroy();
            pool = new T[0];
            length = 0;
        }

        void InitNew()
        {
            for(int i = length; i < pool.Length; i++)
            {
                pool[i] = prototype.Clone();
                pool[i].Disable();
            }
        }

        IEnumerator<T> ResetAndGetEnumerator(int begin)
        {
            for (int i = begin; i < length; i++)
            {
                pool[i].Reset();
                yield return pool[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < length; i++)
                yield return pool[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return pool.GetEnumerator();
        }
    }

    public interface IPoolable<T> : ICloneable<T>
    {
        void Reset();
        void Disable();
        void Destroy();
    }
}
