using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public class GameObjectPool<T> where T : UnityEngine.MonoBehaviour
    {
        T original;
        T[] pool = new T[0];
        int count = 0;

        public GameObjectPool(T original)
        {
            this.original = original;
        }

        public IEnumerable<T> Get(int amount)
        {
            if(pool.Length < amount)
            {
                int w = (pool.Length + 1) * 2;
                w = w - pool.Length > amount - pool.Length ? w : w + amount;
                Algs.IncreseArray(ref pool, w);

                for (int i = count; i < w; i++)
                    pool[i] = UnityEngine.GameObject.Instantiate(original);
            }
            if(count > amount)
            {
                for (int i = count - 1; i > amount - 1; i--)
                    pool[i].gameObject.SetActive(false);
                count = amount;
            }
            else if(count < amount)
            {
                for (; count < amount; count++)
                    pool[count].gameObject.SetActive(true);
            }

            return new ArrayProxy<T>(pool, 0, count);

        }

        public void Clear()
        {
            foreach (var o in pool)
                UnityEngine.GameObject.Destroy(o.gameObject);
            pool = new T[0];
            count = 0;
        }

        struct ArrayProxy<T> : IEnumerable<T>
        {
            T[] array;
            int begin;
            int end;

            public ArrayProxy(T[] array, int begin, int end)
            {
                this.array = array;
                this.begin = begin;
                this.end = end;
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (int i = begin; i < end; i++)
                    yield return array[i];
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
