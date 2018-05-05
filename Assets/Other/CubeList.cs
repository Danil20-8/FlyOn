using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Other
{
    public class CubeList<T>
    {
        public CubeList<T> top = null;
        public CubeList<T> bot = null;
        public CubeList<T> left = null;
        public CubeList<T> right = null;
        public CubeList<T> forward = null;
        public CubeList<T> back = null;

        public T item;
        public CubeList(T item)
        {
            this.item = item;
        }
        public IEnumerable<CubeList<T>> GetCube(int size, Predicate<T> predicate)
        {
            List<CubeList<T>> result = new List<CubeList<T>>();
            if(predicate(item))
                result.Add(this);
            foreach (var t in GetTop(size))
            {
                if (predicate(t.item))
                    result.Add(t);
                foreach (var tr in t.GetRight(size))
                {
                    if (predicate(tr.item))
                        result.Add(tr);
                    foreach (var trf in tr.GetForward(size))
                        if (predicate(trf.item))
                            result.Add(trf);
                    foreach (var trb in tr.GetBack(size))
                        if (predicate(trb.item))
                            result.Add(trb);
                }
            }
            foreach (var r in GetRight(size))
            {
                if (predicate(r.item))
                    result.Add(r);
                foreach (var rb in r.GetBot(size))
                {
                    if (predicate(rb.item))
                        result.Add(rb);
                    foreach (var rbf in rb.GetForward(size))
                        if (predicate(rbf.item))
                            result.Add(rbf);
                    foreach (var rbb in rb.GetBack(size))
                        if (predicate(rbb.item))
                            result.Add(rbb);
                }
            }
            foreach (var b in GetBot(size))
            {
                if (predicate(b.item))
                    result.Add(b);
                foreach (var bl in b.GetLeft(size))
                {
                    if (predicate(bl.item))
                        result.Add(bl);
                    foreach (var blf in bl.GetForward(size))
                        if (predicate(blf.item))
                            result.Add(blf);
                    foreach (var blb in bl.GetBack(size))
                        if (predicate(blb.item))
                            result.Add(blb);
                }
            }
            foreach (var l in GetLeft(size))
            {
                if (predicate(l.item))
                    result.Add(l);
                foreach (var lt in l.GetTop(size))
                {
                    if (predicate(lt.item))
                        result.Add(lt);
                    foreach (var ltf in lt.GetForward(size))
                        if (predicate(ltf.item))
                            result.Add(ltf);
                    foreach (var ltb in lt.GetBack(size))
                        if (predicate(ltb.item))
                            result.Add(ltb);
                }
            }
            return result;
            /*return new CubeList<T>[] {this}.Concat(
                GetTop(size, false).Select(c => { var r = c.GetRight(size); return r.Select(t => t.GetForward(size)).Concat(r.Select(t => t.GetBack(size, false))); }).SelectMany(c => c.SelectMany(t => t)))
                .Concat(
                GetRight(size, false).Select(c => { var r = c.GetBot(size); return r.Select(t => t.GetForward(size)).Concat(r.Select(t => t.GetBack(size, false))); }).SelectMany(c => c.SelectMany(t => t)))
                .Concat(
                GetBot(size, false).Select(c => { var r = c.GetLeft(size); return r.Select(t => t.GetForward(size)).Concat(r.Select(t => t.GetBack(size, false))); }).SelectMany(c => c.SelectMany(t => t)))
                .Concat(
                GetLeft(size, false).Select(c => { var r = c.GetTop(size); return r.Select(t => t.GetForward(size)).Concat(r.Select(t => t.GetBack(size, false))); }).SelectMany(c => c.SelectMany(t => t)));
                */
        }
        public IEnumerable<CubeList<T>> GetTop(int size)
        {
            return new LCube<T>(this, c => c.top, size);
        }
        public IEnumerable<CubeList<T>> GetBot(int size)
        {
            return new LCube<T>(this, c => c.bot, size);
        }
        public IEnumerable<CubeList<T>> GetLeft(int size)
        {
            return new LCube<T>(this, c => c.left, size);
        }
        public IEnumerable<CubeList<T>> GetRight(int size)
        {
            return new LCube<T>(this, c => c.right, size);
        }
        public IEnumerable<CubeList<T>> GetForward(int size)
        {
            return new LCube<T>(this, c => c.forward, size);
        }
        public IEnumerable<CubeList<T>> GetBack(int size)
        {
            return new LCube<T>(this, c => c.back, size);
        }
    }

    class LCube<T>: IEnumerable<CubeList<T>>
    {
        Func<CubeList<T>, CubeList<T>> next;
        CubeList<T> self;
        int count;
        public LCube(CubeList<T> self, Func<CubeList<T>, CubeList<T>> next, int count)
        {
            this.self = self;
            this.next = next;
            this.count = count;
        }

        public IEnumerator<CubeList<T>> GetEnumerator()
        {
            CubeList<T> c = next(self);
            for (int i = 0; i < count && c != null; i++)
            {
                yield return c;
                c = next(c);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
