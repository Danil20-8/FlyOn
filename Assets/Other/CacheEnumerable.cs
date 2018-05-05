using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CacheEnumerable<T> : IEnumerable<T>
{
    List<T> cache = null;
    bool bnew = true;
    IEnumerable<T> source;

    public int Count { get { return bnew ? this.Count() : cache.Count; } }

    public CacheEnumerable(IEnumerable<T> enumerable)
    {
        this.source = enumerable;
    }
    public IEnumerable<T> New()
    {
        bnew = true;
        return this;
    }
    IEnumerator<T> NewEnumerator()
    {
        if (cache != null)
            cache = new List<T>(cache.Capacity);
        else
            cache = new List<T>();
        foreach (var e in source)
        {
            cache.Add(e);
            yield return e;
        }
        bnew = false;
    }
    public IEnumerator<T> GetEnumerator()
    {
        if (bnew)
        {
            return NewEnumerator();
        }
        else
            return cache.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public List<T> GetCache()
    {
        return cache;
    }
}

