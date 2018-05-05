using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    [Serializable]
    public class MyDictionary<Tkey, Tvalue>
    {
        Tkey[] keys = new Tkey[1];
        Tvalue[] values = new Tvalue[1];

        int length = 0;

        public Tvalue this [Tkey key] {
            get { for (int i = 0; i < length; i++) if (keys[i].Equals(key)) return values[i]; Tvalue def = Activator.CreateInstance<Tvalue>(); Add(key, def); return def; }
            set { for (int i = 0; i < length; i++) if (keys[i].Equals(key)) values[i] = value; else Add(key, value); }
        }

        public Tvalue this[Tkey key, Tvalue defaultValue] {
            get { for (int i = 0; i < length; i++) if (keys[i].Equals(key)) return values[i]; Add(key, defaultValue); return defaultValue; }
        }

        public IEnumerable<Tkey> Keys { get { return keys; } }
        public IEnumerable<Tvalue> Values { get { return values; } }

        void Add(Tkey key, Tvalue value)
        {
            if(length == keys.Length)
            {
                Algs.IncreseArray(ref keys, (length + 1) * 2);
                Algs.IncreseArray(ref values, (length + 1) * 2);
            }

            keys[length] = key;
            values[length] = value;

            length++;
        }

        public IEnumerable<MyDictionaryPair> ByElements()
        {
            return new MyDictionaryNumerable(this);
        }

        struct MyDictionaryNumerable : IEnumerable<MyDictionaryPair>
        {
            MyDictionary<Tkey, Tvalue> dict;

            public MyDictionaryNumerable(MyDictionary<Tkey, Tvalue> dict)
            {
                this.dict = dict;
            }

            public IEnumerator<MyDictionaryPair> GetEnumerator()
            {
                for (int i = 0; i < dict.length; i++)
                    yield return new MyDictionaryPair(dict.keys[i], dict.values[i]);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public struct MyDictionaryPair
        {
            public Tkey key;
            public Tvalue value;

            public MyDictionaryPair(Tkey key, Tvalue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }


}
