using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public class LimitedProperty<T>
    {
        T property;
        public int Count { get { return maxCount - count; } }
        int count;
        int maxCount;
        public T Value { get { if (count < maxCount) count++; else throw new Exception("Exceeded limit access"); return property; } }
        public LimitedProperty(T property, int maxCount)
        {
            this.count = 0;
            this.maxCount = maxCount;
            this.property = property;
        }
        public bool Get(out T result, T defaultValue)
        {
            if (count < maxCount) count++;
            else { result = defaultValue; return false; }
            result = property;
            return true;
        }
        public bool IsAble()
        {
            return count < maxCount;
        }
    }

    //I don't know why it's here. Remove it if will not remember
    public class LimitedList<T>
    {
        T[] arr;
        Func<T, float> weightSelector;
        Func<float, float, bool> predicate;
        int min;
        float minWeight;
        int length;
        public LimitedList(int length, Func<T, float> weightSelector, Func<float, float, bool> predicate)
        {
        }
        public void Add(T item)
        {
            if (arr.Length == 0)
            {
                min = 0;
                minWeight = weightSelector(item);
                arr[0] = item;
            }
            else if (arr.Length == length)
            {
                float w = weightSelector(item);
                if (predicate(w, minWeight))
                {
                    minWeight = w;
                    min = arr.Length;
                    arr[arr.Length] = item;
                }
            }
            else
            {
                float w = weightSelector(item);
                if (predicate(w, minWeight))
                {
                    minWeight = w;
                    min = arr.Length;
                    arr[arr.Length] = item;
                }
            }

        }
    }
}
